using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 玩家数值
    [SerializeField] private int HP;
    [SerializeField] private int cost;


    // 玩家卡牌总共卡牌
    [SerializeField] private List<Card> mCards;
    [SerializeField] private Grid<FGridNode> grid;

    [Header("Debug")]
    private bool showGridLine = true;

    private void Start()
    {
        grid = new Grid<FGridNode>(5, 5, 1, FGridNodeConstructorFunc, new Vector3(1, 1));
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridXY = grid.WorldPositionToGridXY(mousepos);
            Debug.Log(gridXY);
        }

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

    public static TextMesh textMeshConstructorFunc(Grid<TextMesh> grid, int x, int y, float cellsize, GameObject parent)
    {
        GameObject text = new GameObject("text", typeof(TextMesh));

        // 设置父物体
        text.transform.SetParent(parent.transform);
        // 设置位置
        text.transform.position = grid.GetWorldPositionLeftBottom(x, y);

        TextMesh textMesh = text.GetComponent<TextMesh>();
        textMesh.text = x + "," + y;
        textMesh.characterSize = 0.2f;

        return textMesh;
    }

    public static FGridNode FGridNodeConstructorFunc(Grid<FGridNode> grid, int x, int y, float cellsize, GameObject parent)
    {
        GameObject go = new GameObject(x + "," + y, typeof(FGridNode));
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = grid.GetLocalPositionMiddleCenter(x, y);

        go.AddComponent<TextMesh>();
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.text = x + "," + y;
        textMesh.characterSize = 0.2f;

        FGridNode fGridNode = go.GetComponent<FGridNode>();

        return fGridNode;
    }
}
