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
     * Modify health of the entity
     *
     * <param name="amount">Numeric change to be applied to entity health</param>
     */
    public void ModifyHealth(int amount);
    /**
     * Gets the current psyche of an entity
     *
     * <returns>Psyche numeric value</returns>
     */
    public int GetCurrentPsyche();
    /**
     * Returns max psyche value of an entity
     *
     * Useful for showing psyche bars and preventing overhealing(if such a thing will be necessary)
     */
    public int GetMaxPsyche();
    /**
     * Modify psyche of the entity
     *
     * <param name="amount">Numeric change to be applied to entity psyche</param>
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
    public List<AbilityScriptableObjectReworked> GetAbilities();
    public List<int> GetCooldowns();

    /**
     * Returns a sprite representing this entity
     */
    public Sprite GetImage();

    /**
     * Returns object this class is attached to
     */
    public GameObject GetObject();

    /**
     * Returns a unit combat component assigned to this object
     */
    public UnitCombat GetUnitCombatScript();
    
    public void ActivateTargetClickArea();
    public void TargetClickAreaDeselected();
    public void DeactivateTargetClickArea();

    public bool IsMaterial();

    public IHittable GetPartner();

    /**
     * If material: disconnect partner and begin multiplying taken damage
     * Else: disable entity
     */
    public void OnPartnerDeath();

    public void Activate(bool setActive);

    public GameObject GetGameObject();

    public void SetPartnerEntity(IHittable partner);

    public bool IsDead();
}
