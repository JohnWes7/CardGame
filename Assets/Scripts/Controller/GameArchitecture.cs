using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class GameArchitecture : Architecture<GameArchitecture>
{
    protected override void Init()
    {
        Debug.Log("GameArchitecture 框架初始化 Init");

        // 注册utility
        RegisterUtility(new LangUtility());
        var logUtility = new LogTextUtility();
        logUtility.Init();
        RegisterUtility(logUtility);
        

        // 注册模型
        RegisterModel(new UnitExtraSOModel());
        RegisterModel(new SceneModel());
        RegisterModel(new RelicModel());
        RegisterModel(new StageModel());

        // 注册系统
        RegisterSystem(new PlayerTechTreeRelicSystem());
    }
}
