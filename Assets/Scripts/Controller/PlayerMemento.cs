using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 弃用
/// </summary>
[System.Serializable]
public class PlayerMemento
{
    public ShipMemento shipMemento;
    
    public Dictionary<string, int> inventoryMemento;

    public PlayerMemento(IShipController ship, Dictionary<ItemSO, int> inventory)
    {
        shipMemento = new ShipMemento(ship);

        inventoryMemento = new Dictionary<string, int>();
        foreach (var item in inventory)
        {
            inventoryMemento.Add(item.Key.name, item.Value);
        }
    }

    public PlayerMemento()
    {

    }

    public void SaveMemento(string filePath)
    {
        string json = JsonConvert.SerializeObject(this);
        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }


            if (File.Exists(filePath))
            {
                Debug.Log("文件存在 开始写入");
                File.WriteAllText(filePath, json);
            }
            else
            {
                Debug.Log("文件不存在 创建文件");
                File.AppendAllText(filePath, json);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("write fail\n" + e.Message);
            return;
        }

        Debug.Log("save successful");
    }

    public static PlayerMemento LoadMemento(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            PlayerMemento playerMemento = JsonConvert.DeserializeObject<PlayerMemento>(json);
            return playerMemento;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"can not read file in {filePath}\n" + e.Message);
            return null;
        }
    }

    public override string ToString()
    {
        string json = JsonConvert.SerializeObject(this);
        return json;
    }
}
