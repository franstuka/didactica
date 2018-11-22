using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class Card : Item { 
    
    public int value;
    public GameObject prefab;


    ////////////////////////////////////////////////////////////////////////////
    //Sección de prueba
    public Card(int newValue)
    {
        value = newValue;
    }
    ////////////////////////////////////////////////////////////////////////////




    public string GetDescription(Card item)
    {
        return item.name;
     
    }
}
