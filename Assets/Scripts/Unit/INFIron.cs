using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无限出铁机器
/// </summary>
public class INFIron : UnitObject, IShipUnit
{
    [SerializeField] private MonoInterface<IShipController> shipController;
    [SerializeField] private Item buffer;

    // 之后提取为scriptObject
    [SerializeField] private ItemSO ironSO;
    [SerializeField] private float timer;
    [SerializeField] private float creatTime;
    [SerializeField] private Vector2Int insertPos;

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

    private void Update()
    {
        // 生成矿物
        if (timer < creatTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;

            // 检测出口位置
            Vector2Int insertPosWithDir = TransformGridPoint(position, insertPos, dir);
            LogUtilsXY.LogOnPos($"Try Create IRON On {insertPosWithDir}", transform.position - Vector3.forward);


            var gridobj = grid?.GetGridObject(insertPosWithDir);
            UnitObject unit = gridobj != null ? gridobj.GetContent() : null;

            // 如果从出口位置是传送带 
            if (unit is IBelt && unit != null)
            {

                var belt = unit as IBelt;
                Item item = buffer != null ? buffer : Item.CreateItemFactory(ironSO);
                item.transform.SetParent(transform);

                item.transform.localRotation = Quaternion.identity;

                Vector2Int createLocalPos = (insertPos - new Vector2Int(0, 1)).VecterRotateByDir(dir);
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
        }
    }
}
