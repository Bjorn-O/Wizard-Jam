using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Modifier", menuName = "SpellCastingComponents/Modifier", order = 1)]
public class Modifier : ScriptableObject
{
    [SerializeField] private string keyword;
    [SerializeField] private Image icon;
    [SerializeField] private Elements element;

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

    Dictionary<Stats, Ref<float>> statValuePairs = new Dictionary<Stats, Ref<float>>();

    public void SetSpell(Spell targetSpell)
    {
        _spell = targetSpell;
        if (statValuePairs.Keys.Count > 0)
        {
            ApplyMod();
            return;
        }

        statValuePairs.Add(Stats.damage, new Ref<float>(() => _spell.damage, value => _spell.damage = value));
        statValuePairs.Add(Stats.manaCost, new Ref<float>(() => _spell.manaCost, value => _spell.manaCost = value));
        statValuePairs.Add(Stats.cooldown, new Ref<float>(() => _spell.cooldown, value => _spell.cooldown = value));
        statValuePairs.Add(Stats.effectScale, new Ref<float>(() => _spell.effectScale, value => _spell.effectScale = value));
        statValuePairs.Add(Stats.effectAmount, new Ref<float>(() => _spell.effectAmount, value => _spell.effectAmount = Mathf.RoundToInt(value)));
        statValuePairs.Add(Stats.castAmount, new Ref<float>(() => _spell.castAmount, value => _spell.castAmount = Mathf.RoundToInt(value)));

        ApplyMod();
    }

    public void ApplyMod()
    {
        foreach (var mod in mods)
        {
            statValuePairs[mod.targetStat].Value = operatorDictionary[mod.operation](statValuePairs[mod.targetStat].Value, mod.value);
        }
        var targetEffect = _spell.SpellEffect;
        foreach (var effect in onHitEffects)
        {
            var component = targetEffect.gameObject.AddComponent(effect.GetType());
            //targetEffect.OnHitTrigger += component.OnHit; 
        }
    }

    // TO DO: Remove Modifier

}



sealed class Ref<T>
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
