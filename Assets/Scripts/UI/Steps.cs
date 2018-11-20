using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Steps : MonoBehaviour {
    private TextMeshProUGUI textMeshPro;
    private float red;
    private float blue;
    private int maxSteps;
    private float percentage;

	// Use this for initialization
	void Start () {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        red = 0;
        blue = 255;
        maxSteps = PlayerMovement.movementsAvaible;
        percentage = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {       
        if (PlayerMovement.movementsAvaible <= 0)
        {
            textMeshPro.text = "0";
            percentage = 0;
        }
        else
        {
            textMeshPro.text = "" + PlayerMovement.movementsAvaible;
            percentage = ((float)PlayerMovement.movementsAvaible / (float)maxSteps) * 100;
        }
        
        red = (255 / 100) * (100 - percentage);
        blue = (255 / 100) * (percentage);
        textMeshPro.color = new Color32((byte)red, 0, (byte)blue, 255);
    }
}
