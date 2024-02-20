using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHoriOpenCloseAni : MonoBehaviour
{
    public float duration;
    private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> curAni;

    public void PlayOpenAni()
    {
        KillCurAni();

        gameObject.SetActive(true);
        curAni = transform.DOScaleX(1f, duration);
        curAni.OnComplete(() => {
            curAni = null;
        });
    }

    public void PlayCloseAni()
    {
        KillCurAni();

        curAni = transform.DOScaleX(0f, duration);
        curAni.OnComplete(() => {
            gameObject.SetActive(false);
            curAni = null;
        });
    }

    private void KillCurAni()
    {
        if (curAni != null)
        {
            curAni.Kill();
            curAni = null;
        }
    }
}
