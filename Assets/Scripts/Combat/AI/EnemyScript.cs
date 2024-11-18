using System;
using System.Collections.Generic;
using System.Text;
using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/**
 * Enemy script with only the most basic of functionalities. In case of need it may be extended by another class.
 */
public class EnemyScript : MonoBehaviour, IHittable, ITooltippable
{
    [FormerlySerializedAs("partner")] [SerializeField] private GameObject Partner;
    [FormerlySerializedAs("stats")] [SerializeField] private EnemyStats Stats;
    [FormerlySerializedAs("targetClickArea")] [SerializeField] private TargetClickArea TargetClickArea;
    [FormerlySerializedAs("hpText")] [SerializeField] private TextMeshPro HpText;
    [FormerlySerializedAs("material")] [SerializeField] private bool Material;
    [SerializeField] private TooltipObject tooltipArea;
    protected IHittable _partner;
    
    private List<AbilityScriptableObjectReworked> _abilities;
    private List<int> _cooldowns;
    private UnitCombat _unitCombat;

    private CombatSystem _combatSystem;
    private CombatUI _combatUI;
    private EntityManager _entityManager;
    private SpriteRenderer _spriteRenderer;

    private IHittable _currentTarget;

    protected int _currentHealth;
    private int _currentPsyche;
    private bool _isDead;
    private bool _partnerIsKilled;
    private bool _isVisible;
    private List<EffectOverTime> _effectsOverTime;
    
    // Start is called before the first frame update
    private void Start()
    {
        _isDead = false;
        _unitCombat = GetComponent<UnitCombat>();
        _currentHealth = Stats.MaxHealthPoints;
        _currentPsyche = Stats.MaxPsychePoints;
        _combatSystem = gameObject.GetComponentInParent<CombatSystem>();
        _entityManager = gameObject.GetComponentInParent<EntityManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (IsMaterial() && Partner is not null)
        {
            _partner = Partner.GetComponent<IHittable>();
            _partner.SetPartnerEntity(this);
        }
        _combatUI = _combatSystem.GetCombatUI();
        _abilities = Stats.Abilities;
        _cooldowns = new List<int>();
        _effectsOverTime = new List<EffectOverTime>();
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

    /**
     * Log beginning of an entity turn and start turn logic for it
     */
    public void ProcessTurn()
    {
        Debug.Log(this.GetEntityName() + " TURN");
        _combatSystem.MovementStage(Stats.Agility, Stats.PreferredRange, _currentTarget.GetUnitCombatScript(), this);
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
    }

    /**
     * Returns a random ability from among those possible to be cast according to cooldown
     */
    private int ChoosePossibleAbility()
    {
        int maxWeight = 0;
        List<AbilityScriptableObjectReworked> candidates = new List<AbilityScriptableObjectReworked>();
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_cooldowns[i] > 0) continue;
            maxWeight += _abilities[i].GetAbilityWeight(this);
            candidates.Add(_abilities[i]);
        }

        
        int result = candidates.Count - 1;
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
     * <param name="i">index of the chosen ability</param>
     */
    private void CastAbility(int i)
    {
        var ability = _abilities[i];
        List<IHittable> targetCandidates = new List<IHittable>();
        List<IHittable> possibleTargets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.AllyMaterial)
            {
                targetCandidates.AddRange(_entityManager.GetMaterialEnemies());
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.EnemyMaterial)
            {
                targetCandidates.AddRange(_entityManager.GetMaterialPlayers());
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.EnemyImmaterial)
            {
                targetCandidates.AddRange(_entityManager.GetImmaterialPlayers());
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.AllyImmaterial)
            {
                targetCandidates.AddRange(_entityManager.GetImmaterialEnemies());
            }
        }

        foreach (var target in targetCandidates)
        {
            if (_combatSystem.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
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
        return Stats.MaxHealthPoints;
    }

    public void ModifyHealth(int amount)
    {
        if (_partnerIsKilled && amount < 0)
        {
            amount *= 2;
        }
        _currentHealth = Math.Max(Math.Min(_currentHealth + amount, Stats.MaxHealthPoints), 0);
        if (_currentHealth <= 0 && !this.IsDead())
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
        return Stats.MaxPsychePoints;
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
        _combatSystem.EnemyDefeated(_unitCombat, IsMaterial());
        Debug.Log(Stats.EnemyName + " has lost all health");
        gameObject.SetActive(false);
    }

    public virtual void OnPartnerDeath()
    {
        _partnerIsKilled = true;
        _partner = null;
    }

    public void OnPsycheDepleted()
    {
        Debug.Log(Stats.EnemyName + " has lost all psyche");
    }
    
    public int GetStrength()
    {
        return Stats.Strength;
    }

    public int GetAgility()
    {
        return Stats.Agility;
    }

    public int GetLuck()
    {
        return Stats.Luck;
    }

    public int GetImagination()
    {
        return Stats.Imagination;
    }
    
    public string GetEntityName()
    {
        return Stats.EnemyName;
    }
    
    public List<AbilityScriptableObjectReworked> GetAbilities()
    {
        return _abilities;
    }

    public List<int> GetCooldowns()
    {
        return _cooldowns;
    }

    public Sprite GetImage()
    {
        return Stats.EnemyImage;
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
        if (tooltipArea.gameObject.activeInHierarchy)
        {
            tooltipArea.gameObject.SetActive(false);
            TargetClickArea.EnableClick(_combatUI, this);
        }
    }

    public void DeactivateTargetClickArea()
    {
        if (_isVisible)
        {
            tooltipArea.gameObject.SetActive(true);
            TargetClickArea.DisableClick();
        }
    }

    public void TargetClickAreaDeselected()
    {
        if (_isVisible)
        {
            tooltipArea.gameObject.SetActive(true);
            TargetClickArea.DisableClick();
        }
    }

    private void UpdateHpText()
    {
        HpText.text = "" + _currentHealth;
    }

    public virtual bool IsMaterial()
    {
        return Material;
    }
    
    public IHittable GetPartner()
    {
        return _partner;
    }
    
    public void Activate(bool setActive)
    {
        if (_currentHealth < 0 || _isDead)
        {
            gameObject.SetActive(false);
            return;
        }
        if (setActive)
        {
            _isVisible = true;
            HpText.gameObject.SetActive(true);
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
            HpText.gameObject.SetActive(false);
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

    public bool IsDead()
    {
        return _isDead;
    }

    public string GetTooltipHeader()
    {
        return Stats.EnemyName;
    }

    public string GetTooltipDesc()
    {
        StringBuilder str = new StringBuilder();
        str.Append("Życie: " + _currentHealth + "/" + Stats.MaxHealthPoints);
        return str.ToString();
    }
    
    public void ApplyOverTimeEffect(EffectOverTime effectOverTime)
    {
        Debug.Log("TIMED EFFECT APPLIED");
        _effectsOverTime.Add(effectOverTime);
    }
}
