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
    public bool isMoving; // for movement sprite
    public bool usedAbility;
    public bool isDead;
    public bool isHighlighted;

    private SpriteRenderer spriteRenderer;

    // Health bar UI sprites
    public Image healthBarImage;
    public Sprite fullHealthBar;
    public Sprite oneHealthBar;
    public Sprite zeroHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hasMoved = false;
        usedAbility = false;
        isDead = false;
        maxHP = 2;
        hpCount = 2;

        UpdateSprite(); // Update  health bar
        UpdateHealthBarPosition();
    }

    void Update()
    {
        if (hpCount == 0) 
        {
            isDead = true; // If HP is zero, mark as dead
        }

        UpdateHealthBarPosition();
    }

    // Get current position
    Vector2Int getPos()
    {
        return gridPosition;
    }

    // Reset ant status
    public void resetAnt()
    {
        hasMoved = false;
        usedAbility = false;
    }

    public void loseHP()
    {
        if (hpCount > 0)
        {
            hpCount -= 1;
            UpdateSprite(); 
        }
    }

    public void gainHP()
    {
        if (hpCount < maxHP)
        {
            hpCount += 1;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        // Update health bar sprite based on HP
        if (healthBarImage != null)
        {
            if (hpCount == maxHP)
            {
                healthBarImage.sprite = fullHealthBar;
            }
            else if (hpCount == 1)
            {
                healthBarImage.sprite = oneHealthBar;
            }
            else if (hpCount == 0)
            {
                healthBarImage.sprite = zeroHealthBar;
            }
        }
    }

    void UpdateHealthBarPosition()
    {
        //positon hp bar above ant
    }
}
