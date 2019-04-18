using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> path = new Stack<Node>();

        Node currentNode = nodes[start];
        Node startNode = nodes[start];
        Node goalNode = nodes[goal];

        openList.Add(currentNode);

        while (openList.Any())
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighborPosition = new Point(currentNode.GridPosition.X + x, currentNode.GridPosition.Y + y);

                    if (neighborPosition != currentNode.GridPosition && nodes.ContainsKey(neighborPosition))
                    {
                        Node neighborNode = nodes[neighborPosition];

                        if (neighborNode.Tile.CanWalk && !closedList.Contains(neighborNode))
                        {
                            int gCost = 0;

                            if (x == 0 || y == 0)
                            {
                                gCost = 10;
                            }
                            else
                            {
                                // If the movement is diagonal, only allow it if near tiles can be walked
                                Point xNeighborPosition = new Point(currentNode.GridPosition.X + x, currentNode.GridPosition.Y);
                                Point yNeighborPosition = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + y);

                                if (nodes.ContainsKey(xNeighborPosition) && nodes.ContainsKey(yNeighborPosition) && nodes[xNeighborPosition].Tile.CanWalk && nodes[yNeighborPosition].Tile.CanWalk)
                                {
                                    gCost = 14;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (openList.Contains(neighborNode))
                            {
                                if (currentNode.G + gCost < neighborNode.G)
                                {
                                    neighborNode.CalcValues(currentNode, goal, gCost);
                                }
                            }
                            else
                            {
                                openList.Add(neighborNode);
                                neighborNode.CalcValues(currentNode, goal, gCost);
                            }


                        }
                    }


                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Any())
            {
                currentNode = openList.OrderBy(x => x.F).First();
            }

            if (currentNode == goalNode)
            {
                while (currentNode != startNode)
                {
                    path.Push(currentNode);
                    currentNode = currentNode.Parent;
                }

                break;
            }
        }

        return path;
    }
}
