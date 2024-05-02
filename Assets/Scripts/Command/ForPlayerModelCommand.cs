using QFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TryCostCurrencyCommand : AbstractCommand
{
    public int cost;
    public bool isCost;

    public TryCostCurrencyCommand(int cost)
    {
        this.cost = cost;
    }

    protected override void OnExecute()
    {
        if (PlayerModel.Instance.GetCurrency() >= cost)
        {
            PlayerModel.Instance.CostCurrency(cost);
            isCost = true;
            return;
        }

        isCost = false;
    }
}

public class GetAddtionalUnitCommand : AbstractCommand
{
    public List<UnitSO> unitSOList;


    protected override void OnExecute()
    {
        unitSOList = PlayerModel.Instance.GetTechRelicInventory().GetUnlockUnits();
    }
}
