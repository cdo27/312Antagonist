using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : BaseTile
{
    public Sprite deployableTile;
    public Sprite groundTile;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        tileType = TileType.Ground;
        isWalkable = true;
    }

    void showDeployableTiles(){
        if(isDeployable){
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = deployableTile; 
        }
    }

    void hideDeployableTiles(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = groundTile; 
    }
}
