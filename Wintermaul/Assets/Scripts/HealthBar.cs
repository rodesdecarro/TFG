using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image Content = null;

    public void SetFillAmount(float fillAmount)
    {
        Content.fillAmount = fillAmount;
    }
}
