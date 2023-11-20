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

    public float Health { get { return health; } set {
            if (value > health)
            {
                OnHeal?.Invoke();
            }

            health = Mathf.Clamp(value, 0, maxHealth);
        }
    }
    public float MaxHealth { get { return maxHealth; } }
    public float Mana { get { return mana; } set { mana = Mathf.Clamp(value, 0, maxMana); } }
    public float MaxMana { get { return maxMana; } }

    public float StartingHealth { get { return startingHealth; } }
    public float StartingMana { get { return startingMana; } }

    public UnityEvent OnHit;
    public UnityEvent OnHeal;
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

    public void Heal(float hp)
    {
        health += hp;
        OnHeal?.Invoke();
    }

    public void TakeDamage(float damage, OnHitEffect[] hitEffects, Vector3 force)
    {
        if (health <= 0)
            return;

        health -= damage;

        if (health <= 0)
        {
            health = 0;
            OnHit?.Invoke();
            OnDeath?.Invoke(force);
            return;
        }

        OnHit?.Invoke();

        if (hitEffects != null)
        {
            foreach (var effect in hitEffects)
            {
                effect.Execute(this);
            }
        }       
    }
}
