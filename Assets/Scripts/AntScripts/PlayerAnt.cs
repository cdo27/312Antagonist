using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnt : MonoBehaviour
{
    public enum AntType { Scout, Builder, Soldier }
    public AntType antType;
    public Vector2Int gridPosition;
    public int hpCount;
    public int maxHP;
    
    public bool hasMoved;
    public bool isMoving; //for movement sprite
    public bool usedAbility;
    public bool isDead;
    public bool isHighlighted;

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

    Vector2Int getPos(){
        return gridPosition;
    }

    public void resetAnt(){
        hasMoved = false;
        usedAbility = false;
    }

    public void loseHP(){
        if(hpCount>0) hpCount -=1;
    }

    public void gainHP(){
        if(hpCount<2) hpCount +=1;
    }

}
