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
    public Transform playerPositionOnScene;
    public List<GameObject> enemiesPosition;
    //public List<Vector3> enemyTargetMovement; //maybe not necesary
    public List<GameObject> staticItemsInScene; //like chest etc
    public int sceneLevel; //just for error control

    private void Awake()
    {
        maxLevel = 1;
        inventoryStored = new List<Item>();
        enemiesPosition = new List<GameObject>();
        //enemyTargetMovement = new List<Vector3>();
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
        Debug.Log("holi");
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
        playerPositionOnScene = player.transform;
        EnemyCombat[] enemies = FindObjectsOfType<EnemyCombat>();
        List<GameObject> enemiesList = new List<GameObject>();
        //Chest[] chests = FindObjectsOfType<Chest>();
        //List<GameObject> chestList = new List<GameObject>();
        sceneLevel = GameManager.instance.GetActualLevel();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesList.Add(enemies[i].gameObject);
        }
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
        if(player != null && sceneLevel == actualLevel)
        {
            player.transform.SetPositionAndRotation(playerPositionOnScene.position,playerPositionOnScene.rotation);
            for (int i = 0; i < enemiesPosition.Count; i++)
            {
                Instantiate(enemiesPosition[i]);
            }
            for (int i = 0; i < staticItemsInScene.Count; i++)
            {
                Instantiate(staticItemsInScene[i]);
            }
        }
        else
            Debug.LogError("Something is going grong on load level");
    }

    public void SaveOnPersistent()
    {
        //Application.persistentDataPath
    }

    public void LoadInPersistent()
    {

    }

    private void Update() //for test
    {
        if(Input.GetKeyDown("0"))
        {
            SavePlayerData();
        }
    }
}