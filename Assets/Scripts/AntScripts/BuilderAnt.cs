using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderAnt : PlayerAnt
{
    // Start is called before the first frame update
    void Start()
    {
        hpCount = 2;
        maxHP = 2;
        antType = PlayerAnt.AntType.Builder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
