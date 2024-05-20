using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using QFramework;
using TMPro;
using UnityEngine;

public class LogTextUtility : IUtility, ICanInit
{
    public GameObject TextPrefab;

    public bool Initialized { get; set; }
    public void Init()
    {
        TextPrefab = Resources.Load<GameObject>("Default/UI/Prefabs/Misc/LogDamageText");
    }

    public void Deinit() { }

    public GameObject LogText(string text, Vector3 position, Color color, float size = 12f, float duration = 1f)
    {
        GameObject textObject = Object.Instantiate(TextPrefab, position, Quaternion.identity);
        var tmp = textObject.GetComponent<TextMeshPro>();
        
        tmp.SetText(text);
        tmp.color = color;
        tmp.fontSize = size;

        var typeWriter = textObject.GetComponent<TypewriterByCharacter>();
        typeWriter.StartCoroutine(DelayDisappear(duration, typeWriter));

        return textObject;
    }

    public IEnumerator DelayDisappear(float time, TypewriterByCharacter typewriter)
    {
        yield return new WaitForSeconds(time);

        // 消失动画
        typewriter.StartDisappearingText();
        typewriter.onTextDisappeared.AddListener(() =>
        {
            Object.Destroy(typewriter.gameObject, 3f);
        });
    }

}
