using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : SpellEffect
{
    public override void OnTriggerEnter(Collider other)
    {
        print("dingus");
    }
}
