using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button moveButton; // The button to show/hide
    public SpriteHandler spriteHandler; // Reference to the SpriteHandler for tile operations
    public float buttonScale = 0.5f; // Scale factor for the button
    private bool buttonDisplayed = false; // State of the button display
    private bool tilesHighlighted = false; // State of the highlighted tiles

    public TurnManager turnManager; // To track if player can move and button show up

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
        // Only interactions during the player's turn
        if (turnManager != null && !turnManager.isPlayerTurn)
        {
            // Clear button and highlights if it's not the player's turn
            if (buttonDisplayed)
            {
                moveButton.gameObject.SetActive(false);
                buttonDisplayed = false;
            }
            if (tilesHighlighted)
            {
                spriteHandler.ClearHighlightedTiles();
                tilesHighlighted = false;
            }
            return; // Exit if it's not the player's turn
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedTilePosition = spriteHandler.movementTilemap.WorldToCell(mouseWorldPos);

            // Check if clicked on highlighted tile
            if (spriteHandler.movementTilemap.HasTile(clickedTilePosition) && spriteHandler.movementTilemap.GetTile(clickedTilePosition) == spriteHandler.highlightTile)
            {
                //Debug.Log("Clicked on a highlighted tile.");
                spriteHandler.MoveCharacter(clickedTilePosition);
                tilesHighlighted = false; // Reset highlighted state after movement
                moveButton.gameObject.SetActive(false);
                buttonDisplayed = false;

            }
            else if (!tilesHighlighted)
            {
                Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

                // Check if the clicked object is the character
                if (hitCollider != null && hitCollider.transform == spriteHandler.characterTransform)
                {
                    //Debug.Log("Character clicked!");
                    if (!buttonDisplayed)
                    {
                        DisplayButton();
                    }
                }
            }
            else
            {
                //Debug.Log("Clicked outside the highlighted tiles.");
                spriteHandler.ClearHighlightedTiles();
                tilesHighlighted = false;

                if (buttonDisplayed)
                {
                    moveButton.gameObject.SetActive(false);
                    buttonDisplayed = false;
                }
            }
        }
    }

    private void DisplayButton()
    {
        //Debug.Log("Displaying move button.");
        Vector3 characterPosition = spriteHandler.characterTransform.position;
        Vector3 buttonPositionOffset = new Vector3(0, 0.35f, 0); // Position the button above the character
        Vector3 buttonPosition = Camera.main.WorldToScreenPoint(characterPosition + buttonPositionOffset);

        moveButton.transform.position = buttonPosition;
        moveButton.gameObject.SetActive(true);
        buttonDisplayed = true;
    }

    private void OnMoveButtonClick()
    {
        //Debug.Log("Move button clicked!");

        // Highlight movement tiles and hide the button
        if (!tilesHighlighted)
        {
            spriteHandler.HighlightMovementTiles();
            tilesHighlighted = true;
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

