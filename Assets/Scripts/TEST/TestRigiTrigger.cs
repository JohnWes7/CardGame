using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRigiTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("aaaaaa");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("BBBBBB");
    }
}
