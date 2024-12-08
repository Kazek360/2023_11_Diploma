using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public abstract class EnemyBehaviourModel
{
    public abstract void ProcessTurn(CombatSystem combatSystem, CombatEnvironmentManager environmentManager);
}
