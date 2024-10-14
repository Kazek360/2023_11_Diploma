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
        _currentHealth = 0;
        Debug.Log("DED");
        OnHealthDepleted();
    }
}
