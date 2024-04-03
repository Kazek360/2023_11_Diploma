using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject SummaryDisplay;
    [SerializeField] private GameObject UpperScreen;
    [SerializeField] private GameObject LowerScreen;
    [FormerlySerializedAs("abilitiesScrollContent")] [SerializeField] private GameObject AbilitiesScrollContent;
    [FormerlySerializedAs("abilityCardPrefab")] [SerializeField] private GameObject AbilityCardPrefab;
    [FormerlySerializedAs("entityManager")] [SerializeField] private GameObject EntityManager;
    [FormerlySerializedAs("movementCounter")] [SerializeField] private GameObject MovementCounter;
    [FormerlySerializedAs("actionCounter")] [SerializeField] private GameObject ActionCounter;
    [FormerlySerializedAs("endTurnButton")] [SerializeField] private GameObject EndTurnButton;
    [FormerlySerializedAs("materialToggle")] [SerializeField] private Toggle MaterialToggle;
    [FormerlySerializedAs("immaterialToggle")] [SerializeField] private Toggle ImmaterialToggle;
    private EntityManager _entityManager;
    private CombatSystem_2 _combatSystem2;
    private ToggleGroup _abilitiesToggleGroup;

    private TextMeshProUGUI _movementCounter, _actionCounter;

    private PlayerScript _currentEntity;

    private List<IHittable> _possibleTargets;
    private IHittable _currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        _possibleTargets = new List<IHittable>();
        _entityManager = EntityManager.GetComponent<EntityManager>();
        _combatSystem2 = EntityManager.GetComponent<CombatSystem_2>();
        _abilitiesToggleGroup = AbilitiesScrollContent.GetComponent<ToggleGroup>();
    }

    /**
     * Called when a player turn begins(enemies don't need it)
     */
    public void ShowCombatUI(IHittable entity)
    {
        AbilitiesScrollContent.SetActive(true);
        MovementCounter.SetActive(true);
        ActionCounter.SetActive(true);
        EndTurnButton.SetActive(true);
        _movementCounter = MovementCounter.GetComponent<TextMeshProUGUI>();
        _actionCounter = ActionCounter.GetComponent<TextMeshProUGUI>();
        _currentEntity = (PlayerScript)entity;
    }

    public void ShowAbilities()
    {
        for (int i = 0; i < _currentEntity.GetAbilities().Count; i++)
        {
            var container = Instantiate(AbilityCardPrefab);
            AbilityCard abilityCard = container.gameObject.GetComponent<AbilityCard>();
            abilityCard.SetAbility(_currentEntity.GetAbilities()[i], _currentEntity.GetCooldowns()[i], i);
            abilityCard.SetController(this);
            container.GetComponent<Toggle>().group = _abilitiesToggleGroup;
            container.transform.SetParent(AbilitiesScrollContent.transform);
            if (_combatSystem2.ActionsLeft <= 0)
            {
                Debug.Log(_combatSystem2.ActionsLeft);
                container.GetComponent<Toggle>().interactable = false;
            }
        }
    }

    
    /**
     * After choosing an ability show possible targets
     *
     * 
     */
    public void ShowTargets(AbilityScriptableObject ability)
    {
        List<IHittable> candidateTargets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObject.TargetTypes.AllyMaterial)
            {
                candidateTargets.AddRange(_entityManager.GetMaterialPlayers());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.EnemyMaterial)
            {
                candidateTargets.AddRange(_entityManager.GetMaterialEnemies());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.AllyImmaterial)
            {
                candidateTargets.AddRange(_entityManager.GetImmaterialPlayers());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.EnemyImmaterial)
            {
                candidateTargets.AddRange(_entityManager.GetImmaterialEnemies());
            }
        }
        _possibleTargets = new List<IHittable>();
        foreach (var target in candidateTargets)
        {
            if (_combatSystem2.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
            {
                _possibleTargets.Add(target);
            }
        }

        foreach (var target in _possibleTargets)
        {
            target.ActivateTargetClickArea();
        }
    }

    private void HideTargets()
    {
        foreach (var target in _possibleTargets)
        {
            target.TargetClickAreaDeselected();
            target.DeactivateTargetClickArea();
        }

        _currentTarget = null;
    }

    /**
     * After turn ends hide interface and destroy children of ability and target lists in order to prevent cloning
     */
    public void DisableCombatUI()
    {
        for (int i = 0; i < AbilitiesScrollContent.transform.childCount; i++) {
            Transform child = AbilitiesScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        AbilitiesScrollContent.SetActive(false);
        MovementCounter.SetActive(false);
        ActionCounter.SetActive(false);
        EndTurnButton.SetActive(false);
    }
    
    public void OnCastAbility()
    {
        var toggled = _abilitiesToggleGroup.GetFirstActiveToggle();
        if (toggled is not null && GetCurrentlySelectedTarget() is not null)
        {
            int selected = toggled.gameObject.GetComponent<AbilityCard>().AbilityOrderInEntityList;
            _currentEntity.CastAbility(selected, GetCurrentlySelectedTarget());
        }
        HideTargets();
        _abilitiesToggleGroup.SetAllTogglesOff();
        DisableCombatUI();
        _combatSystem2.ActionsLeft--;
        ShowCombatUI(_currentEntity);
        ShowAbilities();
        SetActionCounter(_combatSystem2.ActionsLeft);
    }

    /**
     * When ability is selected, populate target list
     */
    public void OnNewAbilitySelected()
    {
        HideTargets();
        var ability = GetCurrentlySelectedAbility();
        if (ability is not null)
        {
            ShowTargets(ability);
        }
    }

    
    private IHittable GetCurrentlySelectedTarget()
    {
        return _currentTarget;
    }


    private AbilityScriptableObject GetCurrentlySelectedAbility()
    {
        var toggled = _abilitiesToggleGroup.GetFirstActiveToggle();
        if (toggled is not null)
        {
            return toggled.gameObject.GetComponent<AbilityCard>().GetAbility;
        }
        return null;
    }

    public void SetMovementCounter(int i)
    {
        //Movement deselects targets in order to avoid choosing a target and then moving beyond
        //the intended range
        HideTargets();  
        
        _abilitiesToggleGroup.SetAllTogglesOff();
        _movementCounter.text = "" + i;
    }
    
    public void SetActionCounter(int i)
    {
        _actionCounter.text = "" + i;
    }

    public void SetTarget(IHittable target)
    {
        if (_currentTarget is not null)
        {
            _currentTarget.TargetClickAreaDeselected();
        }
        _currentTarget = target;
        OnCastAbility();
    }

    public void ToggleMaterial()
    {
        _combatSystem2.ShowStage(true);
    }
    
    public void ToggleImmaterial()
    {
        _combatSystem2.ShowStage(false);
    }

    public void SetMaterialToggle(bool isMaterial)
    {
        MaterialToggle.SetIsOnWithoutNotify(isMaterial);
        ImmaterialToggle.SetIsOnWithoutNotify(!isMaterial);
    }

    public void ShowCombatSummary(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
    {
        UpperScreen.SetActive(false);
        LowerScreen.SetActive(false);
        SummaryDisplay.GetComponent<CombatSummaryScript>().SetValues(IsVictory, players, enemies);
        SummaryDisplay.SetActive(true);
    }
}
