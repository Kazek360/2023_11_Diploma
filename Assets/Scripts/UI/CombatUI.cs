using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject abilitiesScrollContent;
    [SerializeField] private GameObject abilityCardPrefab;
    [SerializeField] private GameObject targetsScrollContent;
    [SerializeField] private GameObject targetCardPrefab;
    [SerializeField] private GameObject entityManager;
    private EntityManager _entityManager;
    private CombatSystem_2 _combatSystem2;

    private PlayerScript _currentEntity;
    // Start is called before the first frame update
    void Start()
    {
        _entityManager = entityManager.GetComponent<EntityManager>();
        _combatSystem2 = entityManager.GetComponent<CombatSystem_2>();
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
        gameObject.SetActive(true);
        _currentEntity = (PlayerScript)entity;
        for (int i = 0; i < _currentEntity.GetAbilities().Count; i++)
        {
            var container = Instantiate(abilityCardPrefab);
            AbilityCard abilityCard = container.gameObject.GetComponent<AbilityCard>();
            abilityCard.SetAbility(_currentEntity.GetAbilities()[i], _currentEntity.GetCooldowns()[i], i);
            abilityCard.SetController(this);
            container.GetComponent<Toggle>().group = abilitiesScrollContent.GetComponent<ToggleGroup>();
            container.transform.SetParent(abilitiesScrollContent.transform);
        }
    }

    /**
     * After choosing an ability show possible targets
     *
     * In the beginning deletes all existing children of the target window to avoid cloning
     */
    public void ShowTargetsList(AbilityScriptableObject ability)
    {
        for (int i = 0; i < targetsScrollContent.transform.childCount; i++) {
            Transform child = targetsScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        
        
        List<IHittable> possibleTargets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObject.TargetTypes.Ally)
            {
                possibleTargets.AddRange(_entityManager.GetPlayers());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.Enemy)
            {
                possibleTargets.AddRange(_entityManager.GetEnemies());
            }
        }
        List<IHittable> targets = new List<IHittable>();
        foreach (var target in possibleTargets)
        {
            if (_combatSystem2.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
            {
                targets.Add(target);
            }
        }

        foreach (var target in targets)
        {
            var container = Instantiate(targetCardPrefab);
            TargetCard targetCard = container.gameObject.GetComponent<TargetCard>();
            targetCard.SetTarget(target);
            container.GetComponent<Toggle>().group = targetsScrollContent.GetComponent<ToggleGroup>();
            container.transform.SetParent(targetsScrollContent.transform);
        }
    }

    /**
     * After turn ends hide interface and destroy children of ability and target lists in order to prevent cloning
     */
    public void OnEndTurn()
    {
        var toggled = abilitiesScrollContent.GetComponent<ToggleGroup>().GetFirstActiveToggle();
        if (toggled is not null && GetCurrentlySelectedTarget() is not null)
        {
            int selected = toggled.gameObject.GetComponent<AbilityCard>().AbilityOrderInEntityList;
            _currentEntity.CastAbility(toggled.gameObject.GetComponent<AbilityCard>().AbilityOrderInEntityList, GetCurrentlySelectedTarget());
        }
        
        for (int i = 0; i < abilitiesScrollContent.transform.childCount; i++) {
            Transform child = abilitiesScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < targetsScrollContent.transform.childCount; i++) {
            Transform child = targetsScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }

    /**
     * When ability is selected, populate target list
     */
    public void OnNewAbilitySelected()
    {
        ShowTargetsList(GetCurrentlySelectedAbility());
    }


    
    private IHittable GetCurrentlySelectedTarget()
    {
        var toggled = targetsScrollContent.GetComponent<ToggleGroup>().GetFirstActiveToggle();
        if (toggled is not null)
        {
            return toggled.gameObject.GetComponent<TargetCard>().GetTargetScript();
        }
        return null;
    }


    private AbilityScriptableObject GetCurrentlySelectedAbility()
    {
        var toggled = abilitiesScrollContent.GetComponent<ToggleGroup>().GetFirstActiveToggle();
        if (toggled is not null)
        {
            return toggled.gameObject.GetComponent<AbilityCard>().Ability;
        }
        return null;
    }
}
