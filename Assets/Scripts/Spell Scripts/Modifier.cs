using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Modifier", menuName = "SpellCastingComponents/Modifier", order = 1)]
public class Modifier : ScriptableObject
{
    [SerializeField] private string keyword;
    public string Keyword { get { return keyword; } }
    [SerializeField] private Image icon;
    [SerializeField] private Elements element;
    public Elements Element { get { return element; } }

    [SerializeField] private Mod[] mods;
    [SerializeField] private OnHitEffect[] onHitEffects;


    private Spell _spell;

    Dictionary<Operators, Func<float, float, float>> operatorDictionary = new Dictionary<Operators, Func<float, float, float>>
    {
            { Operators.Add, (a, b) => a + b },
            { Operators.Subtract, (a, b) => a - b },
            { Operators.Multiply, (a, b) => a * b },
            { Operators.Divide, (a, b) => a / b }
    };

    public void SetSpell(Spell targetSpell)
    {
        //if (!targetSpell.HasModifierSlot) return;

        _spell = targetSpell;

        ApplyMod();
    }

    public void ApplyMod()
    {
        foreach (var mod in mods)
        {
            Ref<float> statRef = _spell.GetStatRefByEnum(mod.targetStat);
            statRef.Value = operatorDictionary[mod.operation](statRef.Value, mod.value);
        }

        var targetEffect = _spell.SpellEffect;
        foreach (var effect in onHitEffects)
        {
            //var component = targetEffect.gameObject.AddComponent(effect.GetType());
            //targetEffect.OnHitTrigger += component.OnHit; 
        }

        _spell.OnApplyModifiers.Invoke();
    }

    // TO DO: Remove Modifier

}

public class Ref<T>
{
    private Func<T> getter;
    private Action<T> setter;
    public Ref(Func<T> getter, Action<T> setter)
    {
        this.getter = getter;
        this.setter = setter;
    }
    public T Value
    {
        get { return getter(); }
        set { setter(value); }
    }
}
