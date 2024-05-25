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
        return GameConfig.PlayerStats.GetStat(StatType.NestorHealth);
    }

    public override int GetStrength()
    {
        return GameConfig.PlayerStats.GetStat(StatType.NestorStrength);
    }

    public override int GetAgility()
    {
        return GameConfig.PlayerStats.GetStat(StatType.NestorAgility);
    }

    public override int GetLuck()
    {
        return GameConfig.PlayerStats.GetStat(StatType.NestorLuck);
    }

    public override int GetImagination()
    {
        return GameConfig.PlayerStats.GetStat(StatType.NestorImagination);
    }
}
