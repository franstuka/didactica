using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSystem : MonoBehaviour {

    #region Singleton

    public static MonsterSystem instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Debug.LogError("More than one instance of monster is trying to active");
            return;
        }

        instance = this;
    }

    #endregion
    public List<Monster> monsterList = new List<Monster>();

    private void Start()
    {
        
        
        
    }

    ////////////////////////////////////////////////////////////////////////////
    //Sección de prueba
    public List<Monster> GetMonsterList()
    {
        monsterList.Add(new Monster("Ojo", 0));
        monsterList.Add(new Monster("Volador", 0));
        monsterList.Add(new Monster("Cuernos", 0));
        monsterList.Add(new Monster("Zombi", 0));
        return monsterList;
    }
    //////////////////////////////////////////////////////////////////////////////

}
