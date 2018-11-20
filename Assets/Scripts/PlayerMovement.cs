﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CombatStats
{

    public int movementsAvaible = 1;
    [SerializeField] Navegation nav;

    private void Start()
    {
        nav = GetComponent<Navegation>();
    }

    // Update is called once per frame
    void Update () {

        if (movementsAvaible <= 0 && nav.GetStopped())
        {
            if(!GameManager.instance.GetOnCombat()) //Enter in combat
            {
                GameManager.instance.OnCombatEnter();
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13)) //layer 13 click detection
            {
                movementsAvaible += nav.SetDestinationPlayerAndCost(hit.point); //update movements and move
            }
        }
        
	}

  
}
