using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSingleton : MonoBehaviour
{
    private static PlayerInput instance;

    public static PlayerInput Instance { get => instance; }

    private void Awake()
    {
        instance = GetComponent<PlayerInput>();
    }
}
