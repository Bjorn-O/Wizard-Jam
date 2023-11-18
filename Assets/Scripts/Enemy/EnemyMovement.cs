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
    private Animator _anim;

    private float targetDistance;
    [SerializeField] private float stopDistance = 10f;
    
    [SerializeField] private float updateDelay = 0.2f;
    [SerializeField] private float detectionDelay= 0.2f;
    
    [SerializeField]private bool chasePlayer = true;

    private float _timer;
    public LayerMask Player;
    
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _targetPosition = target.position;
        _navMeshAgent.enabled = true;
    }

    private void Update()
    {
        if (!_navMeshAgent.enabled)
            return;

        if (_anim != null)
        {
            _anim.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }

        if (chasePlayer)
        {
            UpdateTarget();
        }

        targetDistance = Vector3.Distance(_targetPosition, transform.position);
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
        if(hit.collider != null && hit.collider.CompareTag("Player"))
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
