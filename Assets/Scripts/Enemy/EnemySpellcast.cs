using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellcast : MonoBehaviour
{
    private EnemyReferences _enemyRefs;

    [SerializeField] private Spell[] spells = new Spell[4];
    [SerializeField] private float _distanceToCast = 2.5f;
    [SerializeField] private bool _distanceToCastSameAsStop = true;
    [SerializeField] private float _delaySpellcastTime = 1f;

    private float[] spellCooldowns = new float[4];
    private float _delayTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _enemyRefs = GetComponent<EnemyReferences>();

        if (_distanceToCastSameAsStop)
        {
            _distanceToCast = _enemyRefs.navMeshAgent.stoppingDistance;
        }

        Invoke(nameof(ApplyMods), 0.1f);
    }

    public void ApplyMods()
    {
        foreach (var spell in spells)
        {
            if (spell == null)
                continue;

            foreach (var mod in spell.Modifiers)
            {
                mod.SetSpell(spell);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();

        if (_delayTimer > 0)
        {
            _delayTimer -= Time.deltaTime;
            return;
        }

        CheckToSpellcast();
    }

    private void CheckToSpellcast()
    {
        float targetDistance = Vector3.Distance(_enemyRefs.navMeshAgent.destination, transform.position);
        if (targetDistance <= _distanceToCast)
        {
            int index = -1;
            float dmg = 0;

            for (int i = 0; i < spells.Length; i++)
            {
                Spell spell = spells[i];

                if (spell.damage > dmg && spellCooldowns[i] <= 0)
                {
                    index = i;
                    dmg = spell.damage;
                }
            }

            if (index >= 0)
            {
                _delayTimer = _delaySpellcastTime;
                CastSpell(index);
            }
        }
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < spellCooldowns.Length; i++)
        {
            if (spellCooldowns[i] <= 0)
                continue;

            if (spells[i].cooldown == Mathf.Infinity)
                continue;

            spellCooldowns[i] -= Time.deltaTime;
        }
    }

    private void CastSpell(int index)
    {
        if (spells[index] == null)
            return;

        Spell spell = spells[index];

        if (spell.manaCost > _enemyRefs.characterStats.Mana || spellCooldowns[index] > 0)
            return;

        spellCooldowns[index] = spell.cooldown;
        StartCoroutine(spell.CastSpell());
    }
}
