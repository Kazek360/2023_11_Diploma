using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject scrollContent;
    [SerializeField] private GameObject abilityCardPrefab;

    private IHittable _currentEntity;
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
        _currentEntity = entity;
        for (int i = 0; i < _currentEntity.GetAbilities().Count; i++)
        {
            var container = Instantiate(abilityCardPrefab);
            container.gameObject.GetComponent<AbilityCard>().SetAbility(_currentEntity.GetAbilities()[i], _currentEntity.GetCooldowns()[i]);
            container.transform.SetParent(scrollContent.transform);
            
        }
    }

    public void OnEndTurn()
    {
        for (int i = 0; i < scrollContent.transform.childCount; i++) {
            Transform child = scrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}
