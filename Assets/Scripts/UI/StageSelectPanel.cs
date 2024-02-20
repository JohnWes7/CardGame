using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomInspector;
using Febucci.UI;

public class StageSelectPanel : MonoBehaviour
{
    [SerializeField, ForceFill]
    private TextMeshProUGUI title;
    [SerializeField, ForceFill]
    private TextAnimator_TMP titleTextAnimator;
    [SerializeField, ForceFill]
    private TypewriterByCharacter titleTypeWritter;

    /// <summary>
    /// 更改选择标题的文字
    /// </summary>
    /// <param name="text"></param>
    /// <param name="animation"></param>
    public void ChangeTitleText(string text, bool animation = true)
    {
        if (title)
        {
            titleTextAnimator.SetText(text);
            if (animation)
            {
                titleTypeWritter.StartShowingText(true);
            }
            Debug.Log($"set title {text}");
        }
        
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void QuitButton_OnClick()
    {
        ClosePanel();
    }
}
