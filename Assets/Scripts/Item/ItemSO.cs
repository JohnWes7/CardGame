using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomInspector;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public enum Type
    {
        Ammo,
        Currency
    }

    [Preview(Size.big), AssetsOnly]
    public Sprite mainSprite;
    [Preview(Size.big), AssetsOnly]
    public GameObject prefab;
    public Type type;
    public int maxStack = 100;
}
