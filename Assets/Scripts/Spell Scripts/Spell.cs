using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Spell : MonoBehaviour
{
    [Header("Base stats")]
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float baseManaCost;
    [SerializeField] protected float baseCoolDown;
    [SerializeField] protected float baseEffectScale = 1;
    [SerializeField] protected int baseEffectAmount = 1;
    [SerializeField] protected int baseCastAmount = 1;

    [Header("Stats")]
    public float damage;
    public float manaCost;
    public float cooldown;
    public float effectScale;
    public int effectAmount;
    public int castAmount;

    [Header("Mods")]
    [SerializeField] protected List<Modifier> modifiers = new List<Modifier>();
    public List<Modifier> Modifiers { get { return modifiers; } }

    [Header("Other settings")]
    [SerializeField] protected string spellName;
    [SerializeField] protected Sprite spellIcon;
    public Sprite SpellIcon { get { return spellIcon; } }

    [SerializeField] protected SpellEffect spellEffect;
    [Space()]
    [SerializeField] protected UnityEvent onSpellCast;
    [SerializeField] protected UnityEvent onSpellCancelled;


    private void Awake()
    {
        damage = baseDamage;
        manaCost = baseManaCost;
        cooldown = baseCoolDown;
        effectScale = baseEffectScale;
        effectAmount = baseCastAmount;
        castAmount = baseCastAmount;
    }

    public abstract IEnumerator CastSpell();

    protected abstract void FireSpellEffect(SpellEffect effect, float amount);

    public virtual void AddModifier(Modifier mod)
    {
        modifiers.Add(mod);
        spellName = mod.Keyword + " " + spellName;
    }

    public virtual void RemoveModifier(Modifier mod)
    {
        modifiers.Remove(mod);
        spellName = spellName.Replace(mod.Keyword + " ", "");
    }

    public bool HasModifierSlot { get { return (modifiers.Count < 3); } }
    public SpellEffect SpellEffect { get { return spellEffect; } }
    public UnityEvent OnApplyModifiers;
}