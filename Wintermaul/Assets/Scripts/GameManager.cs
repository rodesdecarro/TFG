using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TowerBtn ClickedBtn { get; private set; }

    [SerializeField]
    private GameObject inGameMenu = null;

    [SerializeField]
    private GameObject pauseMenu = null;

    [SerializeField]
    private GameObject optionsMenu = null;

    [SerializeField]
    private GameObject gameOverMenu = null;

    public ObjectPool Pool { get; set; }

    private bool isGameOver = false;

    public TileScript HoveredTile { get; set; }

    [SerializeField]
    private Text goldTxt = null;

    [SerializeField]
    private Text lifesTxt = null;

    [SerializeField]
    private Text waveTxt = null;

    [SerializeField]
    private GameObject waveBtn = null;

    [SerializeField]
    private int waveSize = 0;

    public List<Monster> ActiveMonsters { get; private set; }

    private int gold;

    public int Gold
    {
        get => gold;
        private set
        {
            gold = value;
            goldTxt.text = value.ToString();
        }
    }

    private int lifes;

    public int Lifes
    {
        get => lifes;
        set
        {
            lifes = value;
            lifesTxt.text = value.ToString();

            if (lifes <= 0)
            {
                GameOver();
            }
        }
    }

    private int wave;
    private bool towerBought;

    public int Wave
    {
        get => wave;
        private set
        {
            wave = value;
            waveTxt.text = value.ToString();
        }
    }

    private void Awake()
    {
        ActiveMonsters = new List<Monster>();
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Gold = 1000;
        Lifes = 50;
        Wave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
        HandleLeftShift();
    }

    private void HandleLeftShift()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) && towerBought)
        {
            DropTower();
        }
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (ClickedBtn == towerBtn || Gold < towerBtn.Price)
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
        if (Gold >= ClickedBtn.Price)
        {
            Gold -= ClickedBtn.Price;
            towerBought = true;
        }

        if (!Input.GetKey(KeyCode.LeftShift) || Gold < ClickedBtn.Price)
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
        towerBought = false;
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

    public void NextWave()
    {
        Wave++;
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveSize; i++)
        {
            Monster monster = Pool.GetObject("Skeleton").GetComponent<Monster>();
            monster.Spawn();
            ActiveMonsters.Add(monster);

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void RemoveMonster(Monster monster)
    {
        ActiveMonsters.Remove(monster);

        if (ActiveMonsters.Count == 0)
        {
            waveBtn.SetActive(true);
        }
    }
}
