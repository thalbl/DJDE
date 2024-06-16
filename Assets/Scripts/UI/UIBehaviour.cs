using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    public GameObject diceButtonsHUD;
    public GameObject dialogue;
    public GameObject normalDiceButton;
    public GameObject doubleDiceButton;

    public void Dialogue(string targetText) {
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

    public void TriggerActivity(int d, int o) {
        dialogue.SetActive(d == 0 ? dialogue.activeInHierarchy : d > 0);
        diceButtonsHUD.SetActive(o == 0 ? diceButtonsHUD.activeInHierarchy : o > 0);
    }

    public void TriggerActivity(int d, int o, int o1, int o2) {
        dialogue.SetActive(d == 0 ? dialogue.activeInHierarchy : d > 0);
        diceButtonsHUD.SetActive(o == 0 ? diceButtonsHUD.activeInHierarchy : o > 0);
        normalDiceButton.SetActive(o == 0 ? diceButtonsHUD.activeInHierarchy : o > 0);
        doubleDiceButton.SetActive(o == 0 ? diceButtonsHUD.activeInHierarchy : o > 0);
    }
}
