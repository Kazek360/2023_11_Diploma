using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLibrary : MonoBehaviour
{
    [SerializeField] private List<AbilityScriptableObject> _abilityScriptableObjects;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Load();
        GameConfig.abilityLibrary = this;
    }

    public AbilityScriptableObject GetAbility(int i)
    {
        return _abilityScriptableObjects[i];
    }

    public List<AbilityScriptableObject> GetAllAbilities()
    {
        return _abilityScriptableObjects;
    }

    public void Load()
    {
        UnlockableAbility ab1 = new UnlockableAbility();
        ab1.Recipient = UnlockableAbility.PlayerType.WIT;
        ab1.AbilitySO = _abilityScriptableObjects[7];
        UnlockedAbilities.GetInstance().UnlockAbility(ab1);
        UnlockableAbility ab2 = new UnlockableAbility();
        ab2.Recipient = UnlockableAbility.PlayerType.NESTOR;
        ab2.AbilitySO = _abilityScriptableObjects[2];
        UnlockedAbilities.GetInstance().UnlockAbility(ab2);
        UnlockableAbility ab3 = new UnlockableAbility();
        ab3.Recipient = UnlockableAbility.PlayerType.NESTOR;
        ab3.AbilitySO = _abilityScriptableObjects[8];
        UnlockedAbilities.GetInstance().UnlockAbility(ab3);
    }
    
    
}
