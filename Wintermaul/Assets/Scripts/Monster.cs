using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationOffset;

    private Stack<Node> path;

    public Point GridPosition { get; set; }

    private Vector3 destination;

    public bool IsActive { get; set; }

    public void Spawn()
    {
        SetToStart();
    }

    private void SetToStart()
    {
        transform.position = LevelManager.Instance.Tiles[LevelManager.Instance.StartPoint].WorldPosition;
        transform.rotation = new Quaternion(0, 0, 0, 1);
        GridPosition = LevelManager.Instance.StartPoint;
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1)));
        SetPath();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

            if (hit.collider != null)
            {
                TileScript tile = hit.collider.GetComponent<TileScript>();

                if (tile != null)
                {
                    GridPosition = tile.GridPosition;
                }
            }

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    destination = path.Pop().Tile.WorldPosition;
                }
                else
                {
                    StartCoroutine(Despawn());
                }
            }

            Animate();
        }
    }

    private IEnumerator Scale(Vector3 from, Vector3 to)
    {
        IsActive = false;

        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime * 2;

            yield return null;
        }

        transform.localScale = to;

        IsActive = true;
    }

    public void SetPath()
    {
        path = AStar.GetPath(GridPosition, LevelManager.Instance.GoalPoint);
        destination = path.Pop().Tile.WorldPosition;
    }

    private Stack<Node> newPath;

    public bool GenerateNewPath()
    {
        newPath = AStar.GetPath(GridPosition, LevelManager.Instance.GoalPoint);

        return newPath.Count > 0;
    }

    public void SetPathToNewPath()
    {
        path = new Stack<Node>(new Stack<Node>(newPath));
        destination = path.Pop().Tile.WorldPosition;
    }

    private void Animate()
    {
        Vector3 vectorToTarget = destination - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed * 2);
    }

    private IEnumerator Despawn()
    {
        GameManager.Instance.LoseLife();
        yield return Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f));

        //Release();
        SetToStart();
    }

    private void Release()
    {
        GameManager.Instance.RemoveMonster(this);
        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
}
