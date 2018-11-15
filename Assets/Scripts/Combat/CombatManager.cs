using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    /*
    private InventarySystem inventarySystem;
    private MonsterSystem monsterSystem;
    private int level;

    private List<Card> cardList = new List<Card>();
    private List<Card> cardListCombat = new List<Card>();
    private List<CardOperation> cardOperationListCombat = new List<CardOperation>();
    private List<Monster> monsterList = new List<Monster>();
    private string selectedMonster;
    private float monsterLife;

    private float sumProbability;
    private float substractionProbability;
    private float multiplyProbability;
    private float divideProbability;

    // Use this for initialization
    void Start () {
        inventarySystem = GetComponent<InventarySystem>();
        monsterSystem = GetComponent<MonsterSystem>();
        level = GameManager.instance.GetActualLevel();        

        cardList = inventarySystem.GetCardList();
        cardOperationListCombat = inventarySystem.GetCardOperationList();        
        monsterList = monsterSystem.GetMonsterList();        

        //Probabilidades de las cartas
        sumProbability = 50.0f - level;
        substractionProbability = 50.0f - level;
        multiplyProbability = 0.0f + level;
        divideProbability = 0.0f + level;

        //Elección de las 6 cartas
        for (int i = 0; i < 6; i++)
        {
            int randomCard = Random.Range(0, cardList.Count);           

            cardListCombat.Add(cardList[randomCard]);

            cardList.RemoveAt(randomCard);
        }

        //Elección de monstruo y vida
        int randomMonster = Random.Range(0, monsterList.Count);
        selectedMonster = monsterList[randomMonster].alias;        
        bool valueNotInCards = false;

        while (!valueNotInCards)
        {
            monsterLife = 0.0f;
            valueNotInCards = true;
            for (int i = 1; i < 6; i++)
            {

                float randomCardOperation = Random.Range(0.0f, 100.0f);

                if (i == 1)
                {
                    if (randomCardOperation >= 0.0f && randomCardOperation < sumProbability)
                    {
                        monsterLife = cardListCombat[i - 1].value + cardListCombat[i].value;
                    }

                    else if (randomCardOperation >= sumProbability && randomCardOperation < sumProbability + substractionProbability)
                    {
                        monsterLife = cardListCombat[i - 1].value - cardListCombat[i].value;
                    }

                    else if (randomCardOperation >= sumProbability + substractionProbability && randomCardOperation < sumProbability + substractionProbability + multiplyProbability)
                    {
                        monsterLife = cardListCombat[i - 1].value * cardListCombat[i].value;
                    }

                    else if (randomCardOperation >= sumProbability + substractionProbability + multiplyProbability && randomCardOperation <= 100.0f)
                    {
                        monsterLife = cardListCombat[i - 1].value / cardListCombat[i].value;
                    }
                }
                else
                {
                    if (randomCardOperation >= 0.0f && randomCardOperation < sumProbability)
                    {
                        monsterLife = monsterLife + cardListCombat[i].value;
                    }

                    else if (randomCardOperation >= sumProbability && randomCardOperation < sumProbability + substractionProbability)
                    {
                        monsterLife = monsterLife - cardListCombat[i].value;
                    }

                    else if (randomCardOperation >= sumProbability + substractionProbability && randomCardOperation < sumProbability + substractionProbability + multiplyProbability)
                    {
                        monsterLife = monsterLife * cardListCombat[i].value;
                    }

                    else if (randomCardOperation >= sumProbability + substractionProbability + multiplyProbability && randomCardOperation <= 100.0f)
                    {
                        monsterLife = monsterLife / cardListCombat[i].value;
                    }
                }

                Debug.Log("Monster Life: " + monsterLife);
            }

            foreach (Card card in cardListCombat)
            {
                if(card.value == monsterLife)
                {
                    valueNotInCards = false;
                }
            }

            Debug.Log("Nueva iteración");
        }

        


        foreach (Card card in cardListCombat)
        {
            print(card.value);
        }    

        Debug.Log("Monster: " + selectedMonster + " Life: " + monsterLife);
        
        
    }*/
}
