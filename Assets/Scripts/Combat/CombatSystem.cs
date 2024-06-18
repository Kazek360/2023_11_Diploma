using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Deprecated 
 */
public class CombatSystem : MonoBehaviour
{
        [FormerlySerializedAs("hexPrefab")] [SerializeField] private Transform HexPrefab;
        [FormerlySerializedAs("mainCamera")] [SerializeField] private Camera MainCamera;
        private Grid<GridObject> _grid;
        [FormerlySerializedAs("unitCombat")] [SerializeField] private UnitCombat UnitCombat;
    
        [FormerlySerializedAs("width")] [SerializeField] private int Width;
        [FormerlySerializedAs("height")] [SerializeField] private int Height;
        [FormerlySerializedAs("cellSize")] [SerializeField] private float CellSize;
        [FormerlySerializedAs("offset")] [SerializeField] private Vector2 Offset;
        
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

            UnitCombat.gameObject.transform.position = _grid.GetGridObject(0, 0).Hex.position;
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
            
            Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            
            _currentGridObject = _grid.GetGridObject(mousePosition);
            
            if (_currentGridObject != null && _currentGridObject.IsHighlighted)
            {
                _currentGridObject.Select();
                if(Input.GetMouseButtonDown(0) && UnitCombat.currentState == UnitCombat.State.Idling)
                {
                    ClearGrid();
                    
                    UnitCombat.StartMoving(_currentGridObject.Hex.position);
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
