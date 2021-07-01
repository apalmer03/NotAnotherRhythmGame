using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBar : MonoBehaviour
{
    public Slider ultimateBar;
    public Ultimate playerUltimate;

    private void Start()
    {
        playerUltimate = GameObject.FindGameObjectWithTag("Player").GetComponent<Ultimate>();
        ultimateBar = GetComponent<Slider>();
        ultimateBar.maxValue = playerUltimate.maxValue;
        ultimateBar.value = 0;
    }

    void Update()
    {
        if (playerUltimate.isFull())
        {
            this.GetComponentInChildren<Image>().color = Color.red;
        }
    }
    public void SetValue(int value)
    {
        ultimateBar.value = value;
    }
}