using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    // Tick是subject 其他观测者看见tick变化后执行相应动作
    public class OnTickEventArgs : EventArgs {
        public ulong tick;
    }

    public static event EventHandler<OnTickEventArgs> Ontick;

    public const float TICK_TIMER_MAX = .2f;

    [SerializeField] private float tickTimer;
    [SerializeField] private ulong tick;

    private void Awake()
    {
        tickTimer = 0;
        tick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer > TICK_TIMER_MAX)
        {
            tick++;
            tickTimer -= TICK_TIMER_MAX;
            if(Ontick != null) Ontick.Invoke(this, new OnTickEventArgs() { tick = this.tick });
        }
    }

    public void DebugEvent(object sender, OnTickEventArgs args)
    {
        Debug.Log(args.tick);
    }
}
