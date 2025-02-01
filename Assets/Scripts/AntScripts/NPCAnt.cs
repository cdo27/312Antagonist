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

}
