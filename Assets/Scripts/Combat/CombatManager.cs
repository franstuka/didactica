using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    [SerializeField] private Transform positionToSpawnEnemy;
    public GameObject PanelVida;
    private List<Card> cardListCombat;

    private GameObject enemySpawned;
    private float sumProbability;
    private float substractionProbability;
    private float multiplyProbability;
    private float divideProbability;

    private int monsterLife;
    private int monsterMovementsOnDefeat;
    private int maxOperations;
    private int maxCardsInHand = 6;

    private void Awake()
    {
        cardListCombat = new List<Card>();
        CreateMonsterValue();
    }

    private void CreateMonsterValue()
    {
        DeterminateMonsterToSpawn();
        GetCombatCardList();
        GetMonsterLife();
        //Elección de monstruo y vida
    }

    private void DeterminateMonsterToSpawn()
    {
        object[] monsterData = GameManager.instance.GetMonsterOnCombat(); //first random combat, second monster name, third level
        bool errorOnFindEnemy = false;

        if((bool)monsterData[0] == false) //not random combat
        {
            bool end = false;
            Dictionary<int, GameObject> enemyListDictionary = GameManager.instance.GetMonsterLevelList((int)monsterData[2]);
            if(enemyListDictionary.Count == 0)
            {
                Debug.LogError("Dictionary size on this level is 0");
                errorOnFindEnemy = true;
            }
            else
            {
                for (int i = 0; i < enemyListDictionary.Count && !end; i++) //find the enemy referenced
                {
                    if(enemyListDictionary[i].name == (string)monsterData[1])
                    {
                        end = true;
                        SpawnEnemy(enemyListDictionary[i]);
                    }
                }
                if(!end)
                {
                    errorOnFindEnemy = true;
                }
            }
            
        }
        if((bool)monsterData[0] == true || errorOnFindEnemy) //spawn a random enemy
        {
            SpawnEnemy(null);
        }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        if(enemy != null)
        {
            enemySpawned = Instantiate(enemy, positionToSpawnEnemy.position,positionToSpawnEnemy.rotation);
            enemySpawned.transform.position = new Vector3 (0,0,0);
            enemySpawned.GetComponent<Navegation>().enabled = false;
            enemySpawned.GetComponent<Animator>().SetTrigger("Combate");
            GetEnemyParameters(enemySpawned);
        }
        else
        {
            
            if(GameManager.instance.GetMonsterLevelList(GameManager.instance.GetActualLevel()).Count > 0)
            {
                //Explanation: Gets the monster list asociated to the actual level and selects one random enemy inside the list
                enemy = GameManager.instance.GetMonsterLevelList(GameManager.instance.GetActualLevel())
                    [Mathf.FloorToInt(Random.Range(0f, 0.999f) * GameManager.instance.GetMonsterLevelList(GameManager.instance.GetActualLevel()).Count)];
                GameObject enemySpawned = Instantiate(enemy, positionToSpawnEnemy.position,positionToSpawnEnemy.rotation);
                enemySpawned.transform.position = new Vector3 (0,0,0);
                enemySpawned.GetComponent<Navegation>().enabled = false;
                enemySpawned.GetComponent<Animator>().SetTrigger("Combate");
                GetEnemyParameters(enemySpawned);
            }
            else
            {
                Debug.LogError("No enemies on level " + GameManager.instance.GetActualLevel() + " list");
            }
            
        }
    }

    private void GetEnemyParameters(GameObject enemySpawned)
    {
        EnemyCombat stats = enemySpawned.GetComponent<EnemyCombat>();
        if(stats != null)
        {
            float[] probabilities = stats.GetEnemyProbabilities();
            sumProbability = probabilities[0];
            substractionProbability = probabilities[1];
            multiplyProbability = probabilities[2];
            divideProbability = probabilities[3];
            maxOperations = stats.GetMaxOperations();
            monsterMovementsOnDefeat = stats.GetEnemyMovementsOnDefeat();
        }
        else
        {
            Debug.LogError("Enemy spawned don't have the EnemyCombat component");
        }
    }

    private void GetCombatCardList() //pueden ser repetidas
    {
        if(maxCardsInHand < maxOperations + 1)
        {
            Debug.LogError("There is more operations than cards in the hand");
        }
        else
        {
            for (int i = 0; i < maxCardsInHand; i++)
            {
                int randomCard = Random.Range(0, InventarySystem.instance.GetCardList().Count);
                cardListCombat.Add(InventarySystem.instance.GetCardList()[randomCard]);
            }
        } 
    }

    #region GetEnemyLife

    private void GetMonsterLife()
    {
        bool end = false;
        int operationsRealizated = 0;
        int searchedNum;
        int result = 0;
        LinkedList<int> cardValues = new LinkedList<int>();
        LinkedList<int> cardOperations = new LinkedList<int>();
        LinkedList<int> numOrder = new LinkedList<int>();
        LinkedList<int> operationOrder = new LinkedList<int>();
        //add values to list
        for (int i = 0; i < cardListCombat.Count; i++)
        {
            cardValues.AddFirst(cardListCombat[i].value);
        }

        //add operations as int
        for (int i = 0; i < InventarySystem.instance.GetCardOperationList().Count; i++)
        {
            switch(InventarySystem.instance.GetCardOperationList()[i].cardOperation)
            {
                //order 0 = sum, 1 = substract, 2 = multiplication, 3 = division
                case CardType.ADD:
                    {
                        cardOperations.AddFirst(0);
                        break;
                    }
                case CardType.SUBSTRACT:
                    {
                        cardOperations.AddFirst(1);
                        break;
                    }
                case CardType.MULTIPLY:
                    {
                        cardOperations.AddFirst(2);
                        break;
                    }
                case CardType.DIVIDE:
                    {
                        cardOperations.AddFirst(3);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Operation not registered");
                        break;
                    }
            }
        }

        //select first element
        searchedNum = Random.Range(0, cardValues.Count);
        numOrder.AddLast(GetCardListValue(searchedNum, ref cardValues, true));
        
        Debug.Log(numOrder.First.Value);
        while (operationsRealizated < maxOperations && !end)
        {
            switch(GetOperationBasedOnPercentages(ref cardOperations))
            {
                case 0:
                    {
                        
                        searchedNum = Random.Range(0, cardValues.Count); //select value
                        Debug.Log(" + ");
                        Debug.Log(GetCardListValue(searchedNum, ref cardValues, false));
                        numOrder.AddLast(GetCardListValue(searchedNum, ref cardValues, true));
                        operationOrder.AddLast(0);
                        DeleteOperationFromList(ref cardOperations, 0);
                        operationsRealizated++;
                        
                        break;
                    }
                case 1:
                    {
                        searchedNum = Random.Range(0, cardValues.Count); //select value
                        Debug.Log(" - ");
                        Debug.Log(GetCardListValue(searchedNum, ref cardValues, false));
                        numOrder.AddLast(GetCardListValue(searchedNum, ref cardValues, true));
                        operationOrder.AddLast(1);
                        DeleteOperationFromList(ref cardOperations, 1);
                        operationsRealizated++;
                        
                        break;
                    }
                case 2:
                    {
                        searchedNum = Random.Range(0, cardValues.Count); //select value
                        Debug.Log(" * ");
                        Debug.Log(GetCardListValue(searchedNum, ref cardValues, false));
                        numOrder.AddLast(GetCardListValue(searchedNum, ref cardValues, true));
                        operationOrder.AddLast(2);
                        DeleteOperationFromList(ref cardOperations, 2);
                        operationsRealizated++;
                        
                        break;
                    }
                case 3:
                    {
                        object[] posibleDivision = ResolveDivisionCase(ref cardValues, ref cardOperations, result); 
                        //object[0] = valid division founded(bool),object[1] no other operations avaible(bool), object[2] divide value(int)

                        if ((bool)posibleDivision[0])
                        {
                            Debug.Log(" / ");
                            Debug.Log(GetCardListValue((int)posibleDivision[2], ref cardValues, false));
                            //if is a valid result, the num will be removed in ResolveDivisionCase
                            DeleteOperationFromList(ref cardOperations, 3);
                            numOrder.AddLast(GetCardListValue((int)posibleDivision[2], ref cardValues, true));
                            operationOrder.AddLast(3);
                            operationsRealizated++; 
                        }
                        else
                        {
                            if ((bool)posibleDivision[1])
                            {
                                end = true;
                            }
                        }
                        break;
                        
                    }
            }
        }
        LinkedListNode<int> numNode = numOrder.First;
        LinkedListNode<int> opNode = operationOrder.First;
        end = false;
        Debug.Log(result + "Result");
        while (opNode != null)
        {
            if(opNode.Value == 2 ) //multiplkication
            {
                if(opNode.Previous.Value == 1)
                {
                    result -= numNode.Value * numNode.Next.Value;
                    Debug.Log(result + "Result");
                }
                else
                {
                    result += numNode.Value * numNode.Next.Value;
                    Debug.Log(result + "Result");
                }
                
            }
            else if(opNode.Value == 3)  //division
            {
                result += numNode.Value / numNode.Next.Value;
                Debug.Log(result + "Result");
            }
        
            opNode = opNode.Next;
            numNode = numNode.Next;
        }
        numNode = numOrder.First;
        opNode = operationOrder.First;
        while (opNode != null)
        {
            if (opNode.Value == 0)  //add
            {
                if(opNode.Next == null || opNode.Next.Value != 2 && opNode.Next.Value != 3)
                {
                    result += numNode.Next.Value;
                    Debug.Log(result + "Result");
                }
                
            }
            else if (opNode.Value == 1)  //subst
            {
                if (opNode.Next == null || opNode.Next.Value != 2 && opNode.Next.Value != 3)
                {
                    result -= numNode.Next.Value;
                    Debug.Log(result + "Result");
                }
            }
            opNode = opNode.Next;
            numNode = numNode.Next;
        }
        if(operationOrder.First.Value != 2 && operationOrder.First.Value != 3) //add first number
        {
            result += numOrder.First.Value;
        }
        numNode = numOrder.First;
        opNode = operationOrder.First;
        while (opNode != null)
        {
            Debug.Log(numNode.Value);
            Debug.Log(opNode.Value + "OP");
            opNode = opNode.Next;
            numNode = numNode.Next;
        }
        Debug.Log(numNode.Value);
        monsterLife = result; //put the result as monsterlife
    }

    private object[] ResolveDivisionCase(ref LinkedList<int> cardValues, ref LinkedList<int> cardOperations, int result)
    {
        // the return:  object[0] = valid division founded(bool),object[1] no other operations avaible(bool), object[2] divide value(int)
        int searchedNum = Random.Range(0, cardValues.Count); //try to get one random
        int value = GetCardListValue(searchedNum, ref cardValues, false);

        if(result % value == 0)//if its correct
        {
            GetCardListValue(searchedNum, ref cardValues, true); //delete number in linked list
            return new object[] { true, false, result / value };
        }
        else //found if there is a valid value in number list
        {
            LinkedListNode<int> node = cardValues.First;
            value = 0;
            for (int i = 0; node != null; i++)
            {
                if (result % node.Value != 0)
                {
                    node = node.Next;
                    continue;
                }
                else
                {
                    value = node.Value;
                    GetCardListValue(i, ref cardValues, true); //delete number in linked list
                    return new object[] { true, false, result / value };
                }
            }
            if(cardOperations.Count > 1) //other operations still avaible
            {
                return new object[] { false, false, 0};
            }
            else //no other operations avaible so its the end 
            {
                return new object[] { false, true, 0};
            }
        }
    }

    private int GetCardListValue(int searched, ref LinkedList<int> cardValues , bool delete)
    {
        LinkedListNode<int> node = cardValues.First;
        int value = 0;
        for (int i = 0; node != null; i++)
        {
            if(i != searched)
            {
                node = node.Next;
                continue;
            }
            else
            {
                value = node.Value;
                if(delete)
                {
                    cardValues.Remove(node);
                }    
                return value;
            }
        }
        Debug.LogError("Position don't found on cardList");
        return value;
    }

    private int GetOperationBasedOnPercentages(ref LinkedList<int> operationList) //add more operations here
    {
        //find value(int): 0 = sum, 1 = substract, 2 = multiplication, 3 = division
        if (!ScaleProbabilities())
        {
            Debug.LogError("Probabilities are bad configurated, enemy has more operations than allowed");
        }
        float randomValue = Random.value * 100f;
        int returnValue = 0;

        //operationList.Find(n) gets the node who contains that valor
        if (randomValue <= sumProbability)
        {
            returnValue = operationList.Find(0).Value;
            return returnValue;  
        }
        else if(randomValue <= sumProbability + substractionProbability)
        {
            returnValue = operationList.Find(1).Value;
            return returnValue;  
        }
        else if(randomValue <= sumProbability + substractionProbability + multiplyProbability)
        {
            returnValue = operationList.Find(2).Value;
            return returnValue;  
        }
        else
        {
            returnValue = operationList.Find(3).Value;
            return returnValue;  
        }
    }

    private void DeleteOperationFromList(ref LinkedList<int> operationList, int operation) //add more operations here
    {
        switch(operation)
        {
            case 0:
                {
                    sumProbability = 0f;
                    operationList.Remove(operationList.Find(0));
                    break;
                }
            case 1:
                {
                    substractionProbability = 0f;
                    operationList.Remove(operationList.Find(1));
                    break;
                }
            case 2:
                {
                    multiplyProbability = 0f;
                    operationList.Remove(operationList.Find(2));
                    break;
                }
            case 3:
                {
                    divideProbability = 0f;
                    operationList.Remove(operationList.Find(3));
                    break;
                }
            default:
                {
                    Debug.LogError("Operation don't defined");
                    break;
                }
        }
    }

    private bool ScaleProbabilities()
    {
        float acumulatedProb = 0f;
        float scaleFactor = 0f;

        acumulatedProb = sumProbability + substractionProbability + multiplyProbability + divideProbability;
        if (acumulatedProb == 0f)
        {
            return false;
        }
        else
        {
            if (acumulatedProb != 100f) 
            {
                scaleFactor = 100f / acumulatedProb;
                sumProbability *= scaleFactor;
                substractionProbability *= scaleFactor;
                multiplyProbability *= scaleFactor;
                divideProbability *= scaleFactor;
            }
        }
        return true;
    }

    #endregion

    public void ResolveCombat(int value)
    {

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            if (value == monsterLife) //end combat
            {
                player.GetComponent<PlayerMovement>().movementsAvaible += monsterMovementsOnDefeat;
                GameManager.instance.OnCombatFinish();
            }
            else //do damage
            {
                player.ChangeStats(CombatStats.CombatStatsType.HP, -1);
                Transform corazon = PanelVida.transform.GetChild(0);
                Destroy(corazon.gameObject);
                enemySpawned.GetComponent<Animator>().SetTrigger("Ataque");
                //enemySpawned.GetComponent<Animator>().ResetTrigger("Ataque");
                //maybe end game
            }
        }
        else
        {
            Debug.LogError("Player not found on resolve combat");
        }
    } 

    public List<Card> GetCombatCards()
    {
        return cardListCombat;
    }

    public int getMonsterLife(){

        return monsterLife;

    }
}
