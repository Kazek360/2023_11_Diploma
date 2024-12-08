using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class MeleeBehaviourModel : EnemyBehaviourModel
{
    private IHittable _target;
    public override void ProcessTurn(IHittable entity, CombatSystem combatSystem, CombatEnvironmentManager environmentManager,
        EntityManager entityManager)
    {
        base.ProcessTurn(entity, combatSystem, environmentManager, entityManager);
        _target = GetClosestEnemy();
        if (_target == null)
        {
            //TODO: TRIGGER NEXT TURN
            return;
        }
        StartCoroutine(MovementPhase());
    }

    private IHittable GetClosestEnemy()
    {
        int dist = 99;
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
        return currTarget;
    }

    private IEnumerator MovementPhase()
    {
        var entityGrid = _environmentManager.GetGridObject(_entity.GetUnitCombatScript().GetGridLocation());
        var targetGrid = _environmentManager.GetGridObject(_target.GetUnitCombatScript().GetGridLocation());
        var path = _environmentManager.FindShortestPathTo(entityGrid, targetGrid, _entity.IsMaterial());
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (DistanceToTarget() <= 1 || _combatSystem.MovementLeft <= 0)
            {
                break;
            }
            var nextHex = path[i];
            _combatSystem.MoveTo(nextHex.X, nextHex.Y);
            while (_entity.GetUnitCombatScript().currentState != UnitCombat.State.Idling)
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
        AttackPhase();
    }

    private void AttackPhase()
    {
        while (_combatSystem.ActionsLeft > 0)
        {
            PerformAction();
        }
        //TODO: TRIGGER NEXT TURN
    }

    private void PerformAction()
    {
        var cooldowns = _entity.GetCooldowns();
        var abilities = _entity.GetAbilities();
        int chosenAbility = ChooseAbility();
        if (chosenAbility != -1)
        {
            CastAbility(chosenAbility);
        }

        for (int i = 0; i < cooldowns.Count; i++)
        {
            cooldowns[i] = Math.Max(0, cooldowns[i] - 1);
        }
    }

    private void ChooseAbility()
    {
        
    }
    
    private void CastAbility(int i)
    {
        var abilities = _entity.GetAbilities();
        var ability = abilities[i];
        List<IHittable> targetCandidates = new List<IHittable>();
        foreach (var targetType in ability.ViableTargets)
        {
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.AllyMaterial)
            {
                targetCandidates.AddRange(_entityManager.GetEntities(false, true));
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.EnemyMaterial)
            {
                targetCandidates.AddRange(_entityManager.GetEntities(true, true));
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.EnemyImmaterial)
            {
                targetCandidates.AddRange(_entityManager.GetEntities(true, false));
            }
            
            if (targetType == AbilityScriptableObjectReworked.TargetTypes.AllyImmaterial)
            {
                targetCandidates.AddRange(_entityManager.GetEntities(false, false));
            }
        }

        List<IHittable> possibleTargets = new List<IHittable>();
        foreach (var target in targetCandidates)
        {
            if (_combatSystem.GetDistance(target.GetUnitCombatScript()) <= ability.Range)
            {
                possibleTargets.Add(target);
            }
        }

        if (possibleTargets.Count > 0)
        {
            int chosenTarget = Random.Range(0, possibleTargets.Count);
            _cooldowns[i] = ability.Cooldown;
            ability.CastAbility(possibleTargets[chosenTarget], this);
        }
    }

    private int DistanceToTarget()
    {
        return _environmentManager.GetDistance(_entity.GetUnitCombatScript(), _target.GetUnitCombatScript());
    }
    
}
