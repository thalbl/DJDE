using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    [SerializeField] ItemCategory category;
    private Item item;

    public Item Item => this.item;

    void Awake()
    {
        this.item = CreateItem(this.category);
    }

    private Item CreateItem(ItemCategory category)
    {
        Item item;

        switch (category)
        {
            case ItemCategory.MEDAL:
                item = new Medal(Medal.Type.GOLD);
                break;
            case ItemCategory.SPECIAL_ITEM:
                item = new SpecialItem();
                break;
            default:
                Debug.Log("Invalid item category!");
                item = new NullItem();
                break;
        }

        return item;
    }
}
