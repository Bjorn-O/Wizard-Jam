using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerMovement _playerMov;

    [SerializeField] private float _dashForce = 3;
    [SerializeField] private float _cooldownTime = 1;
    [SerializeField] private ParticleSystem _speedLines;
    [SerializeField] private float _dashFovAdder = 10;
    [SerializeField] private float _fovTweenTime = 0.25f;
    private float _speedLinesOffsetZ;
    private float _cooldownTimer = 1;
    private float _startingCamFov;
    private bool _dashing = false;

    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMov = GetComponent<PlayerMovement>();

        _speedLinesOffsetZ = _speedLines.transform.localPosition.z;
        _cam = Camera.main;
        _startingCamFov = _cam.fieldOfView;
    }

    private void Update()
    {
        CheckDash();
        Cooldown();
    }

    private void CheckDash()
    {
        if (!_dashing)
            return;

        Vector3 vel = _rb.velocity;
        vel.y = 0;

        if (vel.magnitude - _playerMov.MoveSpeed < 0.5f && _cooldownTimer < _cooldownTime * 0.9f)
        {
            _dashing = false;
            _speedLines.Stop();
            _cam.DOFieldOfView(_startingCamFov, _fovTweenTime);
        }
    }

    private void Cooldown()
    {
        if (_cooldownTimer > 0)
        {
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

        _speedLines.Play();
        _speedLines.transform.position = _cam.transform.position + dir * _speedLinesOffsetZ;
        _speedLines.transform.LookAt(_speedLines.transform.position - (_cam.transform.position - _speedLines.transform.position));
        _cam.DOFieldOfView(_startingCamFov + _dashFovAdder, _fovTweenTime);

        _dashing = true;

    }
}
