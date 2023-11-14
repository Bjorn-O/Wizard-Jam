using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerMovement _playerMov;

    [SerializeField] private float _dashForce = 3;
    [SerializeField] private float _cooldownTime = 1;
    private float _cooldownTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMov = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Cooldown();
    }

    private void Cooldown()
    {
        if (_cooldownTimer > 0)
        {
            print(_rb.velocity.magnitude);
            _cooldownTimer -= Time.deltaTime;
        }
    }

    //Placeholder
    private void OnDash()
    {
        if (_cooldownTimer > 0)
            return;

        Vector3 dir = _playerMov.MovementDir;

        if (_playerMov.MovementInput.magnitude <= 0.1f)
        {
            dir = transform.forward;
            dir.y = 0;
        }

        Vector3 vel = _rb.velocity;
        vel.y = 0;

        if (vel.magnitude < _playerMov.MoveSpeed)
        {
            _rb.velocity = dir * _playerMov.MoveSpeed;
        }

        _rb.AddForce(dir * _dashForce, ForceMode.Impulse);

        _cooldownTimer = _cooldownTime;
    }
}
