using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCalculos : MonoBehaviour {

	private string[] valores;
	private CardGameobject[] cartas;
	public string stringParaCalculo;

	public GameObject combatManager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		cartas = this.GetComponentsInChildren<CardGameobject>();


		
		
		
	}

	public void calcular(){
		stringParaCalculo = " ";
		foreach (var item in cartas)
		{
			stringParaCalculo += item.valorcarta() + " ";
		}
		print(stringParaCalculo);
		int resultado =Mathf.RoundToInt(GetComponent<Calculate>().CalculateRPN(stringParaCalculo)) ;
		combatManager.GetComponent<CombatManager>().ResolveCombat(resultado); 
	}


}
