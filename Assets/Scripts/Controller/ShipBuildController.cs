using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using CustomInspector;
using QFramework;

/// <summary>
/// 飞船建造管理类
/// </summary>
public class ShipBuildController : MonoBehaviour, IController
{
    public class CurUnitChangeArgs : EventArgs
    {
        public UnitSO curUnit;

        public CurUnitChangeArgs(UnitSO curUnit)
        {
            this.curUnit = curUnit;
        }
    }

    // 建造面板
    [HorizontalLine("建造时候打开的面板")]
    [SerializeField, AssetsOnly, ForceFill] private GameObject uiBuildPanelPrefab;
    [SerializeField, ReadOnly] private UIBase uiBuildPanelInstance;

    // 单元建造
    [HorizontalLine("建造需要的字段")]
    // 改为从数据中读取 就算是不是单元仓库也可以放在playermodel中 来储存玩家可以建造的所有单位
    //[SerializeField, ForceFill] private UnitListSO buildableUnit;
    [SerializeField, ReadOnly] private UnitSO curUnit;
    [SerializeField, ReadOnly] private Dir buildDir;
    [SerializeField, ReadOnly] private MonoInterface<IShipController> sc;
    [SerializeField, ReadOnly] private bool isBuilding;

    [HorizontalLine("建造的时候的虚影")]
    [SerializeField, ReadOnly] private SpriteRenderer prefabShadow;
    [SerializeField] private Color shadowColor;

    // 当当前选择的unit变化后调用
    public event EventHandler<CurUnitChangeArgs> OnCurBuildUnitChange;

    public Dir BuildDir { get => buildDir; set => buildDir = value; }
    public MonoInterface<IShipController> Sc { get => sc; set => sc = value; }
    public SpriteRenderer PrefabShadow { get => prefabShadow; set => prefabShadow = value; }

    public bool IsBuilding { get => isBuilding; set => isBuilding = value; }
    

    private void Awake()
    {
        buildDir = default;
        sc.InterfaceObj = GetComponent<IShipController>();
        isBuilding = false;

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
            uiBuildPanelInstance = temp.GetComponent<UIBase>();
            uiBuildPanelInstance.Initialize();
            // 调整为第一个子物体
            temp.transform.SetAsFirstSibling();
        }

        // 添加委托
        // 改为使用事件中心解耦
        EventCenter.Instance.AddEventListener("BuildUnitChange", UiBuildPanelInstance_OnUnitValueChange);

        // 初始关闭面板
        uiBuildPanelInstance.CloseUI();
    }

    private void OnDestroy()
    {
        // 移除事件
        EventCenter.Instance.RemoveEventListener("BuildUnitChange", UiBuildPanelInstance_OnUnitValueChange);
    }

    private void UiBuildPanelInstance_OnUnitValueChange(object sender, object args)
    {
        if (args == null)
        {
            ChangeCurBuildUnit(null);
            return;
        }
        if (args is UnitSO unitSO)
        {
            ChangeCurBuildUnit(unitSO);
            return;
        }
        if (args is BuildPanelController.BuildPanelEventHandler e)
        {
            ChangeCurBuildUnit(e.beClickUnit);
            return;
        }

        Debug.LogWarning("UiBuildPanelInstance_OnUnitValueChange 参数错误");
    }

    //弃用
    [Obsolete("使用UiBuildPanelInstance_OnUnitValueChange代替")]
    private void UiBuildPanelInstance_OnUnitValueChange(object sender, BuildPanelController.BuildPanelEventHandler e)
    {
        ChangeCurBuildUnit(e.beClickUnit);
    }

    private void Update()
    {
        if (isBuilding)
        {
            ShadowFollowPerFrame();
        }
    }

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
            Debug.Log("进入建造模式");

            // 显示ui
            uiBuildPanelInstance.OpenUI();
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
        uiBuildPanelInstance.CloseUI();

        Johnwest.JWUniversalTool.LogWithClassMethodName("LeftBuild", System.Reflection.MethodBase.GetCurrentMethod());
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
        //Debug.Log("CheckDetail");
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
            this.SendCommand(new BuildUnitByBuildControllerCommand(this));
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
            this.SendCommand(new DemolitionUnitByBuildControllerCommand(this));
        }
    }

    public void ResetCurBuildUnit()
    {
        curUnit = null;
    }

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

    /// <summary>
    /// ui元素调用 更改当前
    /// </summary>
    /// <param name="unitSO"></param>
    public void ChangeCurBuildUnit(UnitSO unitSO)
    {
        if (unitSO == null)
        {
            curUnit = null;
            OnCurBuildUnitChange?.Invoke(this, new CurUnitChangeArgs(curUnit));
            // 改用触发事件中心
            EventCenter.Instance.TriggerEvent("ShipBuildControllerCurUnitChange", this, new CurUnitChangeArgs(curUnit));

            return;
        }

        curUnit = unitSO;
        OnCurBuildUnitChange?.Invoke(this, new CurUnitChangeArgs(curUnit));
        // 改用触发事件中心
        EventCenter.Instance.TriggerEvent("ShipBuildControllerCurUnitChange", this, new CurUnitChangeArgs(curUnit));
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    #endregion

}
