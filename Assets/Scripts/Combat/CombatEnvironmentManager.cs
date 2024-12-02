using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CombatEnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform hexPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject background;

    [SerializeField] [TextArea] private String arenaString;
    
    private Grid<GridObject> _grid;
    private GridObject _currentGridObject;

    // Start is called before the first frame update
    private void Awake()
    {
        InitGrid();
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
        background.transform.position = new Vector3(width * cellSize / 2, height * cellSize / 2, -5);
        background.transform.localScale *= cellSize / 1.25f;
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
                if (_grid.GetGridObject(i, j) != null)
                {
                    _grid.GetGridObject(i, j).HideHighlight();
                    _grid.GetGridObject(i, j).HideTargetingHighlight();
                    _grid.GetGridObject(i, j).HideSelect();
                }
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
    public void HighlightNeighborsInRadius(GridObject gridObject, int radius, bool highlightCenter,
        bool _currentIsMaterial, int currentIter = 0)
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
                HighlightNeighborsInRadius(neighbor, radius, highlightCenter, _currentIsMaterial, currentIter + 1);
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
    public void TargetingHighlightNeighborsInRadius(GridObject gridObject, int radius, bool highlightCenter,
        int currentIter = 0)
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
                TargetingHighlightNeighborsInRadius(neighbor, radius, highlightCenter, currentIter + 1);
            }
        }
    }

    /**
     * Highlights every hex in the range of an ability
     *
     * <param name="currentUnit">Unit for which range is calculated</param>
     * <param name="range">Range of the ability</param>
     */
    public void UnityTargetingHighlight(UnitCombat currentUnit, int range)
    {
        TargetingHighlightNeighborsInRadius(
            _grid.GetGridObject((int)currentUnit.GetGridLocation().x, (int)currentUnit.GetGridLocation().y), range,
            true);
    }

    /**
     * A function measuring distance between currently active unit and a target unit
     * <param name='currentUnit'>Starting unit</param>
     * <param name='to'>targeted unit</param>
     * <returns>a distance measured in hex map moves</returns>
     */
    public int GetDistance(UnitCombat currentUnit, UnitCombat to)
    {
        var vec = currentUnit.GetGridLocation();
        var coord = _grid.GetGridObject((int)vec[0], (int)vec[1]).XYZCoordinates;
        var vec2 = to.GetGridLocation();
        var coord2 = _grid.GetGridObject((int)vec2[0], (int)vec2[1]).XYZCoordinates;
        return Math.Max(Math.Max((int)Math.Abs(coord.x - coord2.x), (int)Math.Abs(coord.y - coord2.y)),
            (int)Math.Abs(coord.z - coord2.z));
    }

    /**
     * A function measuring distance between to units
     * <param name='to'>source unit</param>
     * <param name='from'>targeted unit</param>
     * <returns>a distance measured in hex map moves</returns>
     */
    public int GetDistance(GridObject from, GridObject to)
    {
        var coord = from.XYZCoordinates;
        var coord2 = to.XYZCoordinates;
        return Math.Max(Math.Max((int)Math.Abs(coord.x - coord2.x), (int)Math.Abs(coord.y - coord2.y)),
            (int)Math.Abs(coord.z - coord2.z));
    }


    /**
     * A* algorithm for finding best path between units
     * <param name='startHex'>beginning point of the path</param>
     * <param name='finishHex'>ending point of the path</param>
     * <returns>a list of hexes comprising the path, containing start and finish hexes</returns>
     */
    public List<GridObject> FindShortestPathTo(GridObject startHex, GridObject finishHex, bool _currentIsMaterial)
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

                if (neighborHex.GetOccupied(_currentIsMaterial) && neighborHex != finishHex)
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
                currentHex = closedPathHexes.Find(x =>
                    x.G == i && _grid.GetGridObjectNeighbours(currentHex.X, currentHex.Y).Contains(x));
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

    public GridObject GetCurrentGridObject()
    {
        return _currentGridObject;
    }

    public Grid<GridObject> GetGrid()
    {
        return _grid;
    }
    
    public GridObject GetGridObject(int x, int y)
    {
        InitGrid();
        return _grid.GetGridObject(x, y);
    }

    private void InitGrid()
    {
        String[] arenaLines = arenaString.Split("\n");
        height = arenaLines.Length;
        Char[][] tiles = new char[height][];
        for(int i = arenaLines.Length - 1; i >= 0; i--)
        {
            width = Math.Max(arenaLines[i].Length, width);
        }
        for(int i = arenaLines.Length - 1; i >= 0; i--)
        {
            tiles[arenaLines.Length - 1 - i] = new char[width];
            for (int j = 0; j < arenaLines[i].Length; j++)
            {
                tiles[arenaLines.Length - 1 - i][j] = arenaLines[i].ElementAt(j);
            }
        }
        if (_grid is null)
        {
            _grid = new Grid<GridObject>(width, height, cellSize, offset, (x, y) => new GridObject(x, y), tiles);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[y][x].Equals('1'))
                    {
                        Transform visual = Instantiate(hexPrefab, _grid.GetWorldPosition(x, y), Quaternion.identity,
                            gameObject.transform);
                        _grid.GetGridObject(x, y).Hex = visual;
                        _grid.GetGridObject(x, y).HideSelect();
                        _grid.GetGridObject(x, y).HideHighlight();
                    }
                }
            }
        }
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
    public Vector3 XYZCoordinates; //Needed for A*

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