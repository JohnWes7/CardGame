using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class ShieldUnit : UnitObject
{
    [SerializeField, ForceFill]
    private ShieldBody shieldBody;
    [SerializeField]
    private float shieldRadius;

    [HorizontalLine("可能需要保存为 ShieldSO")]
    [SerializeField]
    private int shieldCapacity;

    [HorizontalLine("护盾运行属性")]
    [SerializeField, ReadOnly]
    private int curShildCapacity;
    [SerializeField, ReadOnly]
    private bool shieldState;

    /**
     *护盾逻辑
     *
     *没有电池的时候缓慢回盾
     *
     *如果有电池 则吃电池按照电池的品质恢复
     *
     *护盾破碎后有一定的冷却时间
     *
     */


    private void Start()
    {
        // 初始化当前护盾容量
        curShildCapacity = shieldCapacity;
        // 初始化护盾大小
        shieldBody.transform.localScale = 2 * shieldRadius * new Vector3(1f, 1f) + new Vector3(0, 0, 1f);
        // 护盾状态开启
        shieldState = true;
    }

    public void ShieldBeDamage(DamageInfo damageInfo)
    {
        curShildCapacity -= damageInfo.GetDamageAmount();
        curShildCapacity = Mathf.Clamp(curShildCapacity, 0, shieldCapacity);

        // 如果护盾被打没了就关闭
        if (curShildCapacity <= 0)
        {
            TurnOffShield();
        }
    }

    public void TurnOffShield()
    {
        shieldBody.gameObject.SetActive(false);
        shieldState = false;
    }
}
