using System;
using System.Collections.Generic;
using System.Text;
using Combat;
using TMPro;
using UnityEngine;

/**
 * A script managing behaviour of a physical player in combat
 */
public class PlayerScript : MonoBehaviour, IHittable, ITooltippable
{
    [SerializeField] private UnlockableAbility.PlayerType playerType;
    [SerializeField] private GameObject partner;
    [SerializeField] private Sprite playerImage;
    [SerializeField] private int maxPsychePoints;
    protected IHittable _partner;

    [SerializeField] private string entityName;
    
    [SerializeField] private List<AbilityScriptableObjectReworked> abilities;
    private List<int> _cooldowns;
    private CombatSystem _combatSystem;
    private CombatUI _combatUI;
    private UnitCombat _unitCombat;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private TargetClickArea targetClickArea;
    [SerializeField] private TooltipObject tooltipArea;
    [SerializeField] protected TextMeshPro hpText;
    [SerializeField] protected SpriteRenderer healthBarSprite;
    
    private int _currentPsyche;
    protected bool _partnerIsKilled;
    private bool _isVisible;
    private List<EffectOverTime> _effectsOverTime;
    private List<TimedCombatModifier> _combatModifiers;
    
    // Start is called before the first frame update
    void Start()
    {
        _unitCombat = GetComponent<UnitCombat>();
        _currentPsyche = maxPsychePoints;
        _combatSystem = gameObject.GetComponentInParent<CombatSystem>();
        _combatUI = _combatSystem.GetCombatUI();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UnlockedAbilities.GetInstance();
        if (UnlockedAbilities.GetInstance().NumOfUnlockedAbilities(playerType) == 0)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                UnlockableAbility tmp = new UnlockableAbility();
                tmp.Recipient = playerType;
                tmp.AbilitySO = abilities[i];
                UnlockedAbilities.GetInstance().UnlockAbility(tmp);
            }
        }
        abilities = UnlockedAbilities.GetInstance().GetAbilities(playerType);
        _cooldowns = new List<int>();
        _effectsOverTime = new List<EffectOverTime>();
        _combatModifiers = new List<TimedCombatModifier>();
        // if (IsMaterial() && partner is not null && partner )
        // {
        //     _partner = partner.GetComponent<IHittable>();
        //     _partner.SetPartnerEntity(this);
        // }
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

    public virtual void Setup()
    {
        
    }

    /**
     * Cast an ability upon a target
     *
     * <param name="i">Position of ability in the ability list</param>
     * <param name="target">Intended target entity</param>
     */
    public void CastAbility(int i, List<IHittable> targets)
    {
        var ability = abilities[i];
        _cooldowns[i] = ability.Cooldown;
        ability.CastAbility(targets, this);
    }
    public void CastAbility(int i, List<GridObject> targets)
    {
        var ability = abilities[i];
        _cooldowns[i] = ability.Cooldown;
        ability.CastAbility(targets, this);
    }
    
    
    /**
     * Prepare player for a turn and begin it
     */
    public void ProcessTurn()
    {
        Debug.Log(this.GetEntityName() + " TURN");
        //Debug.Log(_partner.GetEntityName() + " PARTNERED");
        foreach (EffectOverTime effect in _effectsOverTime)
        {
            effect.ProcessTurn(this);
        }
        for (int i = _effectsOverTime.Count - 1; i >= 0; i--)
        {
            if (_effectsOverTime[i].DurationLeft == 0)
            {
                _effectsOverTime.RemoveAt(i);
            }
        }
        
        foreach (TimedCombatModifier modifier in _combatModifiers)
        {
            modifier.ProcessTurn();
        }
        for (int i = _combatModifiers.Count - 1; i >= 0; i--)
        {
            if (_combatModifiers[i].DurationLeft == 0)
            {
                _combatModifiers.RemoveAt(i);
            }
        }

        for (int i = 0; i < _cooldowns.Count; i++)
        {
            _cooldowns[i] = Math.Max(0, _cooldowns[i] - 1);
        }
        _combatUI.ShowCombatUI(this);
        _combatSystem.PlayerTurnStart(GetAgility(), GetImagination(), IsMaterial(), this);
    }


    public virtual int GetCurrentHealth()
    {
        return PlayerStats.GetInstance().GetWitHp();
    }

    public virtual int GetMaxHealth()
    {
        return PlayerStats.GetInstance().GetStat(StatType.WitHealth);
    }

    
    public virtual void ModifyHealth(int amount)
    {
        //Debug.Log("HP CHANGED BY " + amount);
        if (_partnerIsKilled && amount < 0)
        {
            amount *= 2;
        }
        PlayerStats.GetInstance().ModifyWitHealth(amount);
        UpdateHpText();
        healthBarSprite.size = new Vector2(3 * ((float)GetCurrentHealth() / GetMaxHealth()), 0.25f);
        if (PlayerStats.GetInstance().GetWitHp() <= 0 && !this.IsDead())
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

    /**
     * Modify psyche of the player
     *
     * <param name="amount">Numeric change to be applied to player psyche</param>
     */
    public void ModifyPsyche(int amount)
    {
        //Debug.Log("PP CHANGED BY " + amount);
        _currentPsyche = _currentPsyche + amount;
        if (_currentPsyche <= 0)
        {
            OnPsycheDepleted();
        }
    }

    /**
     * Log death of a player and trigger appropriate actions in other scripts
     */
    public virtual void OnHealthDepleted()
    {
        Debug.Log( "Wit has lost all health");
        GetComponentInParent<EntityManager>().PlayerDied();
        gameObject.SetActive(false);
    }

    
    public virtual void OnPartnerDeath()
    {
        _partner = null;
        _partnerIsKilled = true;
    }

    public void OnPsycheDepleted()
    {
        Debug.Log("Wit has lost all psyche");
    }

    public virtual int GetStrength()
    {
        int str = PlayerStats.GetInstance().GetStat(StatType.WitStrength);
        if (_combatModifiers is null)
        {
            return str;
        }
        foreach (var modifier in _combatModifiers)
        {
            if (modifier.ModifiedVar == ModifiedVariable.STRENGTH)
            {
                str = modifier.ModifyVar(str);
            }
        }

        return str;
    }

    public virtual int GetAgility()
    {
        int agi = PlayerStats.GetInstance().GetStat(StatType.WitAgility);
        if (_combatModifiers is null)
        {
            return agi;
        }
        foreach (var modifier in _combatModifiers)
        {
            if (modifier.ModifiedVar == ModifiedVariable.AGILITY)
            {
                agi = modifier.ModifyVar(agi);
            }
        }

        return agi;
    }

    public virtual int GetLuck()
    {
        int lck = PlayerStats.GetInstance().GetStat(StatType.WitLuck);
        if (_combatModifiers is null)
        {
            return lck;
        }
        foreach (var modifier in _combatModifiers)
        {
            if (modifier.ModifiedVar == ModifiedVariable.LUCK)
            {
                lck = modifier.ModifyVar(lck);
            }
        }

        return lck;
    }

    public virtual int GetImagination()
    {
        int img = PlayerStats.GetInstance().GetStat(StatType.WitImagination);
        if (_combatModifiers is null)
        {
            return img;
        }
        foreach (var modifier in _combatModifiers)
        {
            if (modifier.ModifiedVar == ModifiedVariable.IMAGINATION)
            {
                img = modifier.ModifyVar(img);
            }
        }

        return img;
    }

    public string GetEntityName()
    {
        return entityName;
    }

    public List<AbilityScriptableObjectReworked> GetAbilities()
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
        if (_isVisible)
        {
            tooltipArea.gameObject.SetActive(false);
            targetClickArea.EnableClick(_combatUI, this);
        }
    }

    public void DeactivateTargetClickArea()
    {
        if (_isVisible)
        {
            tooltipArea.gameObject.SetActive(true);
            targetClickArea.DisableClick();
        }
    }
    
    public void TargetClickAreaDeselected()
    {
        if (_isVisible)
        {
            tooltipArea.gameObject.SetActive(true);
            targetClickArea.DisableClick();
        }
    }
    
    protected virtual void UpdateHpText()
    {
        hpText.text = "" + PlayerStats.GetInstance().GetWitHp();
    }

    public virtual bool IsMaterial()
    {
        return true;
    }

    public bool IsPlayerControlled()
    {
        return true;
    }

    public IHittable GetPartner()
    {
        return _partner;
    }

    public void Activate(bool setActive)
    {
        if (GetCurrentHealth() < 0 || IsDead())
        {
            gameObject.SetActive(false);
            return;
        }
        if (setActive)
        {
            _isVisible = true;
            hpText.gameObject.SetActive(true);
            tooltipArea.gameObject.SetActive(true);
            healthBarSprite.gameObject.SetActive(true);
            healthBarSprite.gameObject.transform.parent.gameObject.SetActive(true);
            Vector3 pos = gameObject.transform.position;
            pos.Set(pos.x, pos.y, 1);
            Color color = _spriteRenderer.color;
            color.a = 1;
            _spriteRenderer.color = color;
            _spriteRenderer.sortingOrder = 1;
        }
        else
        {
            _isVisible = false;
            hpText.gameObject.SetActive(false);
            tooltipArea.gameObject.SetActive(false);
            healthBarSprite.gameObject.SetActive(false);
            healthBarSprite.gameObject.transform.parent.gameObject.SetActive(false);
            Vector3 pos = gameObject.transform.position;
            pos.Set(pos.x, pos.y, -1);
            Color color = _spriteRenderer.color;
            color.a = 0.4f;
            _spriteRenderer.color = color;
            _spriteRenderer.sortingOrder = 0;
        }
        //gameObject.SetActive(setActive);
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

    public string GetTooltipHeader()
    {
        return GetEntityName();
    }

    public string GetTooltipDesc()
    {
        StringBuilder str = new StringBuilder();
        str.Append("Życie: " + GetCurrentHealth() + "/" + GetMaxHealth());
        return str.ToString();
    }

    public void ApplyOverTimeEffect(EffectOverTime effectOverTime)
    {
        _effectsOverTime.Add(effectOverTime);
    }
    

    public void ApplyTimedCombatModifier(TimedCombatModifier combatModifier)
    {
        _combatModifiers.Add(combatModifier);
    }

    public List<TimedCombatModifier> GetCombatModifiers()
    {
        return _combatModifiers;
    }

    public void OnAbilityCast(int i)
    {
        //TODO: SHIFT IMPLEMENTATION
        throw new NotImplementedException();
    }

    public void AfterTurnEffects()
    {
        //TODO: SHIFT IMPLEMENTATION
        throw new NotImplementedException();
    }
}
