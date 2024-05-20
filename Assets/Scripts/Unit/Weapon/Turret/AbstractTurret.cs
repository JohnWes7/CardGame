using CustomInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractTurret : UnitObject, IShipUnit
{
    [HorizontalLine("所属 ship")]
    [SerializeField] private MonoInterface<IShipController> ship;

    /** 炮塔逻辑
     * 
     * 预备区
     * 子弹装填 如果curItemNum == 0 说明自己什么物品都没有 就按照turretSO里面能填所有类型的子弹
     * 
     * 激发区
     * 开火的时候如果 curProjectileNum == 0 说明没有子弹 从备存区扣除一个弹夹item并且将 curProjectileNum 拉满
     * 以及存一个当前激发区的弹药类型 
     * 如果 激发区 有东西就一直可以开火直到curProjectileNum = 0 就去暂存区拿
     * 
     * 寻敌 开火:
     * 
     */

    // 炮塔属性
    [HorizontalLine("TurretSO")]
    [SerializeField, Foldout, ForceFill] protected TurretSO turretSO;
    public TurretSO TurretSO { get => turretSO; set => turretSO = value; }


    // 弹药
    /// <summary>
    /// 表示当前装填的弹夹的余量
    /// </summary>
    [HorizontalLine("弹药")]
    [SerializeField, ReadOnly] protected int curProjectileNum;
    /// <summary>
    /// 当前装填弹夹的种类
    /// </summary>
    [SerializeField, ReadOnly] protected TurretSO.MagazineInfo curMagazineData;

    protected override void Awake()
    {
        base.Awake();

        curProjectileNum = 0;
        curMagazineData = null;
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    private void OnDrawGizmosSelected()
    {
        if (turretSO)
        {
            Gizmos.color = Color.white; // 设置 Gizmos 颜色
            Gizmos.DrawWireSphere(transform.position, turretSO.radius); // 绘制表示射程范围的圆形
        }
    }

    public override string GetInfo()
    {

        string ammo_consume_rate = "弹药消耗速率:";
        // 消耗速率计算 弹夹子弹数量/每秒发射量 = 弹药item消耗量/s
        float rate = (1 / turretSO.fireGap) / turretSO.magazineInfos[0].projectileInOneMagazineNum;

        return base.GetInfo() +
            $"\n{ammo_consume_rate}\n" +
            $"{rate}/s";
    }

    /// <summary>
    /// 获取弹药子弹
    /// </summary>
    /// <returns></returns>
    protected ProjectileSO GetProjectile()
    {
        // 如果激发区有弹药 直接使用激发区弹药
        if (curProjectileNum > 0)
        {
            curProjectileNum -= 1;
            return curMagazineData?.projectileSO;
        }

        // 如果激发区没有弹药则要去仓库中拿 从高到低拿取弹药
        foreach (TurretSO.MagazineInfo ammo in turretSO.magazineInfos)
        {
            // 如果没有拿到弹药就跳过
            if (!PlayerModel.Instance.GetInventory().TryCostItem(ammo.magazineItem)) continue;
            
            // 如果成功拿到了弹药 设置子弹弹夹和数量
            curMagazineData = ammo;
            curProjectileNum = ammo.projectileInOneMagazineNum;
            curProjectileNum -= 1;
            return curMagazineData.projectileSO;
        }

        //Debug.Log("发射默认子弹");
        if (turretSO.defaultProjectile == null)
        {
            Debug.LogWarning($"{turretSO.name} 默认子弹 defaultProjectile 为null");
        }
        // 发射默认子弹
        return turretSO.defaultProjectile;
    }

    public abstract Transform GetTarget();

    public abstract Vector2 GetFireDir();

    public abstract Vector3 GetProjectileCreatePos();
}
