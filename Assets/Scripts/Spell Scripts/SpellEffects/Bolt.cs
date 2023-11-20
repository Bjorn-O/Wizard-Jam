using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bolt : SpellEffect
{
    [Header("Physics force")]
    [SerializeField] private Transform forcePoint;

    [SerializeField] private float forceMultiplier = 1;
    [SerializeField] private float speed = 20;
    [SerializeField] private string checkLayer = "Enemy";
    [SerializeField] GameObject hitEffect;
    [SerializeField] private float effectTimer;

    public UnityEvent OnHitEvent;
    public bool addedEventListener;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider other)
    {
        int targetMask = LayerMask.NameToLayer(checkLayer);
        if (targetMask == -1) return;

        if (other.gameObject.layer == targetMask || other.gameObject.layer == 0 || other.gameObject.layer == 6)
        {
            CharacterStats enemyStats = other.attachedRigidbody != null ? other.attachedRigidbody.GetComponent<CharacterStats>() : null;

            if(enemyStats != null)
                enemyStats.TakeDamage(damage, null, forcePoint != null ?
                    (other.transform.position - forcePoint.position).normalized * (damage * forceMultiplier) : Vector3.zero);

            OnHitEvent.Invoke();

            var hitMark = Instantiate(hitEffect, transform.position, Quaternion.identity);

            Destroy(hitMark, effectTimer);
        }
    }
}
