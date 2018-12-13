using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class monsterLife : MonoBehaviour {

	// Use this for initialization
	public GameObject combatmanager;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		this.GetComponent<TextMeshProUGUI>().text = combatmanager.GetComponent<CombatManager>().getMonsterLife().ToString();
	}
}
