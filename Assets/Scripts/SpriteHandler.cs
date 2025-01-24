using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    public Button moveButton; 
    private bool isSpriteClicked = false; 
    void Start()
    {
        //button is hidden at start
        if (moveButton != null)
        {
            moveButton.gameObject.SetActive(false);
        }
    }

    //when box collider clicked
    void OnMouseDown()
    {
        Debug.Log("Sprite clicked!"); //when sprite is clicked

        if (moveButton != null)
        {
            //show button
            moveButton.gameObject.SetActive(true);
            isSpriteClicked = true;
        }
    }

    void Update()
    {
        //hide button when clicked off
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            //only if sprite isnt clicked 
            if (!isSpriteClicked)
            {
                Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
                if (hitCollider == null || hitCollider.gameObject != gameObject)
                {
                    Debug.Log("Clicked outside the sprite."); //when clicked off

                    if (moveButton != null)
                    {
                        //hide button
                        moveButton.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                //reset flag
                isSpriteClicked = false;
            }
        }
    }
}
