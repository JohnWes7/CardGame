using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

public class RelicSO : ScriptableObject
{
    public Sprite Image;

    public virtual void OnReceive()
    {
        // 默认没有逻辑
    }
}
