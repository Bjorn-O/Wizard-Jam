using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class ArcaneCircleEffect : SpellEffect
{
    public UnityEvent finishedEvent;
    public bool addedEventListener = false;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private Transform forcePoint;
    [SerializeField] private float forceMultiplier;

    private List<CharacterStats> _bodiesInCircle = new List<CharacterStats>();

    public override void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent<CharacterStats>(out CharacterStats result))
        {
            print(result.name + ": Added!");
            _bodiesInCircle.Add(result);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent<CharacterStats>(out CharacterStats result) && _bodiesInCircle.Contains(result))
        {
            _bodiesInCircle.Remove(result);
        }
    }

    public IEnumerator Explode(int timesToExplode, float explosionDelay)
    {
        for (int i = 0; i < timesToExplode; i++)
        {
            yield return new WaitForSeconds(explosionDelay);
            var explosion = Instantiate(explosionEffect, transform.position, quaternion.identity);
            Destroy(explosion, 1);
            foreach (CharacterStats body in _bodiesInCircle)
            {
                body.TakeDamage(damage, null, forcePoint != null ?
                (body.transform.position - forcePoint.position).normalized * (damage * forceMultiplier) : Vector3.zero);
            }
        }
        finishedEvent.Invoke();
    }
}
