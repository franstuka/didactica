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
    private GameObject player;
    [SerializeField] private PickUpText text;


    private void Start()
    {
        /*
        player = GameObject.Find("Player");
        if (!player)
        {
            Debug.LogError("Can't find player");
        }
             */
    }


    public List<Card> GetCardList()
    {
        return cardList;
    }

    public List<CardOperation> GetCardOperationList()
    {
        cardOperationList.Add(new CardOperation("Sum"));
        cardOperationList.Add(new CardOperation("Substraction"));
        cardOperationList.Add(new CardOperation("Multiply"));
        cardOperationList.Add(new CardOperation("Divide"));
        return cardOperationList;

    }
    //////////////////////////////////////////////////////////////////////////////



    public void AddNewElement(Item item)
    {
        /*if (!item.isDefaultItem)
        {
            inventory.Add(item);
        }*/

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

    public void DeleteElement(Item item)
    {
        inventory.Remove(item);
    }
    
    /*
    public void DropWeapon()
    {
        GameObject spawnThis = Instantiate(weapon.prefab, player.transform.position, player.transform.rotation);
        spawnThis.transform.position += spawnThis.transform.TransformVector(new Vector3(0, dropOffsetY / spawnThis.transform.lossyScale.y, dropOffsetZ / spawnThis.transform.lossyScale.z));  
    }   //Es necesatio multiplicarlo por la escala del objeto sino no funciona bien
    */

    /*private void Update()
    {
        if(Input.GetButtonDown("Drop"))
        {
            DropWeapon();
        }
    }*/
}