using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float mana = 100;
    [SerializeField] private float maxMana = 100;

    public float Health { get { return health; } set { health = Mathf.Clamp(value, 0, maxHealth); } }
    public float Mana { get { return mana; } set { mana = Mathf.Clamp(value, 0, maxMana); } }

    public UnityEvent OnHit;
    public UnityEvent<Vector3> OnDeath;

    public void Start()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        health = maxHealth;
        mana = maxMana;
    }

    public void TakeDamage(float damage, OnHitEffect[] hitEffects, Vector3 force)
    {
        if (health <= 0)
            return;

        health -= damage;
        OnHit?.Invoke();

        if (health <= 0)
        {
            OnDeath?.Invoke(force);
            return;
        }

        if (hitEffects != null)
        {
            foreach (var effect in hitEffects)
            {
                effect.Execute(this);
            }
        }       
    }
}
