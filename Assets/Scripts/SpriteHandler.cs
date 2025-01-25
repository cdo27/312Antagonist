using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    public Button moveButton; // The button to show/hide
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
            // Optionally adjust the button's collider here if necessary
            UpdateButtonCollider();
            // Add click listener to the button
            moveButton.onClick.AddListener(OnMoveButtonClick);
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
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

            if (hitCollider == null || hitCollider.gameObject != this.gameObject)
            {
                Debug.Log("Clicked outside the sprite.");
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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(buttonPosition);
        moveButton.transform.position = screenPos;
    }

    private void OnMoveButtonClick()
    {
        // Hide the button when clicked
        Debug.Log("Move button clicked!");
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
}
