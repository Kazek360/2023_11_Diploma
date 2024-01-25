using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
        [SerializeField] private Transform hexPrefab;
        [SerializeField] private Camera mainCamera;
        private Grid<GridObject> _grid;
        [SerializeField] private UnitCombat unitCombat;
    
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

            unitCombat.gameObject.transform.position = _grid.GetGridObject(0, 0).Hex.position;
            _currentGridObjectNeighbours = _grid.GetGridObjectNeighbours(0, 0);
            foreach (GridObject gridObject in _currentGridObjectNeighbours)
            {
                if (gridObject != null)
                {
                    gridObject.Highlight();
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
                if(Input.GetMouseButtonDown(0) && unitCombat.currentState == UnitCombat.State.Idling)
                {
                    ClearGrid();
                    
                    unitCombat.StartMoving(_currentGridObject.Hex.position);
                    _currentGridObjectNeighbours = _grid.GetGridObjectNeighbours(_currentGridObject.X, _currentGridObject.Y);
                    
                    foreach (GridObject gridObject in _currentGridObjectNeighbours)
                    {
                        if (gridObject != null)
                        {
                            gridObject.Highlight();
                        }
                    }
                }
            }
        }


        /**
         * Unselects every hex in grid
         */
        private void ClearGrid()
        {
            foreach (GridObject gridObject in _currentGridObjectNeighbours)
            {
                if (gridObject != null)
                {
                    gridObject.HideHighlight();
                }
            }
        }

}
