using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { NULL, MEDAL, SPECIAL_ITEM }

public abstract class Item
{
    public ItemCategory Category;
    public abstract void AddItem(Player player);
}

public class NullItem : Item
{
    public override void AddItem(Player player) {}
}

public class Medal : Item
{
    public enum Type { BRONZE, SILVER, GOLD }
    private Type type;

    public Medal(Type type)
    {
        this.type = type;
    }

    public override void AddItem(Player player)
    {
        // TOOD: Refactor
        player.Portfolio.Medals += 1;
    }
}

public class SpecialItem : Item
{
    public SpecialItem() {}

    public override void AddItem(Player player)
    {
        player.Inventory.Items.Add(this);
    }
}

