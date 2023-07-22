using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TaskData))]
public class DrawPenaltyCardTask : MonoBehaviour, IGameTask
{
    private TaskData taskData;
    
    [SerializeField] private TMP_Text cardDescription;

    private Penalty penaltyCard;

    public void HandleInput()
    {
        if(this.taskData.Player.Input.Interact.triggered)
        {   
            this.penaltyCard.AddPenalty(this.taskData.Player);
            this.taskData.Running = false;
        }
    }

    private void DrawCard()
    {
        // Pick at random
        int idx = new System.Random().Next(0, Penalties.Cards.Count);
        this.penaltyCard = Penalties.Cards[idx];
        this.cardDescription.text = penaltyCard.Description;
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