using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    private List<List<T>> gridObject;

    private int height;
    private int width;
    private float cellsize;

    private GameObject parent;
    private List<List<T>> content;



    public Grid (int height, int width, float cellsize, Func<Grid<T>, int, int, float, GameObject, T> constructor, Vector3 originalPos)
    {
        this.height = height;
        this.width = width;
        this.cellsize = cellsize;

        // new parent
        parent = new GameObject("GridParent");
        parent.transform.position = originalPos;

        content = new List<List<T>>();

        for (int i = 0; i < height; i++)
        {
            List<T> oneline = new List<T>();
            for (int j = 0; j < width; j++)
            {
                T obj = constructor.Invoke(this, j, i, cellsize, parent);
                oneline.Add(obj);
                //Debug.DrawLine(GetWorldPositionLeftBottom(j, i), GetWorldPositionLeftBottom(j, i + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPositionLeftBottom(j, i), GetWorldPositionLeftBottom(j + 1, i), Color.white, 100f);
            }

            content.Add(oneline);
        }
    }

    public Vector3 GetWorldPositionLeftBottom(int x, int y)
    {
        return GetLocalPositionLeftBottom(x, y) + parent.transform.position;
    }

    public Vector3 GetWorldPositionMiddelCenter(int x, int y)
    {
        return GetLocalPositionMiddleCenter(x, y) + parent.transform.position;
    }

    public Vector3 GetLocalPositionLeftBottom(int x, int y)
    {
        Vector3 offset = new Vector3(x * cellsize, y * cellsize);
        return offset;
    }

    public Vector3 GetLocalPositionMiddleCenter(int x, int y)
    {
        Vector3 offset = new Vector3(x * cellsize + cellsize / 2, y * cellsize + cellsize / 2);
        return offset;
    }

    public T GetGridObject(int x, int y)
    {
        return content[x][y];
    }

    public Vector2Int LocalPositionToGridXY(Vector3 position)
    {
        int x = 0;
        if (position.x != 0)
        {
            x = Mathf.FloorToInt(position.x / cellsize);
        }
        int y = 0;
        if (position.y != 0)
        {
            y = Mathf.FloorToInt(position.y / cellsize);
        }

        return new Vector2Int(x, y);
    }

    public Vector2Int WorldPositionToGridXY(Vector3 position)
    {
        Vector3 localpos = parent.transform.InverseTransformPoint(position);
        return LocalPositionToGridXY(localpos);
    }

    public void SetOringnal(Vector3 pos)
    {
        if (parent)
        {
            parent.transform.position = pos;
        }
    }

    public Vector3 GetOringnal(Vector3 pos)
    {
        if (parent)
        {
            return parent.transform.position;
        }

        return default(Vector3);
    }

    public GameObject GetParent()
    {
        if (!parent)
        {
            parent = new GameObject();
            parent.transform.position = Vector3.zero;
        }

        return parent;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

}
