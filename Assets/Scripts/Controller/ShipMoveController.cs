using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomInspector;

public class ShipMoveController : MonoBehaviour
{
    [SerializeField, SelfFill] private Rigidbody2D rigi2D;
    [SerializeField, ReadOnly] private Vector2 moveVectorInput;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxRotateSpeed;


    [HorizontalLine("力")]
    [SerializeField] private float forwardForce = 10f;
    [SerializeField] private float turnTorque = 10f;

    private void FixedUpdate()
    {
        
        MovePerDeltaTime(Time.fixedDeltaTime);
        LimitSpeed();
    }

    public void MovePerDeltaTime(float deltaTime)
    {
        // 前进和后退
        if (Mathf.Abs(moveVectorInput.y) > 0f)
        {
            rigi2D.AddForce(forwardForce * moveVectorInput.y * transform.up);
        }

        // 左右转向
        if (Mathf.Abs(moveVectorInput.x) > 0f)
        {
            rigi2D.AddTorque(-moveVectorInput.x * turnTorque);
        }
    }

    private void LimitSpeed()
    {
        if (rigi2D.velocity.magnitude > maxMoveSpeed)
        {
            rigi2D.velocity = rigi2D.velocity.normalized * maxMoveSpeed;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        moveVectorInput = context.ReadValue<Vector2>();
    }
}
