using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    /**
     * A class managing backend side of all combat actions an entity can perform
     */
    public class CombatSystem : MonoBehaviour
    {
        [FormerlySerializedAs("CurrentUnit")] [SerializeField] private UnitCombat currentUnit;
        [FormerlySerializedAs("CombatUI")] [SerializeField] private CombatUI combatUI;
        [SerializeField] private CombatEnvironmentManager combatEnvironmentManager;

        private EntityManager _entityManager;
        private GridObject _currentGridObject;
        private int _startingMovement, _startingActions;
        private int _movementLeft, _actionsLeft;
        private bool _currentIsMaterial;
        private bool _currentIsPlayer;
        private IHittable _currentHittable;
        
        
        private void Awake()
        {
            _currentIsPlayer = false;
            _entityManager = GetComponent<EntityManager>();
        }

        private void Update()
        {

            _currentGridObject = combatEnvironmentManager.GetCurrentGridObject();
            
            if (_currentGridObject != null && _currentGridObject.IsHighlighted)
            {
                if(Input.GetMouseButtonDown(0) && currentUnit.currentState == UnitCombat.State.Idling)
                {
                    MoveToSelected();
                }
            }
        }

        /**
         * Highlights every hex in the range of an ability
         *
         * <param name="range">Range of the ability</param>
         */
        public void UnityTargetingHighlight(int range)
        {
            combatEnvironmentManager.TargetingHighlightNeighborsInRadius(combatEnvironmentManager.GetGridObject((int)currentUnit.GetGridLocation().x, (int)currentUnit.GetGridLocation().y), range, true);
        }


        
        /**
         * Algorithm moving an object to its correct place on a hex map
         * <param name='unitCombat'>moved unit</param>
         * <param name='isMaterial'>is the moved unit material</param>
         */
        public void SetEntityToGrid(UnitCombat unitCombat, bool isMaterial)
        {
            var vec = unitCombat.GetGridLocation();
            combatEnvironmentManager.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(true, isMaterial);
            unitCombat.gameObject.transform.position = combatEnvironmentManager.GetGridObject((int)vec[0], (int)vec[1]).Hex.position;
        }

        /**
         * Set the currently active unit
         * <param name='newUnit'>moved unit</param>
         * <param name='placeInQueue'>place in turn queue that entity takes</param>
         */
        public void SetCurrentUnit(UnitCombat newUnit, int placeInQueue)
        {
            combatEnvironmentManager.ClearGrid();
            currentUnit = newUnit;
            var vec = currentUnit.GetGridLocation();
            _currentGridObject = combatEnvironmentManager.GetGridObject((int)vec[0], (int)vec[1]);
            combatUI.ActivateEntityInQueue(placeInQueue);
        }

        /**
         * Moves the currently active unit to a selected hex and highlights the surrounding hexes.
         * To be used for player controlled entities only.
         */
        private void MoveToSelected()
        {
            _movementLeft--;
            UpdateUICounters();
            var vec = currentUnit.GetGridLocation();
            combatEnvironmentManager.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(false, _currentIsMaterial);
            combatEnvironmentManager.ClearGrid();
                    
            currentUnit.StartMoving(_currentGridObject.Hex.position);
            currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.SetOccupied(true, _currentIsMaterial);
            if (_movementLeft > 0)
            {
               combatEnvironmentManager.HighlightNeighborsInRadius(_currentGridObject, 1, false, _currentIsMaterial);
            }
        }

        
        /**
         * Moves the currently active unit to a specified position on the grid.
         * To be used for player controlled entities only.
         * <param name='x'>x coordinate on hex map</param>
         * <param name='y'>y coordinate on hex map</param>
         */
        public void MoveTo(int x, int y)
        {
            _movementLeft--;
            var vec = currentUnit.GetGridLocation();
            combatEnvironmentManager.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(false, _currentIsMaterial);
            combatEnvironmentManager.ClearGrid();

            _currentGridObject = combatEnvironmentManager.GetGridObject(x, y);
            currentUnit.StartMoving(_currentGridObject.Hex.position);
            currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.SetOccupied(true, _currentIsMaterial);
        }

        /**
         * Begins the player turn by activating the UI and highlighting possible movement hexes
         * <param name='movement'>movement for the turn</param>
         * <param name='actions'>actions for the turn</param>
         * <param name='isMaterial'>is the player material</param>
         * <param name='hittable'>hittable script belonging to current unit</param>
         */
        public void PlayerTurnStart(int movement, int actions, bool isMaterial, IHittable hittable)
        {
            _currentIsPlayer = true;
            SetCurrentMaterial(isMaterial);
            ShowStage(isMaterial);
            _movementLeft = movement;
            _startingMovement = movement;
            _actionsLeft = actions;
            _startingActions = actions;
            _currentHittable = hittable;
            UpdateUICounters();
            combatUI.ShowAbilities();
            combatUI.ShowItems();
            combatEnvironmentManager.HighlightNeighborsInRadius(_currentGridObject, 1, false, _currentIsMaterial);
        }

        public void EnemyTurnSetup(int movement, int prefRange, UnitCombat target, EnemyScript movedEntity)
        {
            _currentIsPlayer = false;
            _movementLeft = movement;
            SetCurrentMaterial(movedEntity.IsMaterial());
            ShowStage(movedEntity.IsMaterial());
        }

        /**
         * Reinitializes movement after previous break
         */
        public void ReturnToMovement()
        {
            if (currentUnit is null || _movementLeft <= 0)
            {
                return;
            }
            combatEnvironmentManager.ClearGrid();
            //combatEnvironmentManager.HighlightNeighborsInRadius(combatEnvironmentManager.GetGridObject((int)currentUnit.GetGridLocation().x, (int)currentUnit.GetGridLocation().y), 1, false, _currentIsMaterial);
            StartCoroutine(AfterClickCooldown());
        }
        
        /**
     * Used in order to avoid immediately moving after click
     */
        private IEnumerator AfterClickCooldown()
        {
            yield return new WaitForSeconds(0.1f);
            combatEnvironmentManager.HighlightNeighborsInRadius(combatEnvironmentManager.GetGridObject((int)currentUnit.GetGridLocation().x, (int)currentUnit.GetGridLocation().y), 1, false, _currentIsMaterial);
        }

        /**
         * Update UI counters with current values
         */
        public void UpdateUICounters()
        {
            combatUI.SetMovementCounter(_movementLeft, _startingMovement);
            combatUI.SetActionCounter(_actionsLeft, _startingActions);
            combatUI.SetHealthCounter(_currentHittable.GetCurrentHealth(), _currentHittable.GetMaxHealth());
            combatUI.SetCharacterDisplay(_currentHittable.GetImage(), _currentHittable.GetEntityName());
        }

        public CombatUI GetCombatUI()
        {
            return combatUI;
        }

        /**
         * Called when an enemy is defeated in order to clear the occupation of their hex
         * <param name='unitDefeated'>which enemy has been defeated</param>
         * <param name='isMaterial'>is the entity material</param>
         */
        public void EnemyDefeated(UnitCombat unitDefeated, bool isMaterial)
        {
            var pos = unitDefeated.GetGridLocation();
            combatEnvironmentManager.GetGridObject((int)pos.x, (int)pos.y).SetOccupied(false, isMaterial);
        }

        /**
         * Set material state of current entity and adjust UI
         * <param name='isMaterial'>is the entity material</param>
         */
        private void SetCurrentMaterial(bool isMaterial)
        {
            _currentIsMaterial = isMaterial;
            combatUI.SetMaterialToggle(isMaterial);
        }

        public bool GetCurrentIsMaterial()
        {
            return _currentIsMaterial;
        }

        /**
         * Show an appropriate stage and adjust UI to its material state
         * <param name='isMaterial'>is the displayed stage material</param>
         */
        public void ShowStage(bool isMaterial)
        {
            _entityManager.ShowEntities(isMaterial);
            var pos = currentUnit.GetGridLocation();
            var currPos = combatEnvironmentManager.GetGridObject((int)pos.x, (int)pos.y);
            if (_currentIsPlayer)
            {
                if (isMaterial)
                {
                    if (_currentIsMaterial && _movementLeft > 0)
                    {
                        combatEnvironmentManager.HighlightNeighborsInRadius(currPos, 1, false, _currentIsMaterial);
                    }
                    else
                    {
                        combatEnvironmentManager.ClearGrid();
                    }

                }
                else
                {
                    if (!_currentIsMaterial && _movementLeft > 0)
                    {
                        combatEnvironmentManager.HighlightNeighborsInRadius(currPos, 1, false, _currentIsMaterial);
                    }
                    else
                    {
                        combatEnvironmentManager.ClearGrid();
                    }
                }
            }
        }
        
        /**
         * Show  the summary of a finished combat
         * <param name='isVictory'>whether the player has won an encounter</param>
         * <param name='players'>list of players in combat</param>
         * <param name='enemies'>list of enemies in combat</param>
         */
        public void ShowCombatSummary(bool isVictory, List<IHittable> players, List<IHittable> enemies)
        {
            combatUI.DisableCombatUI();
            combatEnvironmentManager.ClearGrid();
            combatUI.ShowCombatSummary(isVictory, players, enemies);
        }

        public int ActionsLeft
        {
            get => _actionsLeft;
            set => _actionsLeft = value;
        }

        public int StartingMovement
        {
            get => _startingMovement;
            set => _startingMovement = value;
        }

        public int StartingActions
        {
            get => _startingActions;
            set => _startingActions = value;
        }

        public void DisplayQueue(List<IHittable> entities)
        {
            combatUI.FillQueue(entities);
        }
        
        public void DisableDeadEntityInQueue(int n) {
            combatUI.DisableDeadEntityInQueue(n);
        }

        public void ClearGrid()
        {
            combatEnvironmentManager.ClearGrid();
        }

        public int GetDistance(UnitCombat unitScript)
        {
            return combatEnvironmentManager.GetDistance(currentUnit, unitScript);
        }

        public CombatEnvironmentManager GetCombatEnvironmentManager()
        {
            return combatEnvironmentManager;
        }
    }
}
