using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TowerBtn ClickedBtn { get; private set; }

    [SerializeField]
    private GameObject pauseMenu;

    public TileScript HoveredTile { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (ClickedBtn == towerBtn)
        {
            DropTower();
        }
        else
        {
            ClickedBtn = towerBtn;
            Hover.Instance.Activate(ClickedBtn.Sprite);
        }
    }

    public void BuyTower()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DropTower();
        }
    }

    private void DropTower()
    {
        HoveredTile.ColorTile(Color.white);
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }

    private void HandleEscape()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ClickedBtn == null)
            {
                Pause();
            }
            else
            {
                DropTower();
            }
        }
    }

    private void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
