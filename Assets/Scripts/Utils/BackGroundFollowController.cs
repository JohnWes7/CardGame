using System;
using System.Collections.Generic;
using UnityEngine;


public class BackGroundFollowController : MonoBehaviour
{
    // 所有需要视差滚动的背景数组
    public Transform[] backgrounds;
    // 相机移动的比例，用来移动背景
    public List<Vector2> parallaxScales;
    // 视差滚动的平滑度，确保大于0
    public float smoothing = 1f;

    // 主相机的Transform引用
    private Transform cam;
    // 上一帧相机的位置
    private Vector3 previousCamPos;

    void Awake()
    {
        // 设置相机的引用
        cam = Camera.main.transform;
    }

    void Start()
    {
        // 上一帧的相机位置是当前帧的相机位置
        previousCamPos = cam.position;

        //// 为每个背景层分配对应的视差比例
        //parallaxScales = new List<float>();
        //for (int i = 0; i < backgrounds.Length; i++)
        //{
        //    // 这里取负值是为了反方向移动
        //    parallaxScales[i] = backgrounds[i].position.z * -1;
        //}
    }

    void Update()
    {
        // 对每个背景层执行视差滚动
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // 视差量是相机移动量乘以视差比例
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i].x;
            float parallay = (previousCamPos.y - cam.position.y) * parallaxScales[i].y;

            // 目标x位置是当前背景层位置加上视差量
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            float backgroundTargetPosY = backgrounds[i].position.y + parallay;

            // 创建目标位置
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

            // 使用Lerp在当前位置和目标位置之间插值
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // 更新上一帧相机的位置
        previousCamPos = cam.position;
    }
}

