using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    

    private List<Card> cardListCombat;

    private float monsterLife;

    private float sumProbability;
    private float substractionProbability;
    private float multiplyProbability;
    private float divideProbability;

    private void Awake()
    {
        cardListCombat = new List<Card>();
    }

    private void GetMonsterParameters()
    {
        //nombre y lv
    }

    private void GetCombatCardList() //pueden ser repetidas
    {
        for (int i = 0; i < 6; i++) 
        {
            int randomCard = Random.Range(0, InventarySystem.instance.GetCardList().Count);
            cardListCombat.Add(InventarySystem.instance.GetCardList()[randomCard]);
        }
    }

    public void CreateMonsterValue()
    {
        GetMonsterParameters();
        GetCombatCardList();

        //Elección de monstruo y vida
    }
    
    public void ResolveCombat(int value)
    {

    }

    
}
