using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Singleton

    public static GameManager instance;
    private static SaveDataManager saveDataManager;

    private void Awake()
    {
        bool error = false;
        
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager is trying to active");
            error = true;
        }
        if (saveDataManager != null)
        {
            Debug.LogWarning("More than one instance of saveDataManager is trying to active");
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

    [SerializeField] List<Item> initialItems = new List<Item>();
    [SerializeField] private PauseMenuScript menu;

    private const int startPlayerHp = 5;

    public int actualLevel;
    public bool onCombat;
    public bool levelWasStarted;

    private void Start()
    {
        InicialiceFirstLevelPlayerData(1);
    }

    public void EndLevelLost()
    {
        menu.die.StartFade();
    }

    public void EndLevelWin()
    {
        menu.win.StartFade();
    }

    public void OnOpenGame()
    {
        saveDataManager.LoadInPersistent();
        saveDataManager.LoadPlayerData(true);
    }

    public void OnSceneEnter()
    {
        levelWasStarted = true;
        saveDataManager.LoadPlayerData(false);
    }

    public void OnCombatEnter() //random combat
    {
        onCombat = true;
        saveDataManager.SavePlayerData();
        saveDataManager.SaveLevelData();
        saveDataManager.SaveEnemyData(true);
        ChangeScene("Combat");//TEST
    }

    public void OnCombatEnter(string enemyName , int enemyLevel , Vector3 enemyPosition) //non random combat, we save enemy stuff
    {
        onCombat = true;
        saveDataManager.SavePlayerData();
        saveDataManager.SaveLevelData();
        saveDataManager.SaveEnemyData(false, enemyName, enemyLevel, enemyPosition);
        ChangeScene("Combat");//TEST
    }

    public void OnCombatFinish()
    {
        onCombat = false;
        saveDataManager.SavePlayerData();
        ChangeScene("SampleScene void"); //TEST
    }

    public void ReturnToLevelScene()
    {
        saveDataManager.LoadPlayerData(true);
        saveDataManager.LoadLevelData();
    }

    public void OnSceneExit()
    {
        saveDataManager.SavePlayerData();
        saveDataManager.SaveLevelData();
    }

    public void OnLevelEnded(bool changeToNextLevel)
    {
        levelWasStarted = false;
        saveDataManager.UpdateForNextLevel(changeToNextLevel);
        saveDataManager.SavePlayerData();
        if(changeToNextLevel)
        {
            //ChangeScene("levelName");
        }
        else
        {
            //ChangeScene("MainMenu");
        }
    }

    public void SaveAndQuit()
    {
        saveDataManager.SavePlayerData();
        saveDataManager.SaveLevelData();
        saveDataManager.SaveOnPersistent();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadPlayerDataOnScene());
    }

    public void InicialiceFirstLevelPlayerData(int level)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        actualLevel = level;

        if (player != null)
        {
            player.GetComponent<PlayerMovement>().movementsAvaible = GetLevelMovements(level);
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
        OnSceneExit();
    }

    private int GetLevelMovements(int level) //level movements setup
    {
        switch(level)
        {
            case 1:
            {
                return 5;
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

    public int GetActualLevel()
    {
        return actualLevel;
    }

    public void SetActualLevel(int level)
    {
        actualLevel = level;
    }

    public bool GetOnCombat()
    {
        return onCombat;
    }

    public void SetOnCombat(bool state)
    {
        onCombat = state;
    }

    public bool GetLevelWasStarted()
    {
        return levelWasStarted;
    }

    public void SetLevelWasStarted(bool value)
    {
        levelWasStarted = value;
    }

    IEnumerator LoadPlayerDataOnScene() //this is used on change scene next fixed update, when escena has been changed
    {
        yield return new WaitForFixedUpdate();
        if (onCombat)
        {
            OnSceneEnter();
        }
        else if (levelWasStarted)
        {
            ReturnToLevelScene();
        }
        else
        {
            OnSceneEnter();
        }
    }
}