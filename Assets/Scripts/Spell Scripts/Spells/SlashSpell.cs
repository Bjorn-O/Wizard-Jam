using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSpell : Spell
{
    [SerializeField] private float _slashTime = 0.5f;
    [SerializeField] private float _extraOffsetY = 0.5f;
    [SerializeField] private float _miniDelayBetweenSlashes = 0.1f;
    private float _startingCooldown = 0;
    private bool _modifyParticles = false;

    public override IEnumerator CastSpell()
    {
        _startingCooldown = cooldown;
        cooldown = Mathf.Infinity;

        for (int i = 0; i < castAmount; i++)
        {
            float offsetY = 0;

            for (int j = 0; j < effectAmount; j++)
            {
                SpellEffect spellEffect = spellEffectPool.Get();
                spellEffect.damage = damage;
                spellEffect.transform.localPosition = new Vector3(0, offsetY, 0);

                offsetY += _extraOffsetY;

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
        }

        cooldown = _startingCooldown;
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
