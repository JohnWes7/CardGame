using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 会往从父物体往鼠标方向偏移
/// 负责给 cinemachine 标记需要注视的点 这样这个点随鼠标偏移注视的位置也会随鼠标偏移
/// 
/// </summary>
public class LookTargetByMouse : MonoBehaviour
{
    [SerializeField] private float offsetT;

    private Coroutine TrackMouseCorotine;

    [ContextMenu("StopTrackMouse")]
    public void StopTrackMouse()
    {
        if (TrackMouseCorotine != null)
        {
            StopCoroutine(TrackMouseCorotine);
            TrackMouseCorotine = null;
        }
    }

    public void StartTrackMouse()
    {
        if (TrackMouseCorotine == null)
        {
            TrackMouseCorotine = StartCoroutine(TrackMouse());
        }
        else
        {
            StopCoroutine(TrackMouseCorotine);
            TrackMouseCorotine = StartCoroutine(TrackMouse());
        }
    }

    public IEnumerator TrackMouse()
    {
        while (true)
        {
            if (transform.parent == null)
            {
                break;
            }

            // 直接用屏幕坐标进行偏移因为屏幕坐标不会变化
            Vector2 mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2f, Screen.height / 2f);
            // 计算里中心的偏移 比值
            mousePosition.x /= (Screen.width / 2f);
            mousePosition.y /= (Screen.height / 2f);

            //Debug.Log(mousePosition);
            mousePosition *= offsetT;
            //Vector3 targetPos = transform.parent.TransformPoint((Vector3)mousePosition);
            Vector3 targetPos = (Vector3)mousePosition + transform.parent.position;
            targetPos.z = 0;
            transform.position = targetPos;

            yield return null;
        }
    }

    
}
