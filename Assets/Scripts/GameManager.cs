using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Deploy, Player, Enemy, QueenAnt }
    public GameState gameState;
    public TextMeshProUGUI gameStateText;
    public Button endTurnButton;
    public GridManager gridManager;

    public ScoutAnt scoutAnt;

    void Start()
    {
        gameState = GameState.Player;

        if (gameStateText != null)
        {
            gameStateText.text = $"Game State: {gameState}";
        }

        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
        }
        
    }

    void Update()
    {

    }

    void OnEndTurnButtonClicked()
    {
        // Handle the button click logic here
        Debug.Log("End Turn Button Clicked!");

        if(gameState == GameState.Player){
            gameState = GameState.QueenAnt;
            turnQueenAnt();
        }

        // Update the game state text
        if (gameStateText != null)
        {
            gameStateText.text = $"Game State: {gameState}";
        }
        
    }


    //QueenAnt's turn
    void turnQueenAnt(){
        //reset all player ants move and ability
        scoutAnt.hasMoved = false;
        scoutAnt.usedAbility = false;

        gridManager.moveQueen();
        gameState = GameState.Player;
        //move queenant one step towards nest.
    }

    //AntLion's turn
    void turnAntLion(){

    }
}
