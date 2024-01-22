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

    private PlayerScript _currentEntity;
    // Start is called before the first frame update
    void Start()
    {
        _entityManager = entityManager.GetComponent<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        
        
        List<IHittable> targets = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObject.TargetTypes.Ally)
            {
                targets.AddRange(_entityManager.GetPlayers());
            }
            
            if (targetType == AbilityScriptableObject.TargetTypes.Enemy)
            {
                targets.AddRange(_entityManager.GetEnemies());
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

    public void OnEndTurn()
    {
        var toggled = abilitiesScrollContent.GetComponent<ToggleGroup>().GetFirstActiveToggle();
        if (toggled is not null)
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
