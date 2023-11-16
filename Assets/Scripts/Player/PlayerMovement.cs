using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField] private float _maxForce = 1;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _gravityWhenGrounded = 2;

    [Header("Ground check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 1;
    [SerializeField] private LayerMask _groundLayer;
    private bool _grounded = false;
    public bool Grounded { get { return _grounded; } }

    public bool UseGravity { get; set; } = true;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10;

    private Vector2 _movementInput = Vector2.zero;
    public Vector2 MovementInput { get { return _movementInput; } }
    private Vector3 _movementDir = Vector3.zero;
    public Vector3 MovementDir { get { return _movementDir; } }


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    private void Update()
    {
        GroundCheck();
    }

    void FixedUpdate()
    {
        UpdateMovement();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (!UseGravity)
            return;

        if (_grounded)
        {
            _rb.AddForce(Vector3.down * _rb.mass * _gravityWhenGrounded);
            return;
        }

        _rb.AddForce(Vector3.down * _rb.mass * _gravity);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>().normalized;
    }

    private void OnJump()
    {
        if (!_grounded)
            return;

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void GroundCheck()
    {
        _grounded = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
    }

    private void UpdateMovement()
    {
        _movementDir = transform.TransformDirection(new Vector3(_movementInput.x, 0, _movementInput.y));

        Vector3 currentVel = _rb.velocity;
        currentVel.y = 0;

        Vector3 targetVel = _movementDir * _moveSpeed;

        Vector3 velChange = targetVel - currentVel;

        velChange = Vector3.ClampMagnitude(velChange, _maxForce);

        _rb.AddForce(velChange, ForceMode.VelocityChange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
