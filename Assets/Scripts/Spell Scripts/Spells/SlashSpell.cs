using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSpell : Spell
{
    [SerializeField] private float _slashTime = 0.5f;
    [SerializeField] private Vector3 _extraOffset;
    [SerializeField] private float _miniDelayBetweenSlashes = 0.1f;
    private bool _modifyParticles = false;

    public override IEnumerator CastSpell()
    {
        startingCooldown = cooldown;
        cooldown = Mathf.Infinity;
        float scaleX = 1;

        for (int i = 0; i < castAmount; i++)
        {
            Vector3 offset = Vector3.zero;
            List<SpellEffect> usedSpellEffects = new List<SpellEffect>();

            for (int j = 0; j < effectAmount; j++)
            {
                SpellEffect spellEffect = spellEffectPool.Get();
                spellEffect.damage = damage;
                spellEffect.transform.localPosition = offset;
                spellEffect.transform.localScale = new Vector3(scaleX, 1, 1);

                usedSpellEffects.Add(spellEffect);

                offset += _extraOffset;

                SpellVfxManager spellVfx = spellEffect.GetComponent<SpellVfxManager>();

                if (_modifyParticles)
                {
                    spellVfx.ModifyParticleSystems(effectAmount, GetElementsFromMods().ToArray());
                }
                spellVfx.PlayEffect(true);

                if (j + 1 < effectAmount)
                {
                    yield return new WaitForSeconds(_miniDelayBetweenSlashes);
                }
            }

            _modifyParticles = false;

            yield return new WaitForSeconds(_slashTime);

            foreach (var effect in usedSpellEffects)
            {
                spellEffectPool.Release(effect);
            }

            scaleX *= -1;
        }

        cooldown = startingCooldown;
    }

    protected override void FireSpellEffect(SpellEffect effect, float amount)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        OnApplyModifiers.AddListener(() => { _modifyParticles = true; });
    }
}
