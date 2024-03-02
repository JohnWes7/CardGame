using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

/// <summary>
/// 所有可以建造的物体的父类
/// </summary>
public class UnitObject : MonoBehaviour, ITextInfoDisplay, IBeDamage, IBeRepairUnitObject
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
    [HorizontalLine("当前hp")]
    [SerializeField] protected int curHP;
    

    public UnitSO UnitSO { get => unitSO; set => unitSO = value; }
    public Dir Dir { get => dir; set => dir = value; }
    public Vector2Int Position { get => position; set => position = value; }

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
    }

    

    public virtual string GetInfo()
    {
        string name = unitSO.name;
        // 之后需要做多语言字典
        string curHPText = "组件完整度:";

        return $"{name}\n" +
            $"-------------\n" +
            $"{curHPText}\n" +
            $"{curHP}/{unitSO.maxHP}";
    }

    public void BeDamage(DamageInfo projectile)
    {
        curHP -= projectile.damageAmount;
    }

    public float GetHPScale()
    {
        return unitSO.maxHP == 0 ? 1 : (float)curHP / unitSO.maxHP;
    }

    public void Repair(int amount)
    {
        LogUtilsXY.LogOnPos(amount.ToString(), transform.position, Color.green);
        curHP += amount;
        curHP = Mathf.Clamp(curHP, 0, unitSO.maxHP);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
