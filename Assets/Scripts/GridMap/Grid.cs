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

    /// <summary>
    /// 以最左下角为基准计算所有需要的位置并返回位置的 T
    /// </summary>
    /// <param name="leftButtomPos"></param>
    /// <param name="placePoint"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public List<T> GetObjectPlaceByPosList(Vector2Int leftButtomPos, List<Vector2Int> placePoint, Dir dir)
    {
        List<T> result = new List<T>();
        switch (dir)
        {
            case Dir.up:
                foreach (var item in placePoint)
                {
                    Vector2Int position = leftButtomPos + item;
                    result.Add(GetGridObject(position));
                }
                break;
            case Dir.down:
                foreach (var item in placePoint)
                {
                    Vector2Int offset = new Vector2Int(-item.x, -item.y);
                    Vector2Int position = leftButtomPos + offset;
                    result.Add(GetGridObject(position));
                }
                break;
            case Dir.left:
                foreach (var item in placePoint)
                {
                    Vector2Int offset = new Vector2Int(-item.y, item.x);
                    Vector2Int position = leftButtomPos + offset;
                    result.Add(GetGridObject(position));
                }
                break;
            case Dir.right:
                foreach (var item in placePoint)
                {
                    Vector2Int offset = new Vector2Int(item.y, -item.x);
                    Vector2Int position = leftButtomPos + offset;
                    result.Add(GetGridObject(position));
                }
                break;
            default:
                break;
        }
        return result;
    }

    public List<T> GetRectObjectList(Vector2Int leftButtomPos, Vector2Int widthHeight, Dir dir)
    {
        List<T> result = new List<T>();
        switch (dir)
        {
            case Dir.up:
                for (int i = leftButtomPos.y; i < leftButtomPos.y + widthHeight.y; i++)
                {
                    for (int j = leftButtomPos.x; j < leftButtomPos.x + widthHeight.x; j++)
                    {
                        result.Add(content[j][i]);
                    }
                }
                break;
            case Dir.down:
                break;
            case Dir.left:
                break;
            case Dir.right:
                break;
            default:
                break;
        }

        return result;
    }

    public T GetGridObject(int x, int y)
    {
        if (x < 0 || x >= width)
        {
            return default(T);
        }
        if (y < 0 || y >= height)
        {
            return default(T);
        }

        return content[y][x];
    }

    public T GetGridObject(Vector2Int pos)
    {
        return GetGridObject(pos.x, pos.y);
    }

    public bool isOutOfBound(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width)
        {
            return false;
        }
        if (pos.y < 0 || pos.y >= height)
        {
            return false;
        }
        return true;
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

    public float GetRealWorldWidth()
    {
        return width * cellsize;
    }
    
    public float GetRealWorldHeight()
    {
        return height * cellsize;
    }
}
