using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject scrollContent;
    [SerializeField] private GameObject abilityCardPrefab;

    private PlayerScript _currentEntity;
    // Start is called before the first frame update
    void Start()
    {
        
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
            container.GetComponent<Toggle>().group = scrollContent.GetComponent<ToggleGroup>();
            container.transform.SetParent(scrollContent.transform);
            
        }
    }

    public void OnEndTurn()
    {
        var toggled = scrollContent.GetComponent<ToggleGroup>().GetFirstActiveToggle();
        if (toggled is not null)
        {
            int selected = toggled.gameObject.GetComponent<AbilityCard>().AbilityOrderInEntityList;
            _currentEntity.CastAbility(toggled.gameObject.GetComponent<AbilityCard>().AbilityOrderInEntityList);
        }
        
        for (int i = 0; i < scrollContent.transform.childCount; i++) {
            Transform child = scrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}
