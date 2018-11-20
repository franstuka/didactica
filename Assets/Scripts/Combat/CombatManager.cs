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
        for (int i = 0; i < 6; i++) 
        {
            int randomCard = Random.Range(0, InventarySystem.instance.GetCardList().Count);
            cardListCombat.Add(InventarySystem.instance.GetCardList()[randomCard]);
        }
    }

    public void CreateMonsterValue()
    {
        DeterminateMonsterToSpawn();
        GetCombatCardList();

        //Elección de monstruo y vida
    }
    
    public void ResolveCombat(int value)
    {

    }

    
}
