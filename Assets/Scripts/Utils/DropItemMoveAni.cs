using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class DropItemMoveAni : MonoBehaviour
{
    public float speed = 3.0f;
    public float maxSpeed = 10.0f;
    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            Destroy(this);
            return;
        }

        // 算出当前速度 离target越近速度越快
        float distance = Vector3.Distance(transform.position, target.position);
        float currentSpeed = Mathf.Min(speed + distance * 0.5f, maxSpeed);

        Vector3 dir = (target.position - transform.position).normalized;

        transform.position = transform.position + currentSpeed * Time.deltaTime * dir;
    }
}
