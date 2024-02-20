using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class RightBarInventoryPanel : MonoBehaviour
{
    [SerializeField] private Transform itemContext;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<RightBarInventoryItemIcon> iconList;
    [SerializeField] private SerializableDictionary<ItemSO, RightBarInventoryItemIcon> iconDict;
    [SerializeField] private ItemSO test;

    private void Awake()
    {
        iconDict = new SerializableDictionary<ItemSO, RightBarInventoryItemIcon>();
    }

    private void Start()
    {
        PlayerModel.Instance.GetInventory().OnInventoryChange += Instance_OnInventoryChange;
        //PlayerInventory.Instance.AddItem(test, 50);
        RefreshIcon(PlayerModel.Instance.GetInventory().GetInventory());
        //Johnwest.JWUniversalTool.LogWithClassMethodName(PlayerModel.Instance.GetInventory(), System.Reflection.MethodBase.GetCurrentMethod());
    }

    private void Instance_OnInventoryChange(object sender, PlayerInventory.InventoryEventArgs e)
    {
        RefreshIcon(e.dict);
    }

    public void RefreshIcon(Dictionary<ItemSO, int> inventoryDict)
    {
        foreach (var item in inventoryDict)
        {
            if (iconDict.ContainsKey(item.Key))
            {
                iconDict[item.Key].RefreshIcon(item.Key, item.Value);
            }
            else
            {
                // 没有的icon要重新生成
                GameObject gameObject = Instantiate<GameObject>(itemPrefab, itemContext);
                var rbii = gameObject.GetComponent<RightBarInventoryItemIcon>();
                rbii.RefreshIcon(item.Key, item.Value);
                iconList.Add(rbii);
                iconDict.Add(item.Key, rbii);
            }
        }
    }

    [ContextMenu("LogAllInventory")]
    public void LogAllInventory()
    {
        Debug.Log(PlayerModel.Instance.GetInventory());
    }
}
