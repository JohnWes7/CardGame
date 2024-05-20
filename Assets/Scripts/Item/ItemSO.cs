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


    [Preview(Size.big), AssetsOnly] public Sprite mainSprite;
    [ShowMethod(nameof(GetAutoMainColor))] public Color mainColor = Color.white;
    [Button(nameof(SetAutoColorToMainColor))]
    [Preview(Size.big), AssetsOnly] public GameObject prefab;
    public Type type;
    public int maxStack = 99;


    public Color GetAutoMainColor()
    {
        if (mainSprite == null)
        {
            return Color.white;
        }

        int width = mainSprite.texture.width;
        int height = mainSprite.texture.height;
        float totalR = 0f;
        float totalG = 0f;
        float totalB = 0f;
        int pixelCount = 0;
        for (int y = 0; y < height; ++y)
        {

            for (int x = 0; x < width; ++x)
            {
                Color pixelColor = mainSprite.texture.GetPixel(x, y);

                if (!pixelColor.Equals(Color.clear))
                {
                    totalR += pixelColor.r;
                    totalG += pixelColor.g;
                    totalB += pixelColor.b;
                    ++pixelCount;
                }
            }
        }
        return new Color((totalR / pixelCount), (totalG / pixelCount), (totalB / pixelCount));
    }

    private void SetAutoColorToMainColor()
    {
        this.mainColor = GetAutoMainColor();
    }
}
