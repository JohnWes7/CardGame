using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    // 保存的船
    private ShipMemento shipMemento;
    // 玩家仓库
    private PlayerInventory inventory;
    // 玩家货币
    private int currency;

    public PlayerInventory GetInventory()
    {
        if (inventory == null)
        {
            inventory = new PlayerInventory();
        }
        return inventory;
    }

    public ShipMemento GetShipMemento()
    {
        return shipMemento;
    }

    public void SetShipMemento(ShipMemento shipMemento)
    {
        this.shipMemento = shipMemento;
    }

    /// <summary>
    /// 用来更新当前保存的ship
    /// </summary>
    /// <param name="ship"></param>
    public void SetShipMemento(IShipController ship)
    {
        this.shipMemento = new ShipMemento(ship);
    }

    public void LoadLocalSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "Saves", saveName + ".json");
        try
        {
            // 导入本地存档 生成bean
            string json = File.ReadAllText(path);
            PlayerModelBean bean = JsonConvert.DeserializeObject<PlayerModelBean>(json);

            // 从bean中提取飞船数据
            shipMemento = bean.shipMemento;
            
            // 提取仓库数据 因为数据里面是item的name需要交给inventory转录 覆盖仓库
            GetInventory().LoadFromItemNameNumPairs(bean.inventoryDict);
            Johnwest.JWUniversalTool.LogWithClassMethodName(inventory, System.Reflection.MethodBase.GetCurrentMethod());

            // 提取货币
            currency = bean.currency;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"can not read file in {path}\n" + e.Message);
        }

        if (shipMemento == null)
        {
            Debug.LogError("null save load");
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
        PlayerModelBean bean = new PlayerModelBean(shipMemento, inventory.ToItemNameNumPairs(), currency);

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
        return currency;
    }

    public void AddCurrency(int num)
    {
        currency += num;
    }

    public bool TryCostCurrency(int num)
    {
        if (currency >= num)
        {
            currency -= num;
            return true;
        }
        return false;
    }

    public void CostCurrency(int num)
    {
        currency -= num;
    }
}
