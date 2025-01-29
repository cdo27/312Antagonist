using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : BaseTile
{
    public bool hasBridge = false;
    public Sprite withoutBridge; // Sprite without the bridge
    public Sprite withBridge;    // Sprite with the bridge
    public Sprite highlightedWaterTile; //sprite when highlighted

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        tileType = TileType.Water;
        isWalkable = false; // Not walkable until a bridge is built
        isDeployable = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = withoutBridge; 
    }

    void Update(){
        //if(hasBridge)spriteRenderer.sprite = withBridge;
    }

    public void BuildBridge()
    {
        hasBridge = true;
        isWalkable = true;
        Debug.Log("A bridge has been built. Tile is now walkable.");
        spriteRenderer.sprite = withBridge;
    }

        public override void Highlight()
    {
        spriteRenderer.sprite = highlightedWaterTile; //chaneg to hightlight sprite
        isHighlighted = true;
    }


    public override void Unhighlight()
    {
        if(!hasBridge) spriteRenderer.sprite = withoutBridge; //restore
        if(hasBridge)spriteRenderer.sprite = withBridge; //restore
        isHighlighted = false;
    }

}
