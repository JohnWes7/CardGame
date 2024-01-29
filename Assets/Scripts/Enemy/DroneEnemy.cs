using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : Enemy, IBeDamage
{
    [SerializeField] private GameObject turret;
    [SerializeField] private Rigidbody2D rigi2D;
    [SerializeField] private Transform target;
    [SerializeField] private float LerpT;

    // 如果没有设定目标 有一个警戒范围如果有物体进入了警戒返回 就朝这个物体移动

    // 状态: 待机 如果有任何的目标出现 解除待机开始
    // >
    // 向核心移动 中途如果有unit在自爆范围内 直接自爆造成伤害

    // stretagy pattern 用在炮塔开火
    // 创造一个SO 的父类
    // 子类只重写开火方式 这样开火方式就到了static的SO里面去了

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
        // 如果没有目标就不动开始搜索目标
        if (target == null)
        {
            target = CheckWarningRange();
        }

        // 如果有目标就直接冲上去和他爆了
        if (target != null)
        {
            // 如果距离太近就不动了
            Vector3 dir = target.position - transform.position;
            if (dir.magnitude < 2)
            {
                return;
            }

            dir.z = 0;
            rigi2D.velocity = enemySO.speed * dir.normalized;

            //Quaternion todir = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.zero, dir), 0.2f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            rigi2D.rotation = Mathf.Lerp(rigi2D.rotation, angle, 0.2f);
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



    public void BeDamage(Projectile projectile)
    {
        bodyHP -= projectile.ProjectileSO.damage;
        if (bodyHP <= 0)
        {
            DropItemBySO();
            Destroy(gameObject);
        }
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
