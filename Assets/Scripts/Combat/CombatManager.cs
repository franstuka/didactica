using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    [SerializeField] private Transform positionToSpawnEnemy;

    private List<Card> cardListCombat;

    public float sumProbability;
    public float substractionProbability;
    public float multiplyProbability;
    public float divideProbability;

    private int monsterLife;
    public int monsterMovementsOnDefeat;
    public int maxOperations;
    public int maxCardsInHand = 6;

    private void Awake()
    {
        cardListCombat = new List<Card>();
        CreateMonsterValue();
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
            GameObject enemySpawned = Instantiate(enemy, positionToSpawnEnemy);
            GetEnemyParameters(enemySpawned);
        }
        else
        {
            
            if(GameManager.instance.GetMonsterLevelList(GameManager.instance.GetActualLevel()).Count > 0)
            {
                //Explanation: Gets the monster list asociated to the actual level and selects one random enemy inside the list
                enemy = GameManager.instance.GetMonsterLevelList(GameManager.instance.GetActualLevel())
                    [Mathf.FloorToInt(Random.Range(0f, 0.999f) * GameManager.instance.GetMonsterLevelList(GameManager.instance.GetActualLevel()).Count)];
                GameObject enemySpawned = Instantiate(enemy, positionToSpawnEnemy);
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

    private void CreateMonsterValue()
    {
        DeterminateMonsterToSpawn();
        GetCombatCardList();
        GetMonsterLife();

        //Elección de monstruo y vida
    }
    
    private void GetMonsterLife()
    {
        bool prioritizeDivision = false;
        int operationsRealizated = 0;
        int searchedElement;
        int result = 0;
        LinkedList<int> cardValues = new LinkedList<int>();
        //add values to list
        for (int i = 0; i < cardListCombat.Count; i++)
        {
            cardValues.AddFirst(cardListCombat[i].value);
        }
        //select first element
        searchedElement = Random.Range(0, cardValues.Count);
        GetCardListValue(searchedElement, ref cardValues);
        while (operationsRealizated <= maxOperations)
        {

        }
    }

    private int GetCardListValue(int searched, ref LinkedList<int> cardValues)
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
                cardValues.Remove(node);
                return value;
            }
        }
        Debug.LogError("Position don't found on cardList");
        return value;
    }

    public void ResolveCombat(int value)
    {

    }

    
}
