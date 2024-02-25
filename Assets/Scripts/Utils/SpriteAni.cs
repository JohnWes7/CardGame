using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomInspector;

namespace Johnwest
{
    public class SpriteAni : MonoBehaviour
    {
        [SerializeField, SelfFill]
        private SpriteRenderer spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }

    public class LogSprirtAni : Singleton<LogSprirtAni>
    {
        private GameObject spritePrefab;


        public LogSprirtAni()
        {
            //Debug.Log("load");
            spritePrefab = Resources.Load<GameObject>("Default/UI/Prefabs/SpriteAni");
        }

        public void LogSpriteLocal(Vector3 worldPos, Vector3 endOffset, Sprite sprite, float duration, Transform parent = null, bool offsetZ1 = true)
        {

            // 生成并初始化位置
            GameObject ani = Object.Instantiate(spritePrefab, parent);
            if (offsetZ1)
            {
                worldPos -= Vector3.forward;
            }
            ani.transform.SetPositionAndRotation(worldPos, Quaternion.identity);

            // 设置图像
            SpriteAni spa = ani.GetComponent<SpriteAni>();
            spa.SetSprite(sprite);

            // 图像动画duration
            spa.transform.DOLocalMove(spa.transform.localPosition + endOffset, duration).OnComplete(() => {
                spa.Destroy();
            });
        }

        public void LogSpriteLocal(Vector3 worldPos, Sprite sprite, Transform parent = null, bool offsetZ1 = true)
        {
            LogSpriteLocal(worldPos, Vector3.up, sprite, 1f, parent, offsetZ1);
        }

        public void LogCircleAni(Vector3 worlPos, Vector3 EndScale, float duration)
        {
            
        }

        public void GetPrefabName()
        {
            Debug.Log(spritePrefab.name);
        }
    }
}


