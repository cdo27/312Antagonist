using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : BaseTile
{
    public bool isRevealed = false;
    public bool hasTrap = true;
    public Sprite revealedTrap;
    public Sprite hiddenTrap;
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
}
