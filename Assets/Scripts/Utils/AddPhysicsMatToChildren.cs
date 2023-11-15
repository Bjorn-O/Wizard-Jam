using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPhysicsMatToChildren : MonoBehaviour
{
    [SerializeField] private PhysicMaterial _physicsMat;

    public void AddPhysicsMats()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider col in colliders)
        {
            col.material = _physicsMat;
        }
    }
}
