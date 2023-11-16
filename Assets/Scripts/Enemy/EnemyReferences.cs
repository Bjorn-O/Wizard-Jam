using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyReferences : MonoBehaviour
{
    [HideInInspector]public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
}
