using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedBehaviourModel : EnemyBehaviourModel
{
    private IHittable _target;
    [SerializeField] private int prefDist;
    [SerializeField] private int minDist;
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
        AttackPhase();
        
    }

    private void PositioningPhase(int requiredMovement)
    {
        if (requiredMovement > _combatSystem.MovementLeft)
        {
            RetreatPhase();
            return;
        }

        if (minDist - DistanceToTarget() > _combatSystem.MovementLeft)
        {
            RetreatPhase();
            return;
        }
        
        //TODO GET CLOSER
        
    }

    private void AttackPhase()
    {
        _target = GetClosestEnemy();
        if (_combatSystem.ActionsLeft == 0)
        {
            RetreatPhase();
            return;
        }

        int requiredMovement = RequiredMovementCalc();

        if (requiredMovement == -1)
        {
            RetreatPhase();
            return;
        }

        if (requiredMovement > 0)
        {
            PositioningPhase(requiredMovement);
            return;
        }

        //TODO ATTACK
        PerformAction();
        AttackPhase();

    }

    private void RetreatPhase()
    {
        
    }

    private void PerformAction()
    {
        
        int chosenAbility = ChooseAbility();
        if (chosenAbility != -1)
        {
            CastAbility(chosenAbility);
        }
    }

    private int RequiredMovementCalc()
    {
        int result = -1;
        var abilities = _entity.GetAbilities();
        var cooldowns = _entity.GetCooldowns();
        for (int i = 0; i < abilities.Count; i++)
        {
            if (cooldowns[i] > 0) continue;
            if (result == -1)
            {
                result = 99;
            }
            if (abilities[i].Range <= DistanceToTarget())
            {
                result = 0;
            } else
            {

                result = Math.Min(result, DistanceToTarget() - abilities[i].Range);
            }
        }
        return result;
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
            if (abilities[i].GetAbilityWeight(_entity) > currWeight && abilities[i].Range <= DistanceToTarget())
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
