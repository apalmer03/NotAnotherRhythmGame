using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{
    public int currValue = 0;
    public int maxValue = 100;
    public UltimateBar ultimateBar;
    // Start is called before the first frame update
    
    void Start()
    {
        currValue = 0;   
    }

    public void fillBar(int value, int multiplier)
    {
        currValue += damage;
        ultimateBar.SetValue(currValue);
    }

    public void resetBar()
    {
        currValue = 0;
        ultimateBar.SetValue(0);
    }
    
}