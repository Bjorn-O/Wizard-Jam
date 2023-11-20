using System.Collections;
using TMPro;
using UnityEngine;

public class ArcaneCircle : Spell
{
    [SerializeField] private Transform camPosition;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float maxAngle;
    [SerializeField] private float explosionTimer;

    private bool _modifyParticles = false;

    private void Start()
    {
        OnApplyModifiers.AddListener(() => { _modifyParticles = true; });
    }

    public override IEnumerator CastSpell()
    {            
        RaycastHit hit;
        startingCooldown = cooldown;
        cooldown = Mathf.Infinity;
        for (int i = 0; i < castAmount; i++)
        {
            if (Physics.Raycast(transform.position, camPosition.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                FireSpellEffect(spellEffect, effectAmount, hit.point, hit.normal);
                yield return new WaitForSeconds(multiCastDelay);
            }
            cooldown = startingCooldown;
        }
    }

    protected void FireSpellEffect(SpellEffect effect, int amount, Vector3 location, Vector3 rot)
    {
        SpellEffect spellEff = spellEffectPool.Get();
        PlayerSpellCast._audioSource.PlayOneShot(spellCastSound);
        ArcaneCircleEffect circleEffect = (spellEff as ArcaneCircleEffect);

        if (_modifyParticles)
        {
            SpellVfxManager spellVfx = circleEffect.GetComponent<SpellVfxManager>();
            spellVfx.ModifyParticleSystems(effectAmount, GetElementsFromMods().ToArray());
        }

        circleEffect.transform.localScale *= effectScale;
        circleEffect.transform.position = location;
        circleEffect.transform.up = rot;
        circleEffect.transform.localScale = transform.localScale;
        circleEffect.damage = damage;

        if (!(circleEffect.addedEventListener))
        {
            circleEffect.finishedEvent.AddListener(() => { spellEffectPool.Release(circleEffect); });
            circleEffect.addedEventListener = true;
        }

        circleEffect.StartCoroutine(circleEffect.Explode(amount, explosionTimer));
    }

    // Written to check if the Arcane circle was on the wall, then thought, why the fuck not. Could be funny! 
    private bool CheckAnglesWithinBounds(Vector3 rotationToCheck, float angleLimit)
    {
        if (rotationToCheck.x > angleLimit || rotationToCheck.y > angleLimit || rotationToCheck.z > angleLimit)
        {
            return false;
        }
        return true;
    }

    protected override void FireSpellEffect(SpellEffect effect, float amount)
    {
        throw new System.NotImplementedException();
    }
}
