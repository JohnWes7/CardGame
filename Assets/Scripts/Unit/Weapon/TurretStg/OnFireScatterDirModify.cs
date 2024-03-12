using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireScatterDirModify : MonoBehaviour
{
    public float scatterAngle = 60f;
    public int provity = 5;

    private void OnEnable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.AddListener(OnFireScatterDirModify_OnFire, provity);
    }

    private void OnDisable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.RemoveListener(OnFireScatterDirModify_OnFire);
    }

    private void OnFireScatterDirModify_OnFire(object sender, FireEventArgs e)
    {
        Vector2 direction = e.Target.position - e.CreatePosition;
        float a = scatterAngle / (e.ProjectileCreationParams.Count - 1);

        for (int i = 0; i < e.ProjectileCreationParams.Count; i++)
        {
            float angle = a * (i - (e.ProjectileCreationParams.Count - 1) / 2f);
            Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * direction;

            e.ProjectileCreationParams[i].direction = scatterDirection;
        }
    }
}
