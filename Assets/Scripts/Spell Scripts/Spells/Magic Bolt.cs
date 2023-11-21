using System.Collections;
using UnityEngine;

public class MagicBolt : Spell
{
    [SerializeField] private Transform castTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator _anim;
    private CharacterStats stats;

    private bool _modifyParticles;

    private void Start()
    {
        stats = GetComponentInParent<CharacterStats>();
        OnApplyModifiers.AddListener(() => { _modifyParticles = true; });
    }

    public override void AddModifier(Modifier mod)
    {
        base.AddModifier(mod);
    }

    public override IEnumerator CastSpell()
    {
        for (int i = 0; i < castAmount; i++)
        {
            if (i > 0)
            {
                if (stats.Mana < manaCost)
                {
                    audioSource.PlayOneShot(cantCastSound);
                    yield break;
                }

                stats.Mana -= manaCost;
            }

            FireSpellEffect(spellEffect, effectAmount);
            onSpellCast?.Invoke();

            yield return new WaitForSeconds(multiCastDelay);
        }
    }

    protected override void FireSpellEffect(SpellEffect effect, float amount)
    {
        if (_anim != null)
            _anim.SetTrigger("Shoot");

        audioSource.PlayOneShot(spellCastSound);

        if (amount == 1)
        {
            SpellEffect bolt = spellEffectPool.Get();
            bolt.transform.position =  castTransform.position;
            bolt.transform.rotation =  camTransform.rotation;
            bolt.transform.localScale = Vector3.one * effectScale;
            bolt.damage = damage;

            if (_modifyParticles)
            {
                SpellVfxManager spellVfx = bolt.GetComponent<SpellVfxManager>();
                spellVfx.ModifyParticleSystems(effectAmount, GetElementsFromMods().ToArray());
            }

            if (!(bolt as Bolt).addedEventListener)
            {
                (bolt as Bolt).OnHitEvent.AddListener(() => { spellEffectPool.Release(bolt); PlayerSpellCast._audioSource.PlayOneShot(bolt.spellEffectSound); });
                (bolt as Bolt).addedEventListener = true;
            }
        } else
        {
            float shotPosition = 0 - Mathf.Floor(amount / 2);
            for (int i = 0; i < amount; i++)
            {
                // Skips the 0 on even number of shots
                if (amount % 2 == 0 && shotPosition == 0) shotPosition++;

                // Sets the facing of each shot so they will spread out according to the Animation Curve
                Vector3 shotDirection = new Vector3(shotPosition / 10, castTransform.transform.localPosition.y, curve.Evaluate(shotPosition / 10));
                float radial = Mathf.Atan2(shotDirection.x, shotDirection.z);
                float degrees = radial * (180 / Mathf.PI);
                
                // Grabs the Effect from the ObjectPool
                SpellEffect bolt = spellEffectPool.Get();


                //Checks if the bolt needs to be changed element. This is stupid and dumb but we hadn't prepared for this in the framework. Too bad Bas.
                if (_modifyParticles)
                {
                    SpellVfxManager spellVfx = bolt.GetComponent<SpellVfxManager>();
                    spellVfx.ModifyParticleSystems(effectAmount, GetElementsFromMods().ToArray());
                }

                //Spawn Spell and set position/rotation
                bolt.transform.position = castTransform.position;
                bolt.transform.rotation = camTransform.rotation;
                bolt.transform.Rotate(0, degrees, 0);

                //Apply Spell stats to object
                bolt.transform.localScale = Vector3.one * effectScale;
                bolt.damage = damage;
                if (!(bolt as Bolt).addedEventListener)
                {
                    (bolt as Bolt).OnHitEvent.AddListener(() => { spellEffectPool.Release(bolt); PlayerSpellCast._audioSource.PlayOneShot(bolt.spellEffectSound); });
                    (bolt as Bolt).addedEventListener = true;
                }

                // Give bolt action to release
                shotPosition++;
            }
        }

        _modifyParticles = false;
    }
}
