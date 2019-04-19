﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    private Color32 KoColor = new Color32(255, 127, 127, 255);
    private Color32 OkColor = new Color32(127, 255, 127, 255);

    public SpriteRenderer SpriteRenderer { get; set; }

    public Vector3 WorldPosition
    {
        get
        {
            return new Vector3(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2, 0);
        }
    }

    public bool Debugging { get; set; }

    [SerializeField]
    private bool canBuild = false;

    [SerializeField]
    private bool canWalk = false;

    private bool isEmpty;

    public bool CanBuild { get => canBuild && isEmpty; }
    public bool CanWalk { get => canWalk && isEmpty; }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(Point gridPosition, Vector3 worldPosition, Transform parent)
    {
        isEmpty = true;
        GridPosition = gridPosition;
        transform.position = worldPosition;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPosition, this);
    }

    public void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null && !Debugging)
        {
            GameManager.Instance.HoveredTile = this;

            if (CanBuild)
            {
                ColorTile(OkColor);

                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
            else
            {
                ColorTile(KoColor);
            }
        }
    }

    public void OnMouseExit()
    {
        if (!Debugging)
        {
            ColorTile(Color.white);
        }
    }

    public void PlaceTower()
    {
        // Check if any monster is in the tile
        foreach (Monster monster in GameManager.Instance.ActiveMonsters)
        {
            if (monster.GridPosition == GridPosition)
            {
                return;
            }
        }

        isEmpty = false;

        // Check if exists a path between start and goal
        if (AStar.GetPath(LevelManager.Instance.StartPoint, LevelManager.Instance.GoalPoint).Count == 0)
        {
            isEmpty = true;

            return;
        }

        // Recalculate paths for each monster
        foreach (Monster monster in GameManager.Instance.ActiveMonsters)
        {
            if (!monster.GenerateNewPath())
            {
                // If any monster path is blocked, block the placement
                isEmpty = true;

                return;
            }
        }

        // If no paths are blocked, update them and continue with the placement
        foreach (Monster monster in GameManager.Instance.ActiveMonsters)
        {
            monster.SetPathToNewPath();
        }

        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, WorldPosition, Quaternion.identity);
        tower.transform.SetParent(transform);

        GameManager.Instance.BuyTower();

        if (GameManager.Instance.ClickedBtn == null)
        {
            ColorTile(Color.white);
        }
        else
        {
            ColorTile(KoColor);
        }
    }

    public void ColorTile(Color color)
    {
        SpriteRenderer.color = color;
    }
}
