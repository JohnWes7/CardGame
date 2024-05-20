using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class DroneEnemyV2 : AbstractEnemy, IBeDamage
{
    [SerializeField] private Rigidbody2D rigi2D;
    [SerializeField] private float maxTurnRate = 120f; // 每秒最大转速度, 单位为度
    [SerializeField] private float slowDownDistance = 10f; // 减速距离
    [SerializeField] private float maxSpeed = 5f; // 最大距离
    [SerializeField] private float maxAcceleration = 3f; // 最大加速的
    [SerializeField] private float slowDamping = 0.05f;

    private float targetAngle = 0f;

    private void Start()
    {
        rigi2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveRotatePerFrame();
    }

    private void MoveRotatePerFrame()
    {
        if (target == null)
        {
            target = CheckWarningRange();
        }

        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            float distance = dir.magnitude;

            // 计算新的旋转角度并限制转速
            dir.Normalize();
            targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            float angleDifference = Mathf.DeltaAngle(rigi2D.rotation, targetAngle);
            float maxAngleChange = maxTurnRate * Time.fixedDeltaTime;
            float angleChange = Mathf.Clamp(angleDifference, -maxAngleChange, maxAngleChange);
            rigi2D.rotation += angleChange;

            // 计算目标方向并施加推力
            // 如果对准了目标才开始加速
            if (Mathf.Abs(angleDifference) < 30)
            {
                if (slowDownDistance > 0)
                {
                    float targetSpeed = Mathf.Lerp(0, maxSpeed, distance / slowDownDistance);
                    float currentSpeed = rigi2D.velocity.magnitude;
                    float speedDifference = targetSpeed - currentSpeed;
                    float acceleration = Mathf.Clamp(speedDifference, -maxAcceleration, maxAcceleration);
                    rigi2D.AddForce(transform.up * acceleration);
                }
                else
                {
                    rigi2D.AddForce(transform.up * maxAcceleration);
                }
            }
            else
            {
                rigi2D.velocity *= (1 - slowDamping);
            } 
        }

        // 限制最大速度
        if (rigi2D.velocity.magnitude > maxSpeed)
        {
            rigi2D.velocity = rigi2D.velocity.normalized * maxSpeed;
        }
    }

    private Transform CheckWarningRange()
    {
        var result = Physics2D.CircleCast(transform.position, enemySO.warningRange, Vector2.zero, 0f, enemySO.warningLayer);
        if (result.transform != null)
        {
            return result.transform;
        }
        return null;
    }

    public void BeDamage(DamageInfo damageInfo)
    {
        curHp -= damageInfo.GetDamageAmount();
        if (curHp <= 0)
        {
            DropItemBySO();
            Destroy(gameObject);
        }

        //LogUtilsXY.LogOnPos(damageInfo.GetDamageAmount().ToString(), transform.position, 0.5f, 12f);
        var command = new LogDamageTextCommand
        {
            DamageAmount = damageInfo.GetDamageAmount(),
            Position = transform.position,
            Color = Color.white,
            Duration = 1f,
            randomRadius = 2f
        };
        this.SendCommand(command);
    }

    [ContextMenu("TestDropItemBySO")]
    public void DropItemBySO()
    {
        Dictionary<ItemSO, int> dropInfo = enemySO.GetSpecificDrop();
        foreach (var item in dropInfo)
        {
            Vector2 randomV2 = Random.insideUnitCircle;
            Vector3 randomV3 = new Vector3(randomV2.x, randomV2.y);
            DropItem.DropItemFactory(item.Key, item.Value, randomV3 + transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward));
        }
    }

}