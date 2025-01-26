using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button moveButton; // The button to show/hide
    public SpriteHandler spriteHandler; // Reference to the SpriteHandler for tile operations
    public float buttonScale = 0.5f; // Scale factor for the button
    private bool buttonDisplayed = false; // State of the button display

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
        if (Input.GetMouseButtonDown(0) && !buttonDisplayed)
        {
            DisplayButton();
        }
    }

    private void DisplayButton()
    {
        Debug.Log("Sprite clicked!");

        // Adjust button position to be in the middle top of the character's tile
        Vector3 characterPosition = spriteHandler.characterTransform.position;
        Vector3 buttonPositionOffset = new Vector3(0, 0.35f, 0); // Adjust Y offset according to the sprite's size and desired position
        Vector3 buttonPosition = Camera.main.WorldToScreenPoint(characterPosition + buttonPositionOffset);

        moveButton.transform.position = buttonPosition;
        moveButton.gameObject.SetActive(true);
        buttonDisplayed = true;
        spriteHandler.HighlightMovementTiles();
    }

    private void OnMoveButtonClick()
    {
        Debug.Log("Move button clicked!");
        spriteHandler.ClearHighlightedTiles();
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


