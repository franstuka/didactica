using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Threading;



public class AStarPathfinding {

    public enum TargetDistanceAdvanceDirection { UP_RIGHT, UP_LEFT, DOWN_RIGHT, DOWN_LEFT, UP_DOWN_LEFT_RIGHT };

    private LinkedList<Vector2Int> Heap; //position in grid, in x,y
    
    private const int normalCost = 10;
    private const int diagonalCost = 14;
    private const int enemyInSameCellCost = 6;

    private Vector2Int startNodePos;
    private Vector2Int endNodePos;

    public AStarPathfinding(Vector2Int startNodePos, Vector2Int endNodePos)
    {
        Heap = new LinkedList<Vector2Int>();
        this.startNodePos = startNodePos;
        this.endNodePos = endNodePos;
    }

    public void CalculateTargetDistance()
    {
        int maxX = GridMap.instance.GetGridSizeX();
        int maxY = GridMap.instance.GetGridSizeY();

        //initial node setup
        GridMap.instance.grid[startNodePos.x, startNodePos.y].Node = new AStarNode(0)
        {
            visited = true
        };

        //expand in diagonal nodes
        if (startNodePos.x > 0 && startNodePos.x < maxX - 1 && startNodePos.y > 0 && startNodePos.y < maxY - 1)
        {
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y+1, TargetDistanceAdvanceDirection.UP_RIGHT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y-1, TargetDistanceAdvanceDirection.UP_LEFT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y+1, TargetDistanceAdvanceDirection.DOWN_RIGHT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y-1, TargetDistanceAdvanceDirection.DOWN_LEFT, ref maxX, ref maxY);
        }
        else if (startNodePos.x == 0 && startNodePos.y == 0)
        {
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y+1, TargetDistanceAdvanceDirection.DOWN_RIGHT, ref maxX, ref maxY);
        }
        else if (startNodePos.x == 0 && startNodePos.y == maxY - 1)
        {
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y-1, TargetDistanceAdvanceDirection.DOWN_LEFT, ref maxX, ref maxY);
        }
        else if (startNodePos.x == maxX - 1 && startNodePos.y == 0)
        {
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y+1, TargetDistanceAdvanceDirection.UP_RIGHT, ref maxX, ref maxY);
        }
        else if (startNodePos.x == maxX - 1 && startNodePos.y == maxY - 1)
        {
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y-1, TargetDistanceAdvanceDirection.UP_LEFT, ref maxX, ref maxY);
        }
        else if (startNodePos.x == 0)
        {
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y+1, TargetDistanceAdvanceDirection.UP_RIGHT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y+1, TargetDistanceAdvanceDirection.DOWN_RIGHT, ref maxX, ref maxY);
        }
        else if (startNodePos.y == 0)
        {
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y+1, TargetDistanceAdvanceDirection.DOWN_RIGHT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y-1, TargetDistanceAdvanceDirection.DOWN_LEFT, ref maxX, ref maxY);
        }
        else if (startNodePos.x == maxX - 1)
        {
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y-1, TargetDistanceAdvanceDirection.UP_LEFT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x+1, startNodePos.y-1, TargetDistanceAdvanceDirection.DOWN_LEFT, ref maxX, ref maxY);
        }
        else if (startNodePos.y == maxY - 1)
        {
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y+1, TargetDistanceAdvanceDirection.UP_RIGHT, ref maxX, ref maxY);
            CalculateTargetRecursion(0, startNodePos.x-1, startNodePos.y-1, TargetDistanceAdvanceDirection.UP_LEFT, ref maxX, ref maxY);
        }
        else
            Debug.LogError("No suitable solution");


        /*int subProcessActive = 0;
        new Thread(CalculateTargetRecursion).Start();
        Thread.Sleep*/
    }
    public void CalculateTargetRecursion(int baseCost, int x, int y, TargetDistanceAdvanceDirection direction, ref int maxX, ref int maxY)
    {
        if (direction == TargetDistanceAdvanceDirection.DOWN_RIGHT)
        {
            GridMap.instance.grid[startNodePos.x, startNodePos.y].Node = new AStarNode(baseCost);
            for (int i = 0; i < maxY - y; i++)
            {
                GridMap.instance.grid[x, i + y].Node.AvaibleAdjacentNodes = CheckAvaiblesPositions();
                GridMap.instance.grid[x, i + y].Node.FromFinalCost = baseCost + i * normalCost;
            }
            for (int i = 1; i < maxX - x; i++)
            {
                GridMap.instance.grid[i + x, y].Node.AvaibleAdjacentNodes = CheckAvaiblesPositions();
                GridMap.instance.grid[i + x, y].Node.FromFinalCost = baseCost + i * normalCost;
            }
            if (x < maxX && y < maxY)
                CalculateTargetRecursion(baseCost + diagonalCost, x + 1, y + 1, TargetDistanceAdvanceDirection.DOWN_RIGHT, ref maxX, ref maxY);
        }
        else if (direction == TargetDistanceAdvanceDirection.DOWN_LEFT)
        {
            GridMap.instance.grid[startNodePos.x, startNodePos.y].Node = new AStarNode(baseCost);
            for (int i = 0; i > - y - 1; i--)
            {
                GridMap.instance.grid[x, i + y].Node.AvaibleAdjacentNodes = CheckAvaiblesPositions();
                GridMap.instance.grid[x, i + y].Node.FromFinalCost = baseCost + i * normalCost;
            }
            for (int i = 1; i < maxX - x; i++)
            {
                GridMap.instance.grid[i + x, y].Node.AvaibleAdjacentNodes = CheckAvaiblesPositions();
                GridMap.instance.grid[i + x, y].Node.FromFinalCost = baseCost + i * normalCost;
            }
            if (x < maxX && y >= 0)
                CalculateTargetRecursion(baseCost + diagonalCost, x + 1, y - 1, TargetDistanceAdvanceDirection.DOWN_LEFT, ref maxX, ref maxY);
        }
        else if (direction == TargetDistanceAdvanceDirection.UP_LEFT)
        {

        }
        else if (direction == TargetDistanceAdvanceDirection.UP_RIGHT)
        {

        }
        else
            Debug.LogError("This case don't exist");

    }

    private byte CheckAvaiblesPositions()
    {
        return 0;
    }


}
