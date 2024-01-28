using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPlayerScript : PlayerScript
{
    public void SetPartnerEntity(IHittable partner)
    {
        _partner = partner;
    }

    public override bool IsMaterial()
    {
        return false;
    }

    public new void OnPartnerDeath()
    {
        Destroy(gameObject);
    }
    
    

    public override void OnHealthDepleted()
    {
        Debug.Log( "Nestor has lost all health");
        _partner.OnPartnerDeath();
    }
}
