using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Enemy script with only the most basic of functionalities. In case of need it may be extended by another class.
 * TODO: if such a thing shall be done, make sure to change everything private in here to protected
 */
public class EnemyScript : MonoBehaviour, IHittable
{
    [SerializeField] private GameObject partner;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private TargetClickArea targetClickArea;
    [SerializeField] private TextMeshPro hpText;
    [SerializeField] private bool material;
    protected IHittable _partner;
    
    private List<AbilityScriptableObject> _abilities;
    private List<int> _cooldowns;
    private UnitCombat _unitCombat;

    private CombatSystem_2 _combatSystem2;
    private CombatUI _combatUI;
    private EntityManager _entityManager;

    private IHittable _currentTarget;

    private int _currentHealth;
    private int _currentPsyche;
    private bool _isDead;
    
    // Start is called before the first frame update
    private void Start()
    {
        _isDead = false;
        _unitCombat = GetComponent<UnitCombat>();
        _currentHealth = stats.MaxHealthPoints;
        _currentPsyche = stats.MaxPsychePoints;
        _combatSystem2 = gameObject.GetComponentInParent<CombatSystem_2>();
        _entityManager = gameObject.GetComponentInParent<EntityManager>();
        if (IsMaterial())
        {
            _partner = partner.GetComponent<IHittable>();
            _partner.SetPartnerEntity(this);
        }
        _combatUI = _combatSystem2.GetCombatUI();
        _abilities = stats.Abilities;
        _cooldowns = new List<int>();
        _currentTarget = gameObject.GetComponentInParent<EntityManager>().GetMaterialPlayers()[0];
        for (int i = 0; i < _abilities.Count; i++)
        {
            //Debug.Log("ADDED");
            _cooldowns.Add(0);
        }
        UpdateHpText();
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
        _combatSystem2.MovementStage(stats.Agility, stats.PreferredRange, _currentTarget.GetUnitCombatScript(), this);
    }

    /**
     * For enemies ProcessTurn is split into starting movement and turn after finishing movement
     * in order to prevent simultaneous execution of both
     */
    public void TurnAfterMovement()
    {
        int chosenAbility = ChoosePossibleAbility();
        if (chosenAbility != -1)
        {
            CastAbility(chosenAbility);
        }

        for (int i = 0; i < _cooldowns.Count; i++)
        {
            _cooldowns[i] = Math.Max(0, _cooldowns[i] - 1);
        }
        _entityManager.NextTurn();
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
        List<IHittable> targetCandidates = new List<IHittable>();
        List<IHittable> possibleTargets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObject.TargetTypes.AllyMaterial)
            {
                targetCandidates.AddRange(_entityManager.GetMaterialEnemies());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.EnemyMaterial)
            {
                targetCandidates.AddRange(_entityManager.GetMaterialPlayers());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.EnemyImmaterial)
            {
                targetCandidates.AddRange(_entityManager.GetImmaterialPlayers());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.AllyImmaterial)
            {
                targetCandidates.AddRange(_entityManager.GetImmaterialEnemies());
            }
        }

        foreach (var target in targetCandidates)
        {
            if (_combatSystem2.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
            {
                possibleTargets.Add(target);
            }
        }

        if (possibleTargets.Count > 0)
        {
            int chosenTarget = Random.Range(0, possibleTargets.Count);
            _cooldowns[i] = ability.Cooldown;
            ability.CastAbility(possibleTargets[chosenTarget], this);
        }
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
        if (_partner is null && amount < 0)
        {
            amount *= 2;
        }
        _currentHealth = _currentHealth + amount;
        if (_currentHealth <= 0)
        {
            OnHealthDepleted();
        }
        UpdateHpText();
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

    public virtual void OnHealthDepleted()
    {
        _isDead = true;
        if (_partner is not null)
        {
            _partner.OnPartnerDeath();
        }
        if (IsMaterial())
        {
            _entityManager.OnEnemyDefeated();
        }
        _combatSystem2.EnemyDefeated(_unitCombat, IsMaterial());
        Debug.Log(stats.EnemyName + " has lost all health");
        gameObject.SetActive(false);
    }

    public virtual void OnPartnerDeath()
    {
        _partner = null;
    }

    public void OnPsycheDepleted()
    {
        Debug.Log(stats.EnemyName + " has lost all psyche");
    }
    
    public int GetStrength()
    {
        return stats.Strength;
    }

    public int GetAgility()
    {
        return stats.Agility;
    }

    public int GetLuck()
    {
        return stats.Luck;
    }

    public int GetImagination()
    {
        return stats.Imagination;
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

    public GameObject GetObject()
    {
        return gameObject;
    }

    public UnitCombat GetUnitCombatScript()
    {
        return _unitCombat;
    }
    
    public void ActivateTargetClickArea()
    {
        targetClickArea.EnableClick(_combatUI, this);
    }

    public void DeactivateTargetClickArea()
    {
        targetClickArea.DisableClick();
    }

    public void TargetClickAreaDeselected()
    {
        targetClickArea.Deselect();
    }

    private void UpdateHpText()
    {
        hpText.text = "" + _currentHealth;
    }

    public virtual bool IsMaterial()
    {
        return material;
    }
    
    public IHittable GetPartner()
    {
        return _partner;
    }
    
    public void Activate(bool setActive)
    {
        if (!setActive || !_isDead)
        {
            gameObject.SetActive(setActive);
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetPartnerEntity(IHittable partner)
    {
        _partner = partner;
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
