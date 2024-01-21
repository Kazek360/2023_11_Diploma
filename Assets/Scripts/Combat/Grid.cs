using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;

    private TGridObject[,] _gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
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
                _gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0) * _cellSize + 
               new Vector3(0, y) * _cellSize * .75f + 
               (y % 2 == 1 ? new Vector3(1, 0) * _cellSize * .5f : Vector3.zero) + 
               _originPosition;
    }

    public Tuple<int, int> GetXY(Vector3 worldPosition)
    {
        int roughX = Mathf.RoundToInt((worldPosition - _originPosition).x / _cellSize);
        int roughY = Mathf.RoundToInt((worldPosition - _originPosition).y / _cellSize / .75f);

        Vector3Int roughXY = new Vector3Int(roughX, roughY);
        bool oddRow = roughY % 2 == 1;

        List<Vector3Int> neighbourXYList = new List<Vector3Int>()
        {
            roughXY + new Vector3Int(-1, 0),
            roughXY + new Vector3Int(1, 0),
            
            roughXY + new Vector3Int(oddRow ? 1 : -1, 1),
            roughXY + new Vector3Int(0, 1),
            
            roughXY + new Vector3Int(oddRow ? 1 : -1, -1),
            roughXY + new Vector3Int(0, -1)
        };

        Vector3Int closestXY = roughXY;

        foreach (Vector3Int neighbourXY in neighbourXYList)
        {
            if(Vector3.Distance(worldPosition, GetWorldPosition(neighbourXY.x, neighbourXY.y)) <
            Vector3.Distance(worldPosition, GetWorldPosition(closestXY.x, closestXY.y)))
            {
                closestXY = neighbourXY;
            }
        }
        
        return new Tuple<int, int>(closestXY.x, closestXY.y);
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            _gridArray[x, y] = value;
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        SetGridObject(GetXY(worldPosition).Item1, GetXY(worldPosition).Item2, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            return _gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        return GetGridObject(GetXY(worldPosition).Item1, GetXY(worldPosition).Item2);
    }
}
