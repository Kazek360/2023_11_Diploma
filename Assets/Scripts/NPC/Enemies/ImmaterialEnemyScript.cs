using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmaterialEnemyScript : EnemyScript
{
    public void SetPartnerEntity(IHittable partner)
    {
        _partner = partner;
    }

    public override bool IsMaterial()
    {
        return false;
    }


    public void OnPartnerDeath()
    {
        Destroy(gameObject);
    }
    
}
