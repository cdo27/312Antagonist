using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteHandler : MonoBehaviour
{
    public Tilemap movementTilemap; // Reference to the Tilemap (Ground)
    public Tile highlightTile; // Tile to use for highlighting moves
    public Transform characterTransform; // Reference to the character (Ant)
    private bool isMoving = false; // Whether the character is currently moving
    private Vector3 targetPosition; // Target position for movement

    void Start()
    {
        movementTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        characterTransform = GameObject.Find("Ant").transform;
    }

    public void HighlightMovementTiles()
    {
        Vector3Int basePosition = movementTilemap.WorldToCell(characterTransform.position);
        Debug.Log("Highlighting tiles at position: " + basePosition);

        // Highlight only adjacent tiles (up, down, left, right)
        Vector3Int[] directions = {
            new Vector3Int(0, 1, 0),  // Up
            new Vector3Int(0, -1, 0), // Down
            new Vector3Int(1, 0, 0),  // Right
            new Vector3Int(-1, 0, 0)  // Left
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int tilePosition = basePosition + direction;
            if (!movementTilemap.HasTile(tilePosition)) // Check if the tile is valid
            {
                movementTilemap.SetTile(tilePosition, highlightTile);
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        Vector3Int basePosition = movementTilemap.WorldToCell(characterTransform.position);
        Debug.Log("Clearing highlighted tiles around: " + basePosition);

        // Clear only the highlighted tiles in valid directions
        Vector3Int[] directions = {
            new Vector3Int(0, 1, 0),  // Up
            new Vector3Int(0, -1, 0), // Down
            new Vector3Int(1, 0, 0),  // Right
            new Vector3Int(-1, 0, 0)  // Left
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int tilePosition = basePosition + direction;
            if (movementTilemap.GetTile(tilePosition) == highlightTile)
            {
                movementTilemap.SetTile(tilePosition, null);
            }
        }
    }

    public void MoveCharacter(Vector3Int targetTilePosition)
    {
        if (isMoving) return;

        // Ensure the target position is a valid move (adjacent tile only)
        Vector3Int currentPosition = movementTilemap.WorldToCell(characterTransform.position);
        Vector3Int difference = targetTilePosition - currentPosition;

        if ((Mathf.Abs(difference.x) + Mathf.Abs(difference.y)) != 1)
        {
            Debug.Log("Invalid move. Can only move one tile at a time in cardinal directions.");
            return;
        }

        // Clear highlighted tiles before starting movement
        ClearHighlightedTiles();

        // Convert the tile position to world position
        targetPosition = movementTilemap.CellToWorld(targetTilePosition) + new Vector3(0.5f, 0.5f, 0); // Center on tile
        StartCoroutine(MoveToTile());
    }

    private System.Collections.IEnumerator MoveToTile()
    {
        isMoving = true;

        // Gradually move the character towards the target position
        while (Vector3.Distance(characterTransform.position, targetPosition) > 0.01f)
        {
            characterTransform.position = Vector3.MoveTowards(characterTransform.position, targetPosition, Time.deltaTime * 5f);
            yield return null;
        }

        // Snap to the exact position to prevent floating-point errors
        characterTransform.position = targetPosition;
        isMoving = false;
    }
}
