using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCards : MonoBehaviour {

	// Use this for initialization
	public Transform cardHand;
	public GameObject card1;
	public GameObject card2;
	public GameObject card3;
	public GameObject card4;
	public GameObject card5;
	public GameObject card6;
	public GameObject card7;
	public GameObject card8;
	public GameObject card9;

	public GameObject cardMas;
	public GameObject cardMenos;
	public GameObject cardMult;
	public GameObject cardDiv;

	private List<Card> cardListCombat;
	void Start () {

		cardListCombat=this.GetComponent<CombatManager>().GetCombatCards();
		foreach (Card item in cardListCombat)
		{
			switch (item.value)
			{	
				case 1:
					GameObject card1Battle = Instantiate (card1);
					card1Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 2:
					GameObject card2Battle = Instantiate (card2);
					card2Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 3:
					GameObject card3Battle = Instantiate (card3);
					card3Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 4:
					GameObject card4Battle = Instantiate (card4);
					card4Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 5:
					GameObject card5Battle = Instantiate (card5);
					card5Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 6:
					GameObject card6Battle = Instantiate (card6);
					card6Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 7:
					GameObject card7Battle = Instantiate (card7);
					card7Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 8:
					GameObject card8Battle = Instantiate (card8);
					card8Battle.gameObject.transform.SetParent(cardHand);
					break;
				case 9:
					GameObject card9Battle = Instantiate (card9);
					card9Battle.gameObject.transform.SetParent(cardHand);
					break;

				default:
					print ("Incorrect card value");
            		break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
