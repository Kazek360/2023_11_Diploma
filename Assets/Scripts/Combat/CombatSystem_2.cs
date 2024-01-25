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
        
        private class GridObject
        {
            public Transform Hex;
            public bool IsHighlighted;
            public int X;
            public int Y;

            public GridObject(int x, int y)
            {
                X = x;
                Y = y;
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
                    ClearGrid();
                    
                    currentUnit.StartMoving(_currentGridObject.Hex.position);
                    currentUnit.SetGridLocation(_currentGridObject.X, _currentGridObject.Y);
                    HighlightNeighborsInRadius(_currentGridObject, 1, 0, false);
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
            if (highlightCenter || currentIter != 0)
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

        public int GetDistanceTo(UnitCombat to)
        {
            return 0;
        }

        public int GetPathTo(UnitCombat to)
        {
            throw new NotImplementedException();
        }
        
        public void SetEntityToGrid(UnitCombat unitCombat)
        {
            var vec = unitCombat.GetGridLocation();
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

        public void MoveTo(int x, int y)
        {
            currentUnit.StartMoving(_grid.GetGridObject(x, y).Hex.position);
        }

        public void PlayerTurnStart()
        {
            Debug.Log("PLAYER SET AS UNIT");
            HighlightNeighborsInRadius(_currentGridObject, 1, 0, false);
        }
}
