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
    protected int _movementLeft;
    protected int _actionsLeft;
    
    public virtual void ProcessTurn(IHittable entity, CombatSystem combatSystem, CombatEnvironmentManager environmentManager, EntityManager entityManager)
    {
        _movementLeft = entity.GetAgility();
        _actionsLeft = entity.GetImagination();
        _combatSystem = combatSystem;
        _environmentManager = environmentManager;
        _entityManager = entityManager;
        _entity = entity;
    }
    
    protected IHittable GetClosestEnemy()
    {
        int dist = 999;
        IHittable currTarget = null;
        
        List<IHittable> players = _entityManager.GetEntities(true, _entity.IsMaterial());
        foreach (var target in players)
        {
            int currDist =
                _environmentManager.GetDistance(_entity.GetUnitCombatScript(), target.GetUnitCombatScript());
            if (currDist < dist)
            {
                currTarget = target;
                dist = currDist;
            }
        }

        if (currTarget != null)
        {
            //Debug.Log("ENEMY CHOSEN: " + currTarget.GetEntityName());
        }
        return currTarget;
    }
}
