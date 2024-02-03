using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Belt : UnitObject, IShipUnit, IBelt
{

    [SerializeField] private MonoInterface<IShipController> ship;


    // v2
    /**
     * 只保存有没有物品
     * 然后按照过的时间去增加所有物品的posindex
     * 当物品的一部分需要延申到下一个传送带的时候
     * 用Belt带的方法来确定 : IBelt.IsRangeNull(int upRange) 返回从0到upRange中有没有item如果没有就说明可以延申
     * 等加到了point > 10 的时候就去判断可不可以添加到下一个传送带的开头
     */
    [SerializeField] private List<ItemOnBelt> itemList = new List<ItemOnBelt>();

    //scriptObject 
    private const int beltSpeed = 1;

    [Serializable]
    public class ItemOnBelt
    {
        [SerializeField] private Item item;
        [SerializeField] private int posIndex;
        [SerializeField] private bool alreadyMoved;

        public ItemOnBelt(Item item, int pos)
        {
            this.item = item;
            this.posIndex = pos;
            alreadyMoved = false;
        }

        public int PosIndex { get => posIndex; set => posIndex = value; }
        public Item Item { get => item; set => item = value; }
        public bool AlreadyMoved { get => alreadyMoved; set => alreadyMoved = value; }

        public void UpdateItemPos(BeltPoint[] points, Dir dir)
        {
            item.transform.localPosition = points[posIndex].point.VecterRotateByDir(dir);
        }

        public void UpdateItemPos(int newIndex, BeltPoint[] points, Dir dir)
        {
            posIndex = newIndex;
            item.transform.localPosition = points[posIndex].point.VecterRotateByDir(dir);
        }

        public void SetItemParent(Transform parent)
        {
            item.transform.SetParent(parent);
        }
    }

    public bool TryInsertItem(Item item)
    {
        if (CanInsertItem())
        {
            // 按照顺序插入 排在第一次发现index小于等于4的物品的前面
            // 如果发现有index > 4的话就吧index++
            int insertIndex = 0;
            foreach (var beltitem in itemList)
            {
                if (beltitem.PosIndex > 4)
                {
                    insertIndex++;
                }
                else
                {
                    break;
                }
            }

            var iob = new ItemOnBelt(item, 4);

            iob.Item.transform.SetParent(transform);
            itemList.Insert(insertIndex, iob);
            iob.UpdateItemPos(points, Dir.up);
            return true;
        }

        return false;
    }


    public bool CanInsertItem()
    {
        // 判断是否能插入中间 index 1 ~ 7 不能有item
        foreach (var item in itemList)
        {
            if (item.PosIndex < 8 && item.PosIndex > 0)
            {
                LogUtilsXY.LogOnPos("不能插入", transform.position);
                return false;
            }
        }
        return true;
    }


    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public bool DetectCrash(ItemOnBelt iob, out int maxMovement)
    {
        foreach (var item in itemList)
        {
            if (item == iob)
            {
                continue;
            }

            if (item.PosIndex > iob.PosIndex && item.PosIndex <= iob.PosIndex + 4 + beltSpeed)
            {
                maxMovement = item.PosIndex - item.PosIndex - 4 - beltSpeed;
                if (maxMovement < 0)
                {
                    maxMovement = 0;
                }
                return true;
            }
        }

        maxMovement = beltSpeed;
        return false;
    }

    public void TickV2()
    {
        var tempList = itemList.ToArray();

        for (int i = 0; i < tempList.Length; i++)
        {

            var iob = tempList[i];
            if (iob.AlreadyMoved)
            {
                continue;
            }


            // 先判断会不会和另一个相撞
            if (DetectCrash(tempList[i], out int maxmove))
            {
                // 如果相撞说明不能移动或者只能移动 下一个
                iob.UpdateItemPos(iob.PosIndex + maxmove, points, Dir.up);
                iob.AlreadyMoved = true;
                LogUtilsXY.LogOnPos("crash", iob.Item.transform.position);
                continue;
            }

            // 如果移动之后不会在边界造成冲突则直接移动
            int restNeed = iob.PosIndex + 2 - (points.Length - 1) + beltSpeed;
            if (restNeed <= 0)
            {
                iob.PosIndex = Mathf.Clamp(iob.PosIndex + beltSpeed, 0, points.Length - 3);
                iob.UpdateItemPos(points, Dir.up);
                iob.AlreadyMoved = true;
                LogUtilsXY.LogOnPos("restNeed <= 0", iob.Item.transform.position);
                continue;
            }


            // 如果有边界冲突且下一个地方是传送带
            var gridobj = grid?.GetGridObject(position + Vector2Int.up.VecterRotateByDir(dir));
            UnitObject nextUnit = gridobj != null ? gridobj.GetContent() : null;
            nextUnit = nextUnit == null ? null : nextUnit;
            // UnitObject nextUnit = grid.GetGridObject(position + Vector2Int.up.VecterRotateByDir(dir))?.GetContent();

            if (nextUnit is IBelt)
            {
                var nextBelt = nextUnit as IBelt;
                // 如果是传送带

                int maxNextRest = nextBelt.MaxRestLenghtFrom0Index();

                // 获取最小的代表可以移动的距离
                int canMove = Mathf.Min(restNeed, maxNextRest);

                // 如果移动导致超过lenght 说明要到下一个传送带上去
                if (iob.PosIndex + canMove > points.Length - 1)
                {
                    bool result = nextBelt.EnqueueItem(iob.Item);
                    if (result)
                    {
                        itemList.Remove(iob);
                        LogUtilsXY.LogOnPos("移动到下一个belt", iob.Item.transform.position);
                    }
                    else
                    {
                        LogUtilsXY.LogOnPos("无法移动到下一个belt", iob.Item.transform.position);
                    }
                    
                    continue;
                }
                else
                {
                    iob.UpdateItemPos(iob.PosIndex + canMove, points, Dir.up);
                    iob.AlreadyMoved = true;
                    LogUtilsXY.LogOnPos("检测下一个belt但还没移上去", iob.Item.transform.position);
                    continue;
                }
            }
            else
            {
                iob.PosIndex = Mathf.Clamp(iob.PosIndex + beltSpeed, 0, points.Length - 3);
                iob.UpdateItemPos(points, Dir.up);
                iob.AlreadyMoved = true;
                LogUtilsXY.LogOnPos("没有下一个belt", iob.Item.transform.position);
                continue;
            }
        }
    }

    public int MaxRestLenghtFrom0Index()
    {
        int max = points.Length;
        foreach (var item in itemList)
        {
            if (item.PosIndex - 2 < max)
            {
                max = item.PosIndex - 2;
            }
        }

        if (max < 0)
        {
            max = 0;
        }

        return max;
    }

    public bool EnqueueItem(Item item)
    {
        
        foreach (var myItem in itemList)
        {
            if (myItem.PosIndex < 5)
            {
                return false;
            }
        }

        ItemOnBelt iob = new ItemOnBelt(item, 0);
        itemList.Insert(itemList.Count, iob);
        iob.SetItemParent(transform);
        iob.AlreadyMoved = true;
        iob.UpdateItemPos(points, Dir.up);

        return true;
    }

    [SerializeField] private float testTimer = 0;

    private void Update()
    {
        if (testTimer < 1)
        {
            testTimer += Time.deltaTime;
        }
        else
        {
            testTimer = 0;
            TickV2();

            foreach (var item in itemList)
            {
                item.AlreadyMoved = false;
            }
        }
    }

    public bool TryGrabItem(out Item item)
    {
        throw new NotImplementedException();
    }

    // 之后可能需要改成tick

    [SerializeField] private BeltPoint[] points;



    [Serializable]
    public struct BeltPoint
    {
        public Vector3 point;
        private Item item;

        public Item Item { get => item; }

        public void SetItem(Item item, Dir dir)
        {
            this.item = item;
            item.transform.localPosition = point.VecterRotateByDir(dir);
        }

        public void RemoveItem()
        {
            item = null;
        }
    }

    //private bool TryInsertItemV1(Item item)
    //{
    //    if (CanInsertItem())
    //    {
    //        item.transform.SetParent(transform);

    //        int index = points.Length / 2 - 1;
    //        points[index].SetItem(item, dir);

    //        return true;
    //    }

    //    return false;
    //}

    //private bool CanInsertItemV1()
    //{
    //    bool condition = true;
    //    for (int i = points.Length / 2 - 4; i < points.Length / 2 + 3; i++)
    //    {
    //        if (points[i].Item != null)
    //        {
    //            LogUtilsXY.LogOnPos("不能插入", transform.position - Vector3.forward);
    //            condition = false;
    //            break;
    //        }
    //    }

    //    return condition;
    //}

    //private void TickV1()
    //{
    //    // 判断下一个移动的最大位置
    //    // 即能移动的最大距离

    //    // 先判断下一个是不是传送带
    //    Vector2Int nextBeltPos = position + Vector2Int.up.VecterRotateByDir(dir);
    //    UnitObject unit = grid.GetGridObject(nextBeltPos)?.GetContent();

    //    if (unit is IBelt)
    //    {

    //    }
    //    else
    //    {
    //        // 如果下一个格子不是可以给物品通过的地方
    //        for (int i = points.Length - 1; i >= 0; i--)
    //        {
    //            if (points[i].Item != null)
    //            {
    //                // 每一个物体占有自生和前后两个位置
    //                // <--
    //                // 6 5 4 3 2 1 Item p p
    //                // 那就要从3开始判断
    //                // 如果speed 是1也就是说 必须要 2 + speed + 2 = 这一段必须要没有任何东西

    //                int detectIndexLength = 2 + beltSpeed + 2;
    //                int canMoveLength = 0;
    //                bool condistion = true;

    //                for (int j = i + 1; j <= detectIndexLength + i; j++)
    //                {
    //                    // 越界说明需要移动到下一个传送带
    //                    if (j >= points.Length)
    //                    {
    //                        canMoveLength = j - i - 2;
    //                        condistion = false;
    //                        break;
    //                    }

    //                    if (points[j].Item != null)
    //                    {
    //                        canMoveLength = j - i - 4;
    //                        condistion = false;
    //                        break;
    //                    }
    //                }

    //                if (condistion)
    //                {
    //                    canMoveLength = beltSpeed;
    //                }

    //                // 如果可以移动的距离大于0 能移动几个point就移动多少
    //                if (canMoveLength > 0)
    //                {
    //                    points[i + canMoveLength].SetItem(points[i].Item, dir);
    //                    points[i].RemoveItem();
    //                }
    //            }
    //        }
    //    }
    //}

}
