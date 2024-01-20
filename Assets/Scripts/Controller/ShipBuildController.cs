using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 飞船建造管理类
/// </summary>
public class ShipBuildController : MonoBehaviour
{
    // 状态设计模式
    [SerializeField] private IShipBuildingState state;

    // 单元建造
    [SerializeField] private List<UnitSO> unitCanBuild;
    [SerializeField] private int buildIndex;
    [SerializeField] private Dir buildDir;
    [SerializeField] private MonoInterface<IShipController> sc;
    [SerializeField] private bool isBuilding;

    // 虚影
    [SerializeField] private SpriteRenderer prefabShadow;
    [SerializeField] private Color shadowColor;

    public List<UnitSO> UnitCanBuild { get => unitCanBuild; set => unitCanBuild = value; }
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
        // state = new ShipNotBuildingState(this);

        // 初始化虚影
        GameObject go = new GameObject("shipUnitPrefabShadow", typeof(SpriteRenderer));
        go.transform.SetParent(this.transform);
        prefabShadow = go.GetComponent<SpriteRenderer>();
        prefabShadow.color = shadowColor;
        go.SetActive(false);
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


    // TODO: 右键删除 以及移动绑定
    #region 新版input system方法

    /// <summary>
    /// 进入建造
    /// </summary>
    /// <param name="callbackContext"></param>
    public void StartBuild(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            isBuilding = true;
            PlayerControllerSingleton.Instance.SwitchCurrentActionMap("Build");
            prefabShadow.gameObject.SetActive(true);
            sc.InterfaceObj.SetAllFGridNodeBackGroundActive(true);
            Debug.Log("startBuild");
        }
    }

    /// <summary>
    /// 退出建造
    /// </summary>
    /// <param name="callbackContext"></param>
    public void LeftBuild(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            // 先判断是要从拆除还是退出
            // 如果执行了拆除就不退出
            if (ShipBuildingState.TryDeleteUnitOnMousePos(this))
            {
                return;
            }
            

            isBuilding = false;
            PlayerControllerSingleton.Instance.SwitchCurrentActionMap("Move");

            // 退出build 恢复一些值
            prefabShadow.gameObject.SetActive(false);
            buildDir = Dir.up;
            sc.InterfaceObj.SetAllFGridNodeBackGroundActive(false);
            Debug.Log("LeftBuild");
        }
    }

    /// <summary>
    /// 旋转建造方向 顺时针
    /// </summary>
    /// <param name="callbackContext"></param>
    public void RotateBuildDir(InputAction.CallbackContext callbackContext)
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
    public void CheckUnitDetail(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("CheckDetail");
        }
    }

    /// <summary>
    /// 建造
    /// </summary>
    /// <param name="callbackContext"></param>
    public void BuildUnit(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log($"Build Unit: {unitCanBuild[buildIndex].name}");
            ShipBuildingState.BuildUnit(this);
        }
    }

    public void ChangeBuildUnit(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            int temp = buildIndex + 1;
            if (temp >= unitCanBuild.Count)
            {
                temp = 0;
            }
            buildIndex = temp;
            prefabShadow.sprite = unitCanBuild[BuildIndex].fullsizeSprite;
        }
    }


    public void ShadowFollowPerFrame()
    {
        ShipBuildingState.ShadowPerFrame(this);
    }

    #endregion

}
