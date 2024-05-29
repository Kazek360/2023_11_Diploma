using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplaySwitch : MonoBehaviour
{

    [SerializeField] private GameObject abilityScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        abilityScreen.SetActive(true);
    }
    
    public void TurnOff()
    {
        abilityScreen.SetActive(false);
    }
}
