using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomInspector;

public class ShipMoveController : MonoBehaviour
{
    [SerializeField, SelfFill] private Rigidbody2D rigi2D;
    [SerializeField, ReadOnly] private Vector2 moveVectorInput;
    [HorizontalLine("速度限制")]
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxRotateSpeed;


    [HorizontalLine("力")]
    [SerializeField] private float forwardForce = 10f;
    //[SerializeField] private float turnTorque = 10f;
    [SerializeField] private float rotateA = 5f;
    [HorizontalLine("旋转跟随鼠标")]
    [SerializeField, ReadOnly] private float rotationVelocity_byMouse;
    [SerializeField] private float rotationTime_byMosue = 10f;
    [HorizontalLine("设置")]
    [SerializeField] private bool moveByForce = true;

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
        if (moveByForce)
        {
            // 用力来转向
            if (Mathf.Abs(moveVectorInput.x) > 0f)
            {
                //rigi2D.AddTorque(-moveVectorInput.x * turnTorque);
                rigi2D.angularVelocity = Mathf.Clamp(-moveVectorInput.x * rotateA + rigi2D.angularVelocity, -maxRotateSpeed, maxRotateSpeed);
                Debug.Log(rigi2D.angularVelocity);
            }
        }
        else
        {
            // 鼠标转向
            // 将鼠标屏幕位置转换为世界坐标
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 计算从物体位置指向鼠标位置的向量
            Vector2 direction = (mousePosition - rigi2D.position).normalized;

            // 计算该向量的角度
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 减90度以使物体的y轴指向鼠标

            // 使用SmoothDampAngle来平滑角度变化
            float currentAngle = Mathf.SmoothDampAngle(rigi2D.rotation, targetAngle, ref rotationVelocity_byMouse, rotationTime_byMosue, maxRotateSpeed, deltaTime);

            // 应用旋转到刚体
            rigi2D.rotation = currentAngle;
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
