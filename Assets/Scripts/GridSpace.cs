using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public TMP_Text buttonText;
    private GameController gameController;

    // When the button is clicked, declare the piece, and indicate it as non-interactable
    public void SetSpace()
    {
        if(gameController.playerMove == true)
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
        }
        
    }

    // Create and Allow a Controller Reference
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
}
