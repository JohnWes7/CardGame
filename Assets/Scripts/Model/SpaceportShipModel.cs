using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpaceportShopModel : Singleton<SpaceportShopModel>
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
    }

    /// <summary>
    /// 生成一个商店售卖的单位的列表
    /// </summary>
    /// <param name="level"></param>
    public void GenerateCurUnits()
    {
        currentUnits.Clear();

        for (int i = 0; i < numUnits; i++)
        {
            // Choose a random unit from the list of unlocked units
            int index = Random.Range(0, allUnits.Count);

            // Add the chosen unit to the list of current units
            currentUnits.Add(new SpaceportShopProductInfo(allUnits[index], Mathf.RoundToInt(allUnits[index].cost * inflation)));
        }

        EventCenter.Instance.TriggerEvent("SpaceportShopUpdate", this, this);
    }

    /// <summary>
    /// 刷新商店 并且扣除玩家货币
    /// </summary>
    public void RefreshShop()
    {
        if (PlayerModel.Instance.GetCurrency() >= GetRefreshCost())
        {
            PlayerModel.Instance.CostCurrency(GetRefreshCost());
            refreshCount++;
            GenerateCurUnits();
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
}

public class SpaceportShopProductInfo
{
    public UnitSO unitSO;
    public int cost;

    public SpaceportShopProductInfo(UnitSO unitSO, int cost)
    {
        this.unitSO = unitSO;
        this.cost = cost;
    }
}