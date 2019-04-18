using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Point GridPosition
    {
        get
        {
            return Tile.GridPosition;
        }
    }

    public TileScript Tile { get; private set; }

    public Node Parent { get; private set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Node(TileScript tile)
    {
        Tile = tile;
    }

    public void CalcValues(Node parent, Point goal, int gCost)
    {
        Parent = parent;
        G = parent.G + gCost;
        H = (Mathf.Abs(GridPosition.X - goal.X) + Mathf.Abs(GridPosition.Y - goal.Y)) * 10;
        F = G + H;
    }
}

