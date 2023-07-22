using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(TaskData))]
public class DrawBonusCardTask : MonoBehaviour, IGameTask
{
    private TaskData taskData;
    
    [SerializeField] private TMP_Text cardDescription;

    private Bonus bonusCard;

    public void HandleInput()
    {
        if(this.taskData.Player.Input.Interact.triggered)
        {   
            this.bonusCard.AddBonus(this.taskData.Player);
            this.taskData.Running = false;
        }
    }

    private void DrawCard()
    {
        // Pick at random
        int idx = new System.Random().Next(0, Bonuses.Cards.Count);
        this.bonusCard = Bonuses.Cards[idx];
        this.cardDescription.text = bonusCard.Description;
    }

    void Start()
    {
        this.taskData = GetComponent<TaskData>();
        DrawCard();
    }

    void Update()
    {
        HandleInput();
    }
}
