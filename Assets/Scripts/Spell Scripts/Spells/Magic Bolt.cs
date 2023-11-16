using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : Spell
{
    public override void AddModifier(Modifier mod)
    {
        base.AddModifier(mod);

        throw new System.NotImplementedException();
    }

    public override IEnumerator CastSpell()
    {
        throw new System.NotImplementedException();
    }

    //protected override void FireSpellEffect(SpellEffect effect, float amount)
    //{
    //    throw new System.NotImplementedException();
    //}
}
