using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CombatStats
{
    [SerializeField] private Animator animations;
    public int movementsAvaible = 10;
    [SerializeField] Navegation nav;
    private bool canMove;
    private bool openingChest;

    private void Awake()
    {
        nav = GetComponent<Navegation>();
        canMove = true;
        openingChest = false;
    }

    // Update is called once per frame
    void Update () {
        if (nav.GetStopped())
        {
            animations.SetBool("isWalking", false);
            if(!animations.GetBool("openingAChest"))
                canMove = true;
        }

        if (openingChest)
        {
            animations.SetBool("openingAChest", true);
            StartCoroutine(endOpeningChestAnimation());
        }
        
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
                            StartCoroutine(spinning(hit.point));                            
                            canMove = false;                            
                        }
                    }
                    else
                    {
                        
                        movementsAvaible += nav.SetDestinationPlayerAndCost(hit.point); //update movements and move 
                        if (nav.GetIsMoving())
                        {
                            animations.SetBool("isWalking", true);
                            canMove = false;
                        }
                    }
                }                
            }
        }
        
        
	}

    public void SetCanMove(bool state)
    {
        canMove = state;
    }

    private bool FaceAndCheckObjective(Vector3 targetPos, float stoppingPrecision)  {
        Quaternion angle = CaculateTargetRotation(targetPos);
        if (Mathf.Abs(Quaternion.Angle(angle, transform.rotation)) <= Mathf.Abs(stoppingPrecision))
        {
            return true; 
        }
        else return false;
    }

    private void FaceObjective(Vector3 targetPos)
    {
        float step = nav.angularSpeed * Time.deltaTime;
        Quaternion angle = CaculateTargetRotation(targetPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, step);
    }

    private Quaternion CaculateTargetRotation(Vector3 targetPos)
    {

        float angle = Mathf.Atan2(targetPos.x - transform.position.x, targetPos.z - transform.position.z);

        angle = 180 * angle / Mathf.PI;
        Vector3 vector3 = new Vector3(0, angle, 0);
        return Quaternion.Euler(vector3);
    }

    IEnumerator spinning(Vector3 hit)
    {
        for (; !FaceAndCheckObjective(hit, 3f);)
        {
            FaceObjective(hit);
            yield return new WaitForEndOfFrame();
        }
        
        openingChest = true;
    }

    IEnumerator endOpeningChestAnimation()
    {
        yield return new WaitForSeconds(1.2f);
        animations.SetBool("openingAChest", false);
        canMove = true;
        openingChest = false;
    }

    public override void Die()
    {

        GameManager.instance.OnLevelFail();
        GameManager.instance.InicialiceFirstLevelPlayerData(1);
        GameManager.saveDataManager.ClearEnemyData();

    }
}
