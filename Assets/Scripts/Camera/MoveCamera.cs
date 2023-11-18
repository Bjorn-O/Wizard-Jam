using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = targetTransform.position;
    }
}
