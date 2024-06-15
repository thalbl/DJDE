using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime;
using TMPro;

public class GameActions : MonoBehaviour {
    private GameManager gameManager;

    [SerializeField]
    private int selectedDice;

    [SerializeField] private GameObject dice;
    private GameObject diceClone;

    public GameObject diceButtonsHUD;
    public GameObject dialogue;
    public Text dialogueText;
    public GameObject normalDiceButton;
    public GameObject doubleDiceButton;
    [SerializeField]
    private bool isDoubleDice = false;
    private string targetText = "";

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

        TriggerActivity(1, 1);
        Dialogue("Qual dado?");
        // Adiciona listeners aos botões para chamar os métodos apropriados quando clicados
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
        TriggerActivity(1, -1);
        int nSteps;
        this.selectedDice = 1;

        nSteps = RollDice(this.selectedDice);

        // Current tile
        Tile currentTile = player.CurrentTile;

        // First neighbouring tile
        Tile neighbourTile = currentTile.Neighbours.First();

        
        await Task.Delay(2500);
        Dialogue($"Andará {nSteps} casas!");
        await Task.Delay(1000);
        TriggerActivity(-1, -1);
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
        Dialogue("Dado normal selecionado! Pressione \"E\" para continuar");
        Debug.Log("Dado normal selecionado");
        TriggerActivity(1, -1);
    }

    void SelectDoubleDice() {
        isDoubleDice = true;
        Dialogue("Dado duplo selecionado! Pressione \"E\" para continuar");
        Debug.Log("Dado em dobro selecionado");
        TriggerActivity(1, -1);
    }

    void TriggerActivity(int d, int o) {
        dialogue.SetActive(d == 0 ? dialogue.activeInHierarchy : d > 0);
        diceButtonsHUD.SetActive(o == 0 ? diceButtonsHUD.activeInHierarchy : o > 0);
    }

    void Dialogue(string targetText) {
        if (dialogue != null) {
            TextMeshProUGUI dialogText = dialogue.GetComponentInChildren<TextMeshProUGUI>();
            if (dialogText != null) {
                dialogText.text = targetText;
            }
            else {
                Debug.LogWarning("O componente TextMeshProUGUI não foi encontrado como filho do GameObject.");
            }
        }
        else {
            Debug.LogWarning("O GameObject do tipo Image não foi atribuído.");
        }
    }
}
