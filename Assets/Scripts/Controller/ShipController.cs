using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IShipController
{
    // 飞船核心数值
    [SerializeField] private int HP;
    [SerializeField] private Grid<FGridNode> grid;

    //grid参数
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;

    [Header("Debug")]
    [SerializeField] private bool showGridLine = true;

    [Header("初始化")]
    [SerializeField] private UnitSO core;

    public Grid<FGridNode> Grid { get => grid; set => grid = value; }

    private void Start()
    {
        grid = new Grid<FGridNode>(gridWidth, gridHeight, 1, FGridNodeConstructorFunc, Vector3.zero);


        // grid实体设置到飞船下面
        grid.GetParent().transform.SetParent(transform);
        grid.GetParent().transform.localPosition = new Vector3(-grid.GetRealWorldWidth() / 2, -grid.GetRealWorldHeight() / 2);

        // 生成核心
        Vector2Int coreInitPos = new((gridWidth / 2) - 1, (gridHeight / 2) - 1);
        List<FGridNode> place = grid.GetObjectPlaceByPosList(coreInitPos, core.place, Dir.up);
        UnitObject unitObject = UnitObject.UnitFactoryCreate(core, coreInitPos, Dir.up, grid);
        foreach (var item in place)
        {
            item.SetContent(unitObject);
        }


    }

    private void Update()
    {
        // debug
        if (showGridLine)
        {
            for (int i = 0; i < grid.GetHeight(); i++)
            {
                for (int j = 0; j < grid.GetWidth(); j++)
                {
                    Vector3 lb = grid.GetWorldPositionLeftBottom(j, i);
                    Vector3 lu = grid.GetWorldPositionLeftBottom(j, i + 1);
                    Vector3 rb = grid.GetWorldPositionLeftBottom(j + 1, i);
                    Debug.DrawLine(lb, lu, Color.white);
                    Debug.DrawLine(lb, rb, Color.white);
                }
            }
        }
    }

    public static TextMesh TextMeshConstructorFunc(Grid<TextMesh> grid, int x, int y, float cellsize, GameObject parent)
    {
        GameObject text = new("text", typeof(TextMesh));

        // 设置父物体
        text.transform.SetParent(parent.transform);
        // 设置位置
        text.transform.position = grid.GetWorldPositionLeftBottom(x, y);

        TextMesh textMesh = text.GetComponent<TextMesh>();
        textMesh.text = x + "," + y;
        textMesh.characterSize = 0.2f;

        return textMesh;
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="cellsize"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static FGridNode FGridNodeConstructorFunc(Grid<FGridNode> grid, int x, int y, float cellsize, GameObject parent)
    {
        GameObject go = new(x + "," + y, typeof(FGridNode));
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = grid.GetLocalPositionMiddleCenter(x, y);

        go.AddComponent<TextMesh>();
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.text = x + "," + y;
        textMesh.characterSize = 0.2f;

        FGridNode fGridNode = go.GetComponent<FGridNode>();
        fGridNode.SetPostion(x, y);

        return fGridNode;
    }
}
