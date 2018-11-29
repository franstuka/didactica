using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCalculos : MonoBehaviour {

	private string[] valores;
	private CardGameobject[] cartas;
	public string stringParaCalculo;

	public GameObject text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		cartas = this.GetComponentsInChildren<CardGameobject>();

		 if (Input.GetKeyUp("e"))
        {	
			text.GetComponent<Text>().text = " ";
        
          // text.GetComponent<Text>().text = calcular();
	
		    text.GetComponent<Text>().text = GetComponent<Calculate>().CalculateRPN(calcular()).ToString();
        }

		
		
		
	}

	private string calcular(){
		stringParaCalculo = "";
		for (int i = 0; i < cartas.Length; i++)
		{	
			
			if(i < cartas.Length-1){
				if (cartas[i+1].soyNumeroOoperacion() == true && cartas[i].soyNumeroOoperacion()){
				stringParaCalculo += cartas[i].valorcarta(); 
				}
				else
				{
					stringParaCalculo += cartas[i].valorcarta() + " "; 
				}
			}
			if(i == cartas.Length-1){
				stringParaCalculo += cartas[i].valorcarta() + " ";
			}
		}

		return stringParaCalculo;
	}


}
