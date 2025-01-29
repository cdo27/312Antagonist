using UnityEngine;

public class BaseTile : MonoBehaviour
{
    public enum TileType { Ground, Water, Trap, Deploy }
    public TileType tileType;
    public bool isWalkable;
    public bool isDeployable;
    public bool isHighlighted;

    public int xIndex;
    public int yIndex;

    void Start()
    {


    }

    public virtual void Highlight()
    {

    }

    public virtual void Unhighlight()
    {

    }

}
