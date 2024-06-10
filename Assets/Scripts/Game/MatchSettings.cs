using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public enum Stages { NULL, CITY_STAGE }
public enum Characters { NULL, STANDARD }
public enum Controllers { NULL, HUMAN, CPU }

public static class s_MatchSettings 
{
    public static int NumPlayers = 1;
    public static int Rounds = 20;
    public static bool Bonuses = false;
    public static bool RandomEvents = false;
    public static Stages SelectedStage = Stages.NULL;
    public static List<Characters> SelectedCharacters = new List<Characters> 
    { 
        Characters.NULL,
        Characters.NULL,
        Characters.NULL,
        Characters.NULL
    };
    public static List<Controllers> SelectedControllers = new List<Controllers>
    {
        Controllers.NULL,
        Controllers.NULL,
        Controllers.NULL,
        Controllers.NULL
    };
}

[System.Serializable]
public class GameAssetsData
{
    public Dictionary<Characters, string> CharactersPrefabs;
    public Dictionary<Stages, string> StagesPrefabs;
}

public static class s_GameAssets
{

    public static readonly Dictionary<Characters, string> CharactersPrefabs = new Dictionary<Characters, string>
    {
        { Characters.STANDARD, "Prefabs/Characters/StandardCharacter" }
    };
    public static readonly Dictionary<Stages, string> StagesPrefabs = new Dictionary<Stages, string>
    {
        { Stages.CITY_STAGE, "Prefabs/Stages/MVP" }
    };
}