using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SelectionBase]
public class Tile : MonoBehaviour
{
    [Header("Tile Info")]
    [SerializeField] private bool isBlocked;
    [SerializeField] private bool isStopPoint;
    [SerializeField] private List<GameObject> neighboursObjs;

    // Getters and setters
    public List<Tile> Neighbours;

    public Vector3 Position => GetComponent<Transform>().position;
    public GameTask GameTask => GetComponent<GameTask>();

    public bool IsBlocked
    {
        get { return this.isBlocked;  }
        set { this.isBlocked = value; }
    }

    public bool IsStopPoint => this.isStopPoint;

    void Awake()
    {
        this.Neighbours = new List<Tile>();
        foreach(GameObject tile in this.neighboursObjs)
           this.Neighbours.Add(tile.GetComponent<Tile>());
    }
}
