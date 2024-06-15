using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using QFramework;

/// <summary>
/// 所有可以建造的物体的父类
/// </summary>
public class UnitObject : MonoBehaviour, ITextInfoDisplay, IBeDamage, IBeRepairUnitObject, IController
{
    // 所有unit都通过这里根据unitso来生成
    public static UnitObject UnitFactoryCreate(UnitSO unitSO, Vector2Int position, Dir dir, Grid<FGridNode> grid)
    {
        FGridNode fGridNode = grid.GetGridObject(position);
        if (fGridNode == null)
        {
            Debug.Log("UnitObject: 创建失败 找不到node");
            return null;
        }

        GameObject gameObject = Instantiate<GameObject>(unitSO.prefab);
        UnitObject unitObject = gameObject.GetComponent<UnitObject>();

        // 设置父物体和位置
        gameObject.transform.SetParent(fGridNode.transform);
        gameObject.transform.localPosition = unitSO.spriteBLtoCMOffset.VecterRotateByDir(dir);

        // 根据dir转角度
        gameObject.transform.localRotation = DirExtensions.DirToQuaternion(dir);

        // 如果没有unitObject类就安装一个最基础的父类
        if (unitObject == null)
        {
            gameObject.AddComponent<UnitObject>();
            unitObject = gameObject.GetComponent<UnitObject>();
        }

        // 必须会带有的属性
        unitObject.position = position;
        unitObject.unitSO = unitSO;
        unitObject.dir = dir;

        // 设置索引
        var nodelist = grid.GetObjectPlaceByPosList(position, unitSO.place, dir);
        foreach (var item in nodelist)
        {
            if (item != null)
            {
                item.SetContent(unitObject);
            }
        }

        unitObject.Initialize();

        return unitObject;
    }

    [HorizontalLine("所有unit都有的属性")]
    [SerializeField] protected Vector2Int position;
    [SerializeField] protected Dir dir;
    [SerializeField, Foldout] protected UnitSO unitSO;
    [SerializeField] protected Grid<FGridNode> grid;
    [HorizontalLine("当前hp 和 状态")]
    [SerializeField] protected int curHP;
    [SerializeField] protected bool isOnline = true;


    public UnitSO UnitSO { get => unitSO; set => unitSO = value; }
    public Dir Dir { get => dir; set => dir = value; }
    public Vector2Int Position { get => position; set => position = value; }

    protected virtual void Awake()
    {
        // 注册停止事件
        this.RegisterEvent<SetUnitObjetEnableEvent>(OnStop).UnRegisterWhenDisabled(gameObject);
    }

    protected virtual void OnStop(SetUnitObjetEnableEvent obj)
    {
        Debug.Log($"set {name} state {obj.value}");
        this.enabled = obj.value;
    }

    /// <summary>
    /// 在自己的grid上面找到该位置的unitObject 如果没有则返回null
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public UnitObject GetUnitObjectOnGrid(Vector2Int pos)
    {
        var gridobj = grid?.GetGridObject(pos);
        UnitObject unit = gridobj != null ? gridobj.GetContent() : null;
        unit = unit == null ? null : unit;
        return unit;
    }

    public virtual void Initialize()
    {
        // 初始设置为满血
        curHP = unitSO.maxHP;
        // 初始是在线的
        isOnline = true;
    }

    public virtual string GetInfo()
    {
        string name = unitSO.name;
        string onlineflag = isOnline ? "<color=#00ff00>Online</color>" : "<color=#ff0000>Offline</color>";
        // 之后需要做多语言字典
        string curHPText = "组件完整度:";

        return $"{name}\n" +
            $"-------------\n" +
            $"{onlineflag}\n" +
            $"{curHPText}\n" +
            $"{curHP}/{unitSO.maxHP}";
    }

    public virtual void BeDamage(DamageInfo projectile)
    {
        curHP -= projectile.damageAmount;
        curHP = Mathf.Clamp(curHP, 0, unitSO.maxHP);

        // 用红色字显示伤害
        //LogUtilsXY.LogOnPos(laserProjectile.damageAmount.ToString(), transform.position, Color.red);
        var command = new LogDamageTextCommand
        {
            DamageAmount = projectile.damageAmount,
            Position = transform.position,
            Color = LogDamageTextCommand.GetRedColor(),
            Duration = 1f,
            randomRadius = 2f
        };
        this.SendCommand(command);

        // 如果血量小于0则设置为离线并且关闭update
        if (curHP <= 0)
        {
            SetState(false);
        }
    }

    /// <summary>
    /// 打开和关闭unit
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetState(bool value)
    {
        isOnline = value;
        enabled = value;
        if (value)
        { 
            gameObject.layer = LayerMask.NameToLayer("Ship");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("ShipOffline");
        }

        // 如果有离线特效模块则调用
        if (TryGetComponent<IOfflineFXModular>( out var fxm))
        {
            fxm.SetState(value);
        }
    }

    public virtual float GetHPScale()
    {
        return unitSO.maxHP == 0 ? 1 : (float)curHP / unitSO.maxHP;
    }

    public virtual void Repair(int amount)
    {
        //LogUtilsXY.LogOnPos(amount.ToString(), transform.position, Color.green);
        var command = new LogDamageTextCommand
        {
            DamageAmount = amount,
            Position = transform.position,
            Color = LogDamageTextCommand.GetRepairColor(),
            Duration = 1f,
            randomRadius = 2f
        };
        this.SendCommand(command);

        curHP += amount;
        curHP = Mathf.Clamp(curHP, 0, unitSO.maxHP);

        // 如果修复满血了则设置为在线并且开启update
        if (!isOnline && curHP >= unitSO.maxHP)
        {
            SetState(true);
        }
    }

    public virtual Transform GetTransform()
    {
        return transform;
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}

public struct SetUnitObjetEnableEvent
{
    public bool value;
}