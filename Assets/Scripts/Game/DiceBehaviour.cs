using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceBehaviour : MonoBehaviour
{
    public int DiceValues;
    public int DiceTotal;
    public bool normalDice = false;

    // Start is called before the first frame update
    void Start()
    {   

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollTheDice() {
        Debug.Log(Roll());
    }

    public int Roll() {
        DiceValues = Random.Range(1, 7);

        return DiceValues;
    }
}
