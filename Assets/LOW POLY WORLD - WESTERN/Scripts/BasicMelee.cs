using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMelee : Unit
{
    void Start()
    {
        Init();
       _target = SearchForTarget();
        MoveToTarget();
    }

    void Update()
    {
        if (_target == null)
        {
            return;
        }
        VisionCast();
        if (_targetsCanAttack == null)
        {
            MoveToTarget();
            return;
        }

        Attack(_targetsCanAttack);

    }
}
