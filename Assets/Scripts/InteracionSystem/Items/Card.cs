using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class Card : Item { 
    
    public float Value;
    public GameObject prefab;

    public string GetDescription(Card item)
    {
        return item.name;
     
    }
}
