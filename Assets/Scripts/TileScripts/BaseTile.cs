using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour
{
    public enum TileType { Ground, Water, Trap, Deploy }
    public TileType tileType;
    public bool isWalkable;
    public bool isDeployable;

    public int xIndex;
    public int yIndex;

    public virtual void PerformAction()
    {
        Debug.Log($"Tile of type {tileType} was interacted with.");
    }
}
