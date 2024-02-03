using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.IO;

public class ShipController : MonoBehaviour, IShipController
{
    // 飞船核心数值
    [SerializeField] private int HP;
    [SerializeField] private Grid<FGridNode> grid;

    //grid参数
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;

    [Header("Debug")]
    [SerializeField] private bool showGridLine = true;

    [Header("初始化")]
    [SerializeField] private UnitSO coreSO;
    [SerializeField] private UnitObject coreUnit;
    [SerializeField] private Sprite fgNodeSprite;

    // 如果有核心才能驱动飞船自带的电量和引擎
    [Header("属性")]
    [SerializeField] private float speed;

    [HorizontalLine("dubug方法")]
    [Button(nameof(DebugSaveShipJson))]
    [HideField]
    public bool _bool;



    public Grid<FGridNode> Grid { get => grid; set => grid = value; }

    private void Awake()
    {
        EventCenter.Instance.AddEventListener("PlayerMementoLoad", EventCenter_OnPlayerMementoLoad);
    }

    //private void Start()
    //{
    //    //DefaultInit();
    //}

    public void DefaultInit()
    {
        CreateGrid();

        // 生成核心
        Vector2Int coreInitPos = new((gridWidth / 2) - 1, (gridHeight / 2) - 1);
        List<FGridNode> place = grid.GetObjectPlaceByPosList(coreInitPos, coreSO.place, Dir.up);
        coreUnit = UnitObject.UnitFactoryCreate(coreSO, coreInitPos, Dir.up, grid);
        foreach (var item in place)
        {
            item.SetContent(coreUnit);
        }
    }

    private void CreateGrid()
    {
        grid = new Grid<FGridNode>(gridWidth, gridHeight, 1, FGridNodeConstructorFunc, Vector3.zero);

        // grid实体设置到飞船下面
        grid.GetParent().transform.SetParent(transform);
        grid.GetParent().transform.localPosition = new Vector3(-grid.GetRealWorldWidth() / 2, -grid.GetRealWorldHeight() / 2);

        // 关闭地面显示
        SetAllFGridNodeBackGroundActive(false);
    }

    public void InitByPlayerMemento(PlayerMemento playerMemento)
    {
        ShipMemento shipMemento = playerMemento.shipMemento;
        gridHeight = shipMemento.gridHeightSize;
        gridWidth = shipMemento.gridWidthSize;

        if (grid != null)
        {
            Destroy(grid.GetParent());
            grid = null;
        }

        CreateGrid();

        // 按照json生成所有的unit
        foreach (var item in shipMemento.mementoUnitInfoList)
        {
            UnitSO unitSO = UnitInfoModel.Instance.GetUnit(item.unitName);
            if (unitSO == null)
            {
                Debug.LogError("Can not find this unit: " + item.unitName);
                continue;
            }

            // 生成unit
            UnitObject.UnitFactoryCreate(unitSO, new Vector2Int(item.x, item.y), item.dir, grid);
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
    public FGridNode FGridNodeConstructorFunc(Grid<FGridNode> grid, int x, int y, float cellsize, GameObject parent)
    {
        GameObject go = new(x + "," + y, typeof(FGridNode));
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = grid.GetLocalPositionMiddleCenter(x, y);

        // 添加底色
        GameObject backGround = new GameObject("BackGround");
        SpriteRenderer spriteRenderer = backGround.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fgNodeSprite;
        spriteRenderer.color = new Color(1, 1, 1, 0.2f);

        backGround.transform.SetParent(go.transform);
        backGround.transform.localPosition = new Vector3(0, 0, 1);

        //go.AddComponent<TextMesh>();
        //TextMesh textMesh = go.GetComponent<TextMesh>();
        //textMesh.anchor = TextAnchor.MiddleCenter;
        //textMesh.text = x + "," + y;
        //textMesh.characterSize = 0.2f;

        FGridNode fGridNode = go.GetComponent<FGridNode>();
        fGridNode.SetPostion(x, y);

        return fGridNode;
    }

    public void DebugSaveShipJson()
    {
        PlayerMemento playerMemento = new PlayerMemento(this, PlayerInventory.Instance.GetInventory());
        string jsonpath = Path.Combine(Application.persistentDataPath, "test.json");
        playerMemento.SaveMemento(jsonpath);
    }

    public void SetAllFGridNodeBackGroundActive(bool value)
    {
        foreach (List<FGridNode> fgndoeList in grid.Content)
        {
            foreach (FGridNode node in fgndoeList)
            {
                if (node != null)
                {
                    node.SetBackGroundActive(value);
                }
            }
        }
    }

    public void EventCenter_OnPlayerMementoLoad(object sender, object playerMemento)
    {
        if (playerMemento is PlayerMemento)
        {
            InitByPlayerMemento(playerMemento as PlayerMemento);
            return;
        }

        Debug.LogError("EventCenter_OnPlayerMementoLoad 传入参数无法转换为 PlayerMemento");
        //DefaultInit();
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("PlayerMementoLoad", EventCenter_OnPlayerMementoLoad);
    }
}
