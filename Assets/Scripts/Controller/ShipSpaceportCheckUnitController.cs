using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipSpaceportCheckUnitController : MonoBehaviour, IController
{
    [SerializeField, ReadOnly] private Vector2 mousePosition;
    [SerializeField, ReadOnly] private Vector3 mouseWorldPosition;
    [SerializeField, ReadOnly] private MonoInterface<IShipController> shipController;
    [SerializeField, ReadOnly] private UnitObject curShowUnitObject;

    private void Awake()
    {
        shipController.InterfaceObj = GetComponent<IShipController>();
    }

    public void PlayerInput_OnMousePosition(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            mousePosition = callbackContext.ReadValue<Vector2>();
        }
    }

    private void Update()
    {
        if (PlayerControllerSingleton.Instance.currentActionMap.name.Equals("Build"))
        {
            // 获取鼠标在世界坐标的位置
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            var gridXy = shipController.InterfaceObj.Grid.WorldPositionToGridXY(mouseWorldPosition);
            var gridObject = shipController.InterfaceObj.Grid.GetGridObject(gridXy);

            //Debug.Log($"{gridXy} : {gridObject?.GetContent()?.UnitSO}");
            var unit = gridObject?.GetContent();

            if (curShowUnitObject != unit || unit == null)
            {
                curShowUnitObject = unit;
                this.SendCommand(new SendShowUnitDetailEventCommand(curShowUnitObject));
            }

        }
        else
        {
            curShowUnitObject = null;
            this.SendCommand(new SendShowUnitDetailEventCommand(curShowUnitObject));
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}