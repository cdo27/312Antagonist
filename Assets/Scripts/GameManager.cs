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

    public GameObject antLionBanner;
    public GameObject playerBanner;

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

        if (queenAnt.isDead && gameState != GameState.End)
        {
        HandleQueenAntDefeat();
        }

        //If one ant hasMoved, make other ants hasMoved for one turn move
    }

    public void ShowAntLionTurnBanner()
    {
        if (gameState == GameState.Enemy)
        {
            StartCoroutine(ShowTurnBannerWithDelay(0));
        }
    }

    public void ShowPlayerTurnBanner()
    {
        if (gameState == GameState.Player)
        {
            StartCoroutine(ShowTurnBannerWithDelay(1));
        }
    }

    private IEnumerator ShowTurnBannerWithDelay(int turn)
    {
        if (turn == 0)
        {
            antLionBanner.SetActive(true);
        }else if(turn == 1)
        {
            playerBanner.SetActive(true);
        }

        // banner disappears after a while
        yield return new WaitForSeconds(0.8f);
        antLionBanner.SetActive(false);
        playerBanner.SetActive(false);
    }

    void OnEndTurnButtonClicked()
    {

        // Handle the button click logic here
        Debug.Log("PLAYER TURN END");

        gridManager.UnhighlightAllTiles();
        //uiManager.HideAllAntUI();

        gridManager.selectedAnt = -1;
        uiManager.HideScoutAntUI();
        uiManager.HideBuilderAntUI();
        uiManager.HideSoldierAntUI();

        if (gameState == GameState.Player){
            gameState = GameState.QueenAnt;
            StartCoroutine(turnQueenAnt());
        }

        // Update the game state text
        if (gameStateText != null)
        {
            gameStateText.text = $"Game State: {gameState}";
        }



    }


    // QueenAnt's turn
    private IEnumerator turnQueenAnt()
    {
        // Reset all player ants' moves and abilities
        scoutAnt.resetAnt();
        builderAnt.resetAnt();
        soldierAnt.resetAnt();

        gridManager.moveQueen();

        if (gameState != GameState.End)
        {
            gameState = GameState.Enemy;
            yield return new WaitForSeconds(1f);

            // After the delay, move the Ant Lion
            StartCoroutine(turnAntLion());
        }

        Debug.Log("QUEEN TURN END");
    }

    //AntLion's turn
    private IEnumerator turnAntLion(){
        ShowAntLionTurnBanner();
        yield return new WaitForSeconds(1f);
        gridManager.moveAntLion(); //move antlion towards queen
        yield return new WaitForSeconds(1f);

        if (gameState != GameState.End)
        {
            gameState = GameState.Player;
            ShowPlayerTurnBanner();
        }

        Debug.Log("ANTLION TURN END");
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
    public void HandleQueenAntDefeat()
    {
    Debug.Log("Game Over! QueenAnt has been defeated.");
    gameState = GameState.End;
    gameStateText.text = "Game Over!";
    uiManager.ShowGameOverScreen(); // Display Game Over UI
    }

}
