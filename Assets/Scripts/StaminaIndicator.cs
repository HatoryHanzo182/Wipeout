using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaIndicator : MonoBehaviour
{
    private Image _indicator;

    void Start()
    {
        _indicator = GameObject.Find("StaminaIndicator").GetComponent<Image>();
    }

    void Update()
    {
        _indicator.fillAmount = GameState.Stamina;
        _indicator.color = new Color(1f, GameState.Stamina, 0.2f);
    }
}