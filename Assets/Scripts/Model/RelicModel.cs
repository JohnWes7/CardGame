using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class RelicModel : AbstractModel
{
    // 所有的遗物
    public List<RelicSO> allRelics;

    protected override void OnInit()
    {
        // 从Resources文件夹中加载所有的遗物
        RelicSO[] relicSOs = Resources.LoadAll<RelicSO>("Default/Relic");
        allRelics = new List<RelicSO>(relicSOs);

        Debug.Log("RelicModel: init relic list:" + Johnwest.JWUniversalTool.ListToString(allRelics));
    }

}