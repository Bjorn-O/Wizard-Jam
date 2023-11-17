using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    [Header("Physics force")]
    [SerializeField] private float forceMultiplier = 1;
    [SerializeField] private Transform forcePoint;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Enemy"))
        {
            CharacterStats enemyStats = other.attachedRigidbody.GetComponent<CharacterStats>();

            enemyStats.TakeDamage(damage, null, (other.transform.position - forcePoint.position).normalized * (damage * forceMultiplier));
        }
    }
}
