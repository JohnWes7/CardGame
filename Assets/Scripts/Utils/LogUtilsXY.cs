using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUtilsXY : MonoBehaviour
{
    public static float duration = 1f;
    public static void LogOnMousePos(string text, float charSize = 0.2f)
    {
        LogOnPos(text, Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward, charSize);
    }

    public static void LogOnPos(string text, Vector3 position, float charSize = 0.2f)
    {
        GameObject obj = new GameObject("log", typeof(TextMesh));
        TextMesh tm = obj.GetComponent<TextMesh>();
        LogUtilsXY logUtils = obj.AddComponent<LogUtilsXY>();

        obj.transform.position = position;

        tm.text = text;
        tm.characterSize = charSize;
    }

    private float timer;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        transform.position = transform.position + new Vector3(0, 0.02f);
        if (timer > duration)
        {
            Destroy(gameObject);
        }
    }
}
