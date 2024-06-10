using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject soundManager;

    [Space(10)]

    [Header("Gameplay Components")]
    [SerializeField] private GameObject startingTile;
    [SerializeField] private new Camera camera;

    [Space(10)]

    [Header("Players")]
    [SerializeField] private int currentPlayerId;
    [SerializeField] private List<Player> players;

    [SerializeField] private GameObject Dice;

    void Start()
    {
        InitPlayers();
    }

    private void InitPlayers()
    {
        // Init player list
        this.players = new List<Player>();

        // TODO: Remove
        s_MatchSettings.SelectedCharacters[0] = Characters.STANDARD;
        s_MatchSettings.SelectedControllers[0] = Controllers.HUMAN;
        s_MatchSettings.NumPlayers = 1;

        Vector3 startPosition = startingTile.transform.position;
        // ==========

        // TODO: REFACTOR / REMOVE
        this.currentPlayerId = 0;
        for(int i = 0; i < s_MatchSettings.NumPlayers; ++i)
        {
            var selectedPlayerController = s_MatchSettings.SelectedControllers[i];
            var selectedCharacter = s_MatchSettings.SelectedCharacters[i];
            var selectedCharacterPrefab = s_GameAssets.CharactersPrefabs[selectedCharacter];

            var playerObj = Instantiate(Resources.Load(selectedCharacterPrefab) as GameObject, startPosition, Quaternion.identity);

            this.players.Add(playerObj.GetComponent<Player>());
            this.players[i].AddController(selectedPlayerController);

            // Set players at the starting tile
            this.players[i].CurrentTile = this.startingTile.GetComponent<Tile>();

            // Camera focus on player
            this.camera.GetComponent<CameraFollow>().Target = playerObj.transform;

            // Start player turn
            this.players[this.currentPlayerId].StateMachine.SwitchState(this.players[i].StateMachine.Starting());
        }
    }

    public void NextPlayerTurn()
    {
        int i = ++this.currentPlayerId % s_MatchSettings.NumPlayers;
        this.currentPlayerId = i;
        this.players[i].StateMachine.SwitchState(this.players[i].StateMachine.Starting());
    }

   
}
