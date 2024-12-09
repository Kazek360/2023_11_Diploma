using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using Random = UnityEngine.Random;

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
            _entity.AfterTurnEffects();
            return;
        }
        MovementPhase();
    }

    private void MovementPhase()
    {
        if (_combatSystem.MovementLeft > 0)
        {
            var chase = StartCoroutine(ChaseEnemy());
        }
        else
        {
            AttackPhase();
        }
    }

    private void AttackPhase()
    {
        while (_combatSystem.ActionsLeft > 0)
        {
            Debug.Log("ATTACK");
            int chosenAbilityId = PerformAction();
            if (_target.IsDead())
            {
                _target = GetClosestEnemy();
                if (_target == null || _combatSystem.MovementLeft == 0)
                {
                    _entity.AfterTurnEffects();
                    return;
                }
                MovementPhase();
                return;
            }

            if (chosenAbilityId == -1)
            {
                break;
            }
        }
        //Debug.Log("ATTACK END");
        _entity.AfterTurnEffects();
    }

    private IEnumerator ChaseEnemy()
    {
        Debug.Log("COROUTINE START");
        var entityGrid = _environmentManager.GetGridObject(_entity.GetUnitCombatScript().GetGridLocation());
        var targetGrid = _environmentManager.GetGridObject(_target.GetUnitCombatScript().GetGridLocation());
        var path = _environmentManager.FindShortestPathTo(entityGrid, targetGrid, _entity.IsMaterial());
        
        for (int i = 1; i < path.Count - 1; i++)
        {
            //Debug.Log("LOOP ITER");
            if (DistanceToTarget() <= 1 || _combatSystem.MovementLeft <= 0)
            {
                break;
            }
            var nextHex = path[i];
            _combatSystem.MoveTo(nextHex.X, nextHex.Y);
            UnitCombat unitScript = _entity.GetUnitCombatScript();
            while (unitScript.currentState != UnitCombat.State.Idling)
            {
                Debug.Log("YIELD");
                yield return new WaitForSeconds(0.5f);
                Debug.Log("YIELD OVER");
            }
            Debug.Log("WHILE OVER");
        }
        AttackPhase();
    }

    private int PerformAction()
    {
        
        int chosenAbility = ChooseAbility();
        if (chosenAbility != -1)
        {
            CastAbility(chosenAbility);
        }
        return chosenAbility;
    }

    private int ChooseAbility()
    {
        var abilities = _entity.GetAbilities();
        var cooldowns = _entity.GetCooldowns();
        int currWeight = -1;
        int currCandidate = -1;
        
        for (int i = 0; i < abilities.Count; i++)
        {
            if (cooldowns[i] > 0) continue;
            if (abilities[i].GetAbilityWeight(_entity) > currWeight)
            {
                currWeight = abilities[i].GetAbilityWeight(_entity);
                currCandidate = i;
            }
        }

        return currCandidate;
    }

    private List<IHittable> GetAbilityTargetCandidates(AbilityScriptableObjectReworked abilitySO)
    {
        List<IHittable> targetCandidates = new List<IHittable>();
        foreach (var targetType in abilitySO.ViableTargets)
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

        return targetCandidates;
    }
    
    private void CastAbility(int i)
    {
        var abilities = _entity.GetAbilities();
        var ability = abilities[i];
        List<IHittable> targetCandidates = GetAbilityTargetCandidates(ability);

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
            _entity.OnAbilityCast(i);
            ability.CastAbility(possibleTargets[chosenTarget], _entity);
        }
    }

    private int DistanceToTarget()
    {
        return _environmentManager.GetDistance(_entity.GetUnitCombatScript(), _target.GetUnitCombatScript());
    }
    
}
