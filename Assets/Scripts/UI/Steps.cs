using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Steps : MonoBehaviour {
    private TextMeshProUGUI textMeshPro;
    private int red;
    private int blue;
    private int auxSteps;
    private float auxPercentage;

	// Use this for initialization
	void Start () {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        red = 0;
        blue = 255;
        auxSteps = PlayerMovement.movementsAvaible;
        auxPercentage = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {              
        textMeshPro.text = "" + PlayerMovement.movementsAvaible;        
        auxPercentage = ((float)PlayerMovement.movementsAvaible / (float)auxSteps) * 100;
        red = (int)((255 / 100) * (100 - auxPercentage));
        blue = (int)((255 / 100) * (auxPercentage));
        textMeshPro.color = new Color32((byte)red, 0, (byte)blue, 255);
    }
}
