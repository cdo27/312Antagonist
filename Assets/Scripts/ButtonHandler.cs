using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button moveButton; // The button to show/hide
    public SpriteHandler spriteHandler; // Reference to the SpriteHandler for tile operations
    public float buttonScale = 0.5f; // Scale factor for the button
    private bool buttonDisplayed = false; // State of the button display
    private bool tilesHighlighted = false; // State of the highlighted tiles

    void Start()
    {
        if (moveButton != null)
        {
            moveButton.gameObject.SetActive(false);
            moveButton.transform.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
            UpdateButtonCollider();
            moveButton.onClick.AddListener(OnMoveButtonClick);
        }
    }

    void Update()
    {
        // Check if the mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

            // Check if the clicked object is the character
            if (hitCollider != null && hitCollider.transform == spriteHandler.characterTransform)
            {
                Debug.Log("Character clicked!");
                if (!buttonDisplayed && !tilesHighlighted)
                {
                    DisplayButton();
                }
            }
            else
            {
                // If clicked outside the character or highlighted tiles
                Vector3Int clickedTilePosition = spriteHandler.movementTilemap.WorldToCell(mouseWorldPos);

                if (!spriteHandler.movementTilemap.HasTile(clickedTilePosition) || spriteHandler.movementTilemap.GetTile(clickedTilePosition) != spriteHandler.highlightTile)
                {
                    Debug.Log("Clicked outside the highlighted tiles.");
                    spriteHandler.ClearHighlightedTiles();
                    tilesHighlighted = false;

                    // Hide the button if it is displayed
                    if (buttonDisplayed)
                    {
                        moveButton.gameObject.SetActive(false);
                        buttonDisplayed = false;
                    }
                }
            }
        }
    }

    private void DisplayButton()
    {
        Debug.Log("Displaying move button.");
        Vector3 characterPosition = spriteHandler.characterTransform.position;
        Vector3 buttonPositionOffset = new Vector3(0, 0.35f, 0); // Position the button above the character
        Vector3 buttonPosition = Camera.main.WorldToScreenPoint(characterPosition + buttonPositionOffset);

        moveButton.transform.position = buttonPosition;
        moveButton.gameObject.SetActive(true);
        buttonDisplayed = true;
    }

    private void OnMoveButtonClick()
    {
        Debug.Log("Move button clicked!");

        // Highlight movement tiles and hide the button
        if (!tilesHighlighted)
        {
            spriteHandler.HighlightMovementTiles();
            tilesHighlighted = true;
        }
        else
        {
            spriteHandler.ClearHighlightedTiles();
            tilesHighlighted = false;
        }

        moveButton.gameObject.SetActive(false);
        buttonDisplayed = false;
    }

    private void UpdateButtonCollider()
    {
        RectTransform rectTransform = moveButton.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            BoxCollider2D collider = moveButton.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = rectTransform.rect.size * buttonScale;
            }
        }
    }
}
