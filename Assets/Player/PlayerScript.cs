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
    
    private int _currentPsyche;
    protected bool _partnerIsKilled;
    private bool _isVisible;
    
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
        if (IsMaterial() && partner is not null)
        {
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

    public virtual void Setup()
    {
        
    }

    /**
     * Cast an ability upon a target
     *
     * <param name="i">Position of ability in the ability list</param>
     * <param name="target">Intended target entity</param>
     */
    public void CastAbility(int i, IHittable target)
    {
        var ability = abilities[i];
        _cooldowns[i] = ability.Cooldown;
        ability.CastAbility(target, this);
    }
    
    
    /**
     * Prepare player for a turn and begin it
     */
    public void ProcessTurn()
    {
        for (int i = 0; i < _cooldowns.Count; i++)
        {
            _cooldowns[i] = Math.Max(0, _cooldowns[i] - 1);
        }
        _combatUI.ShowCombatUI(this);
        _combatSystem.PlayerTurnStart(GetAgility(), GetImagination(), IsMaterial());
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
        if (PlayerStats.GetInstance().GetWitHp() <= 0)
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
        return PlayerStats.GetInstance().GetStat(StatType.WitStrength);
    }

    public virtual int GetAgility()
    {
        return PlayerStats.GetInstance().GetStat(StatType.WitAgility);
    }

    public virtual int GetLuck()
    {
        return PlayerStats.GetInstance().GetStat(StatType.WitLuck);
    }

    public virtual int GetImagination()
    {
        return PlayerStats.GetInstance().GetStat(StatType.WitImagination);
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
}
