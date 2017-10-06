using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;

    public string PlayerSide { get; private set; }
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public Text gameOverText;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    private int moveCount = 0;
    public List<List<Text>> Columns { get; private set; }
    public List<List<Text>> Rows { get; private set; }
    public List<List<Text>> Diagonals { get; private set; }

    private void Awake()
    {
        ResetGame();
    }

    private void Start()
    {
        Columns = new List<List<Text>>()
        {
            new List<Text>() { buttonList [0], buttonList [3] , buttonList [6] },
            new List<Text>() { buttonList [1], buttonList [4] , buttonList [7] },
            new List<Text>() { buttonList [2], buttonList [5] , buttonList [8] }
        };

        Rows = new List<List<Text>>()
        {
            new List<Text>() { buttonList [0], buttonList [1] , buttonList [2] },
            new List<Text>() { buttonList [3], buttonList [4] , buttonList [5] },
            new List<Text>() { buttonList [6], buttonList [7] , buttonList [8] }
        };

        Diagonals = new List<List<Text>>()
        {
            new List<Text>() { buttonList [0], buttonList [4], buttonList [8] },
            new List<Text>() { buttonList [2], buttonList [4], buttonList [6] }
        };

        GridSpace.GameController = this;
        PlayerSide = "X";
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    public void EndTurn()
    {
        moveCount++;
        if (   CheckColumns(PlayerSide) 
            || CheckRows(PlayerSide)
            || CheckDiagonals(PlayerSide))
        {
            GameOver(PlayerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("Draw");
        }
        else
            ChangeSides();
    }

    public void ChangeSides()
    {
        if(PlayerSide == "X")
        {
            SetPlayerColors(playerO, playerX);
            PlayerSide = "O";
        }
        else if(PlayerSide == "O")
        {
            SetPlayerColors(playerX, playerO);
            PlayerSide = "X";
        }
    }

    public bool CheckRows(string playerSide)
    {
        // CHeck if all the text element's in a row are the expected value.
        foreach (var row in Rows)
        {
            if(row.TrueForAll(t => t.text == playerSide))
            {
                // All text is the expected value, return true.
                return true;
            }
        }
        // No match return false.
        return false;
    }

    public bool CheckColumns(string input)
    {
        foreach (var column in Columns)
        {
            if(column.TrueForAll(t => t.text == input))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckDiagonals(string input)
    {
        foreach (var diagonal in Diagonals)
        {
            if (diagonal.TrueForAll(t => t.text == input))
            {
                return true;
            }
        }

        return false;
    }

    public void GameOver(string winner)
    {
        SetBoardInteractable(false);
        restartButton.SetActive(true);

        gameOverPanel.SetActive(true);
        gameOverText.text = winner + " Wins!"; // Note the space after the first " and Wins!"
        if(winner == "Draw")
        {
            gameOverText.text = "Draw!";
        }
    }

    public void ResetGame()
    {
        SetPlayerColors(playerX, playerO);
        restartButton.SetActive(false);
        SetBoardInteractable(true);
        foreach (var button in buttonList)
        {
            button.text = string.Empty;
        }
        PlayerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
    }

    private void SetBoardInteractable(bool isInteractable)
    {
        foreach (var button in buttonList)
        {
            button.GetComponentInParent<Button>().interactable = isInteractable;
        }
    }
}
