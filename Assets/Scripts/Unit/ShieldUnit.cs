using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class ShieldUnit : UnitObject, ITextInfoDisplay
{
    [SerializeField, ForceFill]
    private ShieldBody shieldBody;
    [SerializeField, AssetsOnly, ForceFill]
    private ShieldSO shieldSO;
    
    [SerializeField]
    private int curShieldCapacity;
    [SerializeField]
    private float rechargeTimer;
    [SerializeField, ReadOnly]
    private float restatrTimer;
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
        curShieldCapacity = shieldSO.shieldCapacity;
        // 初始化护盾大小
        shieldBody.transform.localScale = 2 * shieldSO.shieldRadius * new Vector3(1f, 1f) + new Vector3(0, 0, 1f);
        // 护盾状态开启
        shieldState = true;
    }

    private void Update()
    {
        if (shieldState)
        {
            ChargeShield(Time.deltaTime);
        }
        else
        {
            ReStartShield(Time.deltaTime);
        }
    }

    public void ShieldBeDamage(DamageInfo damageInfo)
    {
        Debug.Log(damageInfo);
        curShieldCapacity -= damageInfo.GetDamageAmount();
        curShieldCapacity = Mathf.Clamp(curShieldCapacity, 0, shieldSO.shieldCapacity);

        // 如果护盾被打没了就关闭
        if (curShieldCapacity <= 0)
        {
            TurnOffShield();
        }
    }

    private void ChargeShield(float deltaTime)
    {
        // 如果盾是满状态直接return
        if (curShieldCapacity >= shieldSO.shieldCapacity)
        {
            return;
        }

        if (rechargeTimer < shieldSO.rechargeGap )
        {
            rechargeTimer = Mathf.Clamp(rechargeTimer + deltaTime, 0, shieldSO.rechargeGap);
        }
        else
        {
            // 从上往下取电池
            foreach (var item in shieldSO.itemRechargeInfo)
            {
                // 如果能取到电池 则直接加消耗电池的恢复量
                if (PlayerModel.Instance.GetInventory().TryCostItem(item.itemSO))
                {
                    curShieldCapacity = Mathf.Clamp(curShieldCapacity + item.rechargeNum, 0, shieldSO.shieldCapacity);
                    rechargeTimer = 0;
                    return;
                }
            }

            // 如果没有物品 则直接按照默认值恢复
            curShieldCapacity = Mathf.Clamp(curShieldCapacity + shieldSO.defaultRechargeNum, 0, shieldSO.shieldCapacity);
            rechargeTimer = 0;
            return;
        }
    }

    private void ReStartShield(float deltaTime)
    {
        if (restatrTimer < shieldSO.restartTime)
        {
            restatrTimer = Mathf.Clamp(restatrTimer + deltaTime, 0, shieldSO.restartTime);
        }
        else
        {
            curShieldCapacity = shieldSO.shieldCapacity;
            restatrTimer = 0;
            TurnOnShield();
        }
    }

    public void TurnOnShield()
    {
        restatrTimer = 0;
        rechargeTimer = 0;
        shieldBody.gameObject.SetActive(true);
        shieldState = true;
    }

    public void TurnOffShield()
    {
        restatrTimer = 0;
        rechargeTimer = 0;
        shieldBody.gameObject.SetActive(false);
        shieldState = false;
    }

    public override void Repair(int amount)
    {
        LogUtilsXY.LogOnPos(amount.ToString(), transform.position, Color.green);
        curHP += amount;
        curHP = Mathf.Clamp(curHP, 0, unitSO.maxHP);

        // 如果修复满血了则设置为在线并且开启update
        if (!isOnline && curHP >= unitSO.maxHP)
        {
            SetState(true);
            // 如果修复满血了则设置为在线并且重置护盾
            curShieldCapacity = shieldSO.shieldCapacity;
            restatrTimer = 0;
        }
    }

    public override void SetState(bool value)
    {
        base.SetState(value);
        if (value)
        {
            TurnOnShield();
        }
        else
        {
            TurnOffShield();
        }
    }

    public override string GetInfo()
    {
        // 多语言字典
        string cur_shield_cap = "cur_shield_cap:";
        string shield_state = "";
        // 如果在重启则显示还需要多少秒重启成功
        if (shieldState)
        {
            shield_state = $"normal";
        }
        else
        {
            shield_state = $"restart: {shieldSO.restartTime - restatrTimer:F1}";
        }

        return base.GetInfo() +
            $"\n{cur_shield_cap}\n"+
            $"{curShieldCapacity}/{shieldSO.shieldCapacity}\n" +
            $"{shield_state}";
    }
}
