using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using TMPro;
using QFramework;

public class SpaceportShopPanel : SingletonUIBase<SpaceportShopPanel>, IController
{
    [SerializeField, AssetsOnly, ForceFill]
    private GameObject shopItemPrefab;
    [SerializeField, ForceFill]
    private Transform content;
    [SerializeField, ReadOnly]
    private List<GameObject> shopItems = new List<GameObject>();

    [SerializeField, ForceFill]
    private TextMeshProUGUI refreshShopText;

    private void Awake()
    {
        EventCenter.Instance.AddEventListener("SpaceportShopUpdate", EventCenter_OnSpaceportShopUpdate);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("SpaceportShopUpdate", EventCenter_OnSpaceportShopUpdate);
    }

    private void OnEnable()
    {
        UpdateShop();
    }

    private void Update()
    {
        if (refreshShopText)
        {
            refreshShopText.text = SpaceportShopModel.Instance.GetRefreshCost().ToString();
        }
    }

    private void EventCenter_OnSpaceportShopUpdate(object sender, object e)
    {
        Debug.Log("接收事件 SpaceportShopUpdate");
        UpdateShop();
    }


    public override void OpenUI()
    {
        base.OpenUI();

        UpdateShop();
    }

    public void UpdateShop()
    {
        // 清空商店显示
        shopItems.ForEach(Destroy);
        shopItems.Clear();

        var curUnits = SpaceportShopModel.Instance.GetCurrentUnits();
        int index = 0;
        foreach (var productUnit in curUnits)
        {
            var go = Instantiate(shopItemPrefab, content);
            shopItems.Add(go);
            go.GetComponent<SpaceportProduct>().Init(productUnit, index);

            index++;
        }
    }

    public void RefreshShop()
    {
        //SpaceportShopModel.Instance.RefreshShop();
        //Debug.Log("刷新商店\n" + SpaceportShopModel.Instance.GetRefreshCost());
        this.SendCommand<RefreshShopCommand>();
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}


