using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigi2D;

    // 理想中的移动
    // 应该是靠力 但姑且先用设置速度 然后有的组件应该能使得速度增加

    private void Start()
    {
        rigi2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 获取WASD输入值，这里仅处理W键作为加速/减速的例子  
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rigi2D.velocity = 5 * verticalInput * transform.up;
        rigi2D.angularVelocity = horizontalInput * -180;

        //Debug.Log($"h:{horizontalInput}  v:{verticalInput}");
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
    }
}
