using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteHandler : MonoBehaviour
{
    public Tilemap movementTilemap; // Reference to the Tilemap (Ground)
    public Tile highlightTile; // Tile to use for highlighting moves
    public Transform characterTransform; // Reference to the character (Ant)

    void Start()
    {
        movementTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        characterTransform = GameObject.Find("Ant").transform;
    }

    public void HighlightMovementTiles()
    {
        Vector3Int basePosition = movementTilemap.WorldToCell(characterTransform.position);
        Debug.Log("Highlighting tiles at position: " + basePosition);
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                Vector3Int tilePosition = new Vector3Int(basePosition.x + x, basePosition.y + y, basePosition.z);
                int distance = Mathf.Abs(x) + Mathf.Abs(y);
                if (distance > 0 && distance <= 2)
                {
                    movementTilemap.SetTile(tilePosition, highlightTile);
                }
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        Vector3Int basePosition = movementTilemap.WorldToCell(characterTransform.position);
        Debug.Log("Clearing highlighted tiles around: " + basePosition);
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                Vector3Int tilePosition = new Vector3Int(basePosition.x + x, basePosition.y + y, basePosition.z);
                if (movementTilemap.GetTile(tilePosition) == highlightTile)
                {
                    movementTilemap.SetTile(tilePosition, null);
                }
            }
        }
    }
}
