using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : SingletonUIBase<GameOverPanel>
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("GameOverPanel Awake");
    }
}
