using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject abilitiesScrollContent;
    [SerializeField] private GameObject abilityCardPrefab;
    //[SerializeField] private GameObject targetsScrollContent;
    [SerializeField] private GameObject targetCardPrefab;
    [SerializeField] private GameObject entityManager;
    [SerializeField] private GameObject movementCounter, actionCounter;
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private Toggle materialToggle;
    [SerializeField] private Toggle immaterialToggle;
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
        _entityManager = entityManager.GetComponent<EntityManager>();
        _combatSystem2 = entityManager.GetComponent<CombatSystem_2>();
        _abilitiesToggleGroup = abilitiesScrollContent.GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Called when a player turn begins(enemies don't need it)
     */
    public void ShowCombatUI(IHittable entity)
    {
        //gameObject.SetActive(true);
        abilitiesScrollContent.SetActive(true);
        movementCounter.SetActive(true);
        actionCounter.SetActive(true);
        endTurnButton.SetActive(true);
        _movementCounter = movementCounter.GetComponent<TextMeshProUGUI>();
        _actionCounter = actionCounter.GetComponent<TextMeshProUGUI>();
        _currentEntity = (PlayerScript)entity;
    }

    public void ShowAbilities()
    {
        for (int i = 0; i < _currentEntity.GetAbilities().Count; i++)
        {
            var container = Instantiate(abilityCardPrefab);
            AbilityCard abilityCard = container.gameObject.GetComponent<AbilityCard>();
            abilityCard.SetAbility(_currentEntity.GetAbilities()[i], _currentEntity.GetCooldowns()[i], i);
            abilityCard.SetController(this);
            container.GetComponent<Toggle>().group = _abilitiesToggleGroup;
            container.transform.SetParent(abilitiesScrollContent.transform);
            if (_combatSystem2.ActionsLeft <= 0)
            {
                Debug.Log(_combatSystem2.ActionsLeft);
                container.GetComponent<Toggle>().interactable = false;
            }
        }
    }

    /**
     * DEPRECATED
     * After choosing an ability show possible targets
     *
     * In the beginning deletes all existing children of the target window to avoid cloning
     */
    // public void ShowTargetsList(AbilityScriptableObject ability)
    // {
    //     for (int i = 0; i < targetsScrollContent.transform.childCount; i++) {
    //         Transform child = targetsScrollContent.transform.GetChild(i);
    //         Destroy(child.gameObject);
    //     }
    //     
    //     
    //     List<IHittable> possibleTargets = new List<IHittable>();
    //     foreach (var targetType in ability.ViableTargets)
    //     {
    //         if (targetType == AbilityScriptableObject.TargetTypes.AllyMaterial)
    //         {
    //             possibleTargets.AddRange(_entityManager.GetMaterialPlayers());
    //         }
    //         
    //         if (targetType == AbilityScriptableObject.TargetTypes.EnemyMaterial)
    //         {
    //             possibleTargets.AddRange(_entityManager.GetMaterialEnemies());
    //         }
    //     }
    //     List<IHittable> targets = new List<IHittable>();
    //     foreach (var target in possibleTargets)
    //     {
    //         if (_combatSystem2.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
    //         {
    //             targets.Add(target);
    //         }
    //     }
    //
    //     foreach (var target in targets)
    //     {
    //         var container = Instantiate(targetCardPrefab);
    //         TargetCard targetCard = container.gameObject.GetComponent<TargetCard>();
    //         targetCard.SetTarget(target);
    //         container.GetComponent<Toggle>().group = targetsScrollContent.GetComponent<ToggleGroup>();
    //         container.transform.SetParent(targetsScrollContent.transform);
    //     }
    // }
    
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
            //Debug.Log("ACTIVATING");
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
        for (int i = 0; i < abilitiesScrollContent.transform.childCount; i++) {
            Transform child = abilitiesScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        
        // for (int i = 0; i < targetsScrollContent.transform.childCount; i++) {
        //     Transform child = targetsScrollContent.transform.GetChild(i);
        //     Destroy(child.gameObject);
        // }
        //gameObject.SetActive(false);
        abilitiesScrollContent.SetActive(false);
        movementCounter.SetActive(false);
        actionCounter.SetActive(false);
        endTurnButton.SetActive(false);
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
        //ShowTargetsList(GetCurrentlySelectedAbility());
        HideTargets();
        var ability = GetCurrentlySelectedAbility();
        if (ability is not null)
        {
            ShowTargets(ability);
        }
    }


    
    // private IHittable GetCurrentlySelectedTarget()
    // {
    //     var toggled = targetsScrollContent.GetComponent<ToggleGroup>().GetFirstActiveToggle();
    //     if (toggled is not null)
    //     {
    //         return toggled.gameObject.GetComponent<TargetCard>().GetTargetScript();
    //     }
    //     return null;
    // }
    
    private IHittable GetCurrentlySelectedTarget()
    {
        return _currentTarget;
    }


    private AbilityScriptableObject GetCurrentlySelectedAbility()
    {
        var toggled = _abilitiesToggleGroup.GetFirstActiveToggle();
        if (toggled is not null)
        {
            return toggled.gameObject.GetComponent<AbilityCard>().Ability;
        }
        return null;
    }

    public void SetMovementCounter(int i)
    {
        HideTargets();  //Movement deselects targets in order to avoid choosing a target and then moving beyond
                        //the intended range
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
        materialToggle.SetIsOnWithoutNotify(isMaterial);
        immaterialToggle.SetIsOnWithoutNotify(!isMaterial);
    }
}
