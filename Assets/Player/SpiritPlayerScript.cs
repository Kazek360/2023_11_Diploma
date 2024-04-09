using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPlayerScript : PlayerScript, IHittable
{
    public new void SetPartnerEntity(IHittable partner)
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
        base.OnHealthDepleted();
        if (_partner is not null)
        {
            _partner.OnPartnerDeath();
        }
    }
}
