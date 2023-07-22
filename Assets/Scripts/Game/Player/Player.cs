using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    private Tile currentTile;
        
    // Getters and setters
    public Tile CurrentTile
    {
        get { return this.currentTile; }
        set { this.currentTile = value; }
    }

    public Vector3 Position
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    // Where the dice and used item spawns
    public Vector3 SpawnerLocation => GameObject.Find("Spawner").transform.position;

    // Gameplay related
    public Inventory Inventory => GetComponent<Inventory>();
    public Portfolio Portfolio => GetComponent<Portfolio>();

    // TODO: Create a common "Input" interface for both humans and CPU
    // TODO: Create a common StateMachine interface for both humans and CPU
    // Example: private StateMachine stateMachine, private Input input
    private PlayerInput input;
    private PlayerStateMachine stateMachine;
    public PlayerInput Input => this.input;
    public PlayerStateMachine StateMachine => this.stateMachine;

    public void AddController(Controllers controller)
    {
        switch(controller)
        {
            case Controllers.HUMAN:
            {
                this.gameObject.AddComponent(typeof(PlayerInput));
                this.gameObject.AddComponent(typeof(PlayerStateMachine));

                this.input = GetComponent<PlayerInput>();
                this.stateMachine = GetComponent<PlayerStateMachine>();

                break;
            }
            case Controllers.CPU:
            {
                this.gameObject.AddComponent(typeof(CPUStateMachine));
                break;
            }
            default:
                Debug.Log("Player object has no controller assigned to it!");
                break;
        }
    }
}
