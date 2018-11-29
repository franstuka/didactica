using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : CombatStats {

    public enum EnemyState { PATROL,HOLD,COMBAT, RETURNING_TO_POSITION,IDLE };
    
    //[SerializeField] protected NavMeshAgent nav;
    [SerializeField] protected Navegation nav;
    [SerializeField] protected bool staticEnemy;
    //card probabilities
    //The probabilie don't need to sum exactly 100%, the combat manager will set-up there to 100%
    [SerializeField] private float sumProbability;
    [SerializeField] private float substractionProbability;
    [SerializeField] private float multiplyProbability;
    [SerializeField] private float divideProbability;
    [SerializeField] private int numSteepsWinOnDefeat = 10;
    [SerializeField] private int monsterLevel = 1;
    [SerializeField] private int numOfOperations = 2;
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
            case EnemyState.IDLE:
                {
                    break;
                }
            case EnemyState.COMBAT:
                {
                    break;
                }
            case EnemyState.PATROL:
                {
                    if(nav.GetStopped())
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
                    if (nav.GetStopped())
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

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == "Player")
        {
            if (!GameManager.instance.GetOnCombat()) //Enter in combat
            {
                GameManager.instance.OnCombatEnter(gameObject.name,monsterLevel,transform.position);
            }
        }
    }

    private void UpdateAnimator()
    {

    }

    public int GetEnemyLevel()
    {
        return monsterLevel;
    }

    public float[] GetEnemyProbabilities() //first sum, substract, multiply, divide
    {
        return new float[] { sumProbability,substractionProbability,multiplyProbability, divideProbability};
    }
    
    public int GetEnemyMovementsOnDefeat()
    {
        return numSteepsWinOnDefeat;
    }

    public int GetMaxOperations()
    {
        return numOfOperations;
    }

    public int GetEnemyState()
    {
        return System.Convert.ToInt32(activeState);
    }

    public Vector3 GetTarget()
    {
        return target;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    IEnumerator WaitEndFrameToStartIA()
    {
        yield return new WaitForEndOfFrame();
        if(GameManager.instance.GetOnCombat())
        {
            activeState = EnemyState.COMBAT;
        }
        else if (!staticEnemy)
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
