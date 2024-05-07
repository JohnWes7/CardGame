using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneModel : AbstractModel
{
    public bool isModelInit = false;

    protected override void OnInit()
    {
        isModelInit = false;
    }
}