using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBelt
{
    public bool TryInsertItem(Item item);
    public bool CanInsertItem();
}
