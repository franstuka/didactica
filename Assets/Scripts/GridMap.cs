using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour {

    
    public Vector2 WorldSize;
    public float CellRadius;
    public Cell[,] grid;
    private LayerMask bloquedMask;
    private LayerMask chestMask;
    private LayerMask exitMask;
    private LayerMask enemyMask;

    float cellDiameter;
    int gridSizeX;
    int gridSizeY;

    private void Start()
    {
        bloquedMask = LayerMask.GetMask("Unwalkable");
        chestMask = LayerMask.GetMask("Chest");
        exitMask = LayerMask.GetMask("Exit");
        enemyMask = LayerMask.GetMask("Enemy");

        cellDiameter = CellRadius * 2;
        gridSizeX = Mathf.RoundToInt(WorldSize.x / cellDiameter);
        gridSizeY = Mathf.RoundToInt(WorldSize.y / cellDiameter);
        CreateGrid();
    }

    private void Update()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Cell[gridSizeX, gridSizeY];
        Vector3 gridBottonLeft = transform.position - Vector3.right * WorldSize.x / 2 - Vector3.forward * WorldSize.y / 2;
        
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridBottonLeft + Vector3.right * (x * cellDiameter + CellRadius) + Vector3.forward * (y * cellDiameter + CellRadius);

                if(Physics.CheckBox(worldPoint,Vector3.one * CellRadius,Quaternion.identity,bloquedMask)) //celltypes
                {
                    grid[x, y] = new Cell(CellTypes.blocked, worldPoint, 0);
                }
                else if (Physics.CheckSphere(worldPoint, CellRadius, chestMask)) 
                {
                    grid[x, y] = new Cell(CellTypes.chest, worldPoint, 0);
                }
                else if (Physics.CheckSphere(worldPoint, CellRadius, exitMask)) 
                {
                    grid[x, y] = new Cell(CellTypes.exit, worldPoint, 0);
                }
                else if (Physics.CheckSphere(worldPoint, CellRadius, enemyMask))
                {
                    grid[x, y] = new Cell(CellTypes.enemy, worldPoint, 0);
                }
                else//last else
                {
                    grid[x, y] = new Cell(CellTypes.emphy, worldPoint, 0);
                }
            }
        }
    }

    public Cell CellFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + WorldSize.x / 2) / WorldSize.x;
        float percentY = (worldPosition.z + WorldSize.y / 2) / WorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(WorldSize.x, 1, WorldSize.y));

        if(grid != null)
        {
            foreach (Cell n in grid)
            {
                if (n.CellType == CellTypes.emphy)
                {
                    Gizmos.color = Color.white;
                }
                else if (n.CellType == CellTypes.blocked)
                {
                    Gizmos.color = Color.gray;
                }
                else if (n.CellType == CellTypes.enemy)
                {
                    Gizmos.color = Color.red;
                }
                else if (n.CellType == CellTypes.chest)
                {
                    Gizmos.color = Color.yellow;
                }
                else if (n.CellType == CellTypes.exit)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.black;
                }

                Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter *19/20 ));
            }
        }

    }

}
