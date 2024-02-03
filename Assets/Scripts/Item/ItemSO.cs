using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomInspector;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    [Preview(Size.big), AssetsOnly]
    public Sprite mainSprite;
    [Preview(Size.big), AssetsOnly]
    public GameObject prefab;
}
