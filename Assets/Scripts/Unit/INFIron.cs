using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

/// <summary>
/// 无限出铁机器 用于之前的玩法 把物品搬到传送带上 废弃
/// </summary>
public class INFIron : UnitObject, IShipUnit
{
    [SerializeField] protected MonoInterface<IShipController> shipController;
    [SerializeField, ReadOnly] protected Item buffer;

    // 之后提取为scriptObject
    [SerializeField, ForceFill] protected ItemSO createItemSO;
    [SerializeField, ReadOnly] protected float timer;
    [SerializeField] protected float creatTime;
    [SerializeField] protected Vector2Int insertPos;
    [SerializeField] protected GameObject centerIcon;
    [SerializeField] protected Vector3 iconPos = new Vector3(0, -0.31f, -0.1f);

    protected override void Awake()
    {
        base.Awake();
        timer = 0;
        RefreshCenterIcon();
    }


    public void RefreshCenterIcon()
    {
        if (centerIcon != null)
        {
            Destroy(centerIcon);
        }

        centerIcon = Instantiate<GameObject>(createItemSO.prefab, transform);
        centerIcon.transform.localPosition = iconPos;
        centerIcon.transform.localRotation = Quaternion.identity;
    }

    public static Vector2Int TransformGridPoint(Vector2Int LeftButtomPos, Vector2Int Pos, Dir dir)
    {
        return LeftButtomPos + Pos.VecterRotateByDir(dir);
    }

    public IShipController GetShip()
    {
        return shipController.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        shipController.InterfaceObj = sc;
    }

    protected virtual void Update()
    {
        // 生成矿物
        if (timer < creatTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;




            #region 通过传送带运输
            // 检测出口位置
            Vector2Int insertPosWithDir = TransformGridPoint(position, insertPos, dir);
            LogUtilsXY.LogOnPos($"Try Create {createItemSO.name} On {insertPosWithDir}", transform.position - Vector3.forward);


            var gridobj = grid?.GetGridObject(insertPosWithDir);
            UnitObject unit = gridobj != null ? gridobj.GetContent() : null;

            // 如果从出口位置是传送带 
            if (unit is IBelt && unit != null)
            {

                var belt = unit as IBelt;
                Item item = buffer != null ? buffer : Item.CreateItemFactory(createItemSO);
                item.transform.SetParent(transform);

                item.transform.localRotation = Quaternion.identity;

                Vector2 createLocalPos = insertPos - new Vector2(0.5f, 1.5f);
                item.transform.localPosition = new Vector3(createLocalPos.x, createLocalPos.y);
                bool result = belt.TryInsertItem(item);

                if (!result)
                {
                    buffer = item;
                }
                else
                {
                    buffer = null;
                }
            }
            return;
            #endregion

        }
    }


}
