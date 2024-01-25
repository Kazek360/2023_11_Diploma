using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CombatSystem_2 : MonoBehaviour
{
    [SerializeField] private Transform hexPrefab;
        [SerializeField] private Camera mainCamera;
        private Grid<GridObject> _grid;
        [SerializeField] private UnitCombat currentUnit;
    
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        [SerializeField] private Vector2 offset;
        
        private GridObject _currentGridObject;
        private List<GridObject> _currentGridObjectNeighbours;
        
        public class GridObject
        {
            public Transform Hex;
            public bool IsHighlighted;
            public int X;
            public int Y;
            public bool IsOccupied;
            public Vector3 XYZCoordinates;      //Needed for A*

            public GridObject(int x, int y)
            {
                X = x;
                Y = y;
                XYZCoordinates.y = Y;
                XYZCoordinates.x = X - (int)(Y / 2);
                XYZCoordinates.z = 0 - XYZCoordinates.x - XYZCoordinates.y;
                IsOccupied = false;
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
        }
        
        private void Awake()
        {
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
    
        private void Update()
        {
            if (_currentGridObject != null)
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
            if ((highlightCenter || currentIter != 0) && !gridObject.IsOccupied)
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
            var vec = currentUnit.GetGridLocation();
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
         */
        public List<GridObject> GetShortestPathTo(UnitCombat to)
        {
            throw new NotImplementedException();
        }
        
        public void SetEntityToGrid(UnitCombat unitCombat)
        {
            var vec = unitCombat.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).IsOccupied = true;
            unitCombat.gameObject.transform.position = _grid.GetGridObject((int)vec[0], (int)vec[1]).Hex.position;
        }

        public void SetCurrentUnit(UnitCombat newUnit)
        {
            ClearGrid();
            currentUnit = newUnit;
            var vec = currentUnit.GetGridLocation();
            Debug.Log((int)vec[0] + " " + (int)vec[1]);
            _currentGridObject = _grid.GetGridObject((int)vec[0], (int)vec[1]);
            _currentGridObjectNeighbours = _grid.GetGridObjectNeighbours((int)vec[0], (int)vec[1]);
        }

        public void MoveToSelected()
        {
            var vec = currentUnit.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).IsOccupied = false;
            ClearGrid();
                    
            currentUnit.StartMoving(_currentGridObject.Hex.position);
            currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.IsOccupied = true;
            HighlightNeighborsInRadius(_currentGridObject, 1, 0, false);
        }

        public void MoveTo(int x, int y)
        {
            var vec = currentUnit.GetGridLocation();
            _grid.GetGridObject((int)vec[0], (int)vec[1]).IsOccupied = false;
            ClearGrid();

            _currentGridObject = _grid.GetGridObject(x, y);
            currentUnit.StartMoving(_currentGridObject.Hex.position);
            currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
            _currentGridObject.IsOccupied = true;
        }

        public void PlayerTurnStart()
        {
            Debug.Log("PLAYER SET AS UNIT");
            HighlightNeighborsInRadius(_currentGridObject, 1, 0, false);
        }
}
