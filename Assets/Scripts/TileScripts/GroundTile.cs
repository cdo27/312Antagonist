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
        tileInfoUI = FindObjectOfType<TileInformationUI>();
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

    //shows tile information in the bottom right box.
    void OnMouseEnter()
    {
        Debug.Log(tileType);
        if (tileInfoUI != null)
        {
            tileInfoUI.ShowTileInformation(tileType.ToString()); 
        }
    }
    //and hides it again when out
    void OnMouseExit()
    {
        if (tileInfoUI != null)
        {
            tileInfoUI.HideTileInformation();
        }
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

    public override void HighlightAbilityTile()
    {
        spriteRenderer.sprite = highlightedGroundTile;
        isAbilityTile = true;
    }

    public override void UnhighlightAbilityTile()
    {
        spriteRenderer.sprite = groundTile;
        isAbilityTile = false;
    }
    
}
