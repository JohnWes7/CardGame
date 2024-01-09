using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGridNode : MonoBehaviour
{
    [SerializeField] private UnitObject content;
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private Grid<FGridNode> grid;

    public void SetContent(UnitObject content)
    {
        this.content = content;
    }

    public UnitObject GetContent()
    {
        return content;
    }

    public bool CanBuild()
    {
        return content == null;
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
