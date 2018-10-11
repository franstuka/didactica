using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode  {

    public int FromInitialCost;
    public int FromFinalCost;
    public int NodeFinalCost;
    public bool visited;
    public byte AvaibleAdjacentNodes; //0 to 255
    Vector2Int parent = new Vector2Int(-1,-1);
    Vector2Int soon = new Vector2Int(-1, -1);

    public AStarNode(int FromFinalCost)
    {    
        FromInitialCost = int.MaxValue;
        this.FromFinalCost = FromFinalCost;
        NodeFinalCost = int.MaxValue;
        AvaibleAdjacentNodes = 8;
        visited = false;
        parent = new Vector2Int(-1, -1);
        soon = new Vector2Int(-1, -1);
    }



}
