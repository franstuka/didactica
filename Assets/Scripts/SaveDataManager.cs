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
    public List<Vector3> enemyTargetMovement;
    public List<GameObject> staticItemsInScene; //like chest etc
    public int sceneLevel; //just for error control

    private void Awake()
    {
        maxLevel = 1;
        inventoryStored = new List<Item>();
        enemiesPosition = new List<GameObject>();
        enemyTargetMovement = new List<Vector3>();
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
        }
    }

    public void SavePlayerData()
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;

        actualLevel = GameManager.instance.GetActualLevel();

        if (player != null)
        {
            movementsAvaible = player.GetComponent<PlayerMovement>().movementsAvaible;
            playerHP = player.GetComponent<PlayerMovement>().GetHP();
            playerMaxHP = player.GetComponent<PlayerMovement>().GetMaxHP();
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

    }

    public void LoadPlayerData(bool loadInventory)
    {

    }

    public void LoadLevelData()
    {

    }

    public void SaveOnPersistent()
    {

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