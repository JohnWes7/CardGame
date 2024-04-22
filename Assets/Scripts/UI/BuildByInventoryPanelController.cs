using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BuildByInventoryPanelController : UIBase, IController
{
    [SerializeField] private GameObject unitIconPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private List<BuildByInventoryIcon> iconList;

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public override void Initialize(object args = null)
    {
        
    }

    public override void OpenUI()
    {
        base.OpenUI();

        Debug.Log("BuildByInventoryPanelController OpenUI");

        //TODO: 通过事件监听来刷新UI
        // ui 需要在打开的时候 每次inventory有变动就刷新一次 (通过事件)
        // 并且每次打开的时候也初始化刷新

        // 不管ui是不是常驻的
        // 如果是只用这边接受事件 inventory变化后
        InitIcon();
    }

    public void RefreshIcon()
    {
        // 先删除 不需要显示的unit
        // 寻找不需要的unit
        List<BuildByInventoryIcon> removeList =  new List<BuildByInventoryIcon>();
        foreach (BuildByInventoryIcon item in iconList)
        {
            // 如果unit的数量是0或者不在inventory中 就销毁
            if (PlayerModel.Instance.GetPlayerUnitInventory().GetUnitInventory().ContainsKey(item.unitSO) == false ||
                PlayerModel.Instance.GetPlayerUnitInventory().GetUnitInventory()[item.unitSO] == 0)
            {
                Destroy(item.gameObject);
                removeList.Add(item);
            }
        }
        // 删除不需要的unit
        iconList.RemoveAll(x => removeList.Contains(x));
        


        // 刷新所有unit
        foreach (var item in PlayerModel.Instance.GetPlayerUnitInventory().GetUnitInventory())
        {
            bool inlist = false;

            // 如果item在iconList中 就更新
            foreach (var icon in iconList)
            {
                if (icon.unitSO == item.Key)
                {
                    icon.Refresh(item.Key, item.Value);
                    inlist = true;
                    break;
                }
            }

            // 如果不在iconList中 就添加
            if (!inlist)
            {
                var icon = Instantiate<GameObject>(unitIconPrefab, content).GetComponent<BuildByInventoryIcon>();
                icon.Refresh(item.Key, item.Value);
                iconList.Add(icon);
            }
        }

    }

    public void InitIcon()
    {
        // 清空之前的图标
        foreach (var icon in iconList)
        {
            Destroy(icon.gameObject);
        }
        iconList.Clear();

        // 遍历玩家仓库中的所有单位
        foreach (KeyValuePair<UnitSO, int> unit in PlayerModel.Instance.GetPlayerUnitInventory().GetUnitInventory())
        {
            BuildByInventoryIcon icon = Instantiate<GameObject>(unitIconPrefab, content).GetComponent<BuildByInventoryIcon>();
            iconList.Add(icon);
            icon.Refresh(unit.Key, unit.Value);
        }
    }

    public void EventCenter_OnUnitInventoryChange(object sender, object args)
    {
        RefreshIcon();
    }

    private void OnEnable()
    {
        // 监听玩家unit改变事件
        EventCenter.Instance.AddEventListener("UnitInventoryChange", EventCenter_OnUnitInventoryChange);
    }

    private void OnDisable()
    {
        // 取消监听玩家unit改变事件
        EventCenter.Instance.RemoveEventListener("UnitInventoryChange", EventCenter_OnUnitInventoryChange);
    }
}
