using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    private Color32 KoColor = new Color32(255, 127, 127, 255);
    private Color32 OkColor = new Color32(127, 255, 127, 255);

    private SpriteRenderer spriteRenderer;

    public Vector3 WorldPosition
    {
        get
        {
            return new Vector3(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2, 0);
        }
    }

    [SerializeField]
    private bool canBuild;

    [SerializeField]
    private bool canWalk;

    private bool isEmpty;

    public bool CanBuild { get => canBuild && isEmpty; }
    public bool CanWalk { get => canWalk && isEmpty; }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
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
        ColorTile(Color.white);
    }

    public void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, WorldPosition, Quaternion.identity);
        tower.transform.SetParent(transform);
        isEmpty = false;

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

    private void ColorTile(Color color)
    {
        spriteRenderer.color = color;
    }
}
