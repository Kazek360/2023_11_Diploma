using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public abstract class EnemyBehaviourModel
{
    public abstract void ProcessTurn(IHittable entity, CombatSystem combatSystem, CombatEnvironmentManager environmentManager, EntityManager entityManager);
}
