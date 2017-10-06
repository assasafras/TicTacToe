using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

    public Button button;
    public Text buttonText;

    public static GameController GameController { get; set; }

    public void SetSpace()
    {
        buttonText.text = GameController.PlayerSide;
        button.interactable = false;
        GameController.EndTurn();
    }
}
