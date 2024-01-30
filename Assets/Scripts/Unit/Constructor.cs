using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Constructor : UnitObject, IBeGrabItem, IBePutDownGrabItem, IShipUnit, IBeClick
{
    public const int ITEM_CAP_COEFFICIENT = 10;

    [System.Serializable]
    public class ItemInfo
    {
        public FormulaSO.ItemSOValue itemAndNeedValue;
        public int curNum;

        public ItemInfo(FormulaSO.ItemSOValue item)
        {
            itemAndNeedValue = item;
            curNum = 0;
        }
    }

    [System.Serializable]
    public class ConstructorFormulaMatInfo
    {
        [SerializeField] private FormulaSO curFormula;
        [SerializeField] private List<ItemInfo> rawMat;
        [SerializeField] private List<ItemInfo> outPut;
        [SerializeField] private List<ItemSO> rawItemSOList;

        public ConstructorFormulaMatInfo(FormulaSO formulaSO)
        {
            ChangeFormula(formulaSO);
        }

        public void ChangeFormula(FormulaSO formulaSO)
        {
            curFormula = formulaSO;
            rawMat ??= new List<ItemInfo>();
            outPut ??= new List<ItemInfo>();
            rawItemSOList ??= new List<ItemSO>();
            Clear();

            if (curFormula == null)
            {
                return;
            }

            foreach (FormulaSO.ItemSOValue item in formulaSO.rawMat)
            {
                rawMat.Add(new ItemInfo(item));
                rawItemSOList.Add(item.item);
            }

            foreach (var item in formulaSO.outPut)
            {
                outPut.Add(new ItemInfo(item));
            }
        }

        public List<ItemInfo> RawMat { get => rawMat; set => rawMat = value; }
        public List<ItemInfo> OutPut { get => outPut; set => outPut = value; }
        public FormulaSO CurFormula { get => curFormula; set => curFormula = value; }
        public List<ItemSO> RawItemSOList { get => rawItemSOList; set => rawItemSOList = value; }

        public bool ContainRawMat(ItemSO item)
        {
            if (curFormula == null)
            {
                return false;
            }

            foreach (var rawmat in rawMat)
            {
                if (rawmat.itemAndNeedValue.item == item)
                {
                    return true;
                }
            }
            return false;
        }
        
        public ItemInfo GetRawItemInfo(ItemSO item)
        {
            foreach (var rawmat in rawMat)
            {
                if (rawmat.itemAndNeedValue.item == item)
                {
                    return rawmat;
                }
            }

            return null;
        }

        public void Clear()
        {
            rawMat?.Clear();
            outPut?.Clear();
            rawItemSOList?.Clear();
        }

        public override string ToString()
        {
            List<string> rawString = new List<string>();
            foreach (var item in rawMat)
            {
                rawString.Add($"{item.itemAndNeedValue.item.name}:{item.curNum}");
            }

            List<string> outputString = new List<string>();
            foreach (var item in outPut)
            {
                outputString.Add($"{item.itemAndNeedValue.item.name}:{item.curNum}");
            }

            return string.Join("+", rawString) + "-->" + string.Join("+", outputString);
        }
    }


    [SerializeField] private MonoInterface<IShipController> ship;

    // 配方选择
    [SerializeField] private FormulaChoiceSO allFormula;
    //[SerializeField] private FormulaSO curFormula;
    // 点击后选择配方的ui
    [SerializeField] private MonoInterface<IShowConstructor> constructorPanel;
    [SerializeField] private GameObject panelPrefab;

    // 进行制造 数据为 ItemSOvalue代表需要的物品和执行一次所需要的个数, 后面的值代表拥有的个数
    // 明天还是改为用一个内部类来表示
    //[SerializeField] private Dictionary<FormulaSO.ItemSOValue, int> rawMatNum;
    //[SerializeField] private Dictionary<FormulaSO.ItemSOValue, int> outPutNum;
    [SerializeField] private ConstructorFormulaMatInfo matInfo;
    [SerializeField] private float timer;

    private void Start()
    {
        matInfo = new ConstructorFormulaMatInfo(null);
    }

    private void Update()
    {
        ConstructItemPerFrame();
    }

    private void ConstructItemPerFrame()
    {
        
        if (matInfo?.CurFormula == null)
        {
            timer = 0;
            return;
        }

        // 制造的update函数
        // 判断原料是否够
        bool enoughRawMat = true;
        foreach (var itemInfo in matInfo.RawMat)
        {
            if (itemInfo.curNum < itemInfo.itemAndNeedValue.value)
            {
                enoughRawMat = false;
                timer = 0;
                return;
            }
        }

        if (timer < matInfo.CurFormula.timeNeed_Second)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //判断产品出口是否还有空位可以出来
            bool enoughOutSpace = true;
            foreach (var iteminfo in matInfo.OutPut)
            {
                if (iteminfo.curNum + iteminfo.itemAndNeedValue.value > iteminfo.itemAndNeedValue.value * ITEM_CAP_COEFFICIENT)
                {
                    enoughOutSpace = false;
                    return;
                }
            }

            if (enoughOutSpace && enoughRawMat)
            {
                // 制造 减去原料
                foreach (var itemInfo in matInfo.RawMat)
                {
                    itemInfo.curNum -= itemInfo.itemAndNeedValue.value;
                }

                // 添加产品
                foreach (var iteminfo in matInfo.OutPut)
                {
                    iteminfo.curNum += iteminfo.itemAndNeedValue.value;
                }

                // 成功制造重置计时
                transform.DOShakePosition(0.3f);
                timer = 0;
            }
        }
    }

    public bool TryGrabItem(out Item item)
    {
        if (matInfo.CurFormula == null)
        {
            item = null;
            return false;
        }

        foreach (var iteminfo in matInfo.OutPut)
        {
            if (iteminfo.curNum > 0)
            {
                iteminfo.curNum -= 1;
                item = Item.CreateItemFactory(iteminfo.itemAndNeedValue.item);
                item.transform.SetParent(transform);
                item.transform.localPosition = Vector3.zero;
                return true;
            }
            
        }

        item = null;
        return false;
    }

    public bool TryPutDownItem(Item item)
    {
        // 不接受原料之外的item
        var iteminfo = matInfo.GetRawItemInfo(item.ItemSO);
        if (iteminfo == null)
        {
            return false;
        }

        if (iteminfo.curNum < iteminfo.itemAndNeedValue.value * ITEM_CAP_COEFFICIENT)
        {
            iteminfo.curNum += 1;
            Destroy(item.gameObject);
            return true;
        }

        return false;

        //if (rawMatNum == null)
        //{
        //    return false;
        //}

        //if (outPutNum == null)
        //{
        //    return false;
        //}

        //foreach (var rawItem in rawMatNum.Keys)
        //{
        //    Debug.Log(rawItem.item + " " + item.ItemSO);
        //    // 判断类别是否一样
        //    if (rawItem.item == item.ItemSO)
        //    {
        //        if (rawMatNum[rawItem] >= rawItem.value * ITEM_CAP_COEFFICIENT)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            rawMatNum[rawItem] += 1;
        //            Destroy(item.gameObject);
        //            return true;
        //        }
        //    }
        //}

        //return false;
    }

    public List<ItemSO> ItemSOInNeed()
    {
        return matInfo.RawItemSOList;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public void BeClick(object sender)
    {
        // 点击召唤一个面板给玩家选择配方
        if (constructorPanel.InterfaceObj == null)
        {
            LogUtilsXY.LogOnPos("生成面板", transform.position);
            GameObject gameObject = Instantiate<GameObject>(panelPrefab, GameObject.Find("Canvas").transform);
            constructorPanel.InterfaceObj = gameObject.GetComponent<IShowConstructor>();
        }

        // 初始化
        constructorPanel.InterfaceObj.Init(allFormula, matInfo?.CurFormula);
        constructorPanel.InterfaceObj.SetDataObserver(DataToString);
        constructorPanel.InterfaceObj.AddChangeValueListener(ChangeFormula);

    }

    public void ChangeFormula(int index)
    {
        // 如果是越界说明change到null
        if (index < 0 || index >= allFormula.formulas.Count)
        {
            matInfo.ChangeFormula(null);
            return;
        }

        // 如果换了还是相同的就什么也不干
        if (matInfo?.CurFormula == allFormula.formulas[index])
        {
            return;
        }

        // 如果配方改动则清除之前里面的东西 (后续可能要缓存成把还有的物品收回背包)
        matInfo.ChangeFormula(allFormula.formulas[index]);

        //rawMatNum.Clear();
        //outPutNum.Clear();

        //foreach (var item in curFormula.rawMat)
        //{
        //    rawMatNum.Add(item, 0);
        //}

        //foreach (var item in curFormula.outPut)
        //{
        //    outPutNum.Add(item, 0);
        //}
    }

    private string DataToString()
    {
        if (gameObject == null)
        {
            return "";
        }
        return matInfo.ToString();
    }

    private void OnDestroy()
    {
        if (constructorPanel.MonoBehaviour != null)
        {
            Destroy(constructorPanel.InterfaceObj.GetGameObject());
        }
    }

    public ItemSO Peek()
    {
        if (matInfo.CurFormula == null)
        {
            return null;
        }

        foreach (var iteminfo in matInfo.OutPut)
        {
            if (iteminfo.curNum > 0)
            {
                return iteminfo.itemAndNeedValue.item;
            }
        }

        return null;
    }
}
