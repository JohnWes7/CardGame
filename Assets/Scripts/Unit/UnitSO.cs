using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "ScriptableObject/Unit")]
public class UnitSO : ScriptableObject
{
    [System.Serializable]
    public class ItemCost
    {
        public ItemSO itemSO;
        public int cost;

        public ItemCost()
        {
        }

        public ItemCost(ItemSO itemSO, int cost)
        {
            this.itemSO = itemSO;
            this.cost = cost;
        }

        public ItemCost Copy()
        {
            return new ItemCost(itemSO, cost);
        }

        public override string ToString()
        {
            return $"{itemSO} : {cost}";
        }
    }

    [HorizontalLine("创造属性")]
    [Preview, AssetsOnly]
    public GameObject prefab;
    [Preview, AssetsOnly]
    public Sprite fullsizeSprite;
    public List<Vector2Int> place;
    public Vector2 spriteBLtoCMOffset;

    [HorizontalLine("战斗属性")]
    public int maxHP = 100;

    [HorizontalLine("成本")]
    //public List<ItemCost> itemCostList;
    public int cost;

    [HorizontalLine("文本")]
    [Dictionary]
    public SerializableDictionary<string, string> nameInfo = new();
    [Dictionary]
    public SerializableDictionary<string, string> descriptionInfo = new();

    /// <summary>
    /// 获取根据转向
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public Vector2 GetSpritCMtoLBOffsetByDir(Dir dir)
    {
        return dir switch
        {
            Dir.up => -spriteBLtoCMOffset,
            Dir.right => new Vector2(-spriteBLtoCMOffset.y, spriteBLtoCMOffset.x),
            Dir.down => spriteBLtoCMOffset,
            Dir.left => new Vector2(spriteBLtoCMOffset.y, -spriteBLtoCMOffset.x),
            _ => Vector2.zero,
        };
    }
}
