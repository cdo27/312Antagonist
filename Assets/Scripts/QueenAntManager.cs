using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class QueenAntManager : MonoBehaviour
{
    public Vector2Int queenPosition;
    public Vector2Int exitPosition;  // Exit position on the grid
    public Tilemap tilemap; 
    public float moveSpeed = 2.0f;
    public bool hasFinishedMoving = true; // Track if the Queen Ant is done moving

    private bool hasWon = false;     // if the win condition has been met

    void Start()
    {
        // Initialize starting position
        queenPosition = new Vector2Int(6, -5);
        exitPosition = new Vector2Int(13, -1);
    }

    void Update()
    {
        // // Move Queen Ant when clicking for testing (replace this with turn based logic)
        // if (Input.GetMouseButtonDown(0))
        // {
        //     MoveTowardsExit();
        // }
    }

    public void MoveTowardsExit()
    {
        if (hasWon) return; // No movement if the win condition is met

        // Get the world position of the exit
        Vector3 exitWorldPosition = tilemap.GetCellCenterWorld((Vector3Int)exitPosition);

        // Check if the Queen Ant's current world position matches the exit's position
        if (transform.position == exitWorldPosition)
        {
            Debug.Log("Queen Ant has reached the exit!");
            hasWon = true; // Mark the win condition as true
            OnWin(); // Trigger the win logic
            return;
        }

        // Calculate direction to move 1 tile closer to the exit
        Vector2Int direction = exitPosition - queenPosition;

        // Normalize direction to move only 1 tile at a time
        if (direction.x != 0) direction.x = direction.x > 0 ? 1 : -1;
        if (direction.y != 0) direction.y = direction.y > 0 ? 1 : -1;

        // Update the Queen's grid position
        queenPosition += direction;
        hasFinishedMoving = false; // Mark the Queen as moving

        // Convert the grid position to a world position on the Tilemap
        Vector3 targetPosition = tilemap.GetCellCenterWorld((Vector3Int)queenPosition);

        // Smoothly move the Queen Ant to the target position
        StartCoroutine(MoveToPosition(targetPosition));
    }

    // Coroutine to smoothly move the Queen Ant to the target position
    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        Debug.Log("testing queen");
        while ((transform.position - targetPosition).magnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        hasFinishedMoving = true;

        // Snap to the final position
        transform.position = targetPosition;

        // Check for win condition after snapping
        CheckWinCondition();
    }

    // Check if the Queen Ant has reached the exit
    private void CheckWinCondition()
    {
        Vector3 exitWorldPosition = tilemap.GetCellCenterWorld((Vector3Int)exitPosition);

        if (transform.position == exitWorldPosition)
        {
            Debug.Log("Queen Ant has reached the exit! You win!");
            hasWon = true;
            OnWin();
        }
    }

    // Win condition
    private void OnWin()
    {
        Debug.Log("You Win!");
        //play scene, ui, etc.
    }
}
