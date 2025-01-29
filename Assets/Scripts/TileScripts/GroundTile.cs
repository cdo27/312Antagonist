using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : BaseTile
{
    public Sprite deployableTile;
    public Sprite groundTile;
    public Sprite highlightedGroundTile; //sprite when highlighted
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        tileType = TileType.Ground;
        isWalkable = true;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the main tile sprite
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
        spriteRenderer.sprite = groundTile; //restore
        isHighlighted = false;
    }
    
}
