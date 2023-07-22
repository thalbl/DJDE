using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;

public class GameActions : MonoBehaviour
{
    private GameManager gameManager;

    // TODO: Cheats / Remove
    [SerializeField] private int DICEROLL;

    void Awake() => this.gameManager = GetComponent<GameManager>();

    void OnEnable()
    {
        PlayerEvents.OnTurnStart     += StartPlayerTurn;
        PlayerEvents.OnTurnEnd       += EndPlayerTurn;

        PlayerEvents.OnMove          += MovePlayer;
        PlayerEvents.OnOpenMap       += OpenMap;
        PlayerEvents.OnOpenPortfolio += OpenPortfolio;
        PlayerEvents.OnOpenInventory += OpenInventory;
    }

    void OnDisable()
    {
        PlayerEvents.OnTurnStart     -= StartPlayerTurn;
        PlayerEvents.OnTurnEnd       -= EndPlayerTurn;

        PlayerEvents.OnMove          -= MovePlayer;
        PlayerEvents.OnOpenMap       -= OpenMap;
        PlayerEvents.OnOpenPortfolio -= OpenPortfolio;
        PlayerEvents.OnOpenInventory -= OpenInventory;
    }

    private void StartPlayerTurn(Player player)
    {
        // Add monthly wage and other actions
        player.Portfolio.Money += player.Portfolio.Wage;

        // TODO: Spawn selected dice object on the player's head and start animation, etc
    }

    private void EndPlayerTurn(Player player)
    {

        // Current player goes to waiting
        player.StateMachine.SwitchState(player.StateMachine.Waiting());

        // Start the next player's turn
        this.gameManager.NextPlayerTurn();
    }

    // TODO: Delete and create/change to roll a SELECTED DICE OBJ (standard dice = 1 roll, double dice = 2 rolls, etc)
    private int RollDice()
    {
        int nSteps = new System.Random().Next(1, 11);

        if(DICEROLL > 0)
            nSteps = DICEROLL;

        Debug.Log(nSteps);

        return nSteps;
    }

    private async Task MovePlayer(Player player)
    {
        int nSteps = RollDice();

        // Current tile
        Tile currentTile = player.CurrentTile;

        // First neighbouring tile
        Tile neighbourTile = currentTile.Neighbours.First();

        while(nSteps > 0)
        {
            Vector3 origin = currentTile.Position;
            Vector3 destination = neighbourTile.Position;

            float distance = Vector3.Distance(origin, destination);

            // Lerp between the tiles, using the walking speed as the scaling factor
            float walkingSpeed = 10.0f;
            float distanceWalked = 0.0f;

            while(distanceWalked < distance)
            {
                player.Position = Vector3.Lerp(origin, destination, distanceWalked/distance);
                distanceWalked += Time.deltaTime * walkingSpeed;
                await Task.Yield();
            }

            // Set the player's position to the destination, so we don't overshoot his position
            player.Position = destination;

            // We have arrived at the next tile. Update info for the next iteration
            currentTile = neighbourTile;
            player.CurrentTile = currentTile;

            // Check if the tile is a stop point
            if(currentTile.IsStopPoint)
            {
                // Wait until the stop point task is done
                await currentTile.GameTask.Run(player, currentTile);

                // Get the next tile
                neighbourTile = currentTile.GameTask.NextTile;
                
                // Go back to moving state
                continue;
            }

            // Get the first neighbour for the next iteration
            neighbourTile = currentTile.Neighbours.First();

            // Steps are only decremented for non stop point tiles
            nSteps--;
        }

        // If current tile has any associated game task, run it
        await currentTile.GameTask.Run(player, currentTile);
    }

    private void OpenMap(Player player)
    {
        Debug.Log("Opening map.");
    }

    private void OpenPortfolio(Player player)
    {
        Debug.Log("Opening portfolio.");
    }

    private void OpenInventory(Player player)
    {
        Debug.Log("Opening inventory.");
    }
}