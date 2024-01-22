using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Interface to be added to all classes that can be damaged by player or enemy attacks
 */
public interface IHittable
{
    /**
    * Gets the name of an entity
    */
    public string GetEntityName();
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
     * Modifies health of an entity by a desired amount
     */
    public void ModifyHealth(int amount);
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
     * Modifies psyche of an entity by a desired amount
     */
    public void ModifyPsyche(int amount);
    /**
     * Action to be performed when health of an entity runs out
     */
    public void OnHealthDepleted();
    /**
     * Action to be performed when psyche of an entity runs out
     */
    public void OnPsycheDepleted();
    /**
     * Actions performed when the turn of entity has begun
     */
    public void ProcessTurn();

    public int GetStrength();
    public int GetAgility();
    public int GetLuck();
    public int GetImagination();
    public List<AbilityScriptableObject> GetAbilities();
    public List<int> GetCooldowns();

    public Sprite GetImage();
}
