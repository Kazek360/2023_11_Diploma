using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
        [SerializeField] private Transform hex;
        [SerializeField] private Camera mainCamera;
        private Grid<GridObject> _grid;
        [SerializeField] private UnitCombat unitCombat;
    
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        [SerializeField] private Vector2 offset;
        
        private GridObject _lastGridObject;
        
        private class GridObject
        {
            public Transform Visual;
    
            public void Show()
            {
                Visual.Find("Selected").gameObject.SetActive(true);
            }
    
            public void Hide()
            {
                Visual.Find("Selected").gameObject.SetActive(false);
            }
        }
        
        private void Awake()
        {
            _grid = new Grid<GridObject>(width, height, cellSize, offset, (Grid<GridObject> g, int x, int y) => new GridObject());
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Transform visual = Instantiate(hex, _grid.GetWorldPosition(x, y), Quaternion.identity);
                    _grid.GetGridObject(x, y).Visual = visual;
                    _grid.GetGridObject(x, y).Hide();
                }
            }

            unitCombat.gameObject.transform.position = _grid.GetGridObject(0, 0).Visual.position;
        }
    
        private void Update()
        {
            if (_lastGridObject != null)
            {
                _lastGridObject.Hide();
            }
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            
            _lastGridObject = _grid.GetGridObject(mousePosition);
            
            if (_lastGridObject != null)
            {
                _lastGridObject.Show();
                if (Input.GetMouseButtonDown(0))
                {
                    unitCombat.StartMoving(_lastGridObject.Visual.position);
                }
            }
        }
    
}
