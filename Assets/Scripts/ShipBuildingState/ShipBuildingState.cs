using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuildingState : IShipBuildingState
{
    public ShipBuildingState(ShipBuildController sbc)
    {
        Init(sbc);
    }

    public void ChangeIndex(ShipBuildController sbc, int index)
    {
        sbc.BuildIndex = index;
        sbc.PrefabShadow.sprite = sbc.UnitCanBuild[sbc.BuildIndex].fullsizeSprite;
    }

    public void QuitBuild(ShipBuildController sbc)
    {
        sbc.IsBuilding = false;
        sbc.State = new ShipNotBuildingState(sbc);
    }

    public void StartBuild(ShipBuildController sbc)
    {
        // 已经是建造状态了 do nothing
        Init(sbc);
    }

    public void Init(ShipBuildController sbc)
    {
        // 打开虚影 设置图像 调整方向
        sbc.PrefabShadow.gameObject.SetActive(true);
        sbc.IsBuilding = true;
        sbc.PrefabShadow.sprite = sbc.UnitCanBuild[sbc.BuildIndex].fullsizeSprite;
        sbc.PrefabShadow.transform.localRotation = DirExtensions.DirToQuaternion(sbc.BuildDir);
        // 打开底色
        sbc.Sc.InterfaceObj.SetAllFGridNodeBackGroundActive(true);
    }

    public void Update(ShipBuildController sbc)
    {
        // 右键如果没有东西退出建造 有东西删除
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int centerGridXY = sbc.Sc.InterfaceObj.Grid.WorldPositionToGridXY(mp);
            var go = sbc.Sc.InterfaceObj.Grid.GetGridObject(centerGridXY);

            // 有东西删除
            if (go != null && go.GetContent() != null)
            {
                GameObject.Destroy(go.GetContent().gameObject);
            }
            // 退出
            else
            {
                QuitBuild(sbc);
            }
        }

        // 虚影跟随 v1
        UnitSO uso = sbc.UnitCanBuild[sbc.BuildIndex];
        // 获取正中心到左下并且根据dir旋转的偏移值
        var offset = uso.GetSpritCMtoLBOffsetByDir(sbc.BuildDir);

        Vector3 mouseOffsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(offset.x , offset.y);
        Vector2Int gridXY = sbc.Sc.InterfaceObj.Grid.WorldPositionToGridXY(mouseOffsetPos);
        var gridobj = sbc.Sc.InterfaceObj.Grid.GetGridObject(gridXY);
        if (gridobj != null)
        {            
            sbc.PrefabShadow.sprite = uso.fullsizeSprite;
            var shadowPos = gridobj.transform.TransformPoint(-offset) + Vector3.forward;
            sbc.PrefabShadow.gameObject.transform.position = shadowPos;
            //sbc.PrefabShadow.gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
        }
        else
        {
            sbc.PrefabShadow.sprite = null;
        }

        // 建造部分
        // 左键建造
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UnitSO selectUnit = sbc.UnitCanBuild[sbc.BuildIndex];

            // 找到所有被占据的位置
            List<FGridNode> place = sbc.Sc.InterfaceObj.Grid.GetObjectPlaceByPosList(gridXY, selectUnit.place, sbc.BuildDir);

            //保证所有的点都不是null才能算是能建造
            // 检测是否能创建
            bool canbuild = true;
            if (!place.Contains(null))
            {
                // 并且所有点里面都没有东西
                foreach (var item in place)
                {
                    if (item.GetContent() != null)
                    {
                        canbuild = false;
                        break;
                    }
                }
            }
            else
            {
                canbuild = false;
            }

            if (canbuild)
            {
                // 创造
                UnitObject uo = UnitObject.UnitFactoryCreate(selectUnit, gridXY, sbc.BuildDir, sbc.Sc.InterfaceObj.Grid);

                foreach (var item in place)
                {
                    item.SetContent(uo);
                }

                // 如果是飞船unit 设置为创造这个的飞船
                if (uo is IShipUnit)
                {
                    (uo as IShipUnit).SetShip(sbc.Sc.InterfaceObj);
                }
            }
            else
            {
                LogUtilsXY.LogOnMousePos("不能建造");
            }
        }

        // R调整方向
        if (Input.GetKeyDown(KeyCode.R))
        {
            sbc.BuildDir = DirExtensions.GetNextDir(sbc.BuildDir);
            sbc.PrefabShadow.transform.localRotation = DirExtensions.DirToQuaternion(sbc.BuildDir);
        }
    }

    #region 新版input 提取static方法供buildcontroller用

    public static void BuildUnit(ShipBuildController sbc)
    {
        // 找到所有被占据的位置
        UnitSO selectUnit = sbc.UnitCanBuild[sbc.BuildIndex]; // 要建造的单元
        var offset = selectUnit.GetSpritCMtoLBOffsetByDir(sbc.BuildDir);   // 根据旋转找到图像正中心到坐下判定点的偏移值
        Vector3 mouseOffsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + sbc.Sc.InterfaceObj.Grid.GetParent().transform.TransformVector(offset);
        Vector2Int gridXY = sbc.Sc.InterfaceObj.Grid.WorldPositionToGridXY(mouseOffsetPos);
        List<FGridNode> place = sbc.Sc.InterfaceObj.Grid.GetObjectPlaceByPosList(gridXY, selectUnit.place, sbc.BuildDir);

        //保证所有的点都不是null才能算是能建造
        // 检测是否能创建
        bool canbuild = true;
        if (!place.Contains(null))
        {
            // 并且所有点里面都没有东西
            foreach (var item in place)
            {
                if (item.GetContent() != null)
                {
                    canbuild = false;
                    break;
                }
            }
        }
        else
        {
            canbuild = false;
        }

        if (canbuild)
        {
            // 创造
            UnitObject uo = UnitObject.UnitFactoryCreate(selectUnit, gridXY, sbc.BuildDir, sbc.Sc.InterfaceObj.Grid);

            foreach (var item in place)
            {
                item.SetContent(uo);
            }

            // 如果是飞船unit 设置为创造这个的飞船
            if (uo is IShipUnit)
            {
                (uo as IShipUnit).SetShip(sbc.Sc.InterfaceObj);
            }
        }
        else
        {
            LogUtilsXY.LogOnMousePos("不能建造");
        }
    }

    public static void ShadowPerFrame(ShipBuildController sbc)
    {
        // 找到所有被占据的位置
        UnitSO selectUnit = sbc.UnitCanBuild[sbc.BuildIndex]; // 要建造的单元
        var offset = selectUnit.GetSpritCMtoLBOffsetByDir(sbc.BuildDir);   // 根据旋转找到图像正中心到坐下判定点的偏移值
        Vector3 mouseOffsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + sbc.Sc.InterfaceObj.Grid.GetParent().transform.TransformVector(offset);
        Vector2Int gridXY = sbc.Sc.InterfaceObj.Grid.WorldPositionToGridXY(mouseOffsetPos);
        var gridobj = sbc.Sc.InterfaceObj.Grid.GetGridObject(gridXY);

        if (gridobj != null)
        {
            sbc.PrefabShadow.sprite = selectUnit.fullsizeSprite;
            var shadowPos = gridobj.transform.TransformPoint(-offset) - Vector3.forward;
            sbc.PrefabShadow.gameObject.transform.position = shadowPos;
            sbc.PrefabShadow.transform.localRotation = DirExtensions.DirToQuaternion(sbc.BuildDir);
            //sbc.PrefabShadow.gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
        }
        else
        {
            sbc.PrefabShadow.sprite = null;
        }
    }

    public static void RotateBuildDir(ShipBuildController sbc)
    {
        sbc.BuildDir = DirExtensions.GetNextDir(sbc.BuildDir);
        sbc.PrefabShadow.transform.localRotation = DirExtensions.DirToQuaternion(sbc.BuildDir);
    }

    public static bool TryDeleteUnitOnMousePos(ShipBuildController sbc)
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int centerGridXY = sbc.Sc.InterfaceObj.Grid.WorldPositionToGridXY(mp);
        var go = sbc.Sc.InterfaceObj.Grid.GetGridObject(centerGridXY);

        // 有东西删除
        if (go != null && go.GetContent() != null)
        {
            GameObject.Destroy(go.GetContent().gameObject);
            return true;
        }

        return false;
    }

    #endregion
}
