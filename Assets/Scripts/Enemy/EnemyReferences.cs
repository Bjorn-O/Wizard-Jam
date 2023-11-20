using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyReferences : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public EnemyDamage enemyDamage;
    [HideInInspector] public EnemySpellcast enemySpellcast;
    [HideInInspector] public Transform player;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterStats = GetComponent<CharacterStats>();
        enemyDamage = GetComponent<EnemyDamage>();
        enemySpellcast = GetComponent<EnemySpellcast>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
