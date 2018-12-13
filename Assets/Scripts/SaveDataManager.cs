using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveDataManager : MonoBehaviour {

    //player stats to save
    public int playerHP;
    public int playerMaxHP;
    public int movementsAvaible;
    public int actualLevel;
    public int maxLevel;
    public List<Item> inventoryStored;

    //Scene items to save
    public Vector3 playerPositionOnScene;
    public Quaternion playerRotationOnScene;
    //enemies info
    public List<Vector3> enemiesPositionList = new List<Vector3>();
    public List<Quaternion> enemiesOrientationList = new List<Quaternion>();
    public List<string> enemiesNameList = new List<string>();
    public List<int> enemiesLevelList = new List<int>();
    public List<int> enemiesStateList = new List<int>();
    public List<Vector3> enemiesTargetList = new List<Vector3>();
    public List<GameObject> staticItemsInScene; //like chest etc
    public int sceneLevel; //just for error control
    //data saved for combat
    public bool randomCombat;
    public string enemyName; //both needed for search the name in the enemy list and spawn the enemy using his prefab
    public int enemyLevel;
    public Vector3 enemyInCombatPosition;

    private void Awake()
    {
        maxLevel = 1;
        inventoryStored = new List<Item>();
        enemiesPositionList = new List<Vector3>();
        enemiesOrientationList = new List<Quaternion>();
        enemiesNameList = new List<string>();
        enemiesLevelList = new List<int>();
        enemiesStateList = new List<int>();
        enemiesTargetList = new List<Vector3>();
        staticItemsInScene = new List<GameObject>();
    }

    public void UpdateForNextLevel(bool addOneToActualLevel) //this funcition update player level info when he complete it
    {
        if(actualLevel + 1 > maxLevel)
        {
            maxLevel = actualLevel + 1;
        }
        if (addOneToActualLevel)
        {
            actualLevel++;
            sceneLevel++;
        }
    }

    public void SavePlayerData()
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;

        actualLevel = GameManager.instance.GetActualLevel();

        if (player != null)
        {
            movementsAvaible = player.GetComponent<PlayerMovement>().movementsAvaible;
            playerMaxHP = player.GetComponent<PlayerMovement>().GetMaxHP();
            playerHP = player.GetComponent<PlayerMovement>().GetHP();
        }
        else
        {
            Debug.LogError("Cant find player on SaveData");
        }
        if (InventarySystem.instance != null)
        {
            List<Card> cards = InventarySystem.instance.GetCardList();
            List<CardOperation> cardsOp = InventarySystem.instance.GetCardOperationList();
            List<Item> items = InventarySystem.instance.GetInventoryItems();

            inventoryStored.Clear();

            for (int i = 0; i < cards.Count; i++)
            {
                inventoryStored.Add(cards[i]);
            }
            for (int i = 0; i < cardsOp.Count; i++)
            {
                inventoryStored.Add(cardsOp[i]);
            }
            for (int i = 0; i < items.Count; i++)
            {
                inventoryStored.Add(items[i]);
            }
        }
        else
        {
            Debug.LogError("Cant find inventory on SaveData");
        }
    }

    public void SaveLevelData()
    {
        //get info
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        playerPositionOnScene = player.transform.position;
        playerRotationOnScene = player.transform.rotation;
        //find enemies and clear lists
        EnemyCombat[] enemies = FindObjectsOfType<EnemyCombat>();
        enemiesPositionList = new List<Vector3>();
        enemiesOrientationList = new List<Quaternion>();
        enemiesNameList = new List<string>();
        enemiesLevelList = new List<int>();
        enemiesStateList = new List<int>();
        enemiesTargetList = new List<Vector3>();
            
        //save data
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesPositionList.Add(enemies[i].transform.position);
            enemiesOrientationList.Add(enemies[i].transform.rotation);
            enemiesNameList.Add(enemies[i].name);
            enemiesLevelList.Add(enemies[i].GetEnemyLevel());
            enemiesStateList.Add(enemies[i].GetEnemyState());
            enemiesTargetList.Add(enemies[i].transform.position);
        }
        
        //Chest[] chests = FindObjectsOfType<Chest>();
        //List<GameObject> chestList = new List<GameObject>();
        //////////////////////
        sceneLevel = GameManager.instance.GetActualLevel();

        /*
        for (int i = 0; i < chest.Length; i++)
        {
            chestList.Add(chest[i].gameObject);
        }*/
        //save info
    }

    public void LoadPlayerData(bool loadInventory)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;

        GameManager.instance.SetActualLevel(actualLevel);

        if (player != null)
        {
            player.GetComponent<PlayerMovement>().movementsAvaible = movementsAvaible;
            player.GetComponent<PlayerMovement>().ChangeStats(CombatStats.CombatStatsType.MAXHP, playerMaxHP);
            player.GetComponent<PlayerMovement>().ChangeStats(CombatStats.CombatStatsType.HP , playerHP);
        }
        else
        {
            Debug.LogError("Cant find player on LoadData");
        }
        if (loadInventory && InventarySystem.instance != null)
        {
            InventarySystem.instance.ClearAllInventory();

            for (int i = 0; i < inventoryStored.Count; i++)
            {
                InventarySystem.instance.AddNewElement(inventoryStored[i]);
            }
        }
        else if(InventarySystem.instance == null)
            Debug.LogError("Cant find inventory on SaveData");
    }

    public void LoadLevelData()
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        
        bool end;
        if(player != null && sceneLevel == actualLevel)
        {
            player.transform.SetPositionAndRotation(playerPositionOnScene,playerRotationOnScene);
            //spawn enemies

            for (int i = 0; i < enemiesPositionList.Count; i++)
            {
                if(!randomCombat && enemiesPositionList[i] == enemyInCombatPosition)
                {
                    continue;
                }
                end = false;
                for (int j = 0; j < GameManager.instance.GetMonsterLevelList(enemiesLevelList[i]).Count && !end; j++)
                {
                    if (GameManager.instance.GetMonsterLevelList(enemiesLevelList[i])[j].name == enemiesNameList[i])
                    {
                        end = true;
                        GameObject enemySpawned = Instantiate<GameObject>(GameManager.instance.GetMonsterLevelList(enemiesLevelList[i])[j], enemiesPositionList[i], enemiesOrientationList[i]);
                        enemySpawned.GetComponent<EnemyCombat>().SetTarget(enemiesTargetList[i]);
                        enemySpawned.name = enemiesNameList[i];
                    }
                }
            }
            for (int i = 0; i < staticItemsInScene.Count; i++)
            {
                Instantiate(staticItemsInScene[i]);
            }
        }
        else
            Debug.LogError("Something is going grong on load level");
    }

    public void SaveEnemyData(bool randomCombat)
    {
        this.randomCombat = randomCombat;
    }

    public void SaveEnemyData(bool randomCombat ,string name, int level, Vector3 enemyInCombatPosition)
    {
        this.randomCombat = randomCombat;
        enemyName = name;
        enemyLevel = level;
        this.enemyInCombatPosition = enemyInCombatPosition;
    }

    public object[] LoadEnemyData() //dim 3
    {
        return new object[] { randomCombat, enemyName, enemyLevel }; 
    }

    public void ClearEnemyData() //with this is enought for ignore the data stored and use a random enemy and other effects
    {
        randomCombat = true;
    }

    public void SaveOnPersistent()
    {
        //Application.persistentDataPath
    }

    public void LoadInPersistent()
    {

    }
}