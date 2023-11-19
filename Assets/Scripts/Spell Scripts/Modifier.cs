using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "SpellCastingComponents/Modifier", order = 1)]
public class Modifier : ScriptableObject
{
    [SerializeField] private string keyword;
    public string Keyword { get { return keyword; } }
    [SerializeField] private Sprite icon;
    public Sprite Icon { get { return icon; } }
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

    Dictionary<Operators, Func<float, float, float>> reverseOperatorDictionary = new Dictionary<Operators, Func<float, float, float>>
    {
            { Operators.Add, (a, b) => a - b },
            { Operators.Subtract, (a, b) => a + b },
            { Operators.Multiply, (a, b) => a / b },
            { Operators.Divide, (a, b) => a * b }
    };

    public void SetSpell(Spell targetSpell)
    {
        //if (!targetSpell.HasModifierSlot) return;

        SetSpell(targetSpell, false);
    }

    public void SetSpell(Spell targetSpell, bool onApplyImmediately)
    {
        _spell = targetSpell;
        ApplyMod(onApplyImmediately);
    }

    public void ApplyMod(bool onApplyImmediately)
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

        if (onApplyImmediately)
        {
            _spell.OnApplyModifiers?.Invoke();
            return;
        }
        _spell.CountAppliedModifiers();
    }
    
    public void RemoveSpell()
    {
        for (int i = mods.Length - 1; i >= 0; i--)
        {
            Mod mod = mods[i];
            Ref<float> statRef = _spell.GetStatRefByEnum(mod.targetStat);
            statRef.Value = reverseOperatorDictionary[mod.operation](statRef.Value, mod.value);
        }

        _spell.OnApplyModifiers?.Invoke();
    }
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
