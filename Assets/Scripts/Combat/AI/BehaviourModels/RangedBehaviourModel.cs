using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        AttackPhase();
        
    }

    private void PositioningPhase(List<GridObject> requiredMovement)
    {
        if (requiredMovement.Count - 1 > _movementLeft)
        {
            RetreatPhase();
            return;
        }

        /*
         Whether or not the unit can retreat later to minDist
         moves to get in range = requiredMovement.Count - 1
         moves to return to minDist = minDist - (currentDistance - requiredMovement.Count - 1)
         together = minDist - currentDist + 2 * (requiredMovement.Count - 1)
         */
        if (minDist - DistanceToTarget() + (requiredMovement.Count() - 1) * 2 > _movementLeft)
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
            StartCoroutine(WaitToEndTurn());
            return;
        }
        if (_actionsLeft == 0)
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
        
        if (requiredMovement.Count > 0)
        {
            PositioningPhase(requiredMovement);
            return;
        }
        int performedAction = PerformAction();
        
         if (performedAction == -1)  //No action can be performed - theoretically unreachable but just in case
         {
             RetreatPhase();
             return;
         }
        AttackPhase();
    }

    private void RetreatPhase()
    {
        _target = GetClosestEnemy();
        if (_target is null)
        {
            StartCoroutine(WaitToEndTurn());
            return; 
        }
        if (DistanceToTarget() >= prefDist)
        {
            StartCoroutine(WaitToEndTurn());
            return;
        }

        var retreatPath = RetreatRequiredMovementCalc();

        if (retreatPath.Count == 0)
        {
            StartCoroutine(WaitToEndTurn());
            return;
        }
        StartCoroutine(RetreatAlongPath(retreatPath));
    }

    /**
     * Unit will approach target and then start attacking
     */
    private IEnumerator ApproachTarget(List<GridObject> path)
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i < path.Count(); i++)
        {
            if (DistanceToTarget() <= 1 || _movementLeft <= 0)
            {
                break;
            }
            var nextHex = path[i];
            _combatSystem.MoveTo(nextHex.X, nextHex.Y);
            _movementLeft--;
            while (_entity.GetUnitCombatScript().currentState != UnitCombat.State.Idling)
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
        AttackPhase();
    }
    
    /**
     * Unit will go along the passed path
     */
    private IEnumerator RetreatAlongPath(List<GridObject> path)
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i < path.Count(); i++)
        {
            Debug.Log(path[i].X + " " + path[i].Y);
            if (_movementLeft <= 0)
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
        StartCoroutine(WaitToEndTurn());
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
     * Calculate path for repositioning phase
     */
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
            if (abilities[i].Range >= DistanceToTarget())
            {
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
    
    /**
     * Calculate path for retreat phase
     */
    private List<GridObject> RetreatRequiredMovementCalc()
    {
        var path = new List<GridObject>();
        var entityGrid = _environmentManager.GetGridObject(_entity.GetUnitCombatScript().GetGridLocation());
        var targetGrid = _environmentManager.GetGridObject(_target.GetUnitCombatScript().GetGridLocation());
        for (int reqDistance = prefDist; reqDistance > 0; reqDistance--)
        {
            var tmp = _environmentManager.FindShortestPathToDistance(entityGrid, targetGrid, _entity.IsMaterial(),
                reqDistance);
            if (tmp.Count - 1 <= _movementLeft)
            {
                return tmp;
            }
        }
        return path;
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
            if (abilities[i].GetAbilityWeight(_entity) > currWeight && abilities[i].Range >= DistanceToTarget())
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
