using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnt : MonoBehaviour
{
    public enum NPCType { QueenAnt, AntLion }
    public NPCType npcType;
    public Vector2Int gridPosition;
    public int hpCount;

    public bool hasMoved;
    public bool isMoving; //for movement sprite
    public bool isDead;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hasMoved = false;
        isDead = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveToTile(BaseTile targetTile)
    {
        if (isMoving) return;

        StartCoroutine(MoveAnimation(targetTile));
    }

    public IEnumerator MoveAnimation(BaseTile targetTile)
    {
        isMoving = true;
        if (animator != null)
        {
            animator.SetBool("isMoving", true);  // Set movement animation
        }
        else
        {
            Debug.LogWarning("Animator is not assigned to PlayerAnt.");
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetTile.transform.position;
        float moveDuration = 0.5f; // Adjust for speed
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure exact positioning
        gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
        isMoving = false;

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
    }

    //function for getting thrown
    public void GettingThrown(BaseTile targetTile)
    {
        StartCoroutine(MoveToPosition(targetTile));
    }

    private IEnumerator MoveToPosition(BaseTile targetTile)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetTile.transform.position;
        float moveDuration = 0.8f; // Duration for the arc (adjust speed)
        float elapsedTime = 0f;

        // Calculate the arc's peak height (midway point)
        Vector3 peakPosition = new Vector3((startPosition.x + targetPosition.x) / 2, Mathf.Max(startPosition.y, targetPosition.y) + 2f, (startPosition.z + targetPosition.z) / 2);

        // Move the ant in an arc trajectory
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            // Use a quadratic bezier curve to simulate an arc (parabolic path)
            float curveHeight = 4 * (t - t * t); // This makes the movement parabolic

            // Interpolate the positions along the path
            Vector3 currentPosition = Vector3.Lerp(startPosition, peakPosition, t) + Vector3.up * curveHeight;
            currentPosition = Vector3.Lerp(currentPosition, targetPosition, t);

            // Apply the current position to the transform
            transform.position = currentPosition;

            // Rotate the ant around the Z-axis (spin like a clock)
            float rotationAngle = 360 * (elapsedTime / moveDuration); // Spin from 0 to 360 degrees over the duration
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle); // Rotate around Z-axis

            elapsedTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        // Ensure the ant ends up at the exact target position
        transform.position = targetPosition;
        gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);  // Update the grid position

        isMoving = false;  // Stop moving after reaching the target

    }

}
