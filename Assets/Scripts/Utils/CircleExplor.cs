using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomInspector;

namespace Johnwest
{
    public interface ISetFXRange 
    {
        public void SetFXRange(float range);
    }


    public class CircleExplor : MonoBehaviour, ISetFXRange
    {
        public float explorRange;
        public SpriteRenderer outsideCircle;
        public SpriteRenderer insideCircle;

        public float outsideDuration;
        public float insideDuration;

        [ReadOnly]
        public Vector3 startOutSize;
        [ReadOnly]
        public Vector3 startInSize;
        [SerializeField]
        private Color startInColor;
        [SerializeField]
        private Color startOutColor;

        [Button(nameof(Play))]
        [HideField]
        public bool _bool;

        private void Start()
        {
            startOutSize = outsideCircle.transform.localScale;
            startInSize = insideCircle.transform.localScale;
            //startInColor = insideCircle.color;
            //startOutColor = outsideCircle.color;
            Play();
        }

        public void Play()
        {
            outsideCircle.transform.localScale = startOutSize;
            outsideCircle.color = startOutColor;
            insideCircle.transform.localScale = startInSize;
            insideCircle.color = startInColor;

            transform.localScale = new Vector3(explorRange, explorRange, explorRange);
            outsideCircle.transform.DOScale(0, outsideDuration).SetEase(Ease.InBack);
            insideCircle.transform.DOScale(1.8f, insideDuration).SetEase(Ease.Linear);
            

            Color temp = startInColor;
            temp.a = 0;
            insideCircle.DOColor(temp, insideDuration).SetEase(Ease.InBack);

            temp = startOutColor;
            temp.a = 0;
            outsideCircle.DOColor(temp, outsideDuration);
        }

        public void SetFXRange(float range)
        {
            explorRange = range;
        }
    }
}


