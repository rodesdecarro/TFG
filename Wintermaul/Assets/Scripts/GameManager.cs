using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public TowerBtn ClickedBtn { get; private set; }

    [SerializeField]
    private GameObject inGameMenu;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject gameOverMenu;

    private bool isGameOver = false;

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
        if (HoveredTile != null)
        {
            HoveredTile.ColorTile(Color.white);
        }

        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (ClickedBtn == null)
            {
                ShowInGameMenu();
            }
            else
            {
                DropTower();
            }
        }
    }

    public void ShowInGameMenu()
    {
        inGameMenu.SetActive(!inGameMenu.activeSelf);

        ShowPauseMenu();

        if (inGameMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void GameOver()
    {
        DropTower();
        Time.timeScale = 0;
        ShowGameOver();
        isGameOver = true;
    }

    public void ShowGameOver()
    {
        inGameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
