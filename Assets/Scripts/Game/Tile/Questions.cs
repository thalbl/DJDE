using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Questions
{
    public static readonly List<Question> Cards = new List<Question>()
    {
        new Question("Quanto é 2 + 2?", 
                      new Answer("22", new NullItem()), 
                      new Answer("4", new Medal(Medal.Type.GOLD)), 
                      new Answer("Batata", new NullItem()),
                      new Answer("( ͡° ͜ʖ ͡°)", new NullItem()))
    };
}

public class Answer
{
    private string description;
    private Item prize;
    
    public string Description => this.description;
    public Item Prize => this.prize;

    public Answer(string description, Item prize=null)
    {
        this.description = description;
        this.prize = prize;
    }
}

public class Question
{
    private string description;
    private List<Answer> answers;

    public const int MaxAnswers = 4;
    public string Description => this.description;
    public List<Answer> Answers => this.answers;

    public Question(string description, params object[] answers)
    {
        this.description = description;
        this.answers = new List<Answer>();

        if(answers.Length > MaxAnswers)
            throw new ArgumentException("O limite máximo de respostas que podem ser atribuídas para uma pergunta foi ultrapassado!");
        
        foreach(object entry in answers)
        {
            if (entry == null) 
                throw new ArgumentNullException();
            this.answers.Add((Answer)entry);
        }
    }
}