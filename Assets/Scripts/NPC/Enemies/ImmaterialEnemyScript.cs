using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmaterialEnemyScript : EnemyScript, IImmaterial
{
    public void SetPartnerEntity(IHittable partner)
    {
        throw new System.NotImplementedException();
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
