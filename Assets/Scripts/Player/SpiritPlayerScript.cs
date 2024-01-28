using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPlayerScript : PlayerScript, IImmaterial
{
    private IHittable _partner;
    public void SetPartnerEntity(IHittable partner)
    {
        _partner = partner;
    }

    public bool IsMaterial()
    {
        return false;
    }

    public void OnPartnerDeath()
    {
        Destroy(gameObject);
    }
    
    
}
