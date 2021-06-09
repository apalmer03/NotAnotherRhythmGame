using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int currHealth = 0;
    public int maxHealth = 100;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;   
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         DamagePlayer(10);
    //     }
    // }

    public void DamagePlayer(int damage)

    {
        currHealth -= damage;
        healthBar.SetHealth(currHealth);
    }

    public void HealPlayer(int heal)
    {
        if(currHealth + heal > maxHealth)
        {
            currHealth = maxHealth;
            healthBar.SetHealth(currHealth);
        }
        else
        {
            currHealth += heal;
            healthBar.SetHealth(currHealth);
        }
    }
}
