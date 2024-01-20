using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigi2D;

    // 理想中的移动
    // 应该是靠力 但姑且先用设置速度 然后有的组件应该能使得速度增加
    [SerializeField] private Vector2 moveVectorInput;
    [SerializeField] private Vector2 lerpMoveVector;
    [SerializeField] private float lerpT;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveVectorDeadZoneMin;


    private void Start()
    {
        rigi2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region 老input
        //// 获取WASD输入值，这里仅处理W键作为加速/减速的例子  
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //rigi2D.velocity = 5 * verticalInput * transform.up;
        //rigi2D.angularVelocity = horizontalInput * -180;

        //Debug.Log($"h:{horizontalInput}  v:{verticalInput}");
        #endregion


        //var action = playerInput.currentActionMap.FindAction("Move");
        //Debug.Log(action?.ReadValue<Vector2>());
    }


    private void FixedUpdate()
    {
        // vector 进行lerp
        lerpMoveVector = Vector2.Lerp(lerpMoveVector, moveVectorInput, lerpT);
        //Debug.Log(lerpMoveVector);

        if (lerpMoveVector.magnitude < moveVectorDeadZoneMin)
        {
            lerpMoveVector = Vector2.zero;
        }

        // 移动和旋转
        rigi2D.velocity = moveSpeed * lerpMoveVector.y * transform.up;
        rigi2D.angularVelocity = -rotateSpeed * lerpMoveVector.x;
    }

    public void Move(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        moveVectorInput = context.ReadValue<Vector2>();
    }
}
