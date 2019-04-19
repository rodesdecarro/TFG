using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite = null;

    public Sprite Sprite { get => sprite; }

    [SerializeField]
    private GameObject towerPrefab = null;

    [SerializeField]
    private int price = 0;

    public int Price { get => price; }

    [SerializeField]
    private Text priceTxt = null;

    public GameObject TowerPrefab { get => towerPrefab; }

    // Start is called before the first frame update
    void Start()
    {
        priceTxt.text = Price.ToString();
    }
}
