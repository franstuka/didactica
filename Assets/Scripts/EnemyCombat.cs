using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : CombatStats {

    public enum EnemyState { PATROL,HOLD,COMBAT, RETURNING_TO_POSITION };
    
    //[SerializeField] protected NavMeshAgent nav;
    [SerializeField] protected Navegation nav;
    [SerializeField] protected bool staticEnemy;
    [SerializeField] private float offsetDistance = 0.15f;
    //card probabilities
    [SerializeField] private float sumProbability;
    [SerializeField] private float substractionProbability;
    [SerializeField] private float multiplyProbability;
    [SerializeField] private float divideProbability;
    [SerializeField] private int numSteepsWinOnDefeat = 10;
    [SerializeField] private int monsterLevel = 1;
    //Enemy Behaviours
    public EnemyState activeState;
    private Hold hold;
    private Patrol patrol;
    private Vector3 target;

    private void Awake()
    {
        hold = GetComponent<Hold>();
        patrol = GetComponent<Patrol>();
        nav = GetComponent<Navegation>();
    }

    #region See to something functions

    protected bool FaceAndCheckObjective(Vector3 targetPos, float stoppingPrecision) //Estos 2 metodos son para girar el enemigo y hacer que mire hace cualquier sitio
        //el primero comprueba si el enemigo esta dentro del rango de vision, el segundo simplemente este objeto hacia el objetivo
    {
        Quaternion angle = CaculateTargetRotation(targetPos);
        if (Mathf.Abs(Quaternion.Angle(angle, transform.rotation)) <= Mathf.Abs(stoppingPrecision))
        {
            return true; //This is an end condition
        }
        else return false;
    }

    protected void FaceObjective(Vector3 targetPos) 
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

    #endregion

    private void Start()
    {
        target = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        StartCoroutine(WaitEndFrameToStartIA());
    }

    public LinkedList<Vector2Int> GetSavedPath()
    {
        return nav.GetSavedPath();
    }

    private void Update()
    {
        EnemyStateMachine();
        UpdateAnimator();
    }

    private void EnemyStateMachine()
    {
        switch (activeState)
        {
            case EnemyState.COMBAT:
                {
                    break;
                }
            case EnemyState.PATROL:
                {
                    if (transform.position.x <= target.x + offsetDistance && transform.position.x >= target.x - offsetDistance && transform.position.z <= target.z + offsetDistance && transform.position.z >= target.z - offsetDistance)
                    {
                        target = patrol.GetNewWaipoint(target);
                        nav.SetDestination(target);
                    }
                    break;
                }
            case EnemyState.HOLD:
                {
                    if (!FaceAndCheckObjective(hold.DirectionToFace(), 2f))
                    {
                        activeState = EnemyState.RETURNING_TO_POSITION;
                    }
                    break;
                }
            case EnemyState.RETURNING_TO_POSITION:
                {
                    if (transform.position.x <= target.x + offsetDistance && transform.position.x >= target.x - offsetDistance && transform.position.z <= target.z + offsetDistance && transform.position.z >= target.z - offsetDistance)
                    {
                        if (FaceAndCheckObjective(hold.DirectionToFace(), 2f))
                        {
                            activeState = EnemyState.HOLD;
                        }
                        else
                        {
                            FaceObjective(hold.DirectionToFace());
                        }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void UpdateAnimator()
    {

    }

    IEnumerator WaitEndFrameToStartIA()
    {
        yield return new WaitForEndOfFrame();
        if (!staticEnemy)
        {
            activeState = EnemyState.PATROL;
            if (target == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            {
                target = patrol.GetNewWaipoint(target);
            }
            nav.SetDestination(target);
        }
        else
        {
            activeState = EnemyState.RETURNING_TO_POSITION;
            target = hold.ReturnToPosition();
            nav.SetDestination(target);
        }
    }
}
