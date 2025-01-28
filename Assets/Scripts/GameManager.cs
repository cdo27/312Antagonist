using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState { Deploy, Player, Enemy, QueenAnt }
    public GameState gameState;
    public TextMeshProUGUI gameStateText;

    void Start()
    {
        gameState = GameState.Player;

        if (gameStateText != null)
        {
            gameStateText.text = $"Game State: {gameState}";
        }
        
    }

    void Update()
    {
        if (gameStateText != null)
        {
            gameStateText.text = $"Game State: {gameState}";
        }
    }
}
