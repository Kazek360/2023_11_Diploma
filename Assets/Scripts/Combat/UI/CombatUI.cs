using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/**
 * Controller for combat UI
 */
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
    private CombatSystem _combatSystem;
    private ToggleGroup _abilitiesToggleGroup;
    [SerializeField] private TurnQueue _turnQueue;

    private TextMeshProUGUI _movementCounter, _actionCounter;

    private PlayerScript _currentEntity;

    private List<IHittable> _possibleTargets;
    private IHittable _currentTarget;

    public void OnEnable()
    {
        GameConfig.canvas = GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _possibleTargets = new List<IHittable>();
        _entityManager = EntityManager.GetComponent<EntityManager>();
        _combatSystem = EntityManager.GetComponent<CombatSystem>();
        _abilitiesToggleGroup = AbilitiesScrollContent.GetComponent<ToggleGroup>();
    }

    /**
    * Displays UI for the player
    * <param name='entity'>currently active player</param>
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

    /**
    * Populate UI with ability cards
    */
    public void ShowAbilities()
    {
        for (int i = 0; i < _currentEntity.GetAbilities().Count; i++)
        {
            var container = Instantiate(AbilityCardPrefab, AbilitiesScrollContent.transform);
            AbilityCard abilityCard = container.gameObject.GetComponent<AbilityCard>();
            abilityCard.SetAbility(_currentEntity.GetAbilities()[i], _currentEntity.GetCooldowns()[i], i);
            abilityCard.SetController(this);
            container.GetComponent<Toggle>().group = _abilitiesToggleGroup;
            if (_combatSystem.ActionsLeft <= 0)
            {
                container.GetComponent<Toggle>().interactable = false;
            }
        }
    }

    
    /**
    * Show targets and range for the selected ability
    * <param name='ability'>selected ability</param>
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
            if (_combatSystem.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
            {
                _possibleTargets.Add(target);
            }
        }

        foreach (var target in _possibleTargets)
        {
            target.ActivateTargetClickArea();
        }
        
        _combatSystem.ClearGrid();
        
        if (ImmaterialToggle.isOn && (ability.ViableTargets.Contains(AbilityScriptableObject.TargetTypes.AllyImmaterial) || ability.ViableTargets.Contains(AbilityScriptableObject.TargetTypes.EnemyImmaterial)))
        {
            _combatSystem.UnityTargetingHighlight(ability.Range);
        } else if (MaterialToggle.isOn && (ability.ViableTargets.Contains(AbilityScriptableObject.TargetTypes.AllyMaterial) || ability.ViableTargets.Contains(AbilityScriptableObject.TargetTypes.EnemyMaterial)))
        {
            _combatSystem.UnityTargetingHighlight(ability.Range);
        }
        
        //TODO hide
    }

    /**
    * Hide target indicators and clear grid
    */
    private void HideTargets()
    {
        foreach (var target in _possibleTargets)
        {
            target.TargetClickAreaDeselected();
            target.DeactivateTargetClickArea();
        }

        _combatSystem.ClearGrid();
        if (MaterialToggle.isOn && _combatSystem.GetCurrentIsMaterial() || ImmaterialToggle.isOn && !_combatSystem.GetCurrentIsMaterial())
        {
            _combatSystem.ReturnToMovement();
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
    
    /**
    * Re-enable normal UI after casting an ability
    */
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
        _combatSystem.ActionsLeft--;
        ShowCombatUI(_currentEntity);
        ShowAbilities();
        
        SetActionCounter(_combatSystem.ActionsLeft);
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

    /**
    * Reset UI and switch to material layer
    */
    public void ToggleMaterial()
    {
        HideTargets();
        _abilitiesToggleGroup.SetAllTogglesOff();
        _combatSystem.ShowStage(true);
    }
    /**
    * Reset UI and switch to immaterial layer
    */
    public void ToggleImmaterial()
    {
        HideTargets();
        _abilitiesToggleGroup.SetAllTogglesOff();
        _combatSystem.ShowStage(false);
    }

    /**
    * Set material toggle without notifying connected methods
     *
     * <param name="isMaterial">Is the layer the toggle is set to material</param>
    */
    public void SetMaterialToggle(bool isMaterial)
    {
        MaterialToggle.SetIsOnWithoutNotify(isMaterial);
        ImmaterialToggle.SetIsOnWithoutNotify(!isMaterial);
    }

    /**
    * Disable normal UI and display summary of combat
     * <param name="IsVictory">Whether player has won</param>
     * <param name="players">List of all players in combat</param>
     * <param name="enemies">List of all enemies in combat</param>
    */
    public void ShowCombatSummary(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
    {
        UpperScreen.SetActive(false);
        LowerScreen.SetActive(false);
        SummaryDisplay.GetComponent<CombatSummaryScript>().SetValues(IsVictory, players, enemies);
        SummaryDisplay.SetActive(true);
    }

    public void FillQueue(List<IHittable> entities)
    {
        _turnQueue.Initialize();
        foreach (var entity in entities)
        {
            _turnQueue.AddTurnIcon(entity);
        }
    }
    
    public void ActivateEntityInQueue(int place)
    {
        _turnQueue.SetActiveEntity(place);
    }

    public void DisableDeadEntityInQueue(int n)
    {
        _turnQueue.DisableEntity(n);
    }
}
