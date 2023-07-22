using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

enum PlayerStates { WAITING, STARTING, MOVING, INTERACTING }

public class PlayerStateMachine : MonoBehaviour, IStateMachine
{
    // Player components
    private Player player;
    private PlayerInput input;

    // State variables
    private Dictionary<PlayerStates, PlayerBaseState> states;
    private PlayerBaseState currentState;
    private bool isActing;

    // Getters and setters
    public Player Player => this.player;
    public PlayerInput Input => this.input;

    public bool IsActing
    {
        get { return this.isActing; }
        set { this.isActing = value; }
    }

    public PlayerBaseState CurrentState
    {
        get { return this.currentState; }
        set { this.currentState = value; }
    }

    // Public helper access functions
    public PlayerBaseState Waiting()     => this.states[PlayerStates.WAITING];
    public PlayerBaseState Starting()    => this.states[PlayerStates.STARTING];
    public PlayerBaseState Moving()      => this.states[PlayerStates.MOVING];
    public PlayerBaseState Interacting() => this.states[PlayerStates.INTERACTING];

    public void SetState(PlayerBaseState state) => this.currentState = state;

    public void SwitchState(PlayerBaseState state)
    {
        this.currentState.Exit();
        this.currentState = state;
        this.currentState.Enter();
    }

    void Awake()
    {
        // Get the player's components
        this.player = GetComponent<Player>();
        this.input = GetComponent<PlayerInput>();

        // Initialize the states
        this.states = new Dictionary<PlayerStates, PlayerBaseState>();

        // Cache the states
        this.states[PlayerStates.WAITING]     = new PlayerWaitingState(this);
        this.states[PlayerStates.STARTING]    = new PlayerStartingState(this);
        this.states[PlayerStates.MOVING]      = new PlayerMovingState(this);
        this.states[PlayerStates.INTERACTING] = new PlayerInteractingState(this);

        // Set the initial state
        this.isActing = false;
        this.currentState = Waiting();
    }

    void Update()
    {
        this.currentState.Update();
    }
}
