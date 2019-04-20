using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTowerBtn : MonoBehaviour
{
    private int price;

    public int Price
    {
        get => price;
        set
        {
            price = value;
            transform.GetChild(0).GetComponent<Text>().text = $"Upgrade ({price})";
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Button>().interactable = GameManager.Instance.Gold >= Price;
    }
}
