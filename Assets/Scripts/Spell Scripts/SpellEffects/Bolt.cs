using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : SpellEffect
{
    [Header("Physics force")]
    [SerializeField] private Transform forcePoint;

    [SerializeField] private float forceMultiplier = 1;
    [SerializeField] private float speed = 20;
    [SerializeField] private string checkLayer = "Enemy";

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider other)
    {
        int targetMask = LayerMask.NameToLayer(checkLayer);
        if (targetMask == -1) return;

        if (other.attachedRigidbody != null && other.gameObject.layer == targetMask)
        {
            CharacterStats enemyStats = other.attachedRigidbody.GetComponent<CharacterStats>();

            enemyStats.TakeDamage(damage, null, forcePoint != null ?
                (other.transform.position - forcePoint.position).normalized * (damage * forceMultiplier) : Vector3.zero);
        }

        Destroy(this.gameObject);
    }
}
