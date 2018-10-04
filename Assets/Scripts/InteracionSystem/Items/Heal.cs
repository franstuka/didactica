using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal", menuName = "Inventory/Heal")]
public class Heal : Item
{
    public int HealValue;
    public GameObject prefab;

    public string GetDescription(Heal item)
    {
        return item.name;
    }
}
