using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime;

public class GameActions : MonoBehaviour {
    private GameManager gameManager;

    [SerializeField]
    private int selectedDice;

    [SerializeField] private GameObject dice;
    private GameObject diceClone;

    public GameObject diceButtonsHUD;
    public GameObject normalDiceButton;
    public GameObject doubleDiceButton;
    [SerializeField]
    private bool isDoubleDice = false;

    // TODO: Cheats / Remove
    [SerializeField] private int DICEROLL;

    void Awake() => this.gameManager = GetComponent<GameManager>();

    void OnEnable() {
        PlayerEvents.OnTurnStart += StartPlayerTurn;
        PlayerEvents.OnTurnEnd += EndPlayerTurn;

        PlayerEvents.OnMove += MovePlayer;
        PlayerEvents.OnOpenMap += OpenMap;
        PlayerEvents.OnOpenPortfolio += OpenPortfolio;
        PlayerEvents.OnOpenInventory += OpenInventory;
    }

    void OnDisable() {
        PlayerEvents.OnTurnStart -= StartPlayerTurn;
        PlayerEvents.OnTurnEnd -= EndPlayerTurn;

        PlayerEvents.OnMove -= MovePlayer;
        PlayerEvents.OnOpenMap -= OpenMap;
        PlayerEvents.OnOpenPortfolio -= OpenPortfolio;
        PlayerEvents.OnOpenInventory -= OpenInventory;
    }

    private void StartPlayerTurn(Player player) {
        // Add monthly wage and other actions
        player.Portfolio.Money += player.Portfolio.Wage;

        // INSTANTIATE DICE 
        Vector3 dicePosition = new Vector3(0f, 3f, 0f);
        diceClone = Instantiate(dice, player.transform.position + dicePosition, Quaternion.identity);

        diceButtonsHUD.SetActive(true);
        SetBothButtonsTrue();

        // Adiciona listeners aos bot�es para chamar os m�todos apropriados quando clicados
        normalDiceButton.GetComponent<Button>().onClick.AddListener(SelectNormalDice);
        doubleDiceButton.GetComponent<Button>().onClick.AddListener(SelectDoubleDice);

    }

    private void PlayDiceAnimation(GameObject diceObject) {
        Animator diceAnimator = diceObject.GetComponent<Animator>();
        diceAnimator.SetTrigger("RollDice");
    }

    private void DestroyDiceObject(GameObject diceObject) {
        Destroy(diceClone);
    }

    private void EndPlayerTurn(Player player) {
        // Current player goes to waiting
        player.StateMachine.SwitchState(player.StateMachine.Waiting());
        // Start the next player's turn
        this.gameManager.NextPlayerTurn();

    }

    private int RollDice(int selectedDice) {
        int nSteps;
        if (isDoubleDice) {
            nSteps = UnityEngine.Random.Range(1, 7) * 2;

            PlayDiceAnimation(diceClone);

            if (DICEROLL > 0)
                nSteps = DICEROLL;

            Debug.Log(nSteps);
            return nSteps;
        }
        else {
            nSteps = UnityEngine.Random.Range(1, 7);

            PlayDiceAnimation(diceClone);

            if (DICEROLL > 0)
                nSteps = DICEROLL;

            Debug.Log(nSteps);
            return nSteps;
        }
    }

    private async Task MovePlayer(Player player) {
        diceButtonsHUD.SetActive(false);
        int nSteps;
        this.selectedDice = 1;

        nSteps = RollDice(this.selectedDice);

        // Current tile
        Tile currentTile = player.CurrentTile;

        // First neighbouring tile
        Tile neighbourTile = currentTile.Neighbours.First();

        await Task.Delay(2500);
        DestroyDiceObject(diceClone);

        while (nSteps > 0) {
            Vector3 origin = currentTile.Position;
            Vector3 destination = neighbourTile.Position;

            float distance = Vector3.Distance(origin, destination);

            // Lerp between the tiles, using the walking speed as the scaling factor
            float walkingSpeed = 10.0f;
            float distanceWalked = 0.0f;

            while (distanceWalked < distance) {
                player.Position = Vector3.Lerp(origin, destination, distanceWalked / distance);
                distanceWalked += Time.deltaTime * walkingSpeed;
                await Task.Yield();
            }

            // Set the player's position to the destination, so we don't overshoot his position
            player.Position = destination;

            // We have arrived at the next tile. Update info for the next iteration
            currentTile = neighbourTile;
            player.CurrentTile = currentTile;

            // Check if the tile is a stop point
            if (currentTile.IsStopPoint) {
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

    private void OpenMap(Player player) {
        Debug.Log("Opening map.");
    }

    private void OpenPortfolio(Player player) {
        Debug.Log("Opening portfolio.");
    }

    private void OpenInventory(Player player) {
        Debug.Log("Opening inventory.");
    }

    void SelectNormalDice() {
        isDoubleDice = false;
        Debug.Log("Dado normal selecionado");
        SetBothButtonsFalse();
    }

    void SelectDoubleDice() {
        isDoubleDice = true;
        Debug.Log("Dado em dobro selecionado");
        SetBothButtonsFalse();
    }

    void SetBothButtonsFalse() {
        normalDiceButton.SetActive(false);
        doubleDiceButton.SetActive(false);
    }

    void SetBothButtonsTrue() {
        normalDiceButton.SetActive(true);
        doubleDiceButton.SetActive(true);
    }
}
