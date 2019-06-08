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
    private GameObject statsPanel = null;

    [SerializeField]
    private GameObject optionsMenu = null;

    [SerializeField]
    private GameObject cheatsMenu = null;

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
    private Text sizeTxt = null;

    [SerializeField]
    private Text statsTxt = null;

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

    [SerializeField]
    private int maxWave = 0;

    [SerializeField]
    private Text gameOverTxt = null;

    [SerializeField]
    private Button[] speedButtons = null;

    [SerializeField]
    private Slider musicSlider = null;

    [SerializeField]
    private Slider sfxSlider = null;

    private Tower selectedTower;

    public List<Monster> ActiveMonsters { get; private set; }

    private float gold;

    public float Gold
    {
        get => gold;
        set
        {
            gold = value;
            goldTxt.text = ((int)value).ToString();
        }
    }

    public int SpeedModifier { get; private set; }

    private int[] speedsModifiers = new int[] { 1, 3, 5 };

    public void ChangeSpeed(int speedIndex)
    {
        SpeedModifier = speedsModifiers[speedIndex];
        Time.timeScale = SpeedModifier;

        foreach (Button button in speedButtons)
        {
            button.GetComponent<Image>().color = new Color(0, 0, 0, 0.4f);
        }

        speedButtons[speedIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1);
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
            waveTxt.text = $"{value}/{maxWave}";
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

    private bool spawning;

    private void Awake()
    {
        ActiveMonsters = new List<Monster>();
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Update background music and slider instances
        SoundManager.Instance.SetBackgroundMusic("Game");
        SoundManager.Instance.SfxSlider = sfxSlider;
        SoundManager.Instance.MusicSlider = musicSlider;
        SoundManager.Instance.LoadVolume();

        SpeedModifier = 1;
        Gold = initialGold;
        Lifes = initialLifes;
        Wave = initialWave;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
        HandleLeftShift();
        HandleRightBtn();
    }

    private void HandleRightBtn()
    {
        if (Input.GetMouseButtonDown(1) && selectedTower != null)
        {
            UnselectTower();
        }
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
            Time.timeScale = SpeedModifier;
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
        cheatsMenu.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        cheatsMenu.SetActive(false);
    }

    public void ShowCheats()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        cheatsMenu.SetActive(true);
    }

    private void GameOver()
    {
        DropTower();
        Time.timeScale = 0;
        ShowGameOver();
        isGameOver = true;
    }

    private void ShowGameOver()
    {
        inGameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        cheatsMenu.SetActive(false);
        gameOverMenu.SetActive(true);

        SoundManager.Instance.SetBackgroundMusic("MainMenu");
    }

    public void AddGold()
    {
        Gold += 1000;
    }

    public void AddLifes()
    {
        Lifes += 100;
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
            // Wave ended. Reward player with 5% of current gold
            Gold *= 1.05f;

            if (wave == maxWave)
            {
                // The game ends
                GameOver();

                // Add the remaining gold and lifes as bonus points
                Score += (int)Gold * 10;
                Score += Lifes * 100;

                // Update the Game Over text
                gameOverTxt.text = "You win!";
            }
            else
            {
                waveBtn.SetActive(true);
            }
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

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetTooltipText(string text)
    {
        statsTxt.text = text;
        sizeTxt.text = text;
    }

    public void PlaySfx(string name)
    {
        SoundManager.Instance.PlaySfx(name);
    }
}
