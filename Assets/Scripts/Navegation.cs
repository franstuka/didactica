using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navegation : MonoBehaviour {

    
    public float angularSpeed = 120f;
    public float maxSpeed = 3.5f;
    public float acceleration = 4f;
    public bool simpleMode = true;
    [Range (0f , 1f)] public float stoppingDistanceFactor = 0.22f;
    public float maxCorrectionAcceleration = 15f;
    
    private Vector2Int thisLastSquarePosition;
    private Vector2Int targetLastSquarePosition;
    public static AStarPathfinding Astar;
    private LinkedList<Vector2Int> savedPath;
    private new Rigidbody rigidbody;
    private float stoppingDistance;
    public bool stopped = true;
    public bool stopSpin = false;
    public bool stopMove = false;
    private float setStoppedValue = 0.15f;
    private bool isMoving = false;
    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        savedPath = new LinkedList<Vector2Int>();
    }

    private void Start()
    {
        stoppingDistance = GridMap.instance.GetCellRadius() * stoppingDistanceFactor;
        
        savedPath.AddFirst(GridMap.instance.CellCordFromWorldPoint(transform.position)); // in start is the actual position
        Astar = new AStarPathfinding(AStarPathfinding.UpdateMode.ON_TARGET_OR_ORIGIN_MOVE);
    }

    private void Update()
    {
        if(!stopped) //if entity is not idle
            UpdateActualPosition();
    }

    public void SetDestination(Vector3 pos)
    {
        Vector2Int thisActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(transform.position); 
        Vector2Int targetActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(pos); 

        stopped = false; //activate movement if entity was idle.
        stopSpin = false;

        savedPath = Astar.GetPath(thisActualSquarePosition, targetActualSquarePosition);
    }

    public void SetDestinationPlayer(Vector3 pos) //just move player
    {
        Vector2Int thisActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(transform.position);
        Vector2Int targetActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(pos);
        stopped = false;

        if (targetActualSquarePosition != targetLastSquarePosition && Mathf.Abs(thisActualSquarePosition.x - targetActualSquarePosition.x) <=1 
            && Mathf.Abs(thisActualSquarePosition.y - targetActualSquarePosition.y) <= 1)
        {
            savedPath = Astar.GetPath(thisActualSquarePosition, targetActualSquarePosition);
        }
    }

    public int SetDestinationPlayerAndCost(Vector3 pos) //move and update avaible movements
    {
        Vector2Int thisActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(transform.position);
        Vector2Int targetActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(pos);
        stopped = false;
        if (targetActualSquarePosition != targetLastSquarePosition && Mathf.Abs(thisActualSquarePosition.x - targetActualSquarePosition.x) <= 1
            && Mathf.Abs(thisActualSquarePosition.y - targetActualSquarePosition.y) <= 1)
        {
            isMoving = true;
            targetLastSquarePosition = targetActualSquarePosition;
            savedPath = Astar.GetPath(thisActualSquarePosition, targetActualSquarePosition);
            return -GridMap.instance.grid[targetActualSquarePosition.x, targetActualSquarePosition.y].Cost;
        }
        else
        {
            isMoving = false;
            return 0;
        }
    }

    public bool CanOpenChest(Vector3 pos)
    {
        Vector2Int thisActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(transform.position);
        Vector2Int targetActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(pos);
        if (targetActualSquarePosition != targetLastSquarePosition && Mathf.Abs(thisActualSquarePosition.x - targetActualSquarePosition.x) <= 1
            && Mathf.Abs(thisActualSquarePosition.y - targetActualSquarePosition.y) <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateActualPosition()
    {
        Vector2Int thisActualSquarePosition = GridMap.instance.CellCordFromWorldPoint(transform.position); 
        if (thisActualSquarePosition == savedPath.First.Value)
        {
            if(savedPath.First.Next == null) //end of path
            {
                Move(GridMap.instance.grid[thisActualSquarePosition.x, thisActualSquarePosition.y].GlobalPosition); // move to square center
            }
            else
            {
                savedPath.RemoveFirst();
                Move(GridMap.instance.grid[savedPath.First.Value.x, savedPath.First.Value.y].GlobalPosition); // move to next point
            }
        }
        else
        {
            Move(GridMap.instance.grid[savedPath.First.Value.x, savedPath.First.Value.y].GlobalPosition); // move normal
        }
    }

    public void Move(Vector3 position)
    {
        float velZ = rigidbody.velocity.z;
        float velX = rigidbody.velocity.x;
        //float correctionSpin = Vector3.SignedAngle(transform.forward, position - transform.position, transform.up);

        #region nonsimpleMode
        if (!simpleMode)
        {
            if (!stopSpin)
            {
                transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
            }

            if (!stopMove)
            {
                if (stoppingDistance > new Vector2(transform.position.x - position.x, transform.position.z - position.z).magnitude && savedPath.First.Next == null) //stop
                {
                    stopSpin = true;
                    if (Mathf.Abs(velX) < setStoppedValue && Mathf.Abs(velZ) < setStoppedValue)
                    {
                        rigidbody.velocity.Set(0f, rigidbody.velocity.y, 0f);
                        stopped = true;
                    }
                    else
                    {
                        float correctionAccelerationX = Mathf.Abs(-velX / Time.deltaTime) > maxCorrectionAcceleration ? maxCorrectionAcceleration : -velX / Time.deltaTime;
                        float correctionAccelerationZ = Mathf.Abs(-velZ / Time.deltaTime) > acceleration ? acceleration : -velZ / Time.deltaTime;
                        rigidbody.AddRelativeForce(new Vector3(correctionAccelerationX, 0f, correctionAccelerationZ), ForceMode.Acceleration);
                    }
                }
                else
                {
                    //movement
                    float correctionAcceleration = Mathf.Abs(-velX / Time.deltaTime) > maxCorrectionAcceleration ? maxCorrectionAcceleration : -velX / Time.deltaTime;
                    Vector2 toTarget = new Vector2(position.x - transform.position.x, position.z - transform.position.z).normalized;
                    Vector2 toSpeed = new Vector2(velX, velZ).normalized;

                    float finalAcceleration;

                    velZ = rigidbody.velocity.magnitude;

                    finalAcceleration = maxSpeed - velZ < acceleration ? maxSpeed - velZ : +acceleration;
                    rigidbody.AddForce(transform.forward * finalAcceleration * Time.deltaTime * 1000, ForceMode.Acceleration);
                    //rigidbody.AddForce(transform.right * correctionAcceleration * Time.deltaTime , ForceMode.Acceleration);
                }
            }
        }
        #endregion
        else //simple mode
        {
            transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
            
            if (stoppingDistance > new Vector2(transform.position.x - position.x, transform.position.z - position.z).magnitude && savedPath.First.Next == null && !stopped) //stop
            {
                if (Mathf.Abs(velX) < setStoppedValue && Mathf.Abs(velZ) < setStoppedValue)
                {
                    rigidbody.velocity.Set(0f, rigidbody.velocity.y, 0f);
                    stopped = true;
                }
                else
                {
                    rigidbody.AddForce(new Vector3(-velX * 0.5f, 0f, -velZ * 0.5f), ForceMode.VelocityChange);
                }
            }
            else
            {
                //movement
                float finalAcceleration;

                velZ = rigidbody.velocity.magnitude;

                finalAcceleration = maxSpeed - velZ < acceleration ? maxSpeed - velZ : +acceleration;
                rigidbody.AddForce(transform.forward * finalAcceleration * Time.deltaTime * 1000, ForceMode.Acceleration);
                //rigidbody.AddForce(transform.right * correctionAcceleration * Time.deltaTime , ForceMode.Acceleration);
            }
        }
    }

    private void Brake(Vector3 position, ref float velX, ref float velZ)
    {
        
        if (stoppingDistance > new Vector2(transform.position.x - position.x, transform.position.z - position.z).magnitude && savedPath.First.Next == null) //stop
        {
            if (Mathf.Abs(velX) < setStoppedValue && Mathf.Abs(velZ) < setStoppedValue)
            {
                rigidbody.velocity.Set(0f, rigidbody.velocity.y, 0f);
            }
            else
            {
                float correctionAccelerationX = Mathf.Abs(-velX / Time.deltaTime) > maxCorrectionAcceleration ? maxCorrectionAcceleration : -velX / Time.deltaTime;
                float correctionAccelerationZ = Mathf.Abs(-velZ / Time.deltaTime) > acceleration ? acceleration : -velZ / Time.deltaTime;
                rigidbody.AddRelativeForce(new Vector3(correctionAccelerationX, 0f, correctionAccelerationZ), ForceMode.Acceleration);
            }
        }
    }

    public Vector3 GetVelocity()
    {
        return rigidbody.velocity;
    }

    public bool GetStopped()
    {
        return stopped;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public LinkedList<Vector2Int> GetSavedPath()
    {
        return savedPath;
    }
}
