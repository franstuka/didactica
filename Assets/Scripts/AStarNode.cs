﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode  { //By default this is for a quad grid

    public int FromInitialCost;
    public int FromFinalCost;
    public int NodeFinalCost;
    public bool visited;
    public byte AvaibleAdjacentNodes; //0 to 255
    public uint stepsUsed;
    Vector2Int parent;
    LinkedList<Vector2Int> chillds;

    public AStarNode(int FromFinalCost)
    {    
        FromInitialCost = int.MaxValue;
        this.FromFinalCost = FromFinalCost;
        NodeFinalCost = int.MaxValue;
        AvaibleAdjacentNodes = 0;
        stepsUsed = 0;
        visited = false;
        parent = new Vector2Int(-1, -1);
        chillds = new LinkedList<Vector2Int>();
    }

    public void ReduceAvaiblesNodes()
    {
        if (AvaibleAdjacentNodes > 0)
            AvaibleAdjacentNodes--;
    }

    public void SetFinalCost()
    {
        if (!visited)
            NodeFinalCost = FromFinalCost + FromInitialCost;
    }

    public void SetFinalCost(int FromInitialCost)
    {
        if(!visited)
            NodeFinalCost = FromFinalCost + FromInitialCost;
    }

    public void SetParent(int x, int y)
    {
        parent = new Vector2Int(x, y);
    }

    public void AddChill(int x, int y)
    {
        chillds.AddFirst(new Vector2Int(x, y));
    }

    public Vector2Int GetParent()
    {
        return parent;
    }
}
