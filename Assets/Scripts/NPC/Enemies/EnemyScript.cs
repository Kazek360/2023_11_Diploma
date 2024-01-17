using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Enemy script with only the most basic of functionalities. In case of need it may be extended by another class.
 * TODO: if such a thing shall be done, make sure to change everything private in here to protected
 */
public class EnemyScript : MonoBehaviour, IHittable
{
    [SerializeField] private EnemyStats stats;

    private int _currentHealth;
    private int _currentPsyche;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = stats.MaxHealthPoints;
        _currentPsyche = stats.MaxPsychePoints;
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
        return stats.MaxHealthPoints;
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
        return stats.MaxPsychePoints;
    }

    public void ModifyPsyche(int amount)
    {
        _currentPsyche = _currentPsyche + amount;
    }

    public void OnHealthDepleted()
    {
        Debug.Log(stats.Name + " has lost all health");
    }

    public void OnPsycheDepleted()
    {
        Debug.Log(stats.Name + " has lost all psyche");
    }
}
