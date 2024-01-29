using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 回收平台无人机
/// </summary>
public class RecyclePlatformDrone : MonoBehaviour
{
    /**
     * 基本逻辑:
     * 如果设置了target -> 就超target飞去 -> 飞到目标 -> 带着物品实体返回 -> 执行回调把物品实体传过去
     * 
     */
    [SerializeField] private RecyclePlatform recyclePlatform;
    [SerializeField] private DropItem target;
    [SerializeField] private bool isIdle;
    private static float maxSpeed = 10f;
    [SerializeField] private Vector3 curSpeed;
    private const float LERP_T = 1f;

    public bool IsIdle { get => isIdle; set => isIdle = value; }

    public void SetTarget(DropItem target, RecyclePlatform recyclePlatform)
    {
        LogUtilsXY.LogOnPos("开始回收作业", transform.position);
        this.recyclePlatform = recyclePlatform;
        curSpeed = Vector3.zero;
        // 双向链接一下
        this.target = target;
        target.PickUpDrone = this;
        
        // move to target
        StartCoroutine(MoveTo(target.transform, () => {
            //Debug.Log("开始抓取物品");
            LogUtilsXY.LogOnPos("开始抓取物品 开始返回", transform.position);
            target.transform.SetParent(transform);
            target.transform.DOLocalMove(Vector3.zero - new Vector3(0f, 0f, transform.position.z), 0.5f);

            //Debug.Log("开始返回");
            StartCoroutine(MoveTo(recyclePlatform.transform, () => {
                //Debug.Log("成功返回");
                LogUtilsXY.LogOnPos("成功返回", transform.position);
                recyclePlatform.SetDroneParent(this);
                transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
                transform.DOLocalMove(new Vector3(0f, 0f, transform.position.z), 0.3f).OnComplete(() => {
                    LogUtilsXY.LogOnPos("卸货", transform.position);
                    recyclePlatform.ReceiveDropItem(target);
                    recyclePlatform.SetDroneIdel(this);
                });
            }));
        }));
        
    }

    public IEnumerator MoveTo(Transform target, Action nextAction)
    {
        while (true)
        {
            Vector3 dir = target.position - transform.position;
            dir.z = 0;
            if (dir.magnitude < 1)
            {
                break;
            }
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, LERP_T * Time.deltaTime);
            
            curSpeed = Vector3.Lerp(curSpeed, maxSpeed * dir.normalized, LERP_T * Time.deltaTime);
            // 显示速度向量
            Debug.DrawLine(transform.position, transform.position + curSpeed);
            transform.position += curSpeed * Time.deltaTime;

            yield return null;
        }

        nextAction.Invoke();
    }
}
