using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class CombatSystem_2 : MonoBehaviour
{
        [FormerlySerializedAs("hexPrefab")] [SerializeField] private Transform HexPrefab;
        [FormerlySerializedAs("mainCamera")] [SerializeField] private Camera MainCamera;
        private Grid<GridObject> _grid;
        [FormerlySerializedAs("currentUnit")] [SerializeField] private UnitCombat CurrentUnit;
        [FormerlySerializedAs("combatUI")] [SerializeField] private CombatUI CombatUI;
    
        [FormerlySerializedAs("width")] [SerializeField] private int Width;
        [FormerlySerializedAs("height")] [SerializeField] private int Height;
        [FormerlySerializedAs("cellSize")] [SerializeField] private float CellSize;
        [FormerlySerializedAs("offset")] [SerializeField] private Vector2 Offset;

        private EntityManager _entityManager;
        private GridObject _currentGridObject;
        private List<GridObject> _currentGridObjectNeighbours;
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

            public void Highlight()
            {
                IsHighlighted = true;
                Hex.GetChild(1).gameObject.SetActive(true);
            }

            public void HideHighlight()
            {
                IsHighlighted = false;
                Hex.GetChild(1).gameObject.SetActive(false);
            }
            
            public void Select()
            {
                Hex.GetChild(2).gameObject.SetActive(true);
            }

            public void HideSelect()
            {
                Hex.GetChild(2).gameObject.SetActive(false);
            }

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
            _grid = new Grid<GridObject>(Width, Height, CellSize, Offset, (x, y) => new GridObject(x, y));
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Transform visual = Instantiate(HexPrefab, _grid.GetWorldPosition(x, y), Quaternion.identity);
                    _grid.GetGridObject(x, y).Hex = visual;
                    _grid.GetGridObject(x, y).HideSelect();
                    _grid.GetGridObject(x, y).HideHighlight();
                }
            }

        }
    
        private void Update()
        {
            if (_currentGridObject != null)
            {
                _currentGridObject.HideSelect();
            }
            
            Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            
            _currentGridObject = _grid.GetGridObject(mousePosition);
            
            if (_currentGridObject != null && _currentGridObject.IsHighlighted)
            {
                _currentGridObject.Select();
                if(Input.GetMouseButtonDown(0) && CurrentUnit.currentState == UnitCombat.State.Idling)
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
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _grid.GetGridObject(i, j).HideHighlight();
                    _grid.GetGridObject(i, j).HideSelect();
                }
            }
        }

        /**
         * Highlights every hex in a radius
         *
         * gridObject - center
         * radius - highlight radius
         * currentIter - how long has algorithm gone on(0 by default)
         * highlight center - should we highlight center?
         */
        private void HighlightNeighborsInRadius(GridObject gridObject, int radius, int currentIter, bool highlightCenter)
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
                    HighlightNeighborsInRadius(neighbor, radius, currentIter+1, highlightCenter);
                }
            }
            
        }

        public int GetDistance(UnitCombat to)
        {
            var vec = CurrentUnit.GetGridLocation();
            var coord = _grid.GetGridObject((int)vec[0], (int)vec[1]).XYZCoordinates;
            var vec2 = to.GetGridLocation();
            var coord2 = _grid.GetGridObject((int)vec2[0], (int)vec2[1]).XYZCoordinates;
            return Math.Max(Math.Max((int)Math.Abs(coord.x - coord2.x), (int)Math.Abs(coord.y - coord2.y)), (int)Math.Abs(coord.z - coord2.z));
        }
        
        public int GetDistance(GridObject from, GridObject to)
        {
            var coord = from.XYZCoordinates;
            var coord2 = to.XYZCoordinates;
            return Math.Max(Math.Max((int)Math.Abs(coord.x - coord2.x), (int)Math.Abs(coord.y - coord2.y)), (int)Math.Abs(coord.z - coord2.z));
        }

        /**
         * A* algorithm for finding best path between units
         *
         * startHex, finishHex - beginning and ending points of a path
         * 
         * WARNING: RETURNED LIST CONTAINS BOTH STARTING AND FINISH HEX
         */
        public List<GridObject> FindShortestPathTo(GridObject startHex, GridObject finishHex)
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
        
        public void SetEntityToGrid(UnitCombat unitCombat, bool isMaterial)
        {
            var vec = unitCombat.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(true, isMaterial);
            unitCombat.gameObject.transform.position = _grid.GetGridObject((int)vec[0], (int)vec[1]).Hex.position;
        }

        public void SetCurrentUnit(UnitCombat newUnit)
        {
            ClearGrid();
            CurrentUnit = newUnit;
            var vec = CurrentUnit.GetGridLocation();
            _currentGridObject = _grid.GetGridObject((int)vec[0], (int)vec[1]);
            _currentGridObjectNeighbours = _grid.GetGridObjectNeighbours((int)vec[0], (int)vec[1]);
        }

        public void MoveToSelected()
        {
            _movementLeft--;
            UpdateUICounters();
            var vec = CurrentUnit.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(false, _currentIsMaterial);
            ClearGrid();
                    
            CurrentUnit.StartMoving(_currentGridObject.Hex.position);
            CurrentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.SetOccupied(true, _currentIsMaterial);
            if (_movementLeft > 0)
            {
                HighlightNeighborsInRadius(_currentGridObject, 1, 0, false);
            }
        }

        public void MoveTo(int x, int y)
        {
            _movementLeft--;
            var vec = CurrentUnit.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).SetOccupied(false, _currentIsMaterial);
            ClearGrid();

            _currentGridObject = _grid.GetGridObject(x, y);
            CurrentUnit.StartMoving(_currentGridObject.Hex.position);
            CurrentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.SetOccupied(true, _currentIsMaterial);
        }

        public void PlayerTurnStart(int movement, int actions, bool isMaterial)
        {
            _currentIsPlayer = true;
            SetCurrentMaterial(isMaterial);
            ShowStage(isMaterial);
            _movementLeft = movement;
            _actionsLeft = actions;
            UpdateUICounters();
            CombatUI.ShowAbilities();
            HighlightNeighborsInRadius(_currentGridObject, 1, 0, false);
        }

        public void MovementStage(int movement, int prefRange, UnitCombat target, EnemyScript movedEntity)
        {
            _currentIsPlayer = false;
            _movementLeft = movement;
            SetCurrentMaterial(movedEntity.IsMaterial());
            ShowStage(movedEntity.IsMaterial());
            StartCoroutine(MoveAlongPath(prefRange, target, movedEntity));
        }
        
        

        IEnumerator MoveAlongPath(int prefRange, UnitCombat target, EnemyScript movedEntity)
        {
            var vec = CurrentUnit.GetGridLocation();
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
                while (CurrentUnit.currentState != UnitCombat.State.Idling)
                {
                    yield return new WaitForSeconds(0.3f);
                }
                currHex = nextHex;
            }
            movedEntity.TurnAfterMovement();
        }

        private void UpdateUICounters()
        {
            CombatUI.SetMovementCounter(_movementLeft);
            CombatUI.SetActionCounter(_actionsLeft);
        }

        public CombatUI GetCombatUI()
        {
            return CombatUI;
        }

        public void EnemyDefeated(UnitCombat unitDefeated, bool isMaterial)
        {
            var pos = unitDefeated.GetGridLocation();
            _grid.GetGridObject((int)pos.x, (int)pos.y).SetOccupied(false, isMaterial);
        }

        public void SetCurrentMaterial(bool isMaterial)
        {
            _currentIsMaterial = isMaterial;
            CombatUI.SetMaterialToggle(isMaterial);
        }
        public void ShowStage(bool isMaterial)
        {
            _entityManager.ShowEntities(isMaterial);
            var pos = CurrentUnit.GetGridLocation();
            var currPos = _grid.GetGridObject((int)pos.x, (int)pos.y);
            if (_currentIsPlayer)
            {
                if (isMaterial)
                {
                    if (_currentIsMaterial)
                    {
                        HighlightNeighborsInRadius(currPos, 1, 0, false);
                    }
                    else
                    {
                        ClearGrid();
                    }

                }
                else
                {
                    if (!_currentIsMaterial)
                    {
                        HighlightNeighborsInRadius(currPos, 1, 0, false);
                    }
                    else
                    {
                        ClearGrid();
                    }
                }
            }
        }
        
        public void ShowCombatSummary(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
        {
            CombatUI.ShowCombatSummary(IsVictory, players, enemies);
        }

        public int ActionsLeft
        {
            get => _actionsLeft;
            set => _actionsLeft = value;
        }
}
