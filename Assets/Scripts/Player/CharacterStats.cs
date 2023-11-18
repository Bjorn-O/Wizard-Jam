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
    private float startingHealth = 100;
    private float startingMana = 100;

    public float Health { get { return health; } set { health = Mathf.Clamp(value, 0, maxHealth); } }
    public float Mana { get { return mana; } set { mana = Mathf.Clamp(value, 0, maxMana); } }

    public float StartingHealth { get { return startingHealth; } }
    public float StartingMana { get { return startingMana; } }

    public UnityEvent OnHit;
    public UnityEvent<Vector3> OnDeath;

    private bool _startingStatsSet = false;

    public void Awake()
    {
        ResetStats();

        SetStartingStats();
    }

    public void SetStartingStats()
    {
        if (_startingStatsSet)
            return;

        _startingStatsSet = true;

        startingHealth = maxHealth;
        startingMana = maxMana;
    }

    public void ResetStats()
    {
        health = maxHealth;
        mana = maxMana;
    }

    public void SetMaxStats(float givenHealth, float givenMana)
    {
        maxHealth = givenHealth;
        maxMana = givenMana;
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
