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

    public int actualLevel;
    public bool onCombat;
    public bool levelWasStarted;

    [SerializeField] private PauseMenuScript menu;
    [SerializeField] List<Item> initialItems = new List<Item>();
    [SerializeField] private List<GameObject> enemyPrefabsList = new List<GameObject>();

    private Dictionary<int, Dictionary<int, GameObject>> enemyDictionaryByLevel = new Dictionary<int, Dictionary<int, GameObject>>();
    private const int startPlayerHp = 5;

    //enemyPrefabList save all the enemies as start-up info and enemyDictioraryByLevel organice that information as a dictionary ordenated by enemy level,
    //then, the enemyPrefabsList is voided for not store useless data in memory.  

    private void Start()
    {
        InicialiceFirstLevelPlayerData(1);
        SaveEnemiesInDictionary();
    }

    public void EndLevelLost()
    {
        menu.die.StartFade();
    }

    public void EndLevelWin()
    {
        menu.win.StartFade();
    }

    #region scene and savedata Management

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

    public object[] GetMonsterOnCombat()
    {
        return saveDataManager.LoadEnemyData();
    }

    #endregion

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
    
    public Dictionary<int,GameObject> GetMonsterLevelList(int level)
    {
        if(enemyDictionaryByLevel[level].Count == 0)
        {
            Debug.LogError("There isn't enemies on this dictionary level (" + level + ")");
        }

        return enemyDictionaryByLevel[level];
    }

    private void SaveEnemiesInDictionary()
    {
        int enemyLevel;
        for (int i = 0; i < enemyPrefabsList.Count; i++)
        {
            enemyLevel = enemyPrefabsList[i].GetComponent<EnemyCombat>().GetEnemyLevel();
            
            if(enemyDictionaryByLevel.ContainsKey(enemyLevel))
            {
                enemyDictionaryByLevel[enemyLevel].Add(enemyDictionaryByLevel[enemyLevel].Count, enemyPrefabsList[i]);
            }
            else
            {
                enemyDictionaryByLevel[enemyLevel] = new Dictionary<int, GameObject>(); //need to define the dictionary entry
                enemyDictionaryByLevel[enemyLevel].Add(enemyDictionaryByLevel[enemyLevel].Count, enemyPrefabsList[i]);
            }
        }
        enemyPrefabsList.Clear();
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