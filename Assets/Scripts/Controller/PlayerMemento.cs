using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("write fail\n" + e.Message);
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
