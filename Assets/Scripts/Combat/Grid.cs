using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector2 _originPosition;

    private TGridObject[,] _gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<int, int, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        _gridArray = new TGridObject[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x, y] = createGridObject(x, y);
            }
        }
    }

    //Function used to get XY of hex on game scene
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, 0) * _cellSize + 
                _cellSize * .75f * new Vector2(0, y) + 
               (y % 2 == 1 ?  _cellSize * .5f * new Vector2(1, 0) : Vector3.zero) + 
               _originPosition;
    }

    //Function used to get list of neighbours of hex with known XY on grid
    public List<Vector2Int> GetNeighboursXY(int x, int y)
    {
        Vector2Int hexXY = new Vector2Int(x, y);
        bool oddRow = hexXY.y % 2 == 1;        
        
        List<Vector2Int> neighbourXYList = new List<Vector2Int>()
        {
            hexXY + new Vector2Int(-1, 0),
            hexXY + new Vector2Int(1, 0),
            
            hexXY + new Vector2Int(oddRow ? 1 : -1, 1),
            hexXY + new Vector2Int(0, 1),
            
            hexXY + new Vector2Int(oddRow ? 1 : -1, -1),
            hexXY + new Vector2Int(0, -1)
        };
        return neighbourXYList;
    }

    //Function used to get hex's XY on grid
    public Tuple<int, int> GetXY(Vector2 worldPosition)
    {
        int roughX = Mathf.RoundToInt((worldPosition - _originPosition).x / _cellSize);
        int roughY = Mathf.RoundToInt((worldPosition - _originPosition).y / _cellSize / .75f);

        Vector2Int roughXY = new Vector2Int(roughX, roughY);

        Vector2Int closestXY = roughXY;

        foreach (Vector2Int neighbourXY in GetNeighboursXY(roughX, roughY))
        {
            if(Vector2.Distance(worldPosition, GetWorldPosition(neighbourXY.x, neighbourXY.y)) <
            Vector2.Distance(worldPosition, GetWorldPosition(closestXY.x, closestXY.y)))
            {
                closestXY = neighbourXY;
            }
        }
        
        return new Tuple<int, int>(closestXY.x, closestXY.y);
    }
    
    //Function used to get Grid Object, which known XY on grid
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            _gridArray[x, y] = value;
        }
    }

    //Function used to get Grid Object, which known XY on game scene
    public void SetGridObject(Vector2 worldPosition, TGridObject value)
    {
        Tuple<int, int> objectXY = GetXY(worldPosition);
        SetGridObject(objectXY.Item1, objectXY.Item2, value);
    }

    //Function used to get Grid Object, which known XY on grid
    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            return _gridArray[x, y];
        }
        else
        {
            return default;
        }
    }

    //Function used to get Grid Object, which known XY on game scene
    public TGridObject GetGridObject(Vector2 worldPosition)
    {
        Tuple<int, int> objectXY = GetXY(worldPosition);
        return GetGridObject(objectXY.Item1, objectXY.Item2);
    }
    
    //Function used to get Grid Objects, who are neighbours of , which known XY on grid
    public List<TGridObject> GetGridObjectNeighbours(int x, int y)
    {
        List<Vector2Int> neighboursXYList = GetNeighboursXY(x, y);
        List<TGridObject> gridObjects = new List<TGridObject>();
        
        foreach (Vector2Int neighbourXY in neighboursXYList)
        {
            gridObjects.Add(GetGridObject(neighbourXY.x, neighbourXY.y));
        }

        return gridObjects;
    }
}
