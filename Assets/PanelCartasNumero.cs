using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCartasNumero : MonoBehaviour {

	
	private bool moveUp = false;
	private bool moveDown = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (moveUp == true){
			 RectOffset tempPadding = new RectOffset(
			GetComponent<GridLayoutGroup>().padding.left,
			GetComponent<GridLayoutGroup>().padding.right,
			GetComponent<GridLayoutGroup>().padding.top,
			GetComponent<GridLayoutGroup>().padding.bottom);
			tempPadding.top -= 2;
			GetComponent<GridLayoutGroup>().padding = tempPadding;
		}

			if (moveDown == true){
				RectOffset tempPadding = new RectOffset(
				GetComponent<GridLayoutGroup>().padding.left,
				GetComponent<GridLayoutGroup>().padding.right,
				GetComponent<GridLayoutGroup>().padding.top,
				GetComponent<GridLayoutGroup>().padding.bottom);
				tempPadding.top += 2;
				GetComponent<GridLayoutGroup>().padding = tempPadding;
		}

	}

	public void MoveUpPress(){
	
		
		moveUp = true;
	}
		public void MoveUpUnPress(){
	
		
		moveUp = false;
	}


	public void MoveDownPress(){
		moveDown = true;
	}
	public void MoveDownUnPress(){
		moveDown = false;
	}
}
