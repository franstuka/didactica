using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class Card : Item { 
    
    public float value;
    public GameObject prefab;


    ////////////////////////////////////////////////////////////////////////////
    //Sección de prueba
    public Card(float newValue)
    {
        value = newValue;
    }
    ////////////////////////////////////////////////////////////////////////////




    public string GetDescription(Card item)
    {
        return item.name;
    }
}
