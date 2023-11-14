using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  SpellEffect : MonoBehaviour
{
    public OnHitEffect[] hitEffects;

    public abstract void OnTriggerEnter(Collider other);
}
