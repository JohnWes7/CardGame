using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LogUtilsXY : MonoBehaviour
{
    private static GameObject textPrefab;
    private const string PREFAB_PATH = "Default/UI/Prefabs/LogText";

    public static void LogOnMousePos(string text, float charSize = 0.2f)
    {
        LogOnPos(text, Camera.main.ScreenToWorldPoint(Input.mousePosition) + 2 * Vector3.forward, charSize);
    }

    public static void LogOnPos(string text, Vector3 position, Color color, float charSize = 10f, float zOffset = -1, float duration = 1f)
    {
        // 初始化生成
        if (textPrefab == null)
        {
            textPrefab = Resources.Load<GameObject>(PREFAB_PATH);
        }
        GameObject obj = Instantiate<GameObject>(textPrefab);

        // 调整位置
        obj.transform.SetPositionAndRotation(position + new Vector3(0, 0, zOffset), Quaternion.identity);

        // 设置 文字和颜色
        TextMeshPro tmp = obj.GetComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = charSize;
        tmp.color = color;

        // 动画
        tmp.transform.DOLocalMoveY(tmp.transform.localPosition.y + 1f, duration).OnComplete(() => {
            Destroy(obj);
        });
    }

    public static void LogOnPos(string text, Vector3 position, float charSize = 10f, float zOffset = -1, float duration = 1f)
    {
        LogOnPos(text, position, Color.white, charSize, zOffset, duration);
    }

}
