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
    public class PlayerModelBean
    {
        public ShipMemento shipMemento;
        public Dictionary<string, int> inventoryDict;

        public PlayerModelBean()
        {

        }
    }

    private string saveName = "default";
    private ShipMemento shipMemento;
    private PlayerInventory inventory;

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

    public void SetPlayerMemento(ShipMemento shipMemento)
    {
        this.shipMemento = shipMemento;
    }

    public void SetPlayerMemento(IShipController ship)
    {
        this.shipMemento = new ShipMemento(ship);
    }

    public void LoadLocalSave()
    {
        //shipMemento = ShipMemento.LoadMemento(Path.Combine(Application.persistentDataPath, "Saves", saveName + ".json"));
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
        }
        //shipMemento?.SaveMemento(Path.Combine(Application.persistentDataPath, "Saves", saveName + ".json"));

        string json = JsonConvert.SerializeObject(this);
        try
        {
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "Saves", saveName + ".json"), json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("写入失败\n" + e.Message);
        }

        Debug.Log("保存成功");
    }


}
