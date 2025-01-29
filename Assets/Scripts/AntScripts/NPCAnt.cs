using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnt : MonoBehaviour
{
    public enum NPCType { QueenAnt, AntLion }
    public NPCType npcType;
    public Vector2Int gridPosition;
    public int hpCount;

    public bool hasMoved;
    public bool isMoving; //for movement sprite
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        hasMoved = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
