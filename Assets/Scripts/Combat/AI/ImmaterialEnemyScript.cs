using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImmaterialEnemyScript : EnemyScript, IHittable
{
    public new void SetPartnerEntity(IHittable partner)
    {
        _partner = partner;
    }

    public override bool IsMaterial()
    {
        return false;
    }


    public override void OnPartnerDeath()
    {
        Debug.Log("DED");
        OnHealthDepleted();
    }
}
