using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    /**
     * A class managing backend side of all combat actions an entity can perform
     */
    public class CombatSystem : MonoBehaviour
    {
        [FormerlySerializedAs("HexPrefab")] [SerializeField] private Transform hexPrefab;
        [FormerlySerializedAs("MainCamera")] [SerializeField] private Camera mainCamera;
        private Grid<GridObject> _grid;
        [FormerlySerializedAs("CurrentUnit")] [SerializeField] private UnitCombat currentUnit;
        [FormerlySerializedAs("CombatUI")] [SerializeField] private CombatUI combatUI;
    
        [FormerlySerializedAs("Width")] [SerializeField] private int width;
        [FormerlySerializedAs("Height")] [SerializeField] private int height;
        [FormerlySerializedAs("CellSize")] [SerializeField] private float cellSize;
        [FormerlySerializedAs("Offset")] [SerializeField] private Vector2 offset;

        private EntityManager _entityManager;
        private GridObject _currentGridObject;
        private int _movementLeft, _actionsLeft;
        private bool _currentIsMaterial;
        private bool _currentIsPlayer;
        
        public class GridObject
        {
            public Transform Hex;
            public bool IsHighlighted;
            public int X;
            public int Y;
            public bool IsOccupiedMaterial;
            public bool IsOccupiedImmaterial;
            public Vector3 XYZCoordinates;      //Needed for A*

            public int F, G, H; //For A* algorithm

            public GridObject(int x, int y)
            {
                X = x;
                Y = y;
                XYZCoordinates.y = Y;
                XYZCoordinates.x = X - (int)(Y / 2);
                XYZCoordinates.z = 0 - XYZCoordinates.x - XYZCoordinates.y;
                IsOccupiedMaterial = false;
            }

            /**
             * Highlight the hex and make it active for movement
             */
            public void Highlight()
            {
                IsHighlighted = true;
                Hex.Find("Highlighted").gameObject.SetActive(true);
            }

            /**
             * Remove the highlight and deactivate
             */
            public void HideHighlight()
            {
                IsHighlighted = false;
                Hex.Find("Highlighted").gameObject.SetActive(false);
            }
            /**
             * Only highlight the hex
             */
            public void TargetingHighlight()
            {
                Hex.Find("Range").gameObject.SetActive(true);
            }

            /**
             * Remove the highlight
             */
            public void HideTargetingHighlight()
            {
                Hex.Find("Range").gameObject.SetActive(false);
            }
            
            public void Select()
            {
                Hex.Find("Selected").gameObject.SetActive(true);
            }

            public void HideSelect()
            {
                Hex.Find("Selected").gameObject.SetActive(false);
            }

            /**
             * Set the occupation state of hex
             *
             * <param name='occupied'> whether hex is occupied or not</param>
             * <param name='isMaterial'> which layer the occupant belongs to</param>
             */
            public void SetOccupied(bool occupied, bool isMaterial)
            {
                if (isMaterial)
                {
                    IsOccupiedMaterial = occupied;
                }
                else
                {
                    IsOccupiedImmaterial = occupied;
                }
            }
            /**
             * Get the occupation state of hex
             *
             * <param name='isMaterial'> which layer the occupation is checked upon</param>
             */
            public bool GetOccupied(bool isMaterial)
            {
                if (isMaterial)
                {
                    return IsOccupiedMaterial;
                }
                return IsOccupiedImmaterial;
            }
        }
        
        private void Awake()
        {
            _currentIsPlayer = false;
            _entityManager = GetComponent<EntityManager>();
            _grid = new Grid<GridObject>(width, height, cellSize, offset, (x, y) => new GridObject(x, y));
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Transform visual = Instantiate(hexPrefab, _grid.GetWorldPosition(x, y), Quaternion.identity);
                    _grid.GetGridObject(x, y).Hex = visual;
                    _grid.GetGridObject(x, y).HideSelect();
                    _grid.GetGridObject(x, y).HideHighlight();
                }
            }
        }

        private void OnEnable()
        {
            RescaleCamera();
        }

        private void RescaleCamera()
        {
            Camera.main.gameObject.transform.position = new Vector3(width * cellSize / 2, height * cellSize / 8, -10);
            Camera.main.GetComponent<Camera>().orthographicSize = cellSize * 3;
        }

        private void Update()
        {
            if (_currentGridObject is not null)
            {
                _currentGridObject.HideSelect();
            }
            
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            
            _currentGridObject = _grid.GetGridObject(mousePosition);
            
            if (_currentGridObject != null && _currentGridObject.IsHighlighted)
            {
                _currentGridObject.Select();
                if(Input.GetMouseButtonDown(0) && currentUnit.currentState == UnitCombat.State.Idling)
                {
                    MoveToSelected();
                }
            }
        }


        /**
         * Unselects every hex in grid
         */
        public void ClearGrid()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _grid.GetGridObject(i, j).HideHighlight();
                    _grid.GetGridObject(i, j).HideTargetingHighlight();
                    _grid.GetGridObject(i, j).HideSelect();
                }
            }
        }

        /**
         * Highlights every hex in a radius and makes it active
         * <param name='gridObject'> center of the hex circle</param>
         * <param name='radius'> - radius of the highlighted area</param>
         * <param name='highlightCenter'> should the center be highlighted</param>
         * <param name='currentIter'> which iteration of the algorithm is it (0 by default)</param>
         */
        private void HighlightNeighborsInRadius(GridObject gridObject, int radius, bool highlightCenter, int currentIter = 0)
        {
            if ((highlightCenter || currentIter != 0) && !gridObject.GetOccupied(_currentIsMaterial))
            {
                gridObject.Highlight();
            }
            
            
            if (currentIter == radius)
            {
                return;
            }
            
            foreach (GridObject neighbor in _grid.GetGridObjectNeighbours(gridObject.X, gridObject.Y))
            {
                if (neighbor != null)
                {
                    HighlightNeighborsInRadius(neighbor, radius, highlightCenter, currentIter+1);
                }
            }
            
        }
        
        /**
         * Highlights every hex in a radius
         * <param name='gridObject'> center of the hex circle</param>
         * <param name='radius'> - radius of the highlighted area</param>
         * <param name='highlightCenter'> should the center be highlighted</param>
         * <param name='currentIter'> which iteration of the algorithm is it (0 by default)</param>
         */
        private void TargetingHighlightNeighborsInRadius(GridObject gridObject, int radius, bool highlightCenter, int currentIter = 0)
        {
            if ((highlightCenter || currentIter != 0))
            {
                gridObject.TargetingHighlight();
            }
            
            
            if (currentIter == radius)
            {
                return;
            }
            
            foreach (GridObject neighbor in _grid.GetGridObjectNeighbours(gridObject.X, gridObject.Y))
            {
                if (neighbor != null)
                {
                    TargetingHighlightNeighborsInRadius(neighbor, radius, highlightCenter, currentIter+1);
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
            TargetingHighlightNeighborsInRadius(_grid.GetGridObject((int)currentUnit.GetGridLocation().x, (int)currentUnit.GetGridLocation().y), range, true);
        }

        /**
         * A function measuring distance between currently active unit and a target unit
         * <param name='to'>targeted unit</param>
         * <returns>a distance measured in hex map moves</returns>
         */
        public int GetDistance(UnitCombat to)
        {
            var vec = currentUnit.GetGridLocation();
            var coord = _grid.GetGridObject((int)vec[0], (int)vec[1]).XYZCoordinates;
            var vec2 = to.GetGridLocation();
            var coord2 = _grid.GetGridObject((int)vec2[0], (int)vec2[1]).XYZCoordinates;
            return Math.Max(Math.Max((int)Math.Abs(coord.x - coord2.x), (int)Math.Abs(coord.y - coord2.y)), (int)Math.Abs(coord.z - coord2.z));
        }
        
        /**
         * A function measuring distance between to units
         * <param name='to'>source unit</param>
         * <param name='from'>targeted unit</param>
         * <returns>a distance measured in hex map moves</returns>
         */
        private int GetDistance(GridObject from, GridObject to)
        {
            var coord = from.XYZCoordinates;
            var coord2 = to.XYZCoordinates;
            return Math.Max(Math.Max((int)Math.Abs(coord.x - coord2.x), (int)Math.Abs(coord.y - coord2.y)), (int)Math.Abs(coord.z - coord2.z));
        }


        /**
         * A* algorithm for finding best path between units
         * <param name='startHex'>beginning point of the path</param>
         * <param name='finishHex'>ending point of the path</param>
         * <returns>a list of hexes comprising the path, containing start and finish hexes</returns>
         */
        private List<GridObject> FindShortestPathTo(GridObject startHex, GridObject finishHex)
        {
            List<GridObject> openPathHexes = new List<GridObject>();
            List<GridObject> closedPathHexes = new List<GridObject>();

            GridObject currentHex = startHex;

            currentHex.G = 0;
            currentHex.H = GetDistance(currentHex, finishHex);

            openPathHexes.Add(currentHex);

            while (openPathHexes.Count > 0)
            {
                openPathHexes = openPathHexes.OrderBy(x => x.F).ThenByDescending(x => x.G).ToList();
                currentHex = openPathHexes[0];
                
                openPathHexes.Remove(currentHex);
                closedPathHexes.Add(currentHex);

                int g = currentHex.G + 1;

                if (closedPathHexes.Contains(finishHex))
                {
                    break;
                }

                foreach (GridObject neighborHex in _grid.GetGridObjectNeighbours(currentHex.X, currentHex.Y))
                {
                    if (neighborHex is null)
                    {
                        continue;
                    }
                    if (neighborHex.GetOccupied(_currentIsMaterial) && neighborHex!= finishHex)
                    {
                        continue;
                    }

                    if (closedPathHexes.Contains(neighborHex))
                    {
                        continue;
                    }

                    if (!(openPathHexes.Contains(neighborHex)))
                    {
                        neighborHex.G = g;
                        neighborHex.H = GetDistance(neighborHex, finishHex);
                        neighborHex.F = neighborHex.G + neighborHex.H;
                        openPathHexes.Add(neighborHex);
                    }
                    else if (neighborHex.F > g + neighborHex.H)
                    {
                        neighborHex.G = g;
                        neighborHex.F = neighborHex.G + neighborHex.H;
                    }
                }
            }

            List<GridObject> finalPathHexes = new List<GridObject>();

            if (closedPathHexes.Contains(finishHex))
            {
                currentHex = finishHex;
                finalPathHexes.Add(currentHex);

                for (int i = finishHex.G - 1; i >= 0; i--)
                {
                    currentHex = closedPathHexes.Find(x => x.G == i && _grid.GetGridObjectNeighbours(currentHex.X, currentHex.Y).Contains(x));
                    finalPathHexes.Add(currentHex);
                }

                finalPathHexes.Reverse();
            }

            return finalPathHexes;
        }
        
        /**
         * Algorithm moving an object to its correct place on a hex map
         * <param name='unitCombat'>moved unit</param>
         * <param name='isMaterial'>is the moved unit material</param>
         */
        public void SetEntityToGrid(UnitCombat unitCombat, bool isMaterial)
        {
            var vec = unitCombat.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(true, isMaterial);
            unitCombat.gameObject.transform.position = _grid.GetGridObject((int)vec[0], (int)vec[1]).Hex.position;
        }

        /**
         * Set the currently active unit
         * <param name='newUnit'>moved unit</param>
         */
        public void SetCurrentUnit(UnitCombat newUnit, int placeInQueue)
        {
            ClearGrid();
            currentUnit = newUnit;
            var vec = currentUnit.GetGridLocation();
            _currentGridObject = _grid.GetGridObject((int)vec[0], (int)vec[1]);
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
            _grid.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(false, _currentIsMaterial);
            ClearGrid();
                    
            currentUnit.StartMoving(_currentGridObject.Hex.position);
            currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.SetOccupied(true, _currentIsMaterial);
            if (_movementLeft > 0)
            {
                HighlightNeighborsInRadius(_currentGridObject, 1, false);
            }
        }

        
        /**
         * Moves the currently active unit to a specified position on the grid.
         * To be used for player controlled entities only.
         * <param name='x'>x coordinate on hex map</param>
         * <param name='y'>y coordinate on hex map</param>
         */
        private void MoveTo(int x, int y)
        {
            _movementLeft--;
            var vec = currentUnit.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(false, _currentIsMaterial);
            ClearGrid();

            _currentGridObject = _grid.GetGridObject(x, y);
            currentUnit.StartMoving(_currentGridObject.Hex.position);
            currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.SetOccupied(true, _currentIsMaterial);
        }

        /**
         * Begins the player turn by activating the UI and highlighting possible movement hexes
         * <param name='movement'>movement for the turn</param>
         * <param name='actions'>actions for the turn</param>
         * <param name='isMaterial'>is the player material</param>
         */
        public void PlayerTurnStart(int movement, int actions, bool isMaterial)
        {
            _currentIsPlayer = true;
            SetCurrentMaterial(isMaterial);
            ShowStage(isMaterial);
            _movementLeft = movement;
            _actionsLeft = actions;
            UpdateUICounters();
            combatUI.ShowAbilities();
            HighlightNeighborsInRadius(_currentGridObject, 1, false);
        }

        /**
         * Begins the enemy turn.
         * <param name='movement'>movement for the turn</param>
         * <param name='prefRange'>preferred range of the enemy</param>
         * <param name='target'>which player shall the enemy target</param>
         * <param name='movedEntity'>which entity is being moved</param>
         */
        public void MovementStage(int movement, int prefRange, UnitCombat target, EnemyScript movedEntity)
        {
            _currentIsPlayer = false;
            _movementLeft = movement;
            SetCurrentMaterial(movedEntity.IsMaterial());
            ShowStage(movedEntity.IsMaterial());
            StartCoroutine(MoveAlongPath(prefRange, target, movedEntity));
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
            ClearGrid();
            HighlightNeighborsInRadius(_grid.GetGridObject((int)currentUnit.GetGridLocation().x, (int)currentUnit.GetGridLocation().y), 1, false);
        }


        /**
         * A coroutine moving the enemy along the path to a hex on the preferred range from target player
         * <param name='prefRange'>preferred range of the enemy</param>
         * <param name='target'>which player shall the enemy target</param>
         * <param name='movedEntity'>which entity is being moved</param>
         */
        IEnumerator MoveAlongPath(int prefRange, UnitCombat target, EnemyScript movedEntity)
        {
            var vec = currentUnit.GetGridLocation();
            var currHex = _grid.GetGridObject((int)vec[0], (int)vec[1]);
            
            var vec2 = target.GetGridLocation();
            var finishHex = _grid.GetGridObject((int)vec2[0], (int)vec2[1]);
            
            var path = FindShortestPathTo(currHex, finishHex);
        
            for (int i = 1; i < path.Count - 1; i++)
            {
                if (GetDistance(currHex, finishHex) <= prefRange || _movementLeft <= 0)
                {
                    break;
                }
                var nextHex = path[i];
                MoveTo(nextHex.X, nextHex.Y);
                while (currentUnit.currentState != UnitCombat.State.Idling)
                {
                    yield return new WaitForSeconds(0.3f);
                }
                currHex = nextHex;
            }
            movedEntity.TurnAfterMovement();
        }

        /**
         * Update UI counters with current values
         */
        private void UpdateUICounters()
        {
            combatUI.SetMovementCounter(_movementLeft);
            combatUI.SetActionCounter(_actionsLeft);
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
            _grid.GetGridObject((int)pos.x, (int)pos.y).SetOccupied(false, isMaterial);
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
            var currPos = _grid.GetGridObject((int)pos.x, (int)pos.y);
            if (_currentIsPlayer)
            {
                if (isMaterial)
                {
                    if (_currentIsMaterial && _movementLeft > 0)
                    {
                        HighlightNeighborsInRadius(currPos, 1, false);
                    }
                    else
                    {
                        ClearGrid();
                    }

                }
                else
                {
                    if (!_currentIsMaterial && _movementLeft > 0)
                    {
                        HighlightNeighborsInRadius(currPos, 1, false);
                    }
                    else
                    {
                        ClearGrid();
                    }
                }
            }
        }
        
        /**
         * Show  the summary of a finished combat
         * <param name='IsVictory'>whether the player has won an encounter</param>
         * <param name='players'>list of players in combat</param>
         * <param name='enemies'>list of enemies in combat</param>
         */
        public void ShowCombatSummary(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
        {
            combatUI.DisableCombatUI();
            ClearGrid();
            combatUI.ShowCombatSummary(IsVictory, players, enemies);
        }

        public int ActionsLeft
        {
            get => _actionsLeft;
            set => _actionsLeft = value;
        }
        
        public void DisplayQueue(List<IHittable> entities)
        {
            combatUI.FillQueue(entities);
        }
        
        public void DisableDeadEntityInQueue(int n) {
            combatUI.DisableDeadEntityInQueue(n);
        }
    }
}
