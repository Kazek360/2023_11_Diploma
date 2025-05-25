using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Combat;
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
    [SerializeField] private GameObject background_nawia;

    [SerializeField] [TextArea] private String arenaString;
    
    private Grid<GridObject> _grid;
    private GridObject _currentGridObject;

    private CombatSystem _combatSystem;

    // Start is called before the first frame update
    private void Awake()
    {
        InitGrid();
    }
    
    private void Update()
    {
        if (_currentGridObject is not null && !CombatUI.IsTargeting)
        {
            _currentGridObject.HideSelect();
        }
            
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
            
        _currentGridObject = _grid.GetGridObject(mousePosition);
        
        if (_currentGridObject != null && _currentGridObject.IsHighlighted && !CombatUI.IsTargeting)
        {
            _currentGridObject.Select(_combatSystem.GetCurrentIsMaterial());
        }

        if (CombatUI.IsTargeting && CombatUI.IsTargetingAreaRadius > 0)
        {
            DeselectGrid();
            if (_currentGridObject is not null && _currentGridObject.IsInRange)
            {
                _currentGridObject.HideSelect();
                TargetingHighlightNeighborsInRadiusArea(
                    _currentGridObject, 
                    CombatUI.IsTargetingAreaRadius, 
                    true
                    );
            }
        }

        if (Input.GetMouseButtonDown(0) && CombatUI.IsTargeting && _currentGridObject is { IsSelected: true })
        {
            _combatSystem.GetCombatUI().SetTarget(_currentGridObject);
        }
    }

    private void OnEnable()
    {
        RescaleCamera();
    }

    private void RescaleCamera()
    {
        Camera.main.gameObject.transform.position = new Vector3(width * cellSize / 2, height * cellSize / 8, -10);
        Camera.main.GetComponent<Camera>().orthographicSize = cellSize * 3.5f;
        
        background.transform.position = new Vector3(width * cellSize / 2, height * cellSize / 8, -5);
        background.transform.localScale *= 2.755f / cellSize;
        
        background_nawia.transform.position = new Vector3(width * cellSize / 2, height * cellSize / 8, -5);
        background_nawia.transform.localScale *= 2.755f / cellSize;
    }


    /**
     * Removes all additions from every hex in grid
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
     * Unselects every hex in grid
     */
    public void DeselectGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_grid.GetGridObject(i, j) != null)
                {
                    _grid.GetGridObject(i, j).HideSelect();
                }
            }
        }
    }
    
    /**
     * Returns every hex in a radius
     * <param name='gridObject'> center of the hex circle</param>
     * <param name='radius'> - radius of the contained area</param>
     * <param name='containCenter'> should the center be contained</param>
     * <param name='currentIter'> which iteration of the algorithm is it (0 by default)</param>
     * <param name='currentList'> the neighbors we have found up to now</param>
     */
    public List<GridObject> GetNeighborsInRadius(GridObject gridObject, int radius, bool containCenter, int currentIter = 0, List<GridObject> currentList = null)
    {
        if (currentList is null)
        {
            currentList = new List<GridObject>();
        }
        if ((containCenter || currentIter != 0) && !currentList.Contains(gridObject))
        {
            currentList.Add(gridObject);
        }


        if (currentIter == radius)
        {
            return currentList;
        }

        foreach (GridObject neighbor in _grid.GetGridObjectNeighbours(gridObject.X, gridObject.Y))
        {
            if (neighbor != null)
            {
                currentList = GetNeighborsInRadius(neighbor, radius, containCenter, currentIter + 1, currentList);
            }
        }

        return currentList;
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
            gridObject.Highlight(_currentIsMaterial);
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
        HashSet<(int, int)> targetPositions, int currentIter = 0)
    {
        if ((highlightCenter || currentIter != 0))
        {
            if (targetPositions.Contains((gridObject.X, gridObject.Y)))
            {
                gridObject.TargetingHighlight(_combatSystem.GetCurrentIsMaterial());
                gridObject.Select(_combatSystem.GetCurrentIsMaterial());
            }
            else
            {
                gridObject.TargetingHighlight(_combatSystem.GetCurrentIsMaterial());
            }
        }


        if (currentIter == radius)
        {
            return;
        }

        foreach (GridObject neighbor in _grid.GetGridObjectNeighbours(gridObject.X, gridObject.Y))
        {
            if (neighbor != null)
            {
                TargetingHighlightNeighborsInRadius(neighbor, radius, highlightCenter, targetPositions, currentIter + 1);
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
    public void TargetingHighlightNeighborsInRadiusArea(GridObject gridObject, int radius, bool highlightCenter, int currentIter = 0)
    {
        if ((highlightCenter || currentIter != 0))
        {
            gridObject.Select(_combatSystem.GetCurrentIsMaterial());
        }


        if (currentIter == radius)
        {
            return;
        }

        foreach (GridObject neighbor in _grid.GetGridObjectNeighbours(gridObject.X, gridObject.Y))
        {
            if (neighbor != null)
            {
                TargetingHighlightNeighborsInRadiusArea(neighbor, radius, highlightCenter, currentIter + 1);
            }
        }
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


    public List<GridObject> FindShortestPathToDistance(GridObject startHex, GridObject finishHex, bool _currentIsMaterial, int desiredDist)
    {
        List<GridObject> result = null;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GridObject curr = _grid.GetGridObject(i, j);
                if (curr is null)
                {
                    continue;
                }
                if (GetDistance(curr, finishHex) != desiredDist)
                {
                    continue;
                }

                if (result is null)
                {
                    result = FindShortestPathTo(startHex, curr, _currentIsMaterial);
                }
                else
                {
                    var tmp = FindShortestPathTo(startHex, curr, _currentIsMaterial);
                    if (tmp.Count < result.Count)
                    {
                        result = tmp;
                    }
                }
            }
        }

        
        return result;
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
    
    public GridObject GetGridObject(Vector2 vec)
    {
        InitGrid();
        return _grid.GetGridObject((int)vec.x, (int)vec.y);
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

    public void SetCombatSystem(CombatSystem combatSystem)
    {
        _combatSystem = combatSystem;
    }

    private bool GetCurrentIsMaterial()
    {
        if (_combatSystem is null)
        {
            return true;
        }

        return _combatSystem.GetCurrentIsMaterial();
    }

    public void SetLayerVariant(bool isMaterial)
    {
        background.gameObject.SetActive(isMaterial);
        background_nawia.gameObject.SetActive(!isMaterial);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid.GetGridObject(x, y) is not null)
                {
                    _grid.GetGridObject(x, y).SetMaterial(isMaterial);
                }
            }
        }
    }
}

public class GridObject
{
    public Transform Hex;
    public bool IsHighlighted;
    public bool IsInRange;
    public bool IsSelected;
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
        IsInRange = false;
        IsSelected = false;
    }

    public void SetMaterial(bool isMaterial)
    {
        Hex.Find("Default").gameObject.SetActive(isMaterial);
        Hex.Find("Default_nawia").gameObject.SetActive(!isMaterial);
    }

    /**
     * Highlight the hex and make it active for movement
     */
    public void Highlight(bool isMaterial)
    {
        IsHighlighted = true;
        if (isMaterial)
        {
            Hex.Find("Highlighted").gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("HIGHLIGHTING_NAWIA");
            Hex.Find("Highlighted_nawia").gameObject.SetActive(true);
        }
    }

    /**
     * Remove the highlight and deactivate
     */
    public void HideHighlight()
    {
        IsHighlighted = false;
        Hex.Find("Highlighted").gameObject.SetActive(false);
        Hex.Find("Highlighted_nawia").gameObject.SetActive(false);
    }

    /**
     * Only highlight the hex
     */
    public void TargetingHighlight(bool isMaterial)
    {
        IsInRange = true;
        if (isMaterial)
        {
            Hex.Find("Range").gameObject.SetActive(true);
        }
        else
        {
            Hex.Find("Range_nawia").gameObject.SetActive(true);
        }
    }

    /**
     * Remove the highlight
     */
    public void HideTargetingHighlight()
    {
        IsInRange = false;
        Hex.Find("Range").gameObject.SetActive(false);
        Hex.Find("Range_nawia").gameObject.SetActive(false);
    }

    public void Select(bool isMaterial)
    {
        IsSelected = true;
        if (isMaterial)
        {
            Hex.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            Hex.Find("Selected_nawia").gameObject.SetActive(true);
        }
    }

    public void HideSelect()
    {
        IsSelected = false;
        Hex.Find("Selected").gameObject.SetActive(false);
        Hex.Find("Selected_nawia").gameObject.SetActive(false);
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