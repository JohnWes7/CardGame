using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(IOnFire))]
public abstract class OnFireModifyBase : MonoBehaviour
{
    public int priority = 5;

    protected void OnDisable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        if (onFire != null) onFire.priorityEventManager.RemoveListener(OnFireModify);
    }

    protected void OnEnable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        if (onFire != null) onFire.priorityEventManager.AddListener(OnFireModify, priority);
    }

    protected abstract void OnFireModify(object sender, FireEventArgs e);

}

