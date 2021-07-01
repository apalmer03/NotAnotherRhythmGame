using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ultimate : MonoBehaviour
{
    public int currValue = 0;
    public int maxValue = 300;
    public UltimateBar ultimateBar;
    // Start is called before the first frame update
    
    void Start()
    {
        currValue = 0;   
    }

    public void fillBar(int value, int multiplier)
    {
        currValue += value * multiplier;
        if (currValue >= maxValue)
        {
            currValue = maxValue;
        }
        ultimateBar.SetValue(currValue);
    }

    public bool isFull()
    {
        if (currValue == maxValue)
        {
            return true;
        }

        return false;
    }

    public void resetBar()
    {
        currValue = 0;
        ultimateBar.SetValue(0);
        ultimateBar.GetComponentInChildren<Image>().color = Color.yellow;
    }
    
}