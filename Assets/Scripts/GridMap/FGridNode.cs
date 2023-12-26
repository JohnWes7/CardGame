using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGridNode : MonoBehaviour
{
    private GameObject content;
    private int x;
    private int y;
    private Grid<FGridNode> grid;

    public void SetContent(GameObject content)
    {
        content = gameObject;
    }

    public GameObject GetContent()
    {
        return content;
    }

    public void SetPostion(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetGrid(Grid<FGridNode> grid)
    {
        this.grid = grid;
    }
}
