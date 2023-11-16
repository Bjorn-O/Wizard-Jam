using UnityEngine;

public abstract class OnHitEffect : MonoBehaviour
{
    public abstract void OnHit(SpellEffect originEffect);

    public abstract void Execute(CharacterStats stats);
}
