﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GridMap : MonoBehaviour { //By default this is for a quad grid

    #region Singleton

    public static GridMap instance;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        if (instance != null)
        {
            Debug.LogError("More than one instance of grid is trying to active");
            return;
        }
        instance = this;
        bloquedMask = LayerMask.GetMask("Unwalkable");
        chestMask = LayerMask.GetMask("Chest");
        exitMask = LayerMask.GetMask("Exit");
        enemyMask = LayerMask.GetMask("Enemy");
        temporalGridObjects = new List<Vector2Int>();
        cellDiameter = CellRadius * 2;
        gridSizeX = Mathf.RoundToInt(WorldSize.x / cellDiameter);
        gridSizeY = Mathf.RoundToInt(WorldSize.y / cellDiameter);
        renderer = GetComponent<Renderer>();        
        renderer.material.SetFloat("_GridXSize", WorldSize.x);
        renderer.material.SetFloat("_GridYSize", WorldSize.y);
        cellCost = new int[gridSizeX, gridSizeY];
        CreateMatrix();
        AssignCosts();
        CreateGrid();
        clones = new GameObject[gridSizeX, gridSizeY];
        ShowNumbers();
    }
    #endregion

    public Cell[,] grid;
    public bool seeTypes = false;
    public bool seePathCost = false;
    public bool seePathFromStartCost = false;
    public bool seePathFromEndCost = false;
    public bool seeVisitedCells = false;
    public bool seeNumberOfAdjacents = false;
    public bool seeEnemyPath = false;
    public bool seeAStarChilds = false;
    public EnemyCombat enemySelected;

    [SerializeField] private Vector2 WorldSize;
    [SerializeField] private float CellRadius;

    private List<Vector2Int> temporalGridObjects;
    private LayerMask bloquedMask;
    private LayerMask chestMask;
    private LayerMask exitMask;
    private LayerMask enemyMask;
    private float cellDiameter;
    private int gridSizeX;
    private int gridSizeY;


    //NUMBERS
    [SerializeField] private GameObject numberText;
    private CameraIsMoving cameraIsMoving;
    private int[,] Level1Matrix;
    GameObject[,] clones;
    int[,] cellCost;

    //GridShader
    private Renderer renderer;

    //Scene
    private Scene scene;

    private void Start()
    {
        cameraIsMoving = GetComponent<CameraIsMoving>();
        UpdateEnemyPositions();
        ChangeTextRotation();
        
    }

    private void Update()
    {
        UpdateEnemyPositions();
        if (cameraIsMoving.isMoving)
        {
            ChangeTextRotation();
        }
    }

    private void CreateMatrix()
    {
        if (scene.name == "Level 1" || GameManager.instance != null && GameManager.instance.GetActualLevel() == 1 )
        {
            Level1Matrix = new int[,] { {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,8,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,9,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,1,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,3,0,3,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,3,0,2,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,4,0,5,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,3,0,2,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,9,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,9,0,4,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,3,0,5,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,0,2,1,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
        }

        else if (scene.name == "Level 3")
        {
            Level1Matrix = new int[,] { {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,0},
                                        {-1,3,0,0,3,-1,7,6,5,3,9,5,8,-1,1,1,1,2,1,1,1,5,1,1,-9,-9,-9,-1,0,0},
                                        {-1,1,6,1,1,1,2,1,3,4,8,2,7,-1,0,1,3,1,1,1,1,2,2,2,-9,-9,-9,-1,0,0},
                                        {-1,5,2,7,8,9,6,9,1,3,2,1,1,-1,0,1,-1,-1,-1,-1,-1,-1,-1,-1,4,6,1,-1,0,0},
                                        {-1,2,3,1,4,-1,7,8,1,8,6,4,2,-1,0,1,-1,-1,-1,-1,-1,-1,-1,-1,7,5,5,-1,0,0},
                                        {-1,5,5,5,5,-1,6,2,5,9,3,7,3,-1,0,1,-1,-1,-1,-1,-1,-1,-1,-1,1,3,1,-1,0,0},
                                        {-1,6,9,2,5,-1,3,4,3,7,2,5,1,-1,1,1,7,5,1,2,1,1,8,4,3,6,4,-1,0,0},
                                        {-1,7,1,2,7,-1,2,5,6,6,7,6,4,-1,1,2,2,1,9,7,6,1,1,1,9,5,3,-1,0,0},
                                        {-1,8,1,5,6,-1,1,1,5,2,1,3,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,1,7,6,-1,0,0},
                                        {-1,8,4,6,7,-1,-9,-9,-1,-1,-1,1,1,-1,4,6,5,7,8,9,8,9,6,7,2,5,4,-1,0,0},
                                        {-1,2,1,9,5,-1,-9,-9,-1,0,-1,2,2,1,2,3,1,1,2,4,8,5,3,1,3,8,9,-1,0,0},
                                        {-1,3,6,5,2,-1,1,1,-1,-1,-1,1,1,1,3,1,2,8,1,1,3,1,1,2,1,7,6,-1,0,0},
                                        {-1,2,1,7,3,-1,2,5,6,7,2,4,4,-1,1,1,3,5,7,6,6,8,-9,-9,-1,-1,-1,-1,0,0},
                                        {-1,-1,1,-1,-1,-1,4,3,2,4,3,5,7,-1,2,1,1,1,1,1,1,1,-9,-9,-1,0,0,-1,0,0},
                                        {-1,2,4,1,5,-1,3,7,-1,2,4,6,8,-1,7,1,6,-1,-1,-1,-1,-1,-1,-1,-1,1,1,-1,0,0},
                                        {-1,5,7,4,2,1,2,7,1,1,4,7,8,-1,6,3,7,1,1,3,1,1,1,5,9,9,1,-1,0,0},
                                        {-1,3,0,0,9,1,1,6,8,9,2,5,9,-1,7,4,8,9,1,7,1,2,9,5,9,9,1,-1,0,0},
                                        {-1,6,0,0,8,-1,1,3,2,5,7,8,9,-1,7,3,5,7,1,7,1,3,1,1,9,9,4,-1,0,0},
                                        {-1,5,3,1,4,-1,1,1,1,2,1,3,1,-1,1,1,7,6,1,1,1,4,8,1,2,1,3,-1,0,0},
                                        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,0,2,1,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,1,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
        }
    }

    private void AssignCosts()
    {
        if (scene.name == "Level 1" || scene.name == "Level 1 void" || scene.name == "Level 3")
        {            
            for(int x = 0; x < gridSizeX; x++)
            {
                for(int y = 0; y < gridSizeY; y++)
                {                    
                    cellCost[x, y] = Level1Matrix[x, gridSizeY - 1 - y]; 

                }
            }
        }
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
                    grid[x, y] = new Cell(CellTypes.blocked, worldPoint, cellCost[x,y]);
                }
                else if (Physics.CheckBox(worldPoint, Vector3.one * CellRadius, Quaternion.identity, chestMask)) 
                {
                    grid[x, y] = new Cell(CellTypes.chest, worldPoint, cellCost[x,y]);
                }
                else if (Physics.CheckBox(worldPoint, Vector3.one * CellRadius, Quaternion.identity, exitMask)) 
                {
                    grid[x, y] = new Cell(CellTypes.exit, worldPoint, cellCost[x,y]);
                }
                else//last else
                {
                    grid[x, y] = new Cell(CellTypes.emphy, worldPoint, cellCost[x,y]);
                }
            }
        }
    }

    private void ShowNumbers()
    {
        numberText.transform.localScale = new Vector3(1 / WorldSize.y, 1 / WorldSize.x, 1);        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                GameObject clone = Instantiate(numberText);
                clone.transform.SetParent(gameObject.transform);
                clone.transform.position = new Vector3(-WorldSize.x / 2 + x * cellDiameter + CellRadius, 0.01f, -WorldSize.y / 2 + y * cellDiameter + CellRadius);

                TextMeshPro textMesh = clone.GetComponent<TextMeshPro>();             
                textMesh.text = "" + grid[x, y].Cost;
                textMesh.alignment = TextAlignmentOptions.Midline;

                clones[x, y] = clone;
            }
        }
    }

    private void UpdateEnemyPositions()
    {
        RaycastHit[] enemies = Physics.BoxCastAll(transform.position, Vector3.one * Mathf.Max(WorldSize.x, WorldSize.y), Vector3.one, 
            Quaternion.identity, Mathf.Max(WorldSize.x, WorldSize.y) * 2, enemyMask, QueryTriggerInteraction.Ignore);
        CleanNonStaticElementsOnGrid();
        Vector2Int pos;
        for (int i = 0; i< enemies.Length; i++)
        {
            pos = CellCordFromWorldPoint(enemies[i].collider.gameObject.transform.position);
            if(grid[pos.x, pos.y].CellType == CellTypes.emphy)
            {
                grid[pos.x, pos.y].CellType = CellTypes.enemy;
                temporalGridObjects.Add(new Vector2Int(pos.x, pos.y));
            }
        }
    }

    private void ChangeTextRotation()
    {       
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {                
                clones[x,y].transform.rotation = Camera.main.transform.rotation;
            }
        }        

    }

    private void CleanNonStaticElementsOnGrid()
    {
        for(int i = 0; i< temporalGridObjects.Count; i++)
        {
            grid[temporalGridObjects[i].x, temporalGridObjects[i].y].CellType = CellTypes.emphy;  
        }
        temporalGridObjects = new List<Vector2Int>();
    }

    public Cell CellFromWorldPoint(Vector3 worldPosition) 
    {
        
        float percentX = (worldPosition.x + WorldSize.x / 2) / WorldSize.x;
        float percentY = (worldPosition.z + WorldSize.y / 2) / WorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt((gridSizeX) * percentX);
        int y = Mathf.FloorToInt((gridSizeY) * percentY);
        return grid[x, y];
    }

    public Vector2Int CellCordFromWorldPoint(Vector3 worldPosition) 
    {
        
        float percentX = (worldPosition.x + WorldSize.x / 2) / WorldSize.x;
        float percentY = (worldPosition.z + WorldSize.y / 2) / WorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt((gridSizeX) * percentX);
        int y = Mathf.FloorToInt((gridSizeY) * percentY);
        return new Vector2Int(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(WorldSize.x, 1, WorldSize.y));

        if (grid != null)
        {
            if (seeTypes)
            {
                foreach (Cell n in grid )
                {
                    if (n.Cost == 0)
                    {
                        Gizmos.color = Color.black;
                    }
                        if (n.CellType == CellTypes.emphy)
                        {
                            Gizmos.color = Color.white;
                        }
                        else if (n.CellType == CellTypes.blocked)
                        {
                            Gizmos.color = Color.magenta;
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

                        Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                    
                }
            }
            else if (seePathCost && enemySelected != null) //PATH COST
            {
                int maxCost = int.MinValue;
                int minCost = int.MaxValue;
                foreach (Cell n in grid)
                {

                    if (n.Node.NodeFinalCost != int.MaxValue)
                    {
                        if (maxCost < n.Node.NodeFinalCost)
                        {
                            maxCost = n.Node.NodeFinalCost;
                        }
                        else if (minCost > n.Node.NodeFinalCost)
                        {
                            minCost = n.Node.NodeFinalCost;
                        }
                    }
                }
                foreach (Cell n in grid)
                {
                    if (n.Node.NodeFinalCost == int.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)n.Node.NodeFinalCost / maxCost, (float)n.Node.NodeFinalCost / maxCost, (float)n.Node.NodeFinalCost / maxCost, 1); ;
                    }

                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seePathFromStartCost && enemySelected != null) //PATH INITIAL COST
            {
                int maxCost = 1;
                int minCost = 1;
                float factor;

                foreach (Cell n in grid)
                {

                    if (n.Node.FromInitialCost != int.MaxValue)
                    {
                        if (maxCost < n.Node.FromInitialCost)
                        {
                            maxCost = n.Node.FromInitialCost;
                        }
                        else if (minCost > n.Node.FromInitialCost)
                        {
                            minCost = n.Node.FromInitialCost;
                        }
                    }
                }
                factor = minCost / maxCost;
                foreach (Cell n in grid)
                {
                    if (n.Node.FromInitialCost == int.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)n.Node.FromInitialCost / maxCost, (float)n.Node.FromInitialCost / maxCost, (float)n.Node.FromInitialCost / maxCost, 1);
                    }

                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seePathFromEndCost && enemySelected != null) // PATH FROM FINAL COST
            {
                int maxCost = int.MinValue;
                int minCost = 1;
                foreach (Cell n in grid)
                {

                    if (n.Node.FromFinalCost != int.MaxValue)
                    {
                        if (maxCost < n.Node.FromFinalCost)
                        {
                            maxCost = n.Node.FromFinalCost;
                        }
                        else if (minCost > n.Node.FromFinalCost)
                        {
                            minCost = n.Node.FromFinalCost;
                        }
                    }
                }
                foreach (Cell n in grid)
                {
                    if (n.Node.FromFinalCost == int.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)n.Node.FromFinalCost / maxCost, (float)n.Node.FromFinalCost / maxCost, (float)n.Node.FromFinalCost / maxCost, 1);
                    }

                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seeVisitedCells && enemySelected != null) // PATH FROM FINAL COST
            {
                foreach (Cell n in grid)
                {
                    if (n.Node.visited)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }
                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seeNumberOfAdjacents && enemySelected != null) // PATH FROM FINAL COST
            {
                foreach (Cell n in grid)
                {
                    switch (n.Node.AvaibleAdjacentNodes)
                    {
                        case 0:
                            {
                                Gizmos.color = Color.black;
                                break;
                            }
                        case 1:
                            {
                                Gizmos.color = Color.red;
                                break;
                            }
                        case 2:
                            {
                                Gizmos.color = Color.yellow;
                                break;
                            }
                        case 3:
                            {
                                Gizmos.color = Color.green;
                                break;
                            }
                        case 4:
                            {
                                Gizmos.color = Color.cyan;
                                break;
                            }
                        case 5:
                            {
                                Gizmos.color = Color.blue;
                                break;
                            }
                        case 6:
                            {
                                Gizmos.color = Color.magenta;
                                break;
                            }
                        case 7:
                            {
                                Gizmos.color = Color.grey;
                                break;
                            }
                        case 8:
                            {
                                Gizmos.color = Color.clear;
                                break;
                            }
                        default:
                            {
                                Gizmos.color = Color.white;
                                break;
                            }
                    }
                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seeEnemyPath && enemySelected != null)
            {
                int size = enemySelected.GetSavedPath().Count;
                LinkedList<Vector2Int> path = enemySelected.GetSavedPath();
                LinkedListNode<Vector2Int> element = path.First;
                for (int i = 0; i < size; i++)
                {
                    Gizmos.color = new Color((float)i / size, (float)i / size, (float)i / size, 1);
                    Gizmos.DrawCube(grid[element.Value.x,element.Value.y].GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                    element = element.Next;
                }
            }
            else if (seeAStarChilds && enemySelected != null)
            {
                Vector2Int initialPos = new Vector2Int(0,0);
                bool end = false;
                for (int x = 0; x < gridSizeX && !end; x++)
                {
                    for (int y = 0; y < gridSizeY && !end; y++)
                    {
                        if (grid[x,y].Node.FromInitialCost == 0)
                        {
                            end = true;
                            initialPos = new Vector2Int(x, y);
                        }
                    }
                }
                Gizmos.color = Color.red;
                PrintBranchRecursive(initialPos.x, initialPos.y);
            }
        }
    }

    private void PrintBranchRecursive(int x, int y)
    {
        LinkedList<Vector2Int> childList = grid[x, y].Node.GetChillds();
        LinkedListNode<Vector2Int> node = childList.First;
       
        while (node != null)
        {
            Gizmos.DrawLine(grid[node.Value.x, node.Value.y].GlobalPosition, grid[x, y].GlobalPosition);
            PrintBranchRecursive(node.Value.x, node.Value.y);
            node = node.Next;
        }
    }

    public int GetGridSizeX()
    {
        return gridSizeX;
    }

    public int GetGridSizeY()
    {
        return gridSizeY;
    }

    public float GetCellRadius()
    {
        return CellRadius;
    }
}
