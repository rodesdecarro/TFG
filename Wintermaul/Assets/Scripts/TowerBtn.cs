using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite = null;

    public Sprite Sprite { get => sprite; }

    [SerializeField]
    private GameObject towerPrefab = null;

    private Tower Tower { get => TowerPrefab.transform.GetChild(0).GetComponent<Tower>(); }

    public int Price { get => Tower.Price; }

    [SerializeField]
    private Text priceTxt = null;

    public GameObject TowerPrefab { get => towerPrefab; }

    // Start is called before the first frame update
    void Start()
    {
        priceTxt.text = Price.ToString();
    }

    private void Update()
    {
        GetComponent<Button>().interactable = GameManager.Instance.Gold >= Price;
    }

    public void ShowInfo()
    {
        StringBuilder text = new StringBuilder();
        text.Append($"<color='#{Tower.Color}'><b>{Tower.TowerName}</b></color>");
        text.Append("\n");
        text.Append($"\n<b>Range:</b> {Tower.Range}");
        text.Append($"\n<b>Damage:</b> {Tower.MinDamage} - {Tower.MaxDamage}");
        text.Append($"\n<b>Attack cooldown:</b> {Tower.AttackCooldown} s");

        if (Tower.SplashArea > 0)
        {
            text.Append($"\n<b>Splash area:</b> {Tower.SplashArea}");
        }

        if (Tower.CritChance > 0)
        {
            text.Append($"\n<b>Critical attack chance:</b> {Tower.CritChance * 100}%");
        }

        if (Tower.SlowDuration > 0)
        {
            text.Append($"\n<b>Slows</b> damaged enemies for {Tower.SlowDuration} s");
        }

        GameManager.Instance.SetTooltipText(text.ToString());
        GameManager.Instance.ShowStats();
    }
}