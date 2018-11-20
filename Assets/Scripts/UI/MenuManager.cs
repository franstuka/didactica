using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] private GameObject[] buttonsGameobject;  
    private Button[] buttons;

	// Use this for initialization
	void Start () {
        buttons = new Button[buttonsGameobject.Length];

        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = buttonsGameobject[i].GetComponent<Button>();
        }
        
        buttons[0].onClick.AddListener(Play);
        buttons[1].onClick.AddListener(LevelSelection);
        buttons[2].onClick.AddListener(Options);
        buttons[3].onClick.AddListener(Return);
        buttons[4].onClick.AddListener(Level1);
        buttons[5].onClick.AddListener(Level2);       

    }
	
	// Update is called once per frame
	void Play () {
        SceneManager.LoadScene("SampleScene");
    }

    void LevelSelection()
    {
        buttonsGameobject[0].SetActive(false);
        buttonsGameobject[1].SetActive(false);
        buttonsGameobject[2].SetActive(false);
        buttonsGameobject[3].SetActive(true);
        buttonsGameobject[4].SetActive(true);
        buttonsGameobject[5].SetActive(true);

    }

    void Options()
    {
        buttonsGameobject[0].SetActive(false);
        buttonsGameobject[1].SetActive(false);
        buttonsGameobject[2].SetActive(false);
        buttonsGameobject[3].SetActive(true);        
    }

    void Return()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(i <= 2)
            {
                buttonsGameobject[i].SetActive(true);
            }

            else
            {
                buttonsGameobject[i].SetActive(false);
            }
        }        
       
    }

    void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
