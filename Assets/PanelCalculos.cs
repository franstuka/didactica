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
			text.GetComponent<Text>().text = "";
        
           text.GetComponent<Text>().text = calcular();
        }

		
		
		
	}

	private string calcular(){
		stringParaCalculo = "";
		foreach (var item in cartas)
		{
			stringParaCalculo += item.valorcarta();
		}

		return stringParaCalculo;
	}


}
