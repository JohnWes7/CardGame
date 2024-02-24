using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private TurretSO turretSO;
    [SerializeField] private GameObject turret;
    [SerializeField] private Transform target;
    [SerializeField] private float timer;
    private const float TURRET_ROTATE_SPEED = 2f;

    // 弹药

    // 激发区
    /// <summary>
    /// 表示当前装填的弹夹的余量
    /// </summary>
    [SerializeField] private int curProjectilNum;
    /// <summary>
    /// 当前装填弹夹的种类
    /// </summary>
    [SerializeField] private TurretSO.MagazineInfo curMagzineData;


    public GameObject Turret { get => turret; set => turret = value; }
    public Transform Target { get => target; set => target = value; }
    public TurretSO TurretSO { get => turretSO; set => turretSO = value; }

    private void Update()
    {
        FireStretagePreFrame();
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public void FireStretagePreFrame()
    {
        // 如果没有turretSO 就不进行射击
        if (turretSO == null)
        {
            timer = 0;
            return;
        }

        if (timer < turretSO.fireGap)
        {
            timer += Time.deltaTime;
            if (target != null)
            {
                RotateTurret(target.position - turret.transform.position);
            }
            return;
        }

        // 如果有目标判断目标有没有超过射击范围
        if (target != null && (target.transform.position - turret.transform.position).magnitude > turretSO.range)
        {
            target = null; // 超过射击范围就不再索敌
            timer = 0;
        }

        // 如果炮塔当前没有目标 尝试获取目标
        if (target == null)
        {
            RaycastHit2D enemy = Physics2D.CircleCast(Turret.transform.position, turretSO.range, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
            target = enemy.transform;
        }

        // 如果有目标尝试对目标的transfrom攻击
        if (target != null)
        {
            // 瞄准
            RotateTurret(target.position - turret.transform.position);
            // 检查是否瞄准了
            if (CheckTakeAim())
            {
                //Debug.Log("已近瞄准");
                // 如果已经瞄准了就尝试获取弹药
                TurretSO.MagazineInfo projectileInfo = GetProjectile();
                if (projectileInfo != null)
                {
                    // 能获取到弹药
                    // Debug.Log("成功射击");
                    Projectile.ProjectileCreateFactory(projectileInfo.projectileSO, target, turret.transform.position + turret.transform.up, this);
                    timer = 0;
                }
            }
        }
    }

    private void RotateTurret(Vector3 dest)
    {
        float angle = Mathf.Atan2(dest.y, dest.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, TurretSO.rotateSpeed * Time.deltaTime);
    }

    #region 如果要按照一定的角度旋转
    //    using UnityEngine;

    //public class TurretController : MonoBehaviour
    //{
    //    public Transform target; // 目标的 Transform
    //    public float maxRotationSpeed = 20f; // 最大旋转速度，每秒度数

    //    void Update()
    //    {
    //        if (target != null)
    //        {
    //            // 获取炮塔当前的朝向和目标的朝向
    //            Vector3 direction = target.position - transform.position;
    //            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //            // 将角度限制在 -180 到 180 之间
    //            float currentAngle = Mathf.Repeat(transform.eulerAngles.z, 360);
    //            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

    //            // 计算每帧旋转的角度
    //            float maxRotationDelta = maxRotationSpeed * Time.deltaTime;
    //            float rotationAmount = Mathf.Clamp(angleDiff, -maxRotationDelta, maxRotationDelta);

    //            // 使用 Quaternion.Slerp 进行平滑旋转
    //            Quaternion targetRotation = Quaternion.Euler(0, 0, currentAngle + rotationAmount);
    //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * maxRotationSpeed);
    //        }
    //    }
    //}

    #endregion

    private bool CheckTakeAim()
    {
        if (target != null)
        {
            Vector3 dest = target.position - turret.transform.position;
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
    private TurretSO.MagazineInfo GetProjectile()
    {
        // 如果激发区有弹药 直接使用激发区弹药
        if (curProjectilNum > 0)
        {
            curProjectilNum -= 1;
            return curMagzineData;
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
                return curMagzineData;
            }
        }

        Debug.Log("没有弹药");
        return null;

    }
}
