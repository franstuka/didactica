using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarySystem : MonoBehaviour
{
    #region Singleton

    public static InventarySystem instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Debug.LogError("More than one instance of inventory is trying to active");
            return;
        }

        instance = this;
    }

    #endregion

    public List<Item> inventory = new List<Item>();
    public List<Card> cardList = new List<Card>();
    public List<CardOperation> cardOperationList = new List<CardOperation>();
    [SerializeField] private PickUpText text;

    public void AddNewElement(Item item)
    {

        if ( item is Card)
        {
            Card addCardInList = (Card)item;
            cardList.Add(addCardInList);
        }
        else
        {
            if (item is CardOperation)
            {
                CardOperation addCardInList = (CardOperation)item;
                cardOperationList.Add(addCardInList);
            }
            else
                inventory.Add(item);
        }  
    }

    public void DeleteElement(Item item) //only for usable items
    {
        inventory.Remove(item);
    }

    public List<Card> GetCardList()
    {
        return cardList;
    }

    public List<CardOperation> GetCardOperationList()
    {
        return cardOperationList;
    }

    public List<Item> GetInventoryItems()
    {
        return inventory;
    }

    public void ClearAllInventory()
    {
         inventory.Clear();
         cardList.Clear();
         cardOperationList.Clear();
    }
}