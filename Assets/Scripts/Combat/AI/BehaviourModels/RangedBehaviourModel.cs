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

    private void PositioningPhase(List<GridObject> requiredMovement)
    {
        if (requiredMovement.Count - 1 > _combatSystem.MovementLeft)
        {
            RetreatPhase();
            return;
        }

        if (minDist - DistanceToTarget() > _combatSystem.MovementLeft)
        {
            RetreatPhase();
            return;
        }

        StartCoroutine(ApproachTarget(requiredMovement));

    }

    private void AttackPhase()
    {
        _target = GetClosestEnemy();
        if (_target == null)
        {
            _entity.AfterTurnEffects();
            return;
        }
        if (_combatSystem.ActionsLeft == 0)
        {
            RetreatPhase();
            return;
        }

        var requiredMovement = RepositioningRequiredMovementCalc();

        if (requiredMovement is null)
        {
            RetreatPhase();
            return;
        }

        if (requiredMovement.Count - 1 > 0)
        {
            PositioningPhase(requiredMovement);
            return;
        }
        PerformAction();
        AttackPhase();
    }

    private void RetreatPhase()
    {
        _target = GetClosestEnemy();
        if (_target is null)
        {
            _entity.AfterTurnEffects();
            return; 
        }
        if (DistanceToTarget() >= prefDist)
        {
            _entity.AfterTurnEffects();
            return;
        }

        var retreatPath = RetreatRequiredMovementCalc();

        if (retreatPath.Count == 0)
        {
            _entity.AfterTurnEffects();
            return;
        }
        StartCoroutine(RetreatAlongPath(retreatPath));
    }

    private IEnumerator ApproachTarget(List<GridObject> path)
    {
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
    
    private IEnumerator RetreatAlongPath(List<GridObject> path)
    {
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
        _entity.AfterTurnEffects();
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

    private List<GridObject> RepositioningRequiredMovementCalc()
    {
        int reqDistance = -1;
        var abilities = _entity.GetAbilities();
        var cooldowns = _entity.GetCooldowns();
        for (int i = 0; i < abilities.Count; i++)
        {
            if (cooldowns[i] > 0) continue;
            if (reqDistance == -1)
            {
                reqDistance = 99;
            }
            if (abilities[i].Range <= DistanceToTarget())
            {
                reqDistance = 0;
                return new List<GridObject>();
            }
            else
            {

                reqDistance = Math.Min(reqDistance, DistanceToTarget() - abilities[i].Range);
            }
        }

        if (reqDistance == -1)
        {
            return null;
        }
        
        var entityGrid = _environmentManager.GetGridObject(_entity.GetUnitCombatScript().GetGridLocation());
        var targetGrid = _environmentManager.GetGridObject(_target.GetUnitCombatScript().GetGridLocation());
        var path = _environmentManager.FindShortestPathToDistance(entityGrid, targetGrid, _entity.IsMaterial(),
            reqDistance);
        return path;
    }
    
    private List<GridObject> RetreatRequiredMovementCalc()
    {
        var path = new List<GridObject>();
        var entityGrid = _environmentManager.GetGridObject(_entity.GetUnitCombatScript().GetGridLocation());
        var targetGrid = _environmentManager.GetGridObject(_target.GetUnitCombatScript().GetGridLocation());
        for (int reqDistance = prefDist; reqDistance > 0; reqDistance--)
        {
            var tmp = _environmentManager.FindShortestPathToDistance(entityGrid, targetGrid, _entity.IsMaterial(),
                reqDistance);
            if (tmp.Count - 1 <= _combatSystem.MovementLeft)
            {
                return tmp;
            }
        }
        return path;
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
