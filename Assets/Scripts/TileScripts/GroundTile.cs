using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : BaseTile
{
    public Sprite deployableTile;
    public Sprite groundTile;
    public Sprite groundObstacleTile; //sprite with obstacle
    public Sprite highlightedGroundTile; //sprite when highlighted
    public SpriteRenderer spriteRenderer;

    public bool hasObstacle;

    void Start()
    {
        tileType = TileType.Ground;
        isWalkable = true;
        hasObstacle = false;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the main tile sprite
    }

    public void buildObstacle(){
        isWalkable = false;
        hasObstacle = true;
        spriteRenderer.sprite = groundObstacleTile;
        Debug.Log("Obstacle sprite changed");
        // change sprite to ground tile with obstacle
    }

    public void ShowDeployableTiles()
    {
        if (isDeployable)
        {
            spriteRenderer.sprite = deployableTile; 
        }
    }

    public void HideDeployableTiles()
    {
        spriteRenderer.sprite = groundTile; 
    }


    public override void Highlight()
    {
        spriteRenderer.sprite = highlightedGroundTile; //chaneg to hightlight sprite
        isHighlighted = true;
    }


    public override void Unhighlight()
    {
        if (hasObstacle) spriteRenderer.sprite = groundObstacleTile;
        else spriteRenderer.sprite = groundTile; 
        isHighlighted = false;
    }

    public override void HighlightAbilityTile()
    {
        spriteRenderer.sprite = highlightedGroundTile;
        isAbilityTile = true;
    }

    public override void UnhighlightAbilityTile()
    {
        if (hasObstacle) spriteRenderer.sprite = groundObstacleTile;
        else spriteRenderer.sprite = groundTile; 
        isAbilityTile = false;
    }
    
}
