using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CombatStats
{
    [SerializeField] private Animator animations;
    public int movementsAvaible = 10;
    [SerializeField] Navegation nav;
    private bool canMove;

    private void Awake()
    {
        nav = GetComponent<Navegation>();
        canMove = true;
    }

    // Update is called once per frame
    void Update () {
        if (nav.GetStopped())        
            animations.SetBool("isWalking", false);
        
        if (canMove) {
            if (movementsAvaible <= 0 && nav.GetStopped())
            {
                if (!GameManager.instance.GetOnCombat()) //Enter in combat
                {
                    GameManager.instance.OnCombatEnter();
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13)) //layer 13 click detection
                {
                    if (hit.collider.tag == "Chest") //layer 11 click detection
                    {                        
                        
                        if (nav.CanOpenChest(hit.point)) {                         
                            transform.LookAt(hit.transform.position);
                            animations.SetBool("openingAChest", true);
                            canMove = false;
                            StartCoroutine(endOpeningChestAnimation());
                        }
                    }
                    else
                    {
                        
                        movementsAvaible += nav.SetDestinationPlayerAndCost(hit.point); //update movements and move 
                        if(nav.GetIsMoving())
                            animations.SetBool("isWalking", true);
                    }
                }                
            }
        }
        
        
	}

    public void SetCanMove(bool state)
    {
        canMove = state;
    }

    IEnumerator endOpeningChestAnimation()
    {
        yield return new WaitForSeconds(1.2f);
        animations.SetBool("openingAChest", false);
        canMove = true;
    }
  

}
