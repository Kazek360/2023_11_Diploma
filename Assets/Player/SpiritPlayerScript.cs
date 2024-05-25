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
        gameObject.SetActive(false);
    }
    
    

    public override void OnHealthDepleted()
    {
        Debug.Log( "Nestor has lost all health");
        if (_partner is not null)
        {
            _partner.OnPartnerDeath();
        }
        gameObject.SetActive(false);
    }

    public override int GetMaxHealth()
    {
        return PlayerStats.GetInstance().GetStat(StatType.NestorHealth);
    }

    public override int GetStrength()
    {
        return PlayerStats.GetInstance().GetStat(StatType.NestorStrength);
    }

    public override int GetAgility()
    {
        return PlayerStats.GetInstance().GetStat(StatType.NestorAgility);
    }

    public override int GetLuck()
    {
        return PlayerStats.GetInstance().GetStat(StatType.NestorLuck);
    }

    public override int GetImagination()
    {
        return PlayerStats.GetInstance().GetStat(StatType.NestorImagination);
    }
}
