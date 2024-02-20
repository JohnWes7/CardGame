using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using CustomInspector;

/// <summary>
/// 飞船建造管理类
/// </summary>
public class ShipBuildController : MonoBehaviour
{
    public class UnitEventargs : EventArgs
    {
        public UnitSO curUnit;

        public UnitEventargs(UnitSO curUnit)
        {
            this.curUnit = curUnit;
        }
    }

    // 状态设计模式弃用
    [SerializeField] private IShipBuildingState state;

    // 建造面板
    [HorizontalLine("建造时候打开的面板")]
    [SerializeField, AssetsOnly, ForceFill] private GameObject uiBuildPanelPrefab;
    [SerializeField, ReadOnly] private BuildPanelController uiBuildPanelInstance;

    // 单元建造
    //[SerializeField] private List<UnitSO> unitCanBuild;
    //[SerializeField] private int buildIndex;
    [HorizontalLine("建造需要的字段")]
    [SerializeField, ForceFill] private UnitListSO buildableUnit;
    [SerializeField, ReadOnly] private UnitSO curUnit;
    [SerializeField, ReadOnly] private Dir buildDir;
    [SerializeField, ReadOnly] private MonoInterface<IShipController> sc;
    [SerializeField, ReadOnly] private bool isBuilding;

    [HorizontalLine("建造的时候的虚影")]
    [SerializeField, ReadOnly] private SpriteRenderer prefabShadow;
    [SerializeField] private Color shadowColor;
    [HorizontalLine("虚影旁边显示造价")]
    [SerializeField, AssetsOnly, ForceFill] private GameObject uiShadowCostShowerPrefab;
    [SerializeField, ReadOnly] private BuildCostPanel uiShadowCostShower;


    // 当当前选择的unit变化后调用
    public event EventHandler<UnitEventargs> OnCurBuildUnitChange;

    public List<UnitSO> UnitCanBuild { get => buildableUnit.unitSOList; }
    //public int BuildIndex { get => buildIndex; set => buildIndex = value; }
    public Dir BuildDir { get => buildDir; set => buildDir = value; }
    public MonoInterface<IShipController> Sc { get => sc; set => sc = value; }
    public SpriteRenderer PrefabShadow { get => prefabShadow; set => prefabShadow = value; }
    public IShipBuildingState State { get => state; set => state = value; }
    public bool IsBuilding { get => isBuilding; set => isBuilding = value; }
    public BuildCostPanel UiShadowCostShower { get => uiShadowCostShower; set => uiShadowCostShower = value; }



    //private void Awake()
    //{
    //    // delegate 能存多个方法 而且不需要 new 初始化 如果是有返回值的方法则只会保留最后一个
    //    //delegate int test();
    //    //private test testd;

    //    //testd += () =>
    //    //{
    //    //    Debug.Log("m1");
    //    //    return 1;
    //    //};
    //    //testd += () =>
    //    //{
    //    //    Debug.Log("m2");
    //    //    return 2;
    //    //};
    //    //Debug.Log(testd());
    //}

    private void Start()
    {
        //buildIndex = 0;
        buildDir = default;
        sc.InterfaceObj = GetComponent<IShipController>();
        isBuilding = false;

        // 初始状态
        // state = new ShipNotBuildingState(this);

        // 初始化虚影
        GameObject go = new GameObject("shipUnitPrefabShadow", typeof(SpriteRenderer));
        go.transform.SetParent(this.transform);
        prefabShadow = go.GetComponent<SpriteRenderer>();
        prefabShadow.color = shadowColor;
        go.SetActive(false);

        // 初始化建造选择面板
        if (uiBuildPanelInstance == null)
        {
            GameObject temp = Instantiate(uiBuildPanelPrefab, GameObject.Find("Canvas").transform);
            uiBuildPanelInstance = temp.GetComponent<BuildPanelController>();
        }
        uiBuildPanelInstance.RefreshIcon(buildableUnit.unitSOList);

        // 添加委托
        // ui的值变化会 影响cur build unit
        uiBuildPanelInstance.OnUnitValueChange += UiBuildPanelInstance_OnUnitValueChange;
        // cur unit 变化会影响ui变化
        OnCurBuildUnitChange += uiBuildPanelInstance.ShipBuildController_OnCurUnitChange;

        // 初始关闭面板
        uiBuildPanelInstance.ClosePanel();


        // 初始化uiShadowCostShowerPrefab
        var uiShadowCostShowerGO = Instantiate(uiShadowCostShowerPrefab, GameObject.Find("Canvas").transform);
        uiShadowCostShower = uiShadowCostShowerGO.GetComponent<BuildCostPanel>();
        uiShadowCostShower.gameObject.SetActive(false);
        OnCurBuildUnitChange += UiShadowCostShower.ShipBuildController_OnCurBuildChange;
    }

    private void UiBuildPanelInstance_OnUnitValueChange(object sender, BuildPanelController.BuildPanelEventHandler e)
    {
        //Debug.Log(sender);
        ChangeCurBuildUnit(e.beClickUnit);
    }

    private void Update()
    {
        #region 旧版纯手打控制 改用新版Input System控制
        //// 临时控制 按e进入建造右键在ShipBuildState里面退出建造状态
        //if (!IsBuilding && Input.GetKeyDown(KeyCode.E))
        //{
        //    StartBuild();
        //}

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    int temp = buildIndex + 1;
        //    if (temp >= buildUnit.Count)
        //    {
        //        temp = 0;
        //    }
        //    ChangeIndex(temp);
        //}

        //// 点击查看物体详情
        //if (!IsBuilding && Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2Int gridXY = sc.InterfaceObj.Grid.WorldPositionToGridXY(mousePos);
        //    LogUtilsXY.LogOnMousePos($"点击到了: {gridXY}");

        //    var gridobj = sc.InterfaceObj.Grid?.GetGridObject(gridXY);
        //    var unit = gridobj == null ? null : gridobj.GetContent();
        //    if (unit != null && unit is IBeClick)
        //    {
        //        (unit as IBeClick).BeClick(this);
        //    }
        //}

        //state.Update(this);
        #endregion

        if (isBuilding)
        {
            ShadowFollowPerFrame();
        }

    }

    #region 老state模式
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
    #endregion


    #region 新版input system方法

    /// <summary>
    /// 进入建造
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnStartBuild(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            isBuilding = true;
            PlayerControllerSingleton.Instance.SwitchCurrentActionMap("Build");
            prefabShadow.gameObject.SetActive(true);
            sc.InterfaceObj.SetAllFGridNodeBackGroundActive(true);
            Johnwest.JWUniversalTool.LogWithClassMethodName("startBuild", System.Reflection.MethodBase.GetCurrentMethod());

            // 显示ui
            uiBuildPanelInstance.OpenPanel(UnitCanBuild, GetCurBuildUnit());
            uiShadowCostShower.OpenPanel();
            uiShadowCostShower.RefreshUnit(curUnit);
        }
    }

    /// <summary>
    /// 退出建造
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnLeftBuild(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            LeftBuildNow();
        }
    }

    public void LeftBuildNow()
    {
        // 更改map
        PlayerControllerSingleton.Instance.SwitchCurrentActionMap("Move");

        // 退出build 恢复一些值
        isBuilding = false; // 标志退出建造
        prefabShadow.gameObject.SetActive(false); // 关闭虚影 (虚影最好还是改为协程)
        buildDir = Dir.up;
        sc.InterfaceObj.SetAllFGridNodeBackGroundActive(false);

        // 关闭ui
        uiBuildPanelInstance.ClosePanel();
        uiShadowCostShower.ClosePanel();

        Johnwest.JWUniversalTool.LogWithClassMethodName("LeftBuild", System.Reflection.MethodBase.GetCurrentMethod());
    }

    public bool TryDelectUnit()
    {
        // 先判断是要从拆除还是退出
        // 如果执行了拆除就不退出
        if (ShipBuildingState.TryDeleteUnitOnMousePos(this, out UnitObject unitObject))
        {
            // 执行成功拆除之后退还资源
            foreach (var item in unitObject.UnitSO.itemCostList)
            {
                PlayerModel.Instance.GetInventory()?.AddItem(item.itemSO, item.cost);
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// 旋转建造方向 顺时针
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnRotateBuildDir(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            ShipBuildingState.RotateBuildDir(this);
            Debug.Log($"rotate build dir: {buildDir}");
        }
    }

    /// <summary>
    /// 单点来触发有些可被点击物体的回调
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnMoveCheckUnitDetail(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            CheckUnitDetail();
        }
    }

    public void CheckUnitDetail()
    {
        Debug.Log("CheckDetail");
        Vector3 mouseOffsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridXY = Sc.InterfaceObj.Grid.WorldPositionToGridXY(mouseOffsetPos);
        var gridobj = sc.InterfaceObj.Grid.GetGridObject(gridXY);
        UnitObject unit = gridobj == null ? null : gridobj.GetContent();
        if (unit != null && unit is IBeClick)
        {
            (unit as IBeClick).BeClick(this);
        }
    }

    /// <summary>
    /// 建造
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnBuildUnit(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (GetCurBuildUnit() == null)
            {
                return;
            }

            if (!PlayerModel.Instance.GetInventory().HaveEnoughItem(GetCurBuildUnit().itemCostList, out List<UnitSO.ItemCost> missingItem))
            {
                List<string> debugString = new List<string>();
                foreach (var item in missingItem)
                {
                    debugString.Add(item.ToString());
                }
                LogUtilsXY.LogOnMousePos($"缺少物品无法建造:\n{string.Join("\n", debugString)}");
                return;
            }

            //Debug.Log($"Build Unit: {GetCurBuildUnit().name}");
            UnitObject unitObject = ShipBuildingState.BuildUnit(this);

            if (unitObject != null)
            {
                //建造成功 扣除资源
                foreach (var item in GetCurBuildUnit().itemCostList)
                {
                    PlayerModel.Instance.GetInventory()?.CostItem(item.itemSO, item.cost);
                }
            }
        }
    }

    public void PlayerInput_OnBuildCheckDetail(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (GetCurBuildUnit() == null)
            {
                CheckUnitDetail();
            }
        }
    }

    /// <summary>
    /// 取消当前建造的物品
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnCancel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            // 先判断是否需要拆除
            if (!TryDelectUnit())
            {
                ChangeCurBuildUnit(null);
            }
        }
    }

    public void ResetCurBuildUnit()
    {
        curUnit = null;
    }

    /// <summary>
    /// 弃用 不在使用按下某个键来改变build的选择改用ui选择
    /// </summary>
    /// <param name="callbackContext"></param>
    //public void ChangeBuildUnit(InputAction.CallbackContext callbackContext)
    //{
    //    #region 旧更改
    //    //if (callbackContext.performed)
    //    //{
    //    //    int temp = buildIndex + 1;
    //    //    if (temp >= unitCanBuild.Count)
    //    //    {
    //    //        temp = 0;
    //    //    }
    //    //    buildIndex = temp;
    //    //    prefabShadow.sprite = unitCanBuild[BuildIndex].fullsizeSprite;
    //    //}
    //    #endregion

    //}

    /// <summary>
    /// 建造模式开启后显示虚影
    /// </summary>
    public void ShadowFollowPerFrame()
    {
        ShipBuildingState.ShadowPerFrame(this);
    }

    public UnitSO GetCurBuildUnit()
    {
        return curUnit;
    }

    public void ChangeCurBuildUnit(UnitSO unitSO)
    {
        if (unitSO == null)
        {
            curUnit = null;
            OnCurBuildUnitChange?.Invoke(this, new UnitEventargs(curUnit));
            return;
        }

        if (buildableUnit.unitSOList.Contains(unitSO))
        {
            curUnit = unitSO;
            OnCurBuildUnitChange?.Invoke(this, new UnitEventargs(curUnit));
        }
    }

    #endregion

}
