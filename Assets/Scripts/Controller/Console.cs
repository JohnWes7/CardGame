using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    private static Console instance;

    [SerializeField]
    private bool active;

    public bool Active { get => active; set => active = value; }
    public static Console Instance { get => instance; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        active = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            active = !active;
        }

        if (active)
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    Debug.Log(testgo);
            //    // 必须要这么用
            //    Debug.Log(testgo != null ? testgo.name : null);
            //}
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    Destroy(testgo);
            //}
        }

        
    }

    public static TextMesh CreatText(string text, Vector3 localpostion, TextAnchor textAnchor = TextAnchor.MiddleCenter, Transform parent = null)
    {
        GameObject gameObject = new GameObject("text", typeof(TextMesh));
        Transform transform = gameObject.transform;

        transform.position = Vector3.zero;
        transform.localPosition = localpostion;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = textAnchor;

        return textMesh;
    }
}
