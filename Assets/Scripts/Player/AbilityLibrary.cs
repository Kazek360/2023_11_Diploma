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
        GameConfig.abilityLibrary = this;
    }

    public AbilityScriptableObject GetAbility(int i)
    {
        return _abilityScriptableObjects[i];
    }
}
