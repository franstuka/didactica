using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;//Item por defecto?

    public virtual void Use()
    {

        //Debug.Log("Using" + name);
    }
}
