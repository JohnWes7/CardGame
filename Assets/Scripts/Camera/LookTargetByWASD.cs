using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEngine.InputSystem;

public class LookTargetByWASD : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] private Vector3 inputVector;
    [MessageBox("target移动速度", MessageBoxType.Info)]
    [SerializeField] private float moveSpeed;

    private Coroutine wasdCorotine;

    private void Awake()
    {
        EventCenter.Instance.AddEventListener("StartWASDTarget", EventCenter_StartWASDTarget);
        EventCenter.Instance.AddEventListener("StopWASDTarget", EventCenter_StopWASDTarget);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("StartWASDTarget", EventCenter_StartWASDTarget);
        EventCenter.Instance.RemoveEventListener("StopWASDTarget", EventCenter_StopWASDTarget);
    }

    private void EventCenter_StartWASDTarget(object sender, object obj)
    {
        Debug.Log("recive event center: StartWASDTarget");
        StartWASD();
    }

    private void EventCenter_StopWASDTarget(object sender, object obj)
    {
        Debug.Log("recive event center: StopWASDTarget");
        StopWASD();
    }

    public IEnumerator MoveByWASD()
    {
        while (true)
        {
            transform.localPosition += Time.deltaTime * moveSpeed * inputVector;
            yield return null;
        }
    }

    public void StartWASD()
    {
        // 检查之前有没有开启协程 避免重复开启
        if (wasdCorotine != null)
        {
            StopCoroutine(wasdCorotine);
            wasdCorotine = null;
        }

        // 开启协程
        wasdCorotine = StartCoroutine(MoveByWASD());
    }

    public void StopWASD()
    {
        // 检查如果之前有开启协程 就关闭 如果没有开启过协程就什么也不干
        if (wasdCorotine != null)
        {
            StopCoroutine(wasdCorotine);
            wasdCorotine = null;
        }
    }

    /// <summary>
    /// 收到playerinput 的move 回调后根据用户输入的值进行移动
    /// </summary>
    /// <param name="callbackContext"></param>
    public void PlayerInput_OnBuildWASD(InputAction.CallbackContext callbackContext)
    {
        inputVector = callbackContext.ReadValue<Vector2>();
    }
}
