using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class SpriteHandler : MonoBehaviour
{
    public Button moveButton; // The button to show/hide
    public Tilemap movementTilemap; // Reference to the Tilemap (Ground)
    public Tile highlightTile; // Tile to use for highlighting moves
    public Transform characterTransform; // Reference to the character (Ant)
    public float buttonScale = 0.5f; // Scale factor for the button

    private bool isSpriteClicked = false; // Flag to check if sprite is clicked
    private bool buttonDisplayed = false; // State of the button display

    void Start()
    {
        // Initially hide the move button and scale it
        if (moveButton != null)
        {
            moveButton.gameObject.SetActive(false);
            moveButton.transform.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
            UpdateButtonCollider();
            moveButton.onClick.AddListener(OnMoveButtonClick);
            movementTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
            characterTransform = GameObject.Find("Ant").transform;
        }
    }

    void OnMouseDown()
    {
        // Toggle sprite click state and button display
        isSpriteClicked = true;
    }

    void Update()
    {
        if (isSpriteClicked)
        {
            // Handle sprite being clicked
            if (!buttonDisplayed)
            {
                Debug.Log("Sprite clicked!");
                PositionButtonNearSprite();
                moveButton.gameObject.SetActive(true);
                buttonDisplayed = true;
            }
            isSpriteClicked = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Check for clicks outside the sprite to hide the button
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedTilePosition = movementTilemap.WorldToCell(mouseWorldPos);

            if (movementTilemap.HasTile(clickedTilePosition) && movementTilemap.GetTile(clickedTilePosition) == highlightTile)
            {
                // Clicked on a highlighted tile
            }
            else
            {
                // Clicked outside a highlighted tile or outside the sprite
                Debug.Log("Clicked outside the sprite or on non-highlighted tiles.");
                ClearHighlightedTiles();
                if (buttonDisplayed)
                {
                    moveButton.gameObject.SetActive(false);
                    buttonDisplayed = false;
                }
            }
        }
    }

    private void PositionButtonNearSprite()
    {
        // Calculate position to the right of the sprite
        Vector3 spritePosition = transform.position;
        Vector3 buttonPosition = spritePosition + new Vector3(1, 0, 0);
        moveButton.transform.position = Camera.main.WorldToScreenPoint(buttonPosition);
    }

    private void OnMoveButtonClick()
    {
        Debug.Log("Move button clicked!");
        HighlightMovementTiles();
        moveButton.gameObject.SetActive(false);
        buttonDisplayed = false;
    }

    private void UpdateButtonCollider()
    {
        // Adjust the button collider to match the button's visual scale
        RectTransform rectTransform = moveButton.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            BoxCollider2D collider = moveButton.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = rectTransform.rect.size * buttonScale;  // Scale collider size
            }
        }
    }

    private void HighlightMovementTiles()
    {
        // Get the character's position in the Tilemap
        Vector3Int basePosition = movementTilemap.WorldToCell(characterTransform.position);

        // Highlight tiles within a 2-grid radius, excluding the character's current tile
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                Vector3Int tilePosition = new Vector3Int(basePosition.x + x, basePosition.y + y, basePosition.z);
                int distance = Mathf.Abs(x) + Mathf.Abs(y);
                if (distance > 0 && distance <= 2 && !movementTilemap.HasTile(tilePosition))
                {
                    movementTilemap.SetTile(tilePosition, highlightTile);
                }
            }
        }
    }

    private void ClearHighlightedTiles()
    {
        // Clear all highlighted tiles around the character
        Vector3Int basePosition = movementTilemap.WorldToCell(characterTransform.position);
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
