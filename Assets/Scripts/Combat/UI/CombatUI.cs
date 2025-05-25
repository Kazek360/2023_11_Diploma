using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Combat;
using InputSystem;
using TMPro;
using UI;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/**
 * Controller for combat UI
 */
public class CombatUI : UserInterface
{
    public override UserInterfaceState State => UserInterfaceState.Combat;
    
    [SerializeField] private GameObject SummaryDisplay;
    [SerializeField] private GameObject UpperScreen;
    [SerializeField] private GameObject LowerScreen;
    [FormerlySerializedAs("abilitiesScrollContent")] [SerializeField] private GameObject AbilitiesScrollContent;
    [FormerlySerializedAs("abilityCardPrefab")] [SerializeField] private GameObject AbilityCardPrefab;
    [SerializeField] private GameObject ItemsScrollContent;
    [SerializeField] private GameObject ItemCardPrefab;
    [FormerlySerializedAs("entityManager")] [SerializeField] private GameObject EntityManager;
    [FormerlySerializedAs("movementCounter")] [SerializeField] private GameObject MovementCounter;
    [SerializeField] private Image MovementFillImage;
    [FormerlySerializedAs("actionCounter")] [SerializeField] private GameObject ActionCounter;
    [SerializeField] private Image ActionFillImage;
    [SerializeField] private TextMeshProUGUI HealthCounter;
    [SerializeField] private Image HealthFillImage;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private Image CharacterImage;
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
    private GridObject _currentTarget;

    private static bool _isTargeting;
    private static int _isTargetingAreaRadius;
    
    private void Start()
    {
        _isTargeting = false;
        _isTargetingAreaRadius = 0;
        _possibleTargets = new List<IHittable>();
        _entityManager = EntityManager.GetComponent<EntityManager>();
        _combatSystem = EntityManager.GetComponent<CombatSystem>();
        _abilitiesToggleGroup = AbilitiesScrollContent.GetComponent<ToggleGroup>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        InputManager.Instance.Controlls.Player.Disable();
    }
    public override void OnExit()
    {
        base.OnExit();
        InputManager.Instance.Controlls.Player.Enable();
    }
    
    /**
    * Displays UI for the player
    * <param name='entity'>currently active player</param>
    */
    public void ShowCombatUI(IHittable entity)
    {
        AbilitiesScrollContent.SetActive(true);
        ItemsScrollContent.SetActive(true);
        MovementCounter.SetActive(true);
        ActionCounter.SetActive(true);
        HealthCounter.gameObject.SetActive(true);
        CharacterImage.gameObject.SetActive(true);
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
    * Populate UI with item cards
    */
    public void ShowItems()
    {
        //TODO: SHOW ITEM CARDS
        if (_currentEntity.IsMaterial())
        {
            var items = GameConfig.PlayerInventory.GetItems(ItemScriptableObject.ItemType.CONSUMABLE);
            foreach (var itemId in items)
            {
                var itemCard = Instantiate(ItemCardPrefab, ItemsScrollContent.transform);
                ItemCard itemCardScript = itemCard.GetComponent<ItemCard>();
                itemCardScript.SetItem(ItemLibrary.Instance.GetItem(itemId));
                itemCardScript.SetController(this);
                if (_combatSystem.ActionsLeft > 0)
                {
                    itemCard.GetComponent<Button>().interactable = true;
                }
                else
                {
                    itemCard.GetComponent<Button>().interactable = false;
                }
            }
        }
    }
    
    /**
    * Show targets and range for the selected ability
    * <param name='ability'>selected ability</param>
    */
    public void ShowTargets(AbilityScriptableObjectReworked ability)
    {
        _isTargeting = true;
        List<IHittable> candidateTargets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.AllyMaterial)
            {
                candidateTargets.AddRange(_entityManager.GetEntities(true, true));
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.EnemyMaterial)
            {
                candidateTargets.AddRange(_entityManager.GetEntities(false, true));
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.AllyImmaterial)
            {
                candidateTargets.AddRange(_entityManager.GetEntities(true, false));
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.EnemyImmaterial)
            {
                candidateTargets.AddRange(_entityManager.GetEntities(false, false));
            }
        }
        _possibleTargets = new List<IHittable>();
        //List<Tuple<int, int>> targetPositions = new List<Tuple<int, int>>();
        HashSet<(int, int)> targetPositions = new HashSet<(int, int)>();
        foreach (var target in candidateTargets)
        {
            if (_combatSystem.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
            {
                _possibleTargets.Add(target);
                targetPositions.Add(
                        (
                            (int)target.GetUnitCombatScript().GetGridLocation().x, 
                            (int)target.GetUnitCombatScript().GetGridLocation().y
                            )
                    );
            }
        }

        foreach (var target in _possibleTargets)
        {
            target.ActivateTargetClickArea();
        }
        
        _combatSystem.ClearGrid();
        
        if (ImmaterialToggle.isOn && (ability.ViableTargets.Contains(AbilityScriptableObjectReworked.TargetTypes.AllyImmaterial) || ability.ViableTargets.Contains(AbilityScriptableObjectReworked.TargetTypes.EnemyImmaterial)))
        {
            _combatSystem.UnityTargetingHighlight(ability, targetPositions);
        } else if (MaterialToggle.isOn && (ability.ViableTargets.Contains(AbilityScriptableObjectReworked.TargetTypes.AllyMaterial) || ability.ViableTargets.Contains(AbilityScriptableObjectReworked.TargetTypes.EnemyMaterial)))
        {
            _combatSystem.UnityTargetingHighlight(ability, targetPositions);
        }
        
        //TODO hide
    }

    /**
    * Hide target indicators and clear grid
    */
    private void HideTargets()
    {
        _isTargeting = false;
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
        for (int i = 0; i < ItemsScrollContent.transform.childCount; i++) {
            Transform child = ItemsScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        AbilitiesScrollContent.SetActive(false);
        ItemsScrollContent.SetActive(false);
        MovementCounter.SetActive(false);
        ActionCounter.SetActive(false);
        HealthCounter.gameObject.SetActive(false);
        CharacterImage.gameObject.SetActive(false);
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
            
            AbilityScriptableObjectReworked ability = GetCurrentlySelectedAbility();
            if (ability.TargetsGround())
            {
                List<GridObject> validTargets = new List<GridObject>();
                validTargets.Add(GetCurrentlySelectedTarget());
                validTargets.AddRange(_combatSystem.GetCombatEnvironmentManager().GetNeighborsInRadius(GetCurrentlySelectedTarget(), ability.AreaRadius, false));
                _currentEntity.CastAbility(selected, validTargets);
            }
            
            if (ability.TargetsUnits())
            {
                List<IHittable> validTargets = new List<IHittable>();
                List<GridObject> toCheck = _combatSystem.GetCombatEnvironmentManager()
                    .GetNeighborsInRadius(GetCurrentlySelectedTarget(), ability.AreaRadius, true);
                foreach (var gridObject in toCheck)
                {
                    if (ability.TargetsImmaterialAllies &&
                        _combatSystem.GetEntityOnGridObject(gridObject, true, false) is not null)
                    {
                        validTargets.Add(_combatSystem.GetEntityOnGridObject(gridObject, true, false));
                    }

                    if (ability.TargetsImmaterialEnemies &&
                        _combatSystem.GetEntityOnGridObject(gridObject, false, false) is not null)
                    {
                        validTargets.Add(_combatSystem.GetEntityOnGridObject(gridObject, false, false));
                    }

                    if (ability.TargetsMaterialAllies &&
                        _combatSystem.GetEntityOnGridObject(gridObject, true, true) is not null)
                    {
                        validTargets.Add(_combatSystem.GetEntityOnGridObject(gridObject, true, true));
                    }

                    if (ability.TargetsMaterialEnemies &&
                        _combatSystem.GetEntityOnGridObject(gridObject, false, true) is not null)
                    {
                        validTargets.Add(_combatSystem.GetEntityOnGridObject(gridObject, false, true));
                    }
                }
                _currentEntity.CastAbility(selected, validTargets);
            }
        }
        HideTargets();
        _abilitiesToggleGroup.SetAllTogglesOff();
        DisableCombatUI();
        _combatSystem.ActionsLeft--;
        ShowCombatUI(_currentEntity);
        _combatSystem.UpdateUICounters();
        ShowAbilities();
        ShowItems();
        
        SetActionCounter(_combatSystem.ActionsLeft, _combatSystem.StartingActions);
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

    public void OnItemConsumed()
    {
        //todo: IMPLEMENT UI REFRESH
        HideTargets();
        _abilitiesToggleGroup.SetAllTogglesOff();
        DisableCombatUI();
        _combatSystem.ActionsLeft--;
        ShowCombatUI(_currentEntity);
        _combatSystem.UpdateUICounters();
        ShowAbilities();
        ShowItems();
        _entityManager.UpdateEntityHealthbars();
        
        SetActionCounter(_combatSystem.ActionsLeft, _combatSystem.StartingActions);
    }

    
    private GridObject GetCurrentlySelectedTarget()
    {
        return _currentTarget;
    }


    private AbilityScriptableObjectReworked GetCurrentlySelectedAbility()
    {
        var toggled = _abilitiesToggleGroup.GetFirstActiveToggle();
        if (toggled is not null)
        {
            return toggled.gameObject.GetComponent<AbilityCard>().GetAbility;
        }
        return null;
    }

    public void SetMovementCounter(int currMovement, int maxMovement)
    {
        //Movement deselects targets in order to avoid choosing a target and then moving beyond
        //the intended range
        HideTargets();  
        
        _abilitiesToggleGroup.SetAllTogglesOff();
        _movementCounter.text = "Ruchy: " + $"{currMovement}/{maxMovement}" ;
        MovementFillImage.fillAmount = (float)currMovement / maxMovement;
    }
    
    public void SetActionCounter(int currActions, int maxActions)
    {
        _actionCounter.text = "Akcje:\n" + $"{currActions}/{maxActions}";
        ActionFillImage.fillAmount = (float)currActions / maxActions;
    }

    public void SetHealthCounter(int currHealth, int maxHealth)
    {
        HealthCounter.text = "Życie:\n" + $"{currHealth}/{maxHealth}";
        HealthFillImage.fillAmount = (float)currHealth / maxHealth;
    }

    public void SetCharacterDisplay(Sprite charImage, string charName)
    {
        CharacterImage.sprite = charImage;
        CharacterName.text = charName;
    }

    public void SetTarget(GridObject target)
    {
        _currentTarget = target;
        OnCastAbility();
    }

    /**
    * Reset UI and switch to material layer
    */
    public void ToggleMaterial()
    {
        bool tmp = _isTargeting;
        HideTargets();
        //_abilitiesToggleGroup.SetAllTogglesOff();
        _combatSystem.ShowStage(true);
        if (tmp)
        {
            ShowTargets(GetCurrentlySelectedAbility());
        }
    }
    /**
    * Reset UI and switch to immaterial layer
    */
    public void ToggleImmaterial()
    {
        bool tmp = _isTargeting;
        HideTargets();
        //_abilitiesToggleGroup.SetAllTogglesOff();
        _combatSystem.ShowStage(false);
        if (tmp)
        {
            ShowTargets(GetCurrentlySelectedAbility());
        }
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
     * <param name="isVictory">Whether player has won</param>
     * <param name="players">List of all players in combat</param>
     * <param name="enemies">List of all enemies in combat</param>
    */
    public void ShowCombatSummary(bool isVictory, List<IHittable> players, List<IHittable> enemies)
    {
        UpperScreen.SetActive(false);
        LowerScreen.SetActive(false);
        SummaryDisplay.GetComponent<CombatSummaryScript>().SetValues(isVictory, players, enemies);
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

    public static bool IsTargeting => _isTargeting;

    public static int IsTargetingAreaRadius => _isTargetingAreaRadius;
}
