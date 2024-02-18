using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

//归 shipbuildcontroller 调用
public class BuildCostPanel : MonoBehaviour
{
    [SerializeField, AssetsOnly, ForceFill] private GameObject uiCostItemPrefab;
    [SerializeField, ForceFill] private Transform context;
    [SerializeField] private List<BuildCostItem> buildCostItemList;

    public void RefreshUnit(UnitSO unitSO)
    {
        if (unitSO == null)
        {
            gameObject.SetActive(false);
            return;
        }

        foreach (var item in buildCostItemList)
        {
            Destroy(item.gameObject);
        }
        buildCostItemList.Clear();



        // 读取unitso 里面需要多少材料
        if (unitSO.itemCostList.Count < 1)
        {
            var go = Instantiate<GameObject>(uiCostItemPrefab, context);
            BuildCostItem bci = go.GetComponent<BuildCostItem>();
            bci.Refresh(null);
            buildCostItemList.Add(bci);
            return;
        }

        foreach (var item in unitSO.itemCostList)
        {
            var go = Instantiate<GameObject>(uiCostItemPrefab, context);
            BuildCostItem bci = go.GetComponent<BuildCostItem>();
            if (bci != null)
            {
                bci.Refresh(item);
            }
            buildCostItemList.Add(bci);
        }
    }

    public void RefreshPosition(Vector3 ShadowPosition)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(ShadowPosition);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, screenPoint, Camera.main, out localPoint);
        (this.transform as RectTransform).localPosition = localPoint;
    }

    public void ShipBuildController_OnCurBuildChange(object sender, ShipBuildController.UnitEventargs args)
    {
        RefreshUnit(args.curUnit);
    }
}
