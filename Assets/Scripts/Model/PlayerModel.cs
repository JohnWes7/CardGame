using Newtonsoft.Json;
using QFramework;
using Johnwest;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

/// <summary>
/// caretaker
/// </summary>
public class PlayerModel : Singleton<PlayerModel>
{
    [System.Serializable]
    public class PlayerModelBean
    {
        public ShipMemento shipMemento;
        public Dictionary<string, int> inventoryDict;
        public int currency;
        public Dictionary<string, int> unitInventoryDict;
        public List<string> techUnlockList;

        public PlayerModelBean()
        {
        }

        public PlayerModelBean(ShipMemento shipMemento, Dictionary<string, int> inventoryDict, int currency)
        {
            this.shipMemento = shipMemento;
            this.inventoryDict = inventoryDict;
            this.currency = currency;
        }
    }

    private string saveName = "default";

    // 一场游戏中的 存档信息
    // 保存的船
    private ShipMemento shipMemento;
    // 玩家资源仓库
    private PlayerInventory inventory;
    // 玩家货币
    private BindableProperty<int> currency;
    // 玩家单位仓库
    private PlayerUnitInventory playerUnitInventory;
    // 玩家科技树遗物
    private TechRelicInventory techRelicInventory;


    // 可能需要添加长期数据信息 写在另一个类里面更好
    public PlayerModel()
    {
        currency = new BindableProperty<int>(0);
        currency.Register((int value) => {
            EventCenter.Instance.TriggerEvent("CurrencyChange", this, value);
        });
        shipMemento = null;
        inventory = new PlayerInventory();
        playerUnitInventory = new PlayerUnitInventory();
        techRelicInventory = new TechRelicInventory();
    }

    // 获取物品仓库
    public PlayerInventory GetInventory()
    {
        if (inventory == null)
        {
            inventory = new PlayerInventory();
        }
        return inventory;
        }

    // 获取单位仓库
    public PlayerUnitInventory GetPlayerUnitInventory()
    {
        return playerUnitInventory;
    }

    // 获取科技树遗物
    public TechRelicInventory GetTechRelicInventory()
    {
        return techRelicInventory;
    }

    public ShipMemento GetShipMemento()
    {
        return shipMemento;
    }

    /// <summary>
    /// 用来更新当前保存的ship
    /// </summary>
    /// <param name="ship"></param>
    public void SetShipMemento(IShipController ship)
    {
        this.shipMemento = new ShipMemento(ship);
    }

    public void LoadResourceSave(string path)
    {
        string json = Resources.Load<TextAsset>(path).text;
        PlayerModelBean bean = JsonConvert.DeserializeObject<PlayerModelBean>(json);

        LoadFromBean(bean);
    }

    public void LoadLocalSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "Saves", saveName + ".json");
        try
        {
            // 导入本地存档 生成bean
            string json = File.ReadAllText(path);
            PlayerModelBean bean = JsonConvert.DeserializeObject<PlayerModelBean>(json);

            LoadFromBean(bean);
            Debug.Log($"成功导入本地存档: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"file path {path}\n" + e.Message);
        }

        if (shipMemento == null)
        {
            Debug.LogError("null save load");
        }
    }

    private void LoadFromBean(PlayerModelBean bean)
    {
        try
        {
            // 从bean中提取飞船数据
            shipMemento = bean.shipMemento;

            // 提取仓库数据 因为数据里面是item的name需要交给inventory转录 覆盖仓库
            GetInventory().LoadFromItemNameNumPairs(bean.inventoryDict);
            JWUniversalTool.LogWithClassMethodName(inventory, System.Reflection.MethodBase.GetCurrentMethod());

            // 提取货币
            currency.Value = bean.currency;

            // 提取单位仓库数据
            playerUnitInventory.LoadFromDict(bean.unitInventoryDict);
            // 提取科技树遗物
            techRelicInventory.LoadFromList(bean.techUnlockList);
        }
        catch (Exception e)
        {
            Debug.LogError("load from bean error" + e);
        }
        
    }

    public void SaveToLocal()
    {
        if (shipMemento == null)
        {
            Debug.LogError("player memento is null when save");
            return;
        }

        // 要保存的数据存入存放的类
        PlayerModelBean bean = new PlayerModelBean(shipMemento, inventory.ToItemNameNumPairs(), currency.Value);
        // 放入unitinventory
        bean.unitInventoryDict = playerUnitInventory.GetStringIntDict();
        // 放入解锁的科技树
        bean.techUnlockList = techRelicInventory.GetGetTechListString();
        
        // 保存bean到文件
        string json = JsonConvert.SerializeObject(bean);
        string path = Path.Combine(Application.persistentDataPath, "Saves", saveName + ".json");
        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }


            if (File.Exists(path))
            {
                Johnwest.JWUniversalTool.LogWithClassMethodName("文件存在 开始写入", System.Reflection.MethodBase.GetCurrentMethod());
                File.WriteAllText(path, json);
            }
            else
            {
                Johnwest.JWUniversalTool.LogWithClassMethodName("文件不存在 创建文件", System.Reflection.MethodBase.GetCurrentMethod());
                File.AppendAllText(path, json);
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError("写入失败\n" + e.Message);
            return;
        }

        Johnwest.JWUniversalTool.LogWithClassMethodName("保存成功 json:\n" + json, System.Reflection.MethodBase.GetCurrentMethod());
    }

    public int GetCurrency()
    {
        return currency.Value;
    }

    public void SetCurrency(int num)
    {
        currency.Value = num;
    }

    public void AddCurrency(int num)
    {
        currency.Value += num;
    }

    public void CostCurrency(int num)
    {
        currency.Value -= num;
    }

    public bool TryCostCurrency(int num)
    {
        if (currency.Value >= num)
        {
            currency.Value -= num;
            return true;
        }
        return false;
    }

}


public class PlayerUnitInventory
{
    private Dictionary<UnitSO, int> unitInventory;
    private string eventKeyStr = "UnitInventoryChange";

    public PlayerUnitInventory(string eventKeyStr = "UnitInventoryChange")
    {
        this.eventKeyStr = eventKeyStr;
        unitInventory = new Dictionary<UnitSO, int>();
    }

    public void AddUnit(UnitSO unit, int num)
    {
        if (unitInventory.ContainsKey(unit))
        {
            unitInventory[unit] += num;
        }
        else
        {
            unitInventory.Add(unit, num);
        }

        EventCenter.Instance.TriggerEvent(eventKeyStr, this, unitInventory);
    }

    public void RemoveUnit(UnitSO unit, int num)
    {
        if (unitInventory.ContainsKey(unit))
        {
            unitInventory[unit] -= num;
            if (unitInventory[unit] <= 0)
            {
                unitInventory.Remove(unit);
            }
        }

        EventCenter.Instance.TriggerEvent(eventKeyStr, this, unitInventory);
    }

    public Dictionary<UnitSO, int> GetUnitInventory()
    {
        return unitInventory;
    }

    public Dictionary<string, int> GetStringIntDict()
    {
        var result = new Dictionary<string, int>();
        foreach (var item in unitInventory)
        {
            result[item.Key.name] = item.Value;
        }
        return result;
    }

    /// <summary>
    /// 从可序列化的 dict 导入model Key : unit name Value : num
    /// </summary>
    /// <param name="dict"></param>
    public void LoadFromDict(Dictionary<string, int> dict)
    {
        if (dict == null)
        {
            Debug.LogError("PlayerUnitInventory 导入失败 dict 为 null");
            return;
        }

        unitInventory = new Dictionary<UnitSO, int>();
        foreach (var item in dict)
        {
            UnitSO unit = UnitInfoModel.Instance.GetUnit(item.Key);
            if (unit != null)
            {
                unitInventory.Add(unit, item.Value);
            }
            else
            {
                Debug.LogError($"PlayerUnitInventory 导入失败 unit 为 null item key: {item.Key}");
            }
        }
    }
}

public class TechRelicInventory
{
    private List<TechTreeNode> techList;
    
    public TechRelicInventory()
    {
        techList = new List<TechTreeNode>();
    }

    public void AddTech(TechTreeNode tech)
    {
        // 不能添加重复的科技树物品 毕竟解锁两次炮塔也没有用
        if (!techList.Contains(tech))
        {
            techList.Add(tech);
            Debug.Log($"added to tech inventory 获得科技 {tech.name}");
        }
        else
        {
            Debug.Log(tech.name + "该科技已经解锁过了");
        }
    }

    public void RemoveTech(TechTreeNode tech)
    {
        techList.Remove(tech);
    }

    public List<TechTreeNode> GetTechList()
    {
        return techList;
    }

    public List<UnitSO> GetUnlockUnits()
    {
        List<UnitSO> unlockUnits = new List<UnitSO>();
        techList.ForEach(tech =>
        {
            unlockUnits.Add(tech.unlockUnit);
        });

        return unlockUnits;
    }

    public List<string> GetGetTechListString()
    {
        List<string> result = new();
        foreach (TechTreeNode item in techList)
        {
            result.Add(item.name);
        }
        return result;
    }

    /// <summary>
    /// 从可序列化的list 导入model
    /// </summary>
    /// <param name="list"></param>
    public void LoadFromList(List<string> list)
    {
        if (list is null)
        {
            Debug.LogError("TechRelicInventory 导入失败 list 为 null");
            return;
        }

        techList = new List<TechTreeNode>();
        foreach (var item in list)
        {
            TechTreeNode tech = GameArchitecture.Interface.GetModel<RelicModel>().GetRelicSO<TechTreeNode>(item);
            if (tech != null)
            {
                techList.Add(tech);
            }
        }
    }
}
