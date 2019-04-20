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
        text.AppendLine($"<color='#{Tower.Color}'><b>{Tower.Name}</b></color>");
        text.AppendLine();
        text.AppendLine($"<b>Range:</b> {Tower.Range}");
        text.AppendLine($"<b>Damage:</b> {Tower.MinDamage} - {Tower.MaxDamage}");
        text.AppendLine($"<b>Attack cooldown:</b> {Tower.AttackCooldown} s");

        GameManager.Instance.SetTooltipText(text.ToString());
        GameManager.Instance.ShowStats();
    }
}