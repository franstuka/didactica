using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Singleton

    public static GameManager instance;
    private static SaveDataManager saveDataManager;

    private void Awake()
    {
        bool error = false;

        if (instance != null)
        {
            Debug.LogError("More than one instance of GameManager is trying to active");
            error = true;
        }
        if (saveDataManager != null)
        {
            Debug.LogError("More than one instance of saveDataManager is trying to active");
            error = true;
        }
        if(error)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        saveDataManager = GetComponent<SaveDataManager>();
        if(saveDataManager == null)
        {
            saveDataManager = gameObject.AddComponent<SaveDataManager>();
        }
    }

    #endregion

    [SerializeField] private PauseMenuScript menu;

    public void EndLevelLost()
    {
        menu.die.StartFade();
    }

    public void EndLevelWin()
    {
        menu.win.StartFade();
    }

    public void OnSceneEnter()
    {

    }

    public void OnCombatEnter()
    {

    }

    public void ReturnToLevelScene()
    {

    }

    public void OnSceneExit()
    {

    }

    public void SaveAndQuit()
    {

    }

    public void ChangeScene(string sceneName)
    {

    }

    public void InicialicePlayerData()
    {

    }
}