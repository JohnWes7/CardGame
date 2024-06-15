using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleAni : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    public bool IsOnButton = false;
    public bool IsDown = false;
    public float bigState = 1.1f;
    public float normalState = 1f;
    public float smallState = 0.9f;
    public float duration = 0.1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsOnButton = true;
        //判断有没有按下
        if (IsDown)
        {
            ToSmallState();
        }
        else
        {
            ToBigState();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsOnButton = false;

        ToNormalState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDown = true;

        ToSmallState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        IsDown = false;

        if (IsOnButton)
        {
            ToBigState();
        }
    }

    public void ToBigState()
    {
        transform.DOScale(bigState, duration).SetUpdate(true);
    }
    public void ToNormalState()
    {
        transform.DOScale(normalState, duration).SetUpdate(true);
    }
    public void ToSmallState()
    {
        transform.DOScale(smallState, duration).SetUpdate(true);
    }

    
}