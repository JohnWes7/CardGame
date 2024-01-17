public interface IShipController
{
    Grid<FGridNode> Grid { get; set; }

    void SetAllFGridNodeBackGroundActive(bool value);
}