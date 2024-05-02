using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class GameArchitecture : Architecture<GameArchitecture>
{
    protected override void Init()
    {
        Debug.Log("GameArchitecture 框架初始化 Init");
        
        // 注册模型
        RegisterModel(new RelicModel());
        RegisterModel(new StageModel());

        // 注册系统
        RegisterSystem(new PlayerTechTreeRelicSystem());
    }
}
