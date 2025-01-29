using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : BaseTile
{
    public bool isRevealed = false;
    public bool hasTrap = true;
    public Sprite revealedTrap;
    public Sprite hiddenTrap;
    public Sprite highlightedTrapTile; //sprite when highlighted
    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        tileType = TileType.Trap;
        isWalkable = true; 
        isDeployable = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isRevealed) spriteRenderer.sprite = revealedTrap;
        if (!isRevealed) spriteRenderer.sprite = hiddenTrap;
        
    }

    public void RevealTrap()
    {
        isRevealed = true;
        spriteRenderer.sprite = revealedTrap;
    }

    public override void Highlight()
    {
        spriteRenderer.sprite = highlightedTrapTile; //chaneg to hightlight sprite
        isHighlighted = true;
    }


    public override void Unhighlight()
    {
        if(isRevealed) spriteRenderer.sprite = revealedTrap; //restore
        if(!isRevealed) spriteRenderer.sprite = hiddenTrap; //restore
        isHighlighted = false;
    }

    public override void HighlightAbilityTile()
    {
        spriteRenderer.sprite = highlightedTrapTile;
        isAbilityTile = true;
    }

    public override void UnhighlightAbilityTile()
    {
        if(isRevealed) spriteRenderer.sprite = revealedTrap;
        if(!isRevealed) spriteRenderer.sprite = hiddenTrap;
        isAbilityTile = false;
    }
}
