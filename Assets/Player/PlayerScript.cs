using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IHittable
{
    [SerializeField] private GameObject partner;
    [SerializeField] private Sprite playerImage;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxPsychePoints;
    protected IHittable _partner;

    [SerializeField] private string entityName;
    [SerializeField] private int strength;
    [SerializeField] private int agility;
    [SerializeField] private int luck;
    [SerializeField] private int imagination;
    
    [SerializeField] private List<AbilityScriptableObject> abilities;
    private List<int> _cooldowns;
    private CombatSystem_2 _combatSystem2;
    private CombatUI _combatUI;
    private UnitCombat _unitCombat;

    [SerializeField] private TargetClickArea targetClickArea;
    [SerializeField] private TextMeshPro hpText;
    
    private int _currentHealth;
    private int _currentPsyche;
    
    // Start is called before the first frame update
    void Start()
    {
        _unitCombat = GetComponent<UnitCombat>();
        _currentHealth = maxHealthPoints;
        _currentPsyche = maxPsychePoints;
        _combatSystem2 = gameObject.GetComponentInParent<CombatSystem_2>();
        _combatUI = _combatSystem2.GetCombatUI();
        _cooldowns = new List<int>();
        if (IsMaterial())
        {
            Debug.Log(IsMaterial());
            _partner = partner.GetComponent<IHittable>();
            _partner.SetPartnerEntity(this);
        }
        for (int i = 0; i < abilities.Count; i++)
        {
            _cooldowns.Add(0);
        }
        UpdateHpText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastAbility(int i, IHittable target)
    {
        var ability = abilities[i];
        _cooldowns[i] = ability.Cooldown;
        ability.CastAbility(target, this);
        Debug.Log("A");
    }
    
    

    public void ProcessTurn()
    {
        for (int i = 0; i < _cooldowns.Count; i++)
        {
            _cooldowns[i] = Math.Max(0, _cooldowns[i] - 1);
        }
        _combatUI.ShowCombatUI(this);
        _combatSystem2.PlayerTurnStart(agility, imagination, IsMaterial());
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
        //Debug.Log("HP CHANGED BY " + amount);
        if (_partner is null && amount < 0)
        {
            amount *= 2;
        }
        _currentHealth = _currentHealth + amount;
        UpdateHpText();
        if (_currentHealth <= 0)
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
        return maxPsychePoints;
    }

    public void ModifyPsyche(int amount)
    {
        //Debug.Log("PP CHANGED BY " + amount);
        _currentPsyche = _currentPsyche + amount;
        if (_currentPsyche <= 0)
        {
            OnPsycheDepleted();
        }
    }

    public virtual void OnHealthDepleted()
    {
        Debug.Log( "Wit has lost all health");
        GetComponentInParent<EntityManager>().PlayerDied();
        //gameObject.SetActive(false);
    }

    public virtual void OnPartnerDeath()
    {
        _partner = null;
    }

    public void OnPsycheDepleted()
    {
        Debug.Log("Wit has lost all psyche");
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
        return entityName;
    }

    public List<AbilityScriptableObject> GetAbilities()
    {
        return abilities;
    }
    
    public List<int> GetCooldowns()
    {
        return _cooldowns;
    }

    public Sprite GetImage()
    {
        return playerImage;
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
        return true;
    }

    public IHittable GetPartner()
    {
        return _partner;
    }

    public void Activate(bool setActive)
    {
        gameObject.SetActive(setActive);
    }
    
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetPartnerEntity(IHittable partner)
    {
        _partner = partner;
    }
    
    public virtual bool IsDead()
    {
        return false;
    }
}
