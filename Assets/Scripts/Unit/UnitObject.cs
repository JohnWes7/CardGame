using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有可以建造的物体的父类
/// </summary>
public class UnitObject : MonoBehaviour
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

        unitObject.position = position;
        unitObject.unitSO = unitSO;
        unitObject.dir = dir;
        unitObject.grid = grid;

        return unitObject;
    }

    [SerializeField] protected Vector2Int position;
    [SerializeField] protected Dir dir;
    [SerializeField] protected UnitSO unitSO;
    [SerializeField] protected Grid<FGridNode> grid;

    public UnitSO UnitSO { get => unitSO; set => unitSO = value; }

    /// <summary>
    /// 在自己的grid上面找到改位置的unitObject 如果没有则返回null
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

}
