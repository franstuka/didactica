using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    

    #region Singleton

    public static Level instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Debug.LogError("More than one instance of level is trying to active");
            return;
        }

        instance = this;
    }
    #endregion

    private int level = 1;

    public int GetLevel()
    {
        return level;
    }

    public void SetLevel()
    {
        level++;
    }
}
