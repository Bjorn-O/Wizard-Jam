using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    [Header("Physics force")]
    [SerializeField] private float forceMultiplier = 1;
    [SerializeField] private Transform forcePoint;
    [SerializeField] private string checkTag = "Enemy";

    public override void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(checkTag))
        {
            CharacterStats enemyStats = other.attachedRigidbody.GetComponent<CharacterStats>();

            enemyStats.TakeDamage(damage, null, forcePoint != null ? 
                (other.transform.position - forcePoint.position).normalized * (damage * forceMultiplier) : Vector3.zero);
        }
    }
}
