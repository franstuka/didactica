using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster/NewMonster")]
public class Monster : ScriptableObject
{

    public string alias;
    public int life;

    ////////////////////////////////////////////////////////////////////////////
    //Sección de prueba
    public Monster(string newAlias, int newLife)
    {
        alias = newAlias;
        life = newLife;
        
    }
    ////////////////////////////////////////////////////////////////////////////
}
