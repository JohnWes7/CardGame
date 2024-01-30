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
        if (wasdCorotine != null)
        {
            StopCoroutine(wasdCorotine);
            wasdCorotine = null;
        }

        wasdCorotine = StartCoroutine(MoveByWASD());
    }

    public void StopWASD()
    {
        if (wasdCorotine != null)
        {
            StopCoroutine(wasdCorotine);
            wasdCorotine = null;
        }
    }

    public void PlayerInput_OnBuildWASD(InputAction.CallbackContext callbackContext)
    {
        inputVector = callbackContext.ReadValue<Vector2>();
    }
}
