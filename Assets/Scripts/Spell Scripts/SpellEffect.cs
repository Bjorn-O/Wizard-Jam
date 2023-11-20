using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class  SpellEffect : MonoBehaviour
{
    public float damage = 0;
    public IObjectPool<SpellEffect> returnPool;

    public AudioClip spellEffectSound;
    public OnHitEffect[] hitEffects;

    public abstract void OnTriggerEnter(Collider other);
}
