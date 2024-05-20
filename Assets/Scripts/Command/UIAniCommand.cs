using DG.Tweening;
using QFramework;
using UnityEngine;

public class UIMoveCommand : AbstractCommand
{
    public Transform rectTransform;
    public float duration = 0.8f;
    public Vector3 startPos;
    public Vector3 targetPos;
    public Ease ease = Ease.OutExpo;
    public TweenCallback action;

    public UIMoveCommand(Transform rectTransform, Vector3 startPos, Vector3 targetPos)
    {
        this.rectTransform = rectTransform;
        this.targetPos = targetPos;
        this.startPos = startPos;
    }

    protected override void OnExecute()
    {
        rectTransform.localPosition = startPos;
        var tweener = rectTransform.DOLocalMove(targetPos, duration).SetEase(ease);
        if (action != null)
        {
            tweener.OnComplete(action);
        }
    }
}