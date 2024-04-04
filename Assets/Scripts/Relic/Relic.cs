using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

public class Relic : ScriptableObject
{
    public Sprite Image;

    public virtual void OnReceive()
    {
        // 默认没有逻辑
    }
}

[CreateAssetMenu(fileName = "New RelicB", menuName = "Relic/RelicB")]
public class RelicB : Relic
{
    public override void OnReceive()
    {
        
    }
}
