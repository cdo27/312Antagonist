using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnt : MonoBehaviour
{
    public enum AntType { Scout, Builder, Soldier }
    public AntType antType;
    public Vector2Int gridPosition;
    public int hpCount;
    public int maxHP;
    
    public bool hasMoved;
    public bool isMoving; //for movement sprite
    public bool usedAbility;
    public bool isDead;
    public bool isHighlighted;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hasMoved = false;
        usedAbility = false;
        isDead = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hpCount == 0) isDead = true; //play dead animation
    }

    Vector2Int getPos(){
        return gridPosition;
    }

    public void resetAnt(){
        hasMoved = false;
        usedAbility = false;
    }

    public void loseHP(){
        if(hpCount>0) hpCount -=1;
    }

    public void gainHP(){
        if(hpCount<maxHP) hpCount +=1;
    }

    public void MoveToTile(BaseTile targetTile)
    {

        if (isMoving || hasMoved) return; 

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
        hasMoved = true;
        isMoving = false;

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }

        // Check if the tile is a trap
        if (targetTile is TrapTile) loseHP();
    }

}
