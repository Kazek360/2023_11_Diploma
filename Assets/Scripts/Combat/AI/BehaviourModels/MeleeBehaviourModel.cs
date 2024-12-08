using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class MeleeBehaviourModel : EnemyBehaviourModel
{
    private CombatSystem _combatSystem;
    private CombatEnvironmentManager _environmentManager;
    private EntityManager _entityManager;
    private IHittable _entity;
    public override void ProcessTurn(IHittable entity, CombatSystem combatSystem, CombatEnvironmentManager environmentManager, EntityManager entityManager)
    {
        _combatSystem = combatSystem;
        _environmentManager = environmentManager;
        _entityManager = entityManager;
        _entity = entity;
        
        throw new System.NotImplementedException();
    }

    private IHittable GetClosestEnemy()
    {
        int dist = 99;
        IHittable currTarget = null;
        if (_entity.IsMaterial())
        {
            List<IHittable> materialPlayers = _entityManager.GetEntities(true, true);
            foreach (var target in materialPlayers)
            {
                int currDist =
                    _environmentManager.GetDistance(_entity.GetUnitCombatScript(), target.GetUnitCombatScript());
                if (currDist < dist)
                {
                    currTarget = target;
                    dist = currDist;
                }
            }
        }

        return currTarget;
    }
}
