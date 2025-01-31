using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Deploy, Player, Enemy, QueenAnt, End }
    public GameState gameState;
    public TextMeshProUGUI gameStateText;
    public Button endTurnButton;
    public GridManager gridManager;
    public UIManager uiManager;

    public ScoutAnt scoutAnt;
    public BuilderAnt builderAnt;
    public SoldierAnt soldierAnt;
    public QueenAnt queenAnt;

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
        // Update the game state text
        if (gameStateText != null)
        {
            gameStateText.text = $"Game State: {gameState}";
        }

        if (queenAnt.isDead){
            uiManager.ShowGameOverScreen();
        } 

        //If one ant hasMoved, make other ants hasMoved for one turn move
    }

    void OnEndTurnButtonClicked()
    {

        // Handle the button click logic here
        Debug.Log("End Turn Button Clicked!");

        gridManager.UnhighlightAllTiles();
        //uiManager.HideAllAntUI();

        gridManager.selectedAnt = -1;
        uiManager.HideScoutAntUI();
        uiManager.HideBuilderAntUI();
        uiManager.HideSoldierAntUI();

        if (gameState == GameState.Player){
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
        scoutAnt.resetAnt();
        builderAnt.resetAnt();
        soldierAnt.resetAnt();

        gridManager.moveQueen();

        if (gameState != GameState.End)
        {
            gameState = GameState.Enemy;
            turnAntLion();
        }
        //move queenant one step towards nest.
    }

    //AntLion's turn
    void turnAntLion(){
        gridManager.moveAntLion(); //move antlion towards queen

        Debug.Log($"Moved antlion");
        if (gameState != GameState.End)
        {
             Debug.Log($"Back to player turn");
            gameState = GameState.Player;
        }
    }

    public void WinGame(){
        //when queen reaches end
        Debug.Log("Queen has completed her path! You win!");
        gameState = GameState.End;
        gameStateText.text = $"Game State: {gameState}";
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("Menu");
    }
}
