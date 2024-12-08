using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class EnemyBehaviourModel : MonoBehaviour
{
    protected CombatSystem _combatSystem;
    protected CombatEnvironmentManager _environmentManager;
    protected EntityManager _entityManager;
    protected IHittable _entity;
    
    public virtual void ProcessTurn(IHittable entity, CombatSystem combatSystem, CombatEnvironmentManager environmentManager, EntityManager entityManager)
    {
        _combatSystem = combatSystem;
        _environmentManager = environmentManager;
        _entityManager = entityManager;
        _entity = entity;
    }
}
