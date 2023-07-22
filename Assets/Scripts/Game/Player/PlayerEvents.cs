using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class PlayerEvents : MonoBehaviour
{
    public static Action<Player> OnTurnStart;
    public static Action<Player> OnTurnEnd;
    public static Action<Player> OnOpenMap;
    public static Action<Player> OnOpenPortfolio;
    public static Action<Player> OnOpenInventory;

    public static Func<Player, Task> OnMove;
}
