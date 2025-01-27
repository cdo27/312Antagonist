using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For the Button

public class TurnManager : MonoBehaviour
{
    public bool isPlayerTurn = true; // Track whose turn it is (true = Player, false = Queen Ant)
    public Button endTurnButton;    // Reference to the End Turn button
    public QueenAntManager queenAnt; // Reference to the Queen Ant Manager
    public SpriteHandler playerAnt;

    void Start()
    {
        // Assign the End Turn button's onClick event
        endTurnButton.onClick.AddListener(EndTurn);
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            // Allow player actions during their turn
            HandlePlayerTurn();
        }
    }

    void HandlePlayerTurn()
    {
        // Add player actions here (e.g., movement, interacting with tiles, etc.)
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Player performed an action!");
            // Example: Handle player movement logic here
        }
    }

    void EndTurn()
    {
        if (isPlayerTurn)
        {
            //Debug.Log("Ending Player's Turn...");
            isPlayerTurn = false; // Switch to Queen Ant's turn

            // Trigger Queen Ant's movement
            StartCoroutine(HandleQueenAntTurn());
        }
    }

    IEnumerator HandleQueenAntTurn()
    {
        Debug.Log("Queen Ant's Turn!");
        // Call the Queen Ant's move function
        queenAnt.MoveTowardsExit();

        // Wait for the Queen Ant to finish moving
        while (!queenAnt.hasFinishedMoving)
        {
            yield return null; // Wait until the movement coroutine is done
        }

        // End Queen Ant's turn and switch back to the player
        Debug.Log("Queen Ant's Turn Ended.");
        isPlayerTurn = true;
        playerAnt.ResetTurn();
        
    }
}
