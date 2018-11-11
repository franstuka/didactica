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

    private const int startPlayerHp = 5;
    private const int startLevel = 1;
    [SerializeField] List<Item> initialItems = new List<Item>();


    [SerializeField] private PauseMenuScript menu;

    private void Start()
    {
        InicialicePlayerData();
    }

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
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;

        if(player != null)
        {
            player.GetComponent<PlayerMovement>().movementsAvaible = GetLevelMovements(startLevel);
            player.GetComponent<PlayerMovement>().ChangeStats(CombatStats.CombatStatsType.MAXHP, startPlayerHp);
            player.GetComponent<PlayerMovement>().ChangeStats(CombatStats.CombatStatsType.HP, startPlayerHp);
        }
        else
        {
            Debug.LogError("Cant find player on initialization");
        }
        if(InventarySystem.instance != null)
        {
            for (int i = 0; i < initialItems.Count; i++)
            {
                InventarySystem.instance.AddNewElement(initialItems[i]);
            }
        }
        else
        {
            Debug.LogError("Cant find inventory on initialization");
        }
    }

    private int GetLevelMovements(int level) //level movements setup
    {
        switch(level)
        {
            case 1:
            {
                return 30;
            }
            case 2:
            {
                return 40;
            }
            default:
            {
                return 50;
            }
        }
    }

}