using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{

    [SerializeField]
    private bool active;

    [SerializeField] private UnitSO test1;
    [SerializeField] private UnitSO test2;

    private void Start()
    {
        active = true;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            active = !active;
        }

        if (active)
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    CreatText("test", Vector3.zero);
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
