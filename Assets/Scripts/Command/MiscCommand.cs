using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllTrailRendererCommand : AbstractCommand
{
    public GameObject target;

    public ClearAllTrailRendererCommand(GameObject target)
    {
        this.target = target;
    }

    protected override void OnExecute()
    {
        // 获取自己以及子物体上的拖尾组件
        TrailRenderer[] trailRenderers = target.GetComponentsInChildren<TrailRenderer>(true);
        foreach (var trail in trailRenderers)
        {
            trail.Clear();
        }
    }
}
