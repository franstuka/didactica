using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private void Update() //Test on exit combat and give movements to player
    {
        if(GameManager.instance.GetOnCombat())
        {
            if (Input.GetKeyDown("1"))
            {
                GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
                if (player != null)
                {
                    player.GetComponent<PlayerMovement>().movementsAvaible += 20;
                }
                else
                    Debug.LogError("ERROR ON TEST, PLAYER NOT FOUND");
                GameManager.instance.OnCombatFinish();
            }
        }
    }
}
