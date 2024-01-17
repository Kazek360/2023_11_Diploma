using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IHittable
{
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxPsychePoints;

    private int _currentHealth;
    private int _currentPsyche;
    private int _armor;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealthPoints;
        _currentPsyche = maxPsychePoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealthPoints;
    }

    public void DamageHP(int dmg)
    {
        var reducedDmg = (int)(dmg * Math.Pow(0.8f, _armor));  //Reducing damage received 
        _currentHealth = Math.Max(_currentHealth - reducedDmg, 0);
    }

    public int GetCurrentPsyche()
    {
        return _currentPsyche;
    }

    public int GetMaxPsyche()
    {
        return maxPsychePoints;
    }

    public void DamagePsyche(int dmg)
    {
        _currentPsyche = Math.Max(_currentPsyche - dmg, 0);
    }

    public void OnHealthDepleted()
    {
        Debug.Log( "Wit has lost all health");
    }

    public void OnPsycheDepleted()
    {
        Debug.Log("Wit has lost all psyche");
    }
}
