using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// 用来管理相机控制
/// 
/// 相机移动: looktarget
/// 当是普通移动模式的时候 就是普通的鼠标去哪就跟随去哪
/// 到了建造模式相机要锁定中心
/// 
/// 逻辑:
/// 两个状态: 非建造和建造
/// 非建造的时候就是普通视角
/// 
/// 建造
/// 调整camera参数 -> 这样保证视角对齐
/// 设置target WASD 为target 这样才能通过wasd调整
/// 
/// 
/// </summary>
public class CameraFacade : MonoBehaviour
{
    public enum CameraFacadeState
    {
        Move,
        Build
    }

    private CameraFacade instance;

    public CameraFacade Instance { get => instance; set => instance = value; }

    [SerializeField] private LookTargetByMouse byMouse;
    [SerializeField] private LookTargetByWASD byWASD;
    [SerializeField] private CinemachineVirtualCamera moveCinemachine;
    [SerializeField] private CinemachineVirtualCamera buildCinemachine;
    [SerializeField] private CameraZoomController zoomController;

    [SerializeField] private CameraFacadeState defaultState;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        switch (defaultState)
        {
            case CameraFacadeState.Move:
                byMouse.StartTrackMouse();
                zoomController.VirtualCamera = moveCinemachine;
                break;
            case CameraFacadeState.Build:
                break;
            default:
                break;
        }
    }

    public void PlayerInput_OnLeftBuild(CallbackContext callbackContext)
    {
        ToMoveModel();
    }

    public void PlayerInput_OnStartBuild(CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            ToBuildModel();
        }
    }

    public void ToBuildModel()
    {
        Debug.Log("to build");
        if (byMouse != null)
        {
            byMouse.StopTrackMouse();
        }

        buildCinemachine.Priority = 11;
        zoomController.VirtualCamera = buildCinemachine;
    }

    public void ToMoveModel()
    {
        Debug.Log("back to move");
        buildCinemachine.Priority = 9;
        zoomController.VirtualCamera = moveCinemachine;

        byMouse.StartTrackMouse();
    }
}
