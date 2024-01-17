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

    public void ModifyHealth(int amount)
    {
        _currentHealth = _currentHealth + amount;
    }

    public int GetCurrentPsyche()
    {
        return _currentPsyche;
    }

    public int GetMaxPsyche()
    {
        return maxPsychePoints;
    }

    public void ModifyPsyche(int amount)
    {
        _currentPsyche = _currentPsyche + amount;
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
