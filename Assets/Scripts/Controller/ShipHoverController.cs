using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEngine.InputSystem;

public class ShipHoverController : MonoBehaviour
{
    [SerializeField]
    private MonoInterface<IShipController> shipController;

    private void Awake()
    {
        shipController.InterfaceObj = GetComponent<IShipController>();
    }

    public void PlayerInput_MousePos(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            var gridObject = shipController.InterfaceObj.Grid.GetGridObjectByMousePosition();
            EventCenter.Instance.TriggerEvent("DistplayUnitInfo", this, gridObject == null ? null : gridObject.GetContent());
        }
    }

}
