using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCaster : MonoBehaviour
{
    public Spell spell;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spell.CastSpell();
        }
    }
}
