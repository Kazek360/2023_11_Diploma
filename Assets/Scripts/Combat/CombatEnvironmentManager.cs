using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform hexPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector2 offset;
    
    private Grid<GridObject> _grid;
    // Start is called before the first frame update
    private void Awake()
    {
        _grid = new Grid<GridObject>(width, height, cellSize, offset, (x, y) => new GridObject(x, y));
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform visual = Instantiate(hexPrefab, _grid.GetWorldPosition(x, y), Quaternion.identity, gameObject.transform);
                _grid.GetGridObject(x, y).Hex = visual;
                _grid.GetGridObject(x, y).HideSelect();
                _grid.GetGridObject(x, y).HideHighlight();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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