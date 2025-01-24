using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    public Button moveButton; // The button to show/hide
    public float buttonScale = 0.5f; // Scale factor for the button
    private bool isSpriteClicked = false; // Flag to check if sprite is clicked
    private bool isButtonVisible = false; // Additional flag to manage button visibility

    void Start()
    {
        // Initially hide the move button
        if (moveButton != null)
        {
            moveButton.gameObject.SetActive(false);
            // Scale the button
            moveButton.transform.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
            // Add click listener to the button
            moveButton.onClick.AddListener(OnMoveButtonClick);
        }
    }

    void OnMouseDown()
    {
        // Log and handle the sprite click
        Debug.Log("Sprite clicked!");
        isSpriteClicked = true;

        // Toggle button visibility based on current state
        if (moveButton != null)
        {
            if (!isButtonVisible)
            {
                PositionButtonNearSprite();
                moveButton.gameObject.SetActive(true);
                isButtonVisible = true;
            }
            else
            {
                moveButton.gameObject.SetActive(false);
                isButtonVisible = false;
            }
        }
    }

    void Update()
    {
        // Check for mouse click events to reset the sprite click flag
        if (Input.GetMouseButtonDown(0) && !isSpriteClicked)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

            // Hide button if clicked outside the sprite
            if (hitCollider == null || hitCollider.gameObject != gameObject)
            {
                Debug.Log("Clicked outside the sprite.");
                if (moveButton != null && isButtonVisible)
                {
                    moveButton.gameObject.SetActive(false);
                    isButtonVisible = false;
                }
            }
        }

        // Reset the sprite click flag at the end of the frame
        if (!Input.GetMouseButtonDown(0))
        {
            isSpriteClicked = false;
        }
    }

    private void PositionButtonNearSprite()
    {
        // Calculate the position
        Vector3 spritePosition = transform.position;
        Vector3 buttonPosition = spritePosition + new Vector3(1, 0, 0); // Adjust X offset as needed

        // Convert world position to screen position for UI element
        Vector3 screenPos = Camera.main.WorldToScreenPoint(buttonPosition);
        moveButton.transform.position = screenPos;
    }

    private void OnMoveButtonClick()
    {
        // Hide the button when clicked
        Debug.Log("Move button clicked!");
        moveButton.gameObject.SetActive(false);
        isButtonVisible = false;
    }
}
