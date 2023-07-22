using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: Idealmente, gostariamos de ler todas as informacoes estaticas de um arquivo e carrega-las em tempo de execucao. Pode ser um arquivo binario, JSON, etc. Para o MVP do projeto, essa abordagem funciona.
public static class Penalties
{
    public static readonly List<Penalty> Cards = new List<Penalty>()
    {
        new Penalty("Sua geladeira quebrou! Você acaba de gastar 500 reais para comprar uma nova.", new PenaltySubtractMoneyItem(500)),
        new Penalty("Você acaba de ser demovido! Seu salário foi diminuido em 200 reais.", new PenaltyDecreaseWageItem(200))
    };
}
public abstract class PenaltyItem
{
    public abstract void AddPenalty(Player player);
}
public class PenaltySubtractMoneyItem : PenaltyItem
{
    private int amount;
    public PenaltySubtractMoneyItem(int amount)
    {
        this.amount = amount;
    }
    public override void AddPenalty(Player player)
    {
        player.Portfolio.Money -= amount;
        if(player.Portfolio.Money < 0) 
            player.Portfolio.Money = 0;
    }
}
public class PenaltyDecreaseWageItem : PenaltyItem
{
    private int amount;
    public PenaltyDecreaseWageItem(int amount)
    {
        this.amount = amount;
    }
    public override void AddPenalty(Player player)
    {
        player.Portfolio.Wage -= amount;
        if(player.Portfolio.Wage < player.Portfolio.MinWage)
            player.Portfolio.Wage = player.Portfolio.MinWage;
    }
}
public class Penalty
{
    private string description;
    private PenaltyItem item;
    public string Description
    {
        get { return this.description; }
        set { this.description = value; }
    }
    public PenaltyItem Item
    {
        get { return this.item; }
        set { this.item = value; }
    }

    public void AddPenalty(Player player) => this.item.AddPenalty(player);

    public Penalty(string description, PenaltyItem item)
    {
        this.description = description;
        this.item = item;
    }
}