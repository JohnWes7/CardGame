using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInspector;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField, ForceFill]
    private GameObject infoShower;
    [SerializeField, ForceFill]
    private TextMeshProUGUI text;

    private ITextInfoDisplay textInfoDisplay;

    private void Awake()
    {
        EventCenter.Instance.AddEventListener("DistplayUnitInfo", EventCenter_DistplayUnitInfo);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("DistplayUnitInfo", EventCenter_DistplayUnitInfo);
    }

    public void EventCenter_DistplayUnitInfo(object sender, object args)
    {
        if (args == null)
        {
            textInfoDisplay = null;
            Hide();
        }

        //Debug.Log(args);
        if (args is ITextInfoDisplay infoDisplay)
        {
            textInfoDisplay = infoDisplay;
        }
    }

    private void Update()
    {
        if (textInfoDisplay != null)
        {
            Show(textInfoDisplay.GetInfo());
        }
        else
        {
            textInfoDisplay = null;
            Hide();
        }
        
    }

    public void Show(string info)
    {
        infoShower.SetActive(true);
        text.text = info;
    }

    public void Hide()
    {
        // 隐藏UI面板
        infoShower.SetActive(false);
    }
}

public interface ITextInfoDisplay
{
    public string GetInfo();
}
