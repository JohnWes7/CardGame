using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBelt
{
    // 负责传送带部分的 传送带方法
    public bool TryInsertItem(Item insertItem);
    public bool CanInsertItem();
    public bool EnqueueItem(Item enqueueItem);
    public int MaxRestLenghtFrom0Index();
}
