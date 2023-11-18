using System.Collections;
using UnityEngine;

public class MagicBolt : Spell
{
    [SerializeField] private Transform castTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private AnimationCurve curve;

    private bool _modifyParticles;

    private void Start()
    {
        OnApplyModifiers.AddListener(() => { _modifyParticles = true; });
    }

    public override void AddModifier(Modifier mod)
    {
        base.AddModifier(mod);

        throw new System.NotImplementedException();
    }

    public override IEnumerator CastSpell()
    {
        for (int i = 0; i < castAmount; i++)
            {
                FireSpellEffect(spellEffect, effectAmount);
                onSpellCast?.Invoke();
                yield return new WaitForSeconds(multiCastDelay);
            }
        }

    protected override void FireSpellEffect(SpellEffect effect, float amount)
    {
        if (amount == 1)
        {
            var bolt = Instantiate(effect, castTransform.position, castTransform.rotation);
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

                //Spawn Spell and set position/rotation
                SpellEffect bolt = spellEffectPool.Get();
                bolt.transform.position = castTransform.position;
                bolt.transform.rotation = camTransform.rotation;
                bolt.transform.Rotate(0, degrees, 0);
                print(degrees);

                //Apply Spell stats to object
                bolt.transform.localScale *= effectScale;
                bolt.damage = damage;
                if (!(bolt as Bolt).addedEventListener)
                {
                    (bolt as Bolt).OnHitEvent.AddListener(() => { spellEffectPool.Release(bolt); });
                    (bolt as Bolt).addedEventListener = true;
                }

                //Checks if the bolt needs to be changed element. This is stupid and dumb but we hadn't prepared for this in the framework. Too bad Bas.
                if (_modifyParticles)
                {
                    SpellVfxManager spellVfx = bolt.GetComponent<SpellVfxManager>();
                    spellVfx.ModifyParticleSystems(effectAmount, GetElementsFromMods().ToArray());
                }

                // Give bolt action to release
                shotPosition++;
            }
        }
    }

    //Decrapit

    //private Vector3 CalculateShotPoint(float radial)
    //{
    //    Vector3 originPoint = new Vector3(castPoint.position.x - castPoint.localPosition.x,
    //        castPoint.position.y,
    //        castPoint.position.z - castPoint.localPosition.z);
    //    Vector3 castDir = castPoint.position - originPoint;
    //    Vector3 rotatedDirection = castDir * radial;
    //    return castPoint.position + rotatedDirection;
    //}
    //private Vector3 GetPlayerPoint()
    //{
    //    return new Vector3(castPoint.position.x - castPoint.localPosition.x,
    //        castPoint.position.y,
    //        castPoint.position.z - castPoint.localPosition.z);
    //}
}
