﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI stepsText;
    [SerializeField] private TextMeshProUGUI HPText;
    
    private PlayerMovement playerMovement;

    //Steps
    private float red;
    private float green;
    private int maxSteps;

    //Auxiliar variables
    private int lastMovementsAvailable;
    private int lastHP;
    
    // Use this for initialization
    void Start()
    {        
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        red = 0;
        green = 255;
        maxSteps = 10;        
        UpdateSteps();
        UpdateHP();
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
}
