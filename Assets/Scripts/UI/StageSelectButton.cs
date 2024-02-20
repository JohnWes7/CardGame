using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField, ForceFill, AssetsOnly]
    private GameObject SelectPanelPrefab;
    [SerializeField]
    private StageSelectPanel SelectPanel;

    private void Start()
    {
        if (SelectPanel == null)
        {
            var temp = Instantiate<GameObject>(SelectPanelPrefab, Johnwest.JWUniversalTool.GetCanvase());
            SelectPanel = temp.GetComponent<StageSelectPanel>();
        }

        if (SelectPanel)
        {
            SelectPanel.gameObject.SetActive(false);
        }
    }

    public void Button_OnClick()
    {
        Debug.Log("open panel");
        // 打开关卡选择面板
        SelectPanel.OpenPanel();
    }
}
