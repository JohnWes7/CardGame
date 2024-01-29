using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float orthoSizeSpecified;
    [SerializeField] private float lerpT;
    [Header("鼠标滚轮一格屏幕缩放的多少系数")]
    [SerializeField] private float scrollerCOE;
    [Header("屏幕最大最小缩放")]
    [SerializeField] private float maxOrthoSize;
    [SerializeField] private float minOrthoSize;

    public CinemachineVirtualCamera VirtualCamera { get => virtualCamera; set => virtualCamera = value; }

    private void Start()
    {
        if (virtualCamera)
        {
            orthoSizeSpecified = virtualCamera.m_Lens.OrthographicSize;
        }
    }

    private void FixedUpdate()
    {
        if (orthoSizeSpecified > maxOrthoSize)
        {
            orthoSizeSpecified = Mathf.Lerp(orthoSizeSpecified, maxOrthoSize, lerpT);
        }
        if (orthoSizeSpecified < minOrthoSize)
        {
            orthoSizeSpecified = Mathf.Lerp(orthoSizeSpecified, minOrthoSize, lerpT);
        }
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, orthoSizeSpecified, lerpT);
    }

    public void RollerZoom(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            float scorllValue = callbackContext.ReadValue<float>();
            //Debug.Log($"缩放 滚轮的值: {scorllValue}");
            orthoSizeSpecified += scrollerCOE * scorllValue;
        }
    }
}
