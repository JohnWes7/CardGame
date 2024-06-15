using System;
using System.Collections.Generic;
using CustomInspector;
using DG.Tweening;
using UnityEngine;

public interface IBeHitAni
{
    void Execute();
}

public class EnemyBeHitAni : MonoBehaviour, IBeHitAni
{
    public SpriteRenderer[] rendererComponents;
    public SerializableDictionary<SpriteRenderer, Color> originColors = new();
    public float duration = 0.05f;

    public void Awake()
    {
        rendererComponents = GetComponentsInChildren<SpriteRenderer>(true);
        
        // 储存原有颜色
        foreach (var rendererComponent in rendererComponents)
        {
            originColors.Add(rendererComponent, rendererComponent.color);
        }
    }

    public void Execute()
    {
        CancelInvoke(nameof(ResetColor));

        // 如果没有Renderer组件，直接返回
        if (rendererComponents == null || rendererComponents.Length == 0)
        {
            return;
        }

        // 0.1秒内变红，然后0.1秒内恢复原有颜色
        foreach (var rendererComponent in rendererComponents)
        {
            rendererComponent.color = Color.red;
        }

        Invoke(nameof(ResetColor), duration);
    }

    private void ResetColor()
    {
        foreach (var rendererComponent in rendererComponents)
        {
            rendererComponent.DOColor(originColors[rendererComponent], duration);
        }
    }
}
