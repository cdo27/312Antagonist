using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoutAnt : PlayerAnt
{

    // Start is called before the first frame update
    void Start()
    {
        hpCount = 2;
        maxHP = 2;
        antType = PlayerAnt.AntType.Scout;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}