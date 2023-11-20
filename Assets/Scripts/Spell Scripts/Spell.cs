using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

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

    [Header("Meta Stats")]
    [SerializeField] protected float multiCastDelay;

    [Header("Mods")]
    [SerializeField] protected List<Modifier> modifiers = new List<Modifier>();
    public List<Modifier> Modifiers { get { return modifiers; } }

    [Header("Other settings")]
    [SerializeField] protected string spellName;
    [SerializeField] protected Sprite spellIcon;
    public string SpellName { get { return spellName; } }
    public Sprite SpellIcon { get { return spellIcon; } }

    [SerializeField] protected SpellEffect spellEffect;
    [SerializeField] protected int poolSize = 6;
    [SerializeField] protected Transform poolParent;
    [Space()]
    [SerializeField] protected UnityEvent onSpellCast;
    [SerializeField] protected UnityEvent onSpellCancelled;
    [SerializeField] protected AudioClip spellCastSound;

    protected IObjectPool<SpellEffect> spellEffectPool;

    protected Dictionary<Stats, Ref<float>> statValuePairs = new Dictionary<Stats, Ref<float>>();
    protected float startingCooldown = 0;
    private int _appliedMods = 0;

    private void Awake()
    {
        ResetStats();

        if (poolSize > 1)
        {
            spellEffectPool = new ObjectPool<SpellEffect>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, poolSize);
        }
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

    public virtual void ClearModifiers()
    {
        modifiers.Clear();
        ResetStats();
    }

    public virtual void ResetStats()
    {
        damage = baseDamage;
        manaCost = baseManaCost;
        cooldown = baseCoolDown;
        effectScale = baseEffectScale;
        effectAmount = baseEffectAmount;
        castAmount = baseCastAmount;
    }

    public Ref<float> GetStatRefByEnum(Stats stats)
    {
        if (statValuePairs.Keys.Count == 0)
        {
            statValuePairs.Add(Stats.damage, new Ref<float>(() => damage, value => damage = value));
            statValuePairs.Add(Stats.manaCost, new Ref<float>(() => manaCost, value => manaCost = value));
            statValuePairs.Add(Stats.cooldown, new Ref<float>(() => cooldown, value => cooldown = value));
            statValuePairs.Add(Stats.effectScale, new Ref<float>(() => effectScale, value => effectScale = value));
            statValuePairs.Add(Stats.effectAmount, new Ref<float>(() => effectAmount, value => effectAmount = Mathf.RoundToInt(value)));
            statValuePairs.Add(Stats.castAmount, new Ref<float>(() => castAmount, value => castAmount = Mathf.RoundToInt(value)));
        }

        return statValuePairs[stats];
    }

    public List<Elements> GetElementsFromMods()
    {
        List<Elements> elements = new List<Elements>();

        foreach (var mod in modifiers)
        {
            if (mod.Element != Elements.Unaspected)
            {
                elements.Add(mod.Element);
            }
        }

        return elements;
    }

    public void CountAppliedModifiers()
    {
        _appliedMods++;

        if (_appliedMods >= Modifiers.Count)
        {
            OnApplyModifiers?.Invoke();

            _appliedMods = 0;
        }
    }

    public bool HasModifierSlot { get { return (modifiers.Count < 3); } }
    public SpellEffect SpellEffect { get { return spellEffect; } }
    public UnityEvent OnApplyModifiers;

    private SpellEffect CreatePooledItem()
    {
        GameObject effectObject = Instantiate(spellEffect.gameObject, poolParent, false);

        return effectObject.GetComponent<SpellEffect>();
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(SpellEffect effect)
    {
        effect.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(SpellEffect effect)
    {
        effect.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(SpellEffect effect)
    {
        Destroy(effect.gameObject);
    }
}