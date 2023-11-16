using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class MagicBolt : Spell
{
    [SerializeField] private Transform castPoint;
    [SerializeField] private int castSpreadDegrees;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("CastSpell");
        }
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
            var bolt = Instantiate(effect, castPoint.position, Quaternion.identity);
        } else
        {
            // Splits half the shots into the negative, with 0 being the middle if the number is uneven.
            int shotPosition = 0 - Mathf.FloorToInt(amount / 2);
            print(shotPosition);
            float radials = castSpreadDegrees * (Mathf.PI / 180);
            for(int i = 0;i < amount ;i++) 
            {
                var bolt = Instantiate(effect, CalculateShotPoint(radials * shotPosition), castPoint.rotation);
                Quaternion baseRotation = bolt.transform.rotation;
                Quaternion turnRotation = Quaternion.Euler(0f, castSpreadDegrees * shotPosition * -1, 0f);
                bolt.transform.rotation = turnRotation * baseRotation;
                shotPosition++;
            }
        }
    }

    private Vector3 CalculateShotPoint(float radial)
    {
        Vector3 originPoint = new Vector3(castPoint.position.x - castPoint.localPosition.x,
            castPoint.position.y,
            castPoint.position.z - castPoint.localPosition.z);
        Vector3 castDir = castPoint.position - originPoint;
        Vector3 rotatedDirection = castDir * radial;
        return castPoint.position + rotatedDirection;
    }
}
