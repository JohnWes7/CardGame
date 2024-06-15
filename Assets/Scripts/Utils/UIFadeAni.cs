using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIFadeAni : MonoBehaviour
{
    [ReadOnly, SelfFill]
    public Image fadeImg;
    public float duration = 0.8f;

    public void Start()
    {
        fadeImg.DOFade(0f, duration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}


