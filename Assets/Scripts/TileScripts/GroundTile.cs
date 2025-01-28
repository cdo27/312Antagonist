using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : BaseTile
{
    // Start is called before the first frame update
    void Start()
    {
        tileType = TileType.Ground;
        isWalkable = true;
        //isDeployable = false;
        
    }

}
