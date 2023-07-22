using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class GameTask : MonoBehaviour
{
    [SerializeField] private GameObject gameTaskObj;

    public Tile NextTile;

    public async Task Run(Player player, Tile tile)
    {
        // Assure there's always a valid next tile to step into
        this.NextTile = tile.Neighbours.First();

        // No assigned task obj
        if(gameTaskObj == null)
            return;

        // Switch player to the interacting state
        player.StateMachine.SwitchState(player.StateMachine.Interacting());

        // Instatiate task object into the scene
        var task = Instantiate(this.gameTaskObj, tile.Position, Quaternion.identity);

        // Setup data for the task
        var taskData    = task.GetComponent<TaskData>();
        taskData.Player = player;
        taskData.Tile   = tile;

        // Run the task
        taskData.Running = true;

        // Wait until the task is done
        while(taskData.Running)
        {
            await Task.Yield();
        }

        // Set the next tile, if any
        this.NextTile = taskData.NextTile;

        // Switch back to the moving state
        player.StateMachine.SwitchState(player.StateMachine.Moving());

        // Done, destroy task object
        Destroy(task);
    }
}