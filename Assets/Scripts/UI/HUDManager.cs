using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI stepsText;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Button OKButton;
    [SerializeField] private GameObject[] textsAdventureBegins;

    [SerializeField] private GameObject OKButtonGameObject;
    
    [SerializeField] private PlayerMovement playerMovement;
    private Scene scene;

    //Steps
    private float red;
    private float green;
    private int maxSteps;

    //Auxiliar variables
    private int lastMovementsAvailable;
    private int lastHP;

    //Texts
    private bool adventureHasBegun;
    private int storyText;

    // Use this for initialization
    void Start()
    {
        OKButton.onClick.AddListener(NextText);
        red = 0;
        green = 255;
        maxSteps = 10;
        UpdateSteps();
        UpdateHP();
        scene = SceneManager.GetActiveScene();

        if (scene.name == "Level 1") {            
            adventureHasBegun = true;
            textsAdventureBegins[0].SetActive(true);
            OKButtonGameObject.SetActive(true);
            playerMovement.SetCanMove(false);
        }
        else
        {
            adventureHasBegun = false;
        }
        storyText = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastMovementsAvailable != playerMovement.movementsAvaible && playerMovement.movementsAvaible >= 0)
        {
            UpdateSteps();
        }

        if (lastHP != playerMovement.GetHP())
        {
            UpdateHP();       
        }              
       

        if (!adventureHasBegun)
        {
            OKButtonGameObject.SetActive(false);          
        }

    }

    void UpdateSteps()
    {
        if (playerMovement.movementsAvaible <= 0)
        {
            stepsText.color = new Color32(255, 0, 0, 255);
            stepsText.text = "0";
        }

        else if (playerMovement.movementsAvaible > 0 && playerMovement.movementsAvaible <= 10)
        {
            red = (25.5f) * (maxSteps - playerMovement.movementsAvaible);
            green = (25.5f) * (playerMovement.movementsAvaible);
            stepsText.color = new Color32((byte)red, (byte)green, 0, 255);
            stepsText.text = "" + playerMovement.movementsAvaible;

        }

        else
        {
            stepsText.color = new Color32(0, 255, 0, 255);
            stepsText.text = "" + playerMovement.movementsAvaible;
        }

        lastMovementsAvailable = playerMovement.movementsAvaible;
    }

    void UpdateHP()
    {
        HPText.text = "" + playerMovement.GetMaxHP() + "/" + playerMovement.GetHP();
        lastHP = playerMovement.GetHP();
    }

    void NextText()
    {        
        if (adventureHasBegun)
        {
            if (!textsAdventureBegins[4].activeSelf)
            {
                textsAdventureBegins[storyText].SetActive(false);
                storyText++;
                textsAdventureBegins[storyText].SetActive(true);
            }

            else
            {
                textsAdventureBegins[storyText].SetActive(false);
                OKButtonGameObject.SetActive(false);
                adventureHasBegun = false;
                playerMovement.SetCanMove(true);
            }
        }
    }
}
