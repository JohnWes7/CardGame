using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using CustomInspector;
using Johnwest;
using UnityEngine.SceneManagement;

public class StageIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image destImg;
    [SerializeField]
    private TextMeshProUGUI destNameText;
    [SerializeField]
    private StageSelectPanel panel;

    [SerializeField]
    private string stageName;
    [SerializeField]
    private string stageSceneName;


    public void Init(StageSelectPanel panel)
    {
        this.panel = panel;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panel)
        {
            panel.ChangeTitleText(stageName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panel)
        {
            panel.ChangeTitleText("???");
        }
    }

    public void Button_OnClick()
    {
        StartCoroutine(LoadYourAsyncScene(stageSceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // 异步加载新场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 等待加载完毕
        while (!asyncLoad.isDone)
        {
            // 这里可以添加加载进度的显示代码
            // 例如: progress = asyncLoad.progress;
            yield return null;
        }
    }
}
