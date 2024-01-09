using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/Unit")]
public class UnitSO : ScriptableObject
{
    public string unitName;
    public GameObject prefab;
    public Sprite fullsizeSprite;
    public List<Vector2Int> place;
    public Vector2 spriteBLtoCMOffset;

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
            Dir.right => new Vector2(-spriteBLtoCMOffset.x, spriteBLtoCMOffset.y),
            Dir.down => spriteBLtoCMOffset,
            Dir.left => new Vector2(spriteBLtoCMOffset.x, -spriteBLtoCMOffset.y),
            _ => Vector2.zero,
        };
    }
}
