using UnityEngine;
using UnityEngine.Events;

public class Bolt : SpellEffect
{
    [Header("Physics force")]
    [SerializeField] private Transform forcePoint;

    [SerializeField] private float forceMultiplier = 1;
    [SerializeField] private float speed = 20;
    [SerializeField] private string checkLayer = "Enemy";
    [SerializeField] private string groundLayer = "Ground";
    [SerializeField] GameObject hitEffect;
    [SerializeField] private float effectTimer;

    public UnityEvent OnHitEvent;
    public bool addedEventListener;

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 40 || transform.position.y < -5 || Mathf.Abs(transform.position.z) > 40)
            OnHitEvent?.Invoke();

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider other)
    {
        int targetMask = LayerMask.NameToLayer(checkLayer);
        int groundMask = LayerMask.NameToLayer(groundLayer);
        int defaultMask = LayerMask.NameToLayer("Default");
        if (targetMask == -1) return;

        if (other.gameObject.layer == targetMask || other.gameObject.layer == defaultMask || other.gameObject.layer == groundMask)
        {
            CharacterStats enemyStats = other.attachedRigidbody != null ? other.attachedRigidbody.GetComponent<CharacterStats>() : null;

            if (enemyStats != null)
                enemyStats.TakeDamage(damage, null, forcePoint != null ?
                    (other.transform.position - forcePoint.position).normalized * (damage * forceMultiplier) : Vector3.zero);

            print(other.gameObject.name);

            var hitMark = Instantiate(hitEffect, transform.position, Quaternion.identity);
            PlayerSpellCast._audioSource.PlayOneShot(spellEffectSound);
            Destroy(hitMark, effectTimer);
            OnHitEvent?.Invoke();
        }
    }
}
