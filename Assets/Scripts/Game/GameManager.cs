using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
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

    void Start() {
        ConfigureMatchSettings();
        InitPlayers();
    }

    // Método para configurar as informações da partida, removendo valores hardcoded
    private void ConfigureMatchSettings() {
        // Aqui você pode configurar os jogadores e personagens dinamicamente (exemplo)
        // Pode ser vindo de uma UI ou carregado de algum sistema de salvamento
        // Exemplo básico para 2 jogadores, um humano e um CPU
        s_MatchSettings.SelectedCharacters[0] = Characters.STANDARD;
        s_MatchSettings.SelectedControllers[0] = Controllers.HUMAN;

        s_MatchSettings.SelectedCharacters[1] = Characters.STANDARD;
        s_MatchSettings.SelectedControllers[1] = Controllers.HUMAN;

        s_MatchSettings.NumPlayers = 2; // Definir o número correto de jogadores
    }

    private void InitPlayers() {
        this.players = new List<Player>();
        this.currentPlayerId = 0;

        Vector3 startPosition = startingTile.transform.position;

        for (int i = 0; i < s_MatchSettings.NumPlayers; i++) {
            CreatePlayer(i, startPosition);
        }
    }

    // Método modularizado para criar jogadores
    private void CreatePlayer(int playerIndex, Vector3 startPosition) {
        var selectedController = s_MatchSettings.SelectedControllers[playerIndex];
        var selectedCharacter = s_MatchSettings.SelectedCharacters[playerIndex];
        var selectedCharacterPrefab = s_GameAssets.CharactersPrefabs[selectedCharacter];

        // Instancia o jogador a partir do prefab do personagem selecionado
        var playerObj = Instantiate(Resources.Load(selectedCharacterPrefab) as GameObject, startPosition, Quaternion.identity);

        // Adiciona o jogador à lista e configura o controlador
        var playerComponent = playerObj.GetComponent<Player>();
        this.players.Add(playerComponent);
        playerComponent.AddController(selectedController);

        // Define o tile inicial para o jogador
        playerComponent.CurrentTile = startingTile.GetComponent<Tile>();

        // Focar a câmera no jogador criado
        camera.GetComponent<CameraFollow>().Target = playerObj.transform;

        // Inicia o turno do primeiro jogador
        if (playerIndex == currentPlayerId) {
            playerComponent.StateMachine.SwitchState(playerComponent.StateMachine.Starting());
        }
    }

    public void NextPlayerTurn() {
        // Muda para o próximo jogador, garantindo que os turnos sejam rotacionados
        int i = ++this.currentPlayerId % s_MatchSettings.NumPlayers;
        this.currentPlayerId = i;

        // Troca o estado para iniciar o turno do próximo jogador
        this.players[i].StateMachine.SwitchState(this.players[i].StateMachine.Starting());
    }
}
