using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransform.position;
    }
}
