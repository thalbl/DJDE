using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(TaskData))]
public class SelectPathTask : MonoBehaviour, IGameTask
{
    private TaskData taskData;

    private Tile selectedTile;
    private List<Tile> neighboursList;
    private int idx;

    [SerializeField] private GameObject arrow;

    public void HandleInput()
    {
        int nTiles = this.neighboursList.Count;

        // Auxiliar lambda for modular function
        Func<int, int, int> mod = (int x, int m) => { return (x % m + m) % m; };

        // Check for player input
        if(this.taskData.Player.Input.Left.triggered || this.taskData.Player.Input.Up.triggered)
            this.idx = mod(--this.idx, nTiles);
        if(this.taskData.Player.Input.Right.triggered || this.taskData.Player.Input.Down.triggered)
            this.idx = mod(++this.idx, nTiles);

        this.selectedTile = this.neighboursList[this.idx];
        
        // Rotate visual arrow torwards the selected tile
        this.arrow.transform.LookAt(this.selectedTile.transform);

        if(this.taskData.Player.Input.Interact.triggered)
        {
            // Done selecting, set the next tile and stop
            this.taskData.NextTile = selectedTile;
            this.taskData.Running = false;
        }
    }

    void Start()
    {
        this.taskData = GetComponent<TaskData>();

        //Get the list of neighbours
        this.neighboursList = this.taskData.Tile.Neighbours;
        this.idx = 0;

        // Auxiliar visual arrow
        this.arrow = Instantiate(this.arrow, transform.position, Quaternion.identity);
        this.arrow.transform.parent = this.gameObject.transform;

        this.selectedTile = this.neighboursList[this.idx];
        this.arrow.transform.LookAt(this.selectedTile.transform);
    }

    void Update()
    {
        HandleInput();
    }
}
