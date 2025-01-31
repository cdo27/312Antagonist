using UnityEngine;

public class BaseTile : MonoBehaviour
{
    public enum TileType { Ground, Water, Trap, Deploy }
    public TileType tileType;
    public bool isWalkable;
    public bool isDeployable;
    public bool isHighlighted;
    public bool isAbilityTile;

    public bool hasCharacter;

    public int xIndex;
    public int yIndex;

    public TileInformationUI tileInfoUI;

    void Start()
    {
       
    }

    void OnMouseEnter()
    {
      
    }

    void OnMouseExit()
    {
       
    }

    public virtual void Highlight()
    {

    }

    public virtual void Unhighlight()
    {

    }

    public virtual void HighlightAbilityTile()
    {

    }

    public virtual void UnhighlightAbilityTile()
    {

    }

}
