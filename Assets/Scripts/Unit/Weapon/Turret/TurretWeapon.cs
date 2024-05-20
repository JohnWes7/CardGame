using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeapon : UnitObject, IShipUnit, IBePutDownGrabItem
{
    [SerializeField] private MonoInterface<IShipController> ship;

    /** 炮塔逻辑
     * 
     * 预备区
     * 子弹装填 如果curItemNum == 0 说明自己什么物品都没有 就按照turretSO里面能填所有类型的子弹
     * 
     * 装填的时候只能一个一个装 当装了一个的时候子弹类型就锁定了 就只能装该种类型的子弹
     * 所以如果 curItemNum != 0 说明已经有子弹装填进去了 之后就只能装填相同种类的子弹Item
     * 
     * 激发区
     * 开火的时候如果 curProjectilNum == 0 说明没有子弹 从备存区扣除一个弹夹item并且将 curProjectilNum 拉满
     * 以及存一个当前激发区的弹药类型 
     * 如果 激发区 有东西就一直可以开火直到curProjectilNum = 0 就去暂存区拿
     * 
     * 
     */

    // 炮塔属性
    [SerializeField] private TurretSO turretSO;
    [SerializeField] private GameObject turret;
    [SerializeField] private Transform target;
    [SerializeField] private float timer;
    private const float TURRET_ROTATE_SPEED = 2f;

    // 弹药
    // 缓存区
    [SerializeField] private int bufferItemNum;
    [SerializeField] private TurretSO.MagazineInfo bufferMagzineData;
    private const int BUFFER_ITEM_NUM_CAP = 20;
    // 激发区
    [SerializeField] private int curProjectilNum;
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
        if (target != null && (target.transform.position - turret.transform.position).magnitude > turretSO.radius)
        {
            target = null; // 超过射击范围就不再索敌
            timer = 0;
        }

        // 如果炮塔当前没有目标 尝试获取目标
        if (target == null)
        {
            RaycastHit2D enemy = Physics2D.CircleCast(Turret.transform.position, turretSO.radius, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
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
                    
                    Projectile.ProjectileCreateFactory(new Projectile.ProjectileCreationParams(projectileInfo.projectileSO, target, transform.position + transform.up, this));
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

    private TurretSO.MagazineInfo GetProjectile()
    {
        // 如果激发区有弹药 直接使用激发区弹药
        if (curProjectilNum > 0)
        {
            curProjectilNum -= 1;
            return curMagzineData;
        }

        // 如果激发区没有 则开始用暂存区的弹药
        if (bufferItemNum <= 0)
        {
            return null;
        }
        else
        {
            // 扣除暂存区弹药
            bufferItemNum -= 1;
            // 添加到激发区
            curMagzineData = bufferMagzineData;
            curProjectilNum = curMagzineData.projectileInOneMagazineNum - 1;

            return curMagzineData;
        }
    }

    public bool TryPutDownItem(Item item)
    {
        // 如果暂存区满了就不进行装填
        if (bufferItemNum >= BUFFER_ITEM_NUM_CAP)
        {
            return false;
        }


        if (!ItemSOInNeed().Contains(item.ItemSO))
        {
            return false;
        }

        // 如果就是现在这种就直接加
        if (item.ItemSO == bufferMagzineData.magazineItem)
        {
            bufferItemNum += 1;
            Destroy(item.gameObject);
            LogUtilsXY.LogOnPos($"装填 {item.ItemSO}", transform.position);
            return true;
        }

        // 如果是要切换子弹就bufferItemNum 肯定要等于0
        if (bufferItemNum <= 0)
        {
            foreach (var infos in turretSO.magazineInfos)
            {
                if (infos.magazineItem == item.ItemSO)
                {
                    bufferMagzineData = infos;
                    bufferItemNum += 1;
                    LogUtilsXY.LogOnPos($"切换弹药 并装填 {item.ItemSO}", transform.position);
                    Destroy(item.gameObject);
                    return true;
                }
            }
        }

        LogUtilsXY.LogOnPos($"有BUG 属于需要之一但没法装填进去: {item.ItemSO}", transform.position);
        return false;
    }

    public List<ItemSO> ItemSOInNeed()
    {
        var result = new List<ItemSO>();
        if (bufferItemNum > 0)
        {
            result.Add(bufferMagzineData.magazineItem);
            return result;
        }

        foreach (var item in turretSO.magazineInfos)
        {
            result.Add(item.magazineItem);
        }
        return result;
    }
}

