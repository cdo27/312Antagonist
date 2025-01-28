using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnt : MonoBehaviour
{
    public enum AntType { Scout, Builder, Warrior }
    public Vector2Int gridPosition;
    public int hpCount;
    public int moveRadius;

    public bool hasMoved;
    public bool isMoving; //for movement sprite
    public bool usedAbility;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        hasMoved = false;
        usedAbility = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseDown()
    {
        Debug.Log("Player Ant was clicked!");
    }
}
