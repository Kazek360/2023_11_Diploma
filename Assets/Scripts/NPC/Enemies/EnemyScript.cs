using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Enemy script with only the most basic of functionalities. In case of need it may be extended by another class.
 * TODO: if such a thing shall be done, make sure to change everything private in here to protected
 */
public class EnemyScript : MonoBehaviour, IHittable
{
    [SerializeField] private EnemyStats stats;
    
    [SerializeField] private int strength;
    [SerializeField] private int agility;
    [SerializeField] private int luck;
    [SerializeField] private int imagination;
    
    private List<AbilityScriptableObject> _abilities;
    private List<int> _cooldowns;

    private int _currentHealth;
    private int _currentPsyche;
    
    // Start is called before the first frame update
    private void Start()
    {
        _currentHealth = stats.MaxHealthPoints;
        _currentPsyche = stats.MaxPsychePoints;
        _abilities = stats.Abilities;
        _cooldowns = new List<int>();
        for (int i = 0; i < _abilities.Count; i++)
        {
            //Debug.Log("ADDED");
            _cooldowns.Add(0);
        }
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessTurn()
    {
        Debug.Log(this.GetEntityName() + " TURN");
        int chosenAbility = ChoosePossibleAbility();
        if (chosenAbility != -1)
        {
            CastAbility(chosenAbility);
        }

        for (int i = 0; i < _cooldowns.Count; i++)
        {
            _cooldowns[i] = Math.Max(0, _cooldowns[i] - 1);
        }
        gameObject.GetComponentInParent<EntityManager>().NextTurn();
    }

    /**
     * Returns a random ability from among those possible to be cast according to cooldown
     *
     * TODO: Add distance check when hextiles added
     */
    private int ChoosePossibleAbility()
    {
        int maxWeight = 0;
        List<AbilityScriptableObject> candidates = new List<AbilityScriptableObject>();
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_cooldowns[i] > 0) continue;
            maxWeight += _abilities[i].GetAbilityWeight(this);
            candidates.Add(_abilities[i]);
        }

        
        int result = candidates.Count - 1;
        //Debug.Log(result);
        int weightChosen = Random.Range(1, maxWeight + 1);
        int weightCurrent = 0;
        for (int i = 0; i < candidates.Count; i++)
        {
            if (weightCurrent > weightChosen)
            {
                result = i - 1;
                break;
            }
            weightCurrent += candidates[i].GetAbilityWeight(this);
        }
        //Debug.Log(result);
        if (result < 0)
        {
            return -1;
        }
        return _abilities.IndexOf(candidates[result]);
    }
    
    
    /**
     * Casts an ability to a random target from among viable ones
     *
     * i - index of the chosen ability
     */
    private void CastAbility(int i)
    {
        var ability = _abilities[i];
        List<IHittable> possibleTargets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObject.TargetTypes.Ally)
            {
                possibleTargets.AddRange(gameObject.GetComponentInParent<EntityManager>().GetEnemies());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.Enemy)
            {
                possibleTargets.AddRange(gameObject.GetComponentInParent<EntityManager>().GetPlayers());
            }
        }
        

        int chosenTarget = Random.Range(0, possibleTargets.Count);
        _cooldowns[i] = ability.Cooldown;
        ability.CastAbility(possibleTargets[chosenTarget], this);
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
        if (_currentHealth == 0)
        {
            OnHealthDepleted();
        }
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
        if (_currentPsyche == 0)
        {
            OnPsycheDepleted();
        }
    }

    public void OnHealthDepleted()
    {
        Debug.Log(stats.EnemyName + " has lost all health");
    }

    public void OnPsycheDepleted()
    {
        Debug.Log(stats.EnemyName + " has lost all psyche");
    }
    
    public int GetStrength()
    {
        return strength;
    }

    public int GetAgility()
    {
        return agility;
    }

    public int GetLuck()
    {
        return luck;
    }

    public int GetImagination()
    {
        return imagination;
    }
    
    public string GetEntityName()
    {
        return stats.EnemyName;
    }
    
    public List<AbilityScriptableObject> GetAbilities()
    {
        return _abilities;
    }

    public List<int> GetCooldowns()
    {
        return _cooldowns;
    }

    public Sprite GetImage()
    {
        return stats.EnemyImage;
    }
}
