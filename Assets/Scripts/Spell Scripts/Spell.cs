using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Spell : MonoBehaviour
{
    protected float baseDamage;
    protected float baseManaCost;
    protected float baseCoolDown;
    protected float baseEffectScale;
    protected int baseEffectAmount;
    protected int baseCastAmount;

    public float damage;
    public float manaCost;
    public float cooldown;
    public float effectScale;
    public int effectAmount;
    public int castAmount;


    protected Modifier[] modifiers = new Modifier[3];

    [SerializeField] protected SpellEffect spellEffect;
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

    public abstract void FireSpellEffect(SpellEffect effect, float amount);

    public abstract void AddModifier(Modifier mod);

    public SpellEffect SpellEffect { get { return spellEffect; } }
}