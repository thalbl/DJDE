using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// TODO: Idealmente, gostariamos de ler todas as informacoes estaticas de um arquivo e carrega-las em tempo de execucao. Pode ser um arquivo binario, JSON, etc. Para o MVP do projeto, essa abordagem funciona.
public static class Bonuses
{
   public static readonly List<Bonus> Cards = new List<Bonus>()
   {
       new Bonus("Você acaba de ganhar 100 reais!", new BonusAddMoneyItem(100)),
       new Bonus("Você acaba de ser promovido! Seu salário foi aumentado em 300 reais!", new BonusIncreaseWageItem(300))
   };
}

public abstract class BonusItem
{
    public abstract void AddBonus(Player player);
}

public class BonusAddMoneyItem : BonusItem
{
    private int amount;

    public BonusAddMoneyItem(int amount)
    {
        this.amount = amount;
    }

    // TODO: check for overflow
    public override void AddBonus(Player player) => player.Portfolio.Money += this.amount;
}

public class BonusIncreaseWageItem : BonusItem
{
    private int amount;

    public BonusIncreaseWageItem(int amount)
    {
        this.amount = amount;
    }

    public override void AddBonus(Player player) => player.Portfolio.Wage += this.amount;
}

public class Bonus
{
    private string description;
    private BonusItem item;

    public string Description
    {
        get { return this.description; }
        set { this.description = value; }
    }

    public BonusItem Item
    {
        get { return this.item; }
        set { this.item = value; }
    }

    public void AddBonus(Player player) => this.item.AddBonus(player);

    public Bonus(string description, BonusItem item)
    {
        this.description = description;
        this.item = item;
    }
}
