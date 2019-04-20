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
    private Text scoreTxt = null;

    [SerializeField]
    private GameObject waveBtn = null;

    [SerializeField]
    private GameObject sellTowerBtn = null;

    [SerializeField]
    private GameObject upgradeTowerBtn = null;

    [SerializeField]
    private int initialGold = 0;

    [SerializeField]
    private int initialLifes = 0;

    [SerializeField]
    private int initialWave = 0;

    [SerializeField]
    private int waveSize = 0;

    private Tower selectedTower;

    public List<Monster> ActiveMonsters { get; private set; }

    private int gold;

    public int Gold
    {
        get => gold;
        set
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

    private int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreTxt.text = value.ToString();
        }
    }

    public bool spawning;

    private void Awake()
    {
        ActiveMonsters = new List<Monster>();
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Gold = initialGold;
        Lifes = initialLifes;
        Wave = initialWave;
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
        UnselectTower();

        if (ClickedBtn == towerBtn || Gold < towerBtn.Price)
        {
            DropTower();
        }
        else
        {
            ClickedBtn = towerBtn;
            Hover.Instance.Activate(ClickedBtn.Sprite, ClickedBtn.TowerPrefab.transform.GetChild(0).GetComponent<Tower>().Range);
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
            if (selectedTower != null)
            {
                UnselectTower();
            }
            else if (ClickedBtn == null)
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
        string monsterType = "";

        switch (Wave % 3)
        {
            case 0:
                monsterType = "BigSkeleton";
                break;
            case 1:
                monsterType = "Skeleton";
                break;
            case 2:
                monsterType = "Spider";
                break;
        }

        spawning = true;

        float waveSizeModifier = 1;

        for (int i = 0; i < waveSize * waveSizeModifier; i++)
        {
            Monster monster = Pool.GetObject(monsterType).GetComponent<Monster>();

            waveSizeModifier = monster.WaveSizeModifier;

            monster.Spawn();
            ActiveMonsters.Add(monster);

            if ((i + 1) >= waveSize * waveSizeModifier)
            {
                spawning = false;
            }

            yield return new WaitForSeconds(0.5f / monster.SpeedModifier);
        }
    }

    public void RemoveMonster(Monster monster)
    {
        ActiveMonsters.Remove(monster);

        if (ActiveMonsters.Count == 0 && !spawning)
        {
            waveBtn.SetActive(true);
        }
    }

    public void ClickTower(Tower tower)
    {
        if (selectedTower == tower)
        {
            UnselectTower();
        }
        else
        {
            UnselectTower();
            SelectTower(tower);
        }
    }

    private void SelectTower(Tower tower)
    {
        selectedTower = tower;
        selectedTower.Select();

        sellTowerBtn.SetActive(true);
        sellTowerBtn.transform.GetChild(0).GetComponent<Text>().text = $"Sell ({selectedTower.Price / 2})";

        if (selectedTower.Upgrade != null)
        {
            upgradeTowerBtn.SetActive(true);
            upgradeTowerBtn.GetComponent<UpgradeTowerBtn>().Price = selectedTower.Upgrade.Price - selectedTower.Price / 2;
        }
    }

    private void UnselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
            selectedTower = null;
        }

        sellTowerBtn.SetActive(false);
        upgradeTowerBtn.SetActive(false);
    }

    public void SellTower()
    {
        // Update player's gold
        Gold += selectedTower.Price / 2;

        // Set the tile parent as Empty
        selectedTower.transform.GetComponentInParent<TileScript>().IsEmpty = true;

        // Recalculate monster paths
        foreach (Monster monster in ActiveMonsters)
        {
            monster.GenerateNewPath();
            monster.SetPathToNewPath();
        }

        // Remove the tower
        Destroy(selectedTower.transform.parent.gameObject);

        UnselectTower();
    }

    public void UpgradeTower()
    {
        int upgradePrice = selectedTower.Upgrade.Price - selectedTower.Price / 2;

        if (Gold >= upgradePrice)
        {
            // Update player's gold
            Gold -= upgradePrice;

            // Place the upgraded tower
            Tower newTower = selectedTower.transform.GetComponentInParent<TileScript>().BuildTower(selectedTower.UpgradePrefab);

            // Remove the tower
            Destroy(selectedTower.transform.parent.gameObject);

            UnselectTower();

            SelectTower(newTower);
        }
    }
}