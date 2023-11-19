using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _sensitivity = 100;

    [Header("Look Sway Settings")]
    [SerializeField, Min(0)] private float _swaySpeed = 5f;
    [SerializeField, Tooltip("The multiplier for the sway caused by looking around"), Min(0)] private float _lookSwayMultiplier = 0.26f;
    [SerializeField] Vector3 _movementSwayStrength = new Vector3(5f, 1f, 1f);
    [SerializeField, Space()] Vector3 _minSwayRotation = new Vector3(-15, -5, -5);
    [SerializeField] Vector3 _maxSwayRotation = new Vector3(15, 5, 5);

    [Header("Multiplier")]
    public float _currentMultiplier = 1f;

    private Vector2 _lookPos;
    private float _xRotation;
    private float _yRotation;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _camera = Camera.main.transform;
    }

    private void Update()
    {
        CamTilt();
    }

    private void OnLook(InputValue inputValue)
    {
        _lookPos = inputValue.Get<Vector2>();
        _lookPos *= _sensitivity * Time.deltaTime;

        _yRotation += _lookPos.x;

        _xRotation -= _lookPos.y;
        _xRotation = Mathf.Clamp(_xRotation, -89f, 89f);

        _cameraHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        transform.Rotate(Vector3.up, _lookPos.x);
    }

    private void CamTilt()
    {
        if (Time.frameCount < 10) return;

        Vector2 mouse = _lookPos * _lookSwayMultiplier;
        var movement = transform.InverseTransformDirection(_rb.velocity);
        movement.x *= _movementSwayStrength.x;
        movement.y *= _movementSwayStrength.y;
        movement.z *= _movementSwayStrength.z;

        movement *= _currentMultiplier;

        var rotationX = Quaternion.AngleAxis(mouse.x, Vector3.up);
        var rotationY = Quaternion.AngleAxis(-mouse.y, Vector3.right);

        var rotationZ = Quaternion.AngleAxis(-movement.x, Vector3.forward);
        var rotationY2 = Quaternion.AngleAxis(movement.y, Vector3.right);
        var rotationY3 = Quaternion.AngleAxis(movement.z, Vector3.right);
        var quatTargetRotation = rotationZ * rotationX * rotationY * rotationY2 * rotationY3;
        var angles = GetSignedEulerAngles(quatTargetRotation.eulerAngles);

        var targetRotation = Quaternion.Euler(
            Mathf.Clamp(angles.x, _minSwayRotation.x, _maxSwayRotation.x),
            Mathf.Clamp(angles.y, _minSwayRotation.y, _maxSwayRotation.y),
            Mathf.Clamp(angles.z, _minSwayRotation.z, _maxSwayRotation.z)
        );

        _camera.localRotation = Quaternion.Slerp(_camera.localRotation, targetRotation, _swaySpeed * Time.deltaTime);
    }

    public Vector3 GetSignedEulerAngles(Vector3 angles)
    {
        Vector3 signedAngles = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            signedAngles[i] = (angles[i] + 180f) % 360f - 180f;
        }
        return signedAngles;
    }
}
