using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private void Update() //Test on exit combat and give movements to player
    {

        if (Input.GetKeyDown("1"))
         {
            GameManager.saveDataManager.SaveLevelData();
         }


        if (Input.GetKeyDown("2"))
        {
            GameManager.instance.ChangeScene("SampleScene");
        }

        if (Input.GetKeyDown("3"))
        {
            Debug.Log(GameManager.instance.GetMonsterOnCombat()[0] + " " + GameManager.instance.GetMonsterOnCombat()[1] +" " + GameManager.instance.GetMonsterOnCombat()[2]);
        }

        if (Input.GetKeyDown("4"))
        {
            if(FindObjectOfType<CombatManager>() != null)
                FindObjectOfType<CombatManager>().ResolveCombat(0);
        }
        if (Input.GetKeyDown("5"))
        {
            if (FindObjectOfType<CombatManager>() != null)
                FindObjectOfType<CombatManager>().ResolveCombat(1);
        }
    }
}
