using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    private Vector3 _targetPosition;
    private NavMeshAgent _navMeshAgent;

    private float targetDistance;
    [SerializeField] private float stopDistance = 10f;
    
    [SerializeField] private float updateDelay = 0.2f;
    [SerializeField] private float detectionDelay= 0.2f;
    
    [SerializeField]private bool chasePlayer = true;
    private float _timer;
    public LayerMask Player;
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _targetPosition = target.position;
    }

    private void Update()
    {
        if (chasePlayer)
        {
            UpdateTarget();
        }

        targetDistance = Vector3.Distance(_targetPosition, transform.position);
        Debug.Log(targetDistance);
        if (targetDistance <= stopDistance)
        {
            CheckLineOfSight();
        }
        else
        {
            chasePlayer = true;
        }
    }

    void UpdateTarget()
    {
        _timer += Time.deltaTime;
        if (_timer >= updateDelay)
        {
            _targetPosition = target.position;
            _navMeshAgent.destination = _targetPosition; 
            _timer = 0f;
        }
    }

    public void CheckLineOfSight()
    {
        _targetPosition = target.position;
        RaycastHit hit;
        bool canSeePlayer = Physics.Raycast(transform.position, target.position - transform.position, out hit);
        Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
        if(hit.collider.CompareTag("Player"))
        {
            chasePlayer = false;
            StartCoroutine(StopMove(0.2f));
        }
        else
        {
            chasePlayer = true;
        }
        
    }

    /*private void StopMove()
    {
        _navMeshAgent.destination = gameObject.transform.position;
    }*/

    IEnumerator StopMove(float waitTime)
    {
        {
            yield return new WaitForSeconds(waitTime);
            _navMeshAgent.destination = gameObject.transform.position;
        }
    }
}
