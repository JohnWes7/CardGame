using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BuildPanelController : MonoBehaviour
{
    public class BuildPanelEventHandler : EventArgs
    {
        public  UnitSO beClickUnit;

        public BuildPanelEventHandler(UnitSO beClickUnit)
        {
            this.beClickUnit = beClickUnit;
        }
    }


    [SerializeField] private Image selectFlag;
    [SerializeField] private Transform uiContent;
    [SerializeField] private List<UnitIcon> uiUnitIconList;
    [SerializeField] private GameObject uiIconPrefab;
    [SerializeField] private UnitSO value;

    private const int MOVE_ANI_X = 120;
    private const int ORINGIN_X_POS = -900; 

    public event EventHandler<BuildPanelEventHandler> OnUnitValueChange;

    // 逻辑: 
    /**
     * 点击之后 改变value 改变value 触发OnUnitValueChange
     * 
     */
    //private void Awake()
    //{
    //    Debug.Log(transform.localPosition);
    //}

    public void OpenPanel(List<UnitSO> showUnitList, UnitSO selectIndex)
    {
        transform.DOLocalMoveX(ORINGIN_X_POS, 0.2f);
    }

    public void ClosePanel()
    {
        transform.DOLocalMoveX(transform.localPosition.x - MOVE_ANI_X, 0.2f);
    }

    /// <summary>
    /// 设置标记选择的那个框跟踪的位置
    /// </summary>
    public void SetSelectFlag(UnitSO unitSO)
    {
        if (unitSO == null)
        {
            selectFlag.gameObject.SetActive(false);
            return;
        }

        UnitIcon unitIcon = GetIconByUnitSO(unitSO);
        if (unitIcon == null)
        {
            selectFlag.gameObject.SetActive(false);
            return;
        }

        
        if (selectFlag.gameObject.activeInHierarchy)
        {
            selectFlag.transform.DOLocalMove(transform.InverseTransformPoint(unitIcon.transform.position), 0.2f);
        }
        else
        {
            selectFlag.transform.localPosition = transform.InverseTransformPoint(unitIcon.transform.position);
            selectFlag.gameObject.SetActive(true);
        }
    }

    private UnitIcon GetIconByUnitSO(UnitSO unitSO)
    {
        foreach (UnitIcon item in uiUnitIconList)
        {
            if (item.UnitSO == unitSO)
            {
                return item;
            }
        }
        return null;
    }

    public void RefreshIcon(List<UnitSO> showUnitList)
    {
        // 清除之前的icon
        foreach (var item in uiUnitIconList)
        {
            Destroy(item.gameObject);
        }
        uiUnitIconList.Clear();

        //生成新的icon
        foreach (var item in showUnitList)
        {
            GameObject gameObject = Instantiate<GameObject>(uiIconPrefab, uiContent);
            gameObject.name = item.name;
            UnitIcon unitIcon = gameObject.GetComponent<UnitIcon>();
            unitIcon.RefreshIcon(item);
            uiUnitIconList.Add(unitIcon);

            // 添加事件
            unitIcon.OnClick.AddListener(() => {
                //Debug.Log(item);
                value = item;
                SetSelectFlag(item);
                OnUnitValueChange?.Invoke(this, new BuildPanelEventHandler(item));
            });
        }
    }
}
