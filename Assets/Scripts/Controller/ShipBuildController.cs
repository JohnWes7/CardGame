using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飞船建造管理类
/// </summary>
public class ShipBuildController : MonoBehaviour
{

    // 状态设计模式
    [SerializeField] private IShipBuildingState state;

    // 单元建造
    [SerializeField] private List<UnitSO> buildUnit;
    [SerializeField] private int buildIndex;
    [SerializeField] private Dir buildDir;
    [SerializeField] private MonoInterface<IShipController> sc;
    [SerializeField] private bool isBuilding;

    // 虚影
    [SerializeField] private SpriteRenderer prefabShadow;
    [SerializeField] private Color shadowColor;

    public List<UnitSO> BuildUnit { get => buildUnit; set => buildUnit = value; }
    public int BuildIndex { get => buildIndex; set => buildIndex = value; }
    public Dir BuildDir { get => buildDir; set => buildDir = value; }
    public MonoInterface<IShipController> Sc { get => sc; set => sc = value; }
    public SpriteRenderer PrefabShadow { get => prefabShadow; set => prefabShadow = value; }
    public IShipBuildingState State { get => state; set => state = value; }
    public bool IsBuilding { get => isBuilding; set => isBuilding = value; }

    private void Start()
    {
        buildIndex = 0;
        buildDir = default;
        sc.InterfaceObj = GetComponent<IShipController>();
        isBuilding = false;

        // 初始状态
        state = new ShipNotBuildingState(this);

        // 初始化虚影
        GameObject go = new GameObject("shipUnitPrefabShadow", typeof(SpriteRenderer));
        go.transform.SetParent(this.transform);
        prefabShadow = go.GetComponent<SpriteRenderer>();
        prefabShadow.color = shadowColor;


    }

    private void Update()
    {
        // 临时控制 按e进入建造右键在ShipBuildState里面退出建造状态
        if (!isBuilding && Input.GetKeyDown(KeyCode.E))
        {
            StartBuild();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int temp = buildIndex + 1;
            if (temp >= buildUnit.Count)
            {
                temp = 0;
            }
            ChangeIndex(temp);
        }

        state.Update(this);
    }

    public void QuitBuild()
    {
        isBuilding = false;
        state.QuitBuild(this);
    }

    public void StartBuild()
    {
        isBuilding = true;
        state.StartBuild(this);
    }

    public void ChangeIndex(int index)
    {
        state.ChangeIndex(this, index);
    }

}
