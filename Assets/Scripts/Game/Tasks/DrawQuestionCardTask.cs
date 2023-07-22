using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TaskData))]
public class DrawQuestionCardTask : MonoBehaviour, IGameTask
{
    private TaskData taskData;

    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private List<TMP_Text> answersTexts;

    private Question questionCard;
    private Answer[,] answers;
    private int x, y;

    private Answer selectedAnswer;

    private void BoundaryCheck(ref int x, ref int y)
    {
        int n = Question.MaxAnswers / 2;

        if(x < 0)  x = 0;
        if(x >= n) x = n - 1;
        if(y < 0)  y = 0;
        if(y >= n) y = n - 1;
    }

    public void HandleInput()
    {
        // Select answer
        if(this.taskData.Player.Input.Left.triggered)
            this.y--;
        if(this.taskData.Player.Input.Right.triggered)
            this.y++;
        if(this.taskData.Player.Input.Up.triggered)
            this.x--;
        if(this.taskData.Player.Input.Down.triggered)
            this.x++;

        // Check for out of bounds index
        BoundaryCheck(ref this.x, ref this.y);

        this.selectedAnswer = this.answers[this.x, this.y];

        // Highlight by index
        GetComponent<HighlightBackground>().Idx = this.x + this.y * (Question.MaxAnswers / 2);

        if(this.taskData.Player.Input.Interact.triggered)
        {
            this.selectedAnswer.Prize.AddItem(this.taskData.Player);
            this.taskData.Running = false;
        }
    }

    private void DrawCard()
    {
        // Pick at random
        int idx = new System.Random().Next(0, Questions.Cards.Count);
        this.questionCard = Questions.Cards[idx];

        // Set up the UI
        this.cardDescription.text = questionCard.Description;

        for(int i = 0; i < questionCard.Answers.Count; ++i)
        {
            this.answersTexts[i].text = this.questionCard.Answers[i].Description;
        }

        // Set up the answers matrix
        int n = Question.MaxAnswers / 2;
        this.answers = new Answer[n, n];

        // Column major
        for(int i = 0; i < n; ++i)
            for(int j = 0; j < n; ++j)
                this.answers[j, i] = this.questionCard.Answers[n*i + j];
        
        // First answer as default
        this.x = this.y = 0;
        this.selectedAnswer = this.answers[this.x, this.y];
    }

    private void HighlightAnswer(Answer answer)
    {

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
