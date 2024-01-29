using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalItemPrefab : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mainImage;

    private void Start()
    {
        StartCoroutine(LaterInit());
    }

    public IEnumerator LaterInit()
    {
        yield return null;
        IItemSO itemSO = GetComponent<IItemSO>();
        if (itemSO != null)
        {
            mainImage.sprite = itemSO.ItemSO.mainSprite;
        }
    }
}
