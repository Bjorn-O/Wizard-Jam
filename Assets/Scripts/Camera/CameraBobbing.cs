using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    private Rigidbody _playerRb;
    private PlayerMovement _playerMov;

    [Header("Configuration")]
    [SerializeField] private bool _enable = true;
    [SerializeField, Range(0, 0.1f)] private float _Amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f;
    [SerializeField] private float _resetSpeed = 1;

    private Transform _camera;
    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        _playerMov = FindObjectOfType<PlayerMovement>();
        _playerRb = _playerMov.GetComponent<Rigidbody>();
        _camera = Camera.main.transform;
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        if (!_enable) return;
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _Amplitude;

        float upDot = Vector3.Dot(transform.forward, Vector3.up);
        float downDot = Vector3.Dot(transform.forward, Vector3.down);
        float highestDot = upDot > downDot ? upDot : downDot;

        float lookUpPercentage = Mathf.Abs(highestDot - 1);
        print(lookUpPercentage);

        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _Amplitude * (2 * lookUpPercentage);

        return pos;
    }
    private void CheckMotion()
    {
        float speed = new Vector3(_playerRb.velocity.x, 0, _playerRb.velocity.z).magnitude;
        if (speed < _toggleSpeed) return;
        if (!_playerMov.Grounded) return;

        PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion)
    {
        _camera.position += transform.TransformVector(motion);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + transform.localPosition.y, transform.position.z);
        pos += transform.forward * 15.0f;
        return pos;
    }
    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, _resetSpeed * Time.deltaTime);
    }
}
