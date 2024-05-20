using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomInspector;
using QFramework;

public class StageInfoPanel : MonoBehaviour, IController
{
    [SerializeField, ForceFill]
    private TextMeshProUGUI stageinfoText;

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    private void Start()
    {
        UpdateStageInfo();
    }

    private void UpdateStageInfo()
    {
        var command = new GetStageIndexCommand();
        this.SendCommand(command);
        stageinfoText.text = $"星域 #{command.mStageIndex:D2}";
    }
}
