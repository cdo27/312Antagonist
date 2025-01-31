using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierAnt : PlayerAnt
{
    //Soldier Ant Throw Action variables
    public int soldierThrowPhase; //not throwing = -1, selecting target = 0, selecting tile to throw to = 1
    public int soldierThrowTarget; //no target = -1, scout = 0, builder = 1, antlion(1) = 2, antlion(2) = 3

    // Start is called before the first frame update
    void Start()
    {
        soldierThrowPhase = -1;
        soldierThrowTarget = -1;
        hpCount = 2;
        maxHP = 2;
        antType = PlayerAnt.AntType.Soldier;

    }

    // Update is called once per frame
    void Update()
    {

    }
}