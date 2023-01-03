using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
// A player is defined as the current GameObject, its child text component, and a button
public class Player
{
    public Image panel;
    public TMP_Text text;
    public Button button;
}
[System.Serializable]
// Activate/Deactivate a Player panel based on whose turn it is
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    // Responsible for the Flow/Logic of the game. Must understand the state of all the buttons
    public TMP_Text[] buttonList;

    // Player vs. Computer
    private string playerSide;
    private string computerSide;
    public bool playerMove;
    public float delay;
    private int value;

    // Game Over Component
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    // Track the Moves, Winner
    private int moveCount;
    private bool winner = true;  // Avoid changing it to true for every possible winning combo

    // Restart Component
    public GameObject restartButton;

    // Players & Player Colors
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    // Display Panel
    public GameObject startInfo;

    // Enable the player to start the game first
    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        moveCount = 0;
        restartButton.SetActive(false);
        playerMove = true;
    }

    // Prompt the CPU for an avaliable space on the grid
    void Update()
    {
        if (playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if(delay >= 100)
            {
                value = Random.Range(0, 8);
                if(buttonList[value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList[value].text = GetComputerSide();
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
        }
    }

    // For each button, link the controller with the grid spaces
    void SetGameControllerReferenceOnButtons()
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }
    // Prompt player for piece, assign the opposite piece to the computer side, then start the game 
    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }
        StartGame();
    }
    // Once a button is clicked, play the game
    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    // Button is clicked & it's interactable
    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    // Once it's clicked and set, indicate a made move, go through the logic(win conditions, change sides, change turns, etc.)
    public void EndTurn()
    {
        // Move is made  
        ++moveCount;

    // Player Wins
        // Top Row
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            GameOver(playerSide);
        // Middle Row
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
            GameOver(playerSide);
        // Bottom Row
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
            GameOver(playerSide);
        // Left Column
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
            GameOver(playerSide);
        // Middle Column
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
            GameOver(playerSide);
        // Right Column
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
            GameOver(playerSide);
        // Negative Sloped Diagonal
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
            GameOver(playerSide);
        // Positive Sloped Diagonal
        else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
            GameOver(playerSide);

    // Computer Wins 
        // Top Row
        else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide)
            GameOver(computerSide);
        // Middle Row
        else if (buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide)
            GameOver(computerSide);
        // Bottom Row
        else if (buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide)
            GameOver(computerSide);
        // Left Column
        else if (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide)
            GameOver(computerSide);
        // Middle Column
        else if (buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide)
            GameOver(computerSide);
        // Right Column
        else if (buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide)
            GameOver(computerSide);
        // Negative Sloped Diagonal
        else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide)
            GameOver(computerSide);
        // Positive Sloped Diagonal
        else if (buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide)
            GameOver(computerSide);

        // If all the spaces are occupied, indicate no winner
        else if (moveCount >= 9)
        {
            winner = false;
            GameOver("draw");
        }
        // Move onto the next player
        else
        {
            ChangeSides();
            delay = 10;
        }
            
    }
    // Modify the Player Buttons when sides are changed 
    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    // After finding a 3 of a kind, disable all of the buttons, display the winner(either Player or CPU)
    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);
        if (winningPlayer == "draw" && winner == false)
        {
            SetGameOverText("It's a Draw!");
            SetPlayerColorsInactive();
        }
        else
            SetGameOverText(winningPlayer + " Wins!");
        // Display the Play Again button 
        restartButton.SetActive(true);  
    }

    // Modify the side, activate the side's panel(account for the computer side)
    void ChangeSides()
    {
        playerMove = (playerMove == true) ? false : true;
        if (playerMove == true)
            ActivatePanel(computerSide);
        else
            ActivatePanel(playerSide);
    }

    // Modify the text appropriate to the state of the game!(Winning Move or Full Board)
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        // Activate the Game Over & Restart button, Prompt User for piece in Deactivated State
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);   // Hide the display panel
        playerMove = true;
        delay = 10;

        // Empty the GridSpaces
        for (int i = 0; i < buttonList.Length; i++)
            buttonList[i].text = "";    
    }

    // The board interactability is dependent on the state of the game!
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
    // If in waiting state: Activate. If in game state: Deactivate.
    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }
    // Prompt the user for a starting panel, when no one wins
    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
    // Highlight the player panel based on either the player or computer piece
    void ActivatePanel(string previousPlayer)
    {
        if (previousPlayer == "X")
            SetPlayerColors(playerO, playerX);
        else
            SetPlayerColors(playerX, playerO);
    }

}
