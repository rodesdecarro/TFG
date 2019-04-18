using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
    private TileScript goal;
    private TileScript start;

    private Stack<Node> path;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ClickTile();
    }

    private void SetStart(TileScript tile)
    {
        start = tile;
        start.Debugging = true;
        start.SpriteRenderer.color = Color.green;
    }

    private void SetGoal(TileScript tile)
    {
        goal = tile;
        goal.Debugging = true;
        goal.SpriteRenderer.color = Color.red;
    }

    private void ResetStart()
    {
        start.SpriteRenderer.color = Color.white;
        start.Debugging = false;
        start = null;
        ResetPath();
    }

    private void ResetGoal()
    {
        goal.SpriteRenderer.color = Color.white;
        goal.Debugging = false;
        goal = null;
        ResetPath();
    }

    private void ResetPath()
    {
        if (path != null)
        {
            foreach (Node node in path)
            {
                node.Tile.SpriteRenderer.color = Color.white;
                node.Tile.Debugging = false;
            }

            path = null;
        }
    }

    private void SetPath(Stack<Node> newPath)
    {
        path = newPath;

        foreach (Node node in path)
        {
            if (!node.Tile.Debugging)
            {
                node.Tile.SpriteRenderer.color = Color.magenta;
                node.Tile.Debugging = true;
            }
        }
    }

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();

                if (tile != null && tile.CanWalk)
                {
                    if (start == null)
                    {
                        SetStart(tile);
                    }
                    else if (start == tile)
                    {
                        if (goal == null)
                        {
                            ResetStart();
                        }
                        else
                        {
                            ResetGoal();
                        }
                    }
                    else if (goal == null)
                    {
                        SetGoal(tile);
                        Stack<Node> newPath = AStar.GetPath(start.GridPosition, goal.GridPosition);
                        SetPath(newPath);
                    }
                    else
                    {
                        ResetStart();
                        ResetGoal();
                        SetStart(tile);
                    }
                }
            }
        }
    }
}
