using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : SpellEffect
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Enemy"))
        {
            CharacterStats enemyStats = other.attachedRigidbody.GetComponent<CharacterStats>();

            enemyStats.TakeDamage(damage, null);
        }
    }
}
