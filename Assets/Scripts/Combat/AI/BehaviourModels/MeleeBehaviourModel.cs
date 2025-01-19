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
        MovementPhase();
    }

    private void MovementPhase()
    {
        _target = GetClosestEnemy();
        if (_target == null)
        {
            StartCoroutine(WaitToEndTurn());
            return;
        }
        if (_movementLeft > 0)
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
        while (_actionsLeft > 0)
        {
            int chosenAbilityId = PerformAction();
            if (_target.IsDead())
            {
                MovementPhase();
                return;
            }

            if (chosenAbilityId == -1)
            {
                break;
            }
        }
        
        StartCoroutine(WaitToEndTurn());
    }

    /**
     * Get as close to target as possible
     */
    private IEnumerator ChaseEnemy()
    {
        var entityGrid = _environmentManager.GetGridObject(_entity.GetUnitCombatScript().GetGridLocation());
        var targetGrid = _environmentManager.GetGridObject(_target.GetUnitCombatScript().GetGridLocation());
        var path = _environmentManager.FindShortestPathTo(entityGrid, targetGrid, _entity.IsMaterial());
        
        for (int i = 1; i < path.Count - 1; i++)
        {
            //Debug.Log("LOOP ITER");
            if (DistanceToTarget() <= 1 || _movementLeft <= 0)
            {
                break;
            }
            var nextHex = path[i];
            _combatSystem.MoveTo(nextHex.X, nextHex.Y);
            _movementLeft--;
            UnitCombat unitScript = _entity.GetUnitCombatScript();
            while (unitScript.currentState != UnitCombat.State.Idling)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        AttackPhase();
    }

    /**
     * Choose ability with greatest weight and use it if able
     */
    private int PerformAction()
    {
        
        int chosenAbility = ChooseAbility();
        if (chosenAbility != -1)
        {
            _actionsLeft--;
            CastAbility(chosenAbility);
        }
        return chosenAbility;
    }

    /**
     * Choose an available ability with the greatest weight
     */
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

    /**
     * Get all units that passed ability would be able to target
     */
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
    
    /**
     * Cast ability on a viable target
     *
     * <param name="i" > Position of an ability in unit's ability vector.</param>
     */
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
    
    /**
     * Calculate distance to currently chosen target in a straight line
     */
    private int DistanceToTarget()
    {
        return _environmentManager.GetDistance(_entity.GetUnitCombatScript(), _target.GetUnitCombatScript());
    }
    
    /**
     * Just wait for animations to run out
     */
    private IEnumerator WaitToEndTurn()
    {
        yield return new WaitForSeconds(0.3f);
        _entity.AfterTurnEffects();
    }
}
