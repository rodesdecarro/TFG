using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;

    public Sprite Sprite { get => sprite; }

    [SerializeField]
    private GameObject towerPrefab;

    public GameObject TowerPrefab { get => towerPrefab; }
}
