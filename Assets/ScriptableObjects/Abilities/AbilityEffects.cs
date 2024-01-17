using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffects : MonoBehaviour
{
    /**
     * What I'm about to do is not just stupid - it is a monument to all my sins as a programmer
     * It is a manifesto of desperation and hubris
     * It is a clear sign that I should have chosen Extended Polish instead of Extended Math and that giving
     * me access to a working computer was the greatest mistake of our universe
     *
     * But it is also the only way to do this without using at least 2000 classes that I could think of
     * May God forgive me
     *
     * Ok, so what you have here is a list of all modifiers that an ability can apply to any entity
     * They will be used to create every ability
     * And yes, they all have to be here because otherwise it would devolve into either
     * writing every ability from scratch or creating some convoluted JSON-parsing system that
     * would plague my dreams
     */

    public static void DealHealthDamage(IHittable target, int damage)
    {
        target.ModifyHealth(-damage);
    }

    public static void HealHealth(IHittable target, int amount)
    {
        target.ModifyHealth(Math.Min(amount, target.GetMaxHealth() - target.GetCurrentHealth()));
    }
    
    public static void DealPsycheDamage(IHittable target, int damage)
    {
        target.ModifyPsyche(-damage);
    }
    
    public static void HealPsyche(IHittable target, int amount)
    {
        target.ModifyPsyche(Math.Min(amount, target.GetMaxPsyche() - target.GetCurrentPsyche()));
    }

    public static void AAAAAAA()
    {
        Debug.Log("AAAAAAA");
    }
}
