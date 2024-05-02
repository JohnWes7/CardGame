using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SpaceportShopModel : Singleton<SpaceportShopModel>, ICanSendCommand
{
    private List<UnitSO> allUnits;
    private List<SpaceportShopProductInfo> currentUnits;

    public int level = 1;
    public int numUnits = 4;
    public int refreshCount = 0;
    public float inflation = 1f;

    public SpaceportShopModel()
    {
        UnitListSO unitListSO = Resources.Load<UnitListSO>("Default/Unit/DefaultUnit/default_unit");

        this.allUnits = unitListSO.unitSOList;
        this.currentUnits = new();

        // 打印初始化的时候所有能随机出来的单位
        Debug.Log("SpaceportShopModel: init unit list:" + Johnwest.JWUniversalTool.ListToString(allUnits));
    }

    /// <summary>
    /// 生成一个商店售卖的单位的列表
    /// </summary>
    /// <param name="level"></param>
    public void GenerateCurUnits()
    {
        currentUnits.Clear();

        List<UnitSO> allPool = new List<UnitSO>(allUnits);

        // 获取解锁的单位
        var getAdditionCommand = new GetAddtionalUnitCommand();
        this.SendCommand(getAdditionCommand);
        allPool.AddRange(getAdditionCommand.unitSOList);

        for (int i = 0; i < numUnits; i++)
        {
            // Choose a random unit from the list of unlocked units
            int index = Random.Range(0, allPool.Count);


            // Add the chosen unit to the list of current units
            currentUnits.Add(new SpaceportShopProductInfo(allPool[index], Mathf.RoundToInt(allPool[index].cost * inflation)));
        }

        EventCenter.Instance.TriggerEvent("SpaceportShopUpdate", this, this);
    }

    /// <summary>
    /// 刷新商店 并且扣除玩家货币
    /// </summary>
    public void RefreshShop()
    {
        var costcommand = new TryCostCurrencyCommand(GetRefreshCost());
        this.SendCommand(costcommand);

        if (costcommand.isCost)
        {
            refreshCount++;
            GenerateCurUnits();
            Debug.Log("刷新商店成功\n" + GetRefreshCost());
        }
        else
        {
            Debug.Log("货币不足 无法刷新");
        }
    }

    public int GetRefreshCost()
    {
        return 2 * refreshCount;
    }

    // Unlocks a new unit, allowing it to be sold in the shop
    public void UnlockUnit(UnitSO unit)
    {
        allUnits.Add(unit);
    }

    public List<SpaceportShopProductInfo> GetCurrentUnits()
    {
        return currentUnits;
    }

    public void SetBoughtFlag(int index, bool value = true)
    {
        currentUnits[index].isBought = value;
        EventCenter.Instance.TriggerEvent("SpaceportShopCurBoughtChange", this, currentUnits);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}

public class SpaceportShopProductInfo
{
    public UnitSO unitSO;
    public int cost;
    public bool isBought = false;

    public SpaceportShopProductInfo(UnitSO unitSO, int cost)
    {
        this.unitSO = unitSO;
        this.cost = cost;
    }
}

