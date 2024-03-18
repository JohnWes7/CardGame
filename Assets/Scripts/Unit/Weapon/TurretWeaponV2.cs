using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using Unity.VisualScripting;

public class TurretWeaponV2 : UnitObject, IShipUnit
{
    [SerializeField] private MonoInterface<IShipController> ship;

    /** 炮塔逻辑
     * 
     * 预备区
     * 子弹装填 如果curItemNum == 0 说明自己什么物品都没有 就按照turretSO里面能填所有类型的子弹
     * 
     * 激发区
     * 开火的时候如果 curProjectilNum == 0 说明没有子弹 从备存区扣除一个弹夹item并且将 curProjectilNum 拉满
     * 以及存一个当前激发区的弹药类型 
     * 如果 激发区 有东西就一直可以开火直到curProjectilNum = 0 就去暂存区拿
     * 
     * 寻敌 开火:
     * 
     */

    // 炮塔属性
    [SerializeField, Foldout, ForceFill] private TurretSO turretSO;
    [SerializeField] private GameObject turret;
    [SerializeField, ForceFill] private Transform projectileCreatPos;
    [SerializeField, ReadOnly] private Transform target;
    [SerializeField, ReadOnly] private float timer;

    // 逻辑组件
    [HorizontalLine("Stg")]
    [SerializeField, ReadOnly] private MonoInterface<ITurretFireStg> turretFireStg;

    private const float TURRET_ROTATE_SPEED = 2f;

    // 弹药
    // 激发区
    /// <summary>
    /// 表示当前装填的弹夹的余量
    /// </summary>
    [SerializeField, ReadOnly] private int curProjectilNum;
    /// <summary>
    /// 当前装填弹夹的种类
    /// </summary>
    [SerializeField, ReadOnly] private TurretSO.MagazineInfo curMagzineData;


    public GameObject Turret { get => turret; set => turret = value; }
    public Transform Target { get => target; set => target = value; }
    public TurretSO TurretSO { get => turretSO; set => turretSO = value; }
    public float Timer { get => timer; set => timer = value; }
    public Transform ProjectileCreatPos { get => projectileCreatPos; set => projectileCreatPos = value; }

    private void Awake()
    {
        curProjectilNum = 0;
        curMagzineData = null;
        timer = 0;
        turretFireStg.InterfaceObj = GetComponent<ITurretFireStg>();
    }

    private void Update()
    {
        FireStretagePreFrame(Time.deltaTime);
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public virtual void FireStretagePreFrame(float deltaTime)
    {
        // 如果没有turretSO 就不进行射击
        if (turretSO == null)
        {
            timer = 0;
            return;
        }

        // 如果有目标判断目标有没有超过射击范围
        if (target != null && (target.transform.position - (turret != null ? turret.transform.position : transform.position)).magnitude > turretSO.radius)
        {
            target = null; // 超过射击范围就不再索敌
        }

        // 如果炮塔当前没有目标 尝试获取目标
        if (target == null)
        {
            RaycastHit2D enemy = Physics2D.CircleCast(
                turret != null ? turret.transform.position : transform.position,
                turretSO.radius, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
            target = enemy.transform;
        }

        if (timer < turretSO.fireGap)
        {
            timer += deltaTime;
            if (target != null)
            {
                RotateTurret(target.position - (turret != null ? turret.transform.position : transform.position), deltaTime);
            }
            return;
        }


        // 如果有目标尝试对目标的transfrom攻击
        if (target != null)
        {
            // 瞄准
            RotateTurret(target.position - (turret != null ? turret.transform.position : transform.position), deltaTime);
            // 检查是否瞄准了
            if (CheckTakeAim())
            {
                //Debug.Log("已近瞄准");
                // 如果已经瞄准了就尝试获取弹药
                ProjectileSO projectileInfo = GetProjectile();
                if (projectileInfo != null)
                {
                    // 能获取到弹药
                    // Debug.Log("成功射击");

                    // 创建子弹 如果projectileCreatPos 是null 就用turret的位置
                    //Projectile.ProjectileCreateFactory(
                    //    projectileInfo, 
                    //    target, 
                    //    projectileCreatPos != null ? projectileCreatPos.position : turret.transform.position, 
                    //    this);
                    //timer = 0;

                    // 改用策略模式
                    if (turretFireStg.InterfaceObj != null)
                    {
                        turretFireStg.InterfaceObj.FireStg(this, projectileInfo);
                    }

                    timer = 0;
                }
            }
        }
    }

    protected virtual void RotateTurret(Vector3 dest, float deltaTime)
    {
        float angle = Mathf.Atan2(dest.y, dest.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, TurretSO.rotateSpeed * deltaTime);
    }

    protected virtual bool CheckTakeAim()
    {
        if (target != null)
        {
            Vector3 dest = target.position - (turret != null ? turret.transform.position : transform.position);
            Vector3 curAim = turret.transform.up;
            dest.z = 0;
            curAim.z = 0;
            if (Mathf.Abs(Vector3.Angle(dest, curAim)) < 5)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 获取弹药子弹
    /// </summary>
    /// <returns></returns>
    protected ProjectileSO GetProjectile()
    {
        // 如果激发区有弹药 直接使用激发区弹药
        if (curProjectilNum > 0)
        {
            curProjectilNum -= 1;
            return curMagzineData?.projectileSO;
        }

        // 如果激发区没有弹药则要去仓库中拿 从高到低拿取弹药
        foreach (TurretSO.MagazineInfo ammo in turretSO.magazineInfos)
        {
            if (PlayerModel.Instance.GetInventory().TryCostItem(ammo.magazineItem))
            {
                // 如果成功拿到了弹药 设置子弹弹夹和数量
                curMagzineData = ammo;
                curProjectilNum = ammo.projectileInOneMagazineNum;
                curProjectilNum -= 1;
                return curMagzineData.projectileSO;
            }
        }

        Debug.Log("没有弹药");
        return null;

    }

    public override string GetInfo()
    {

        string ammo_consum_rate = "弹药消耗速率:";
        // 消耗速率计算 弹夹子弹数量/每秒发射量 = 弹药item消耗量/s
        float rate = (1 / turretSO.fireGap) / turretSO.magazineInfos[0].projectileInOneMagazineNum;

        return base.GetInfo() +
            $"\n{ammo_consum_rate}\n" +
            $"{rate}/s";
    }

    private void OnDrawGizmosSelected()
    {
        if (turretSO)
        {
            Gizmos.color = Color.white; // 设置 Gizmos 颜色
            Gizmos.DrawWireSphere(transform.position, turretSO.radius); // 绘制表示射程范围的圆形
        }
    }
}
