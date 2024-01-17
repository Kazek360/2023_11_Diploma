using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Interface to be added to all classes that can be damaged by player or enemy attacks
 */
public interface IHittable
{
    /**
     * Gets the current health of an entity
     */
    public int GetCurrentHealth();
    /**
     * Returns max health value of an entity
     *
     * Useful for showing health bars and preventing overhealing(if such a thing will be necessary)
     */
    public int GetMaxHealth();
    /**
     * Lowers health of an entity by a desired amount
     */
    public void DamageHP(int dmg);
    /**
     * Gets the current psyche of an entity
     */
    public int GetCurrentPsyche();
    /**
     * Returns max psyche value of an entity
     *
     * Useful for showing psyche bars and preventing overhealing(if such a thing will be necessary)
     */
    public int GetMaxPsyche();
    /**
     * Lowers psyche of an entity by a desired amount
     */
    public void DamagePsyche(int dmg);
    /**
     * Action to be performed when health of an entity runs out
     */
    public void OnHealthDepleted();
    /**
     * Action to be performed when psyche of an entity runs out
     */
    public void OnPsycheDepleted();
}
