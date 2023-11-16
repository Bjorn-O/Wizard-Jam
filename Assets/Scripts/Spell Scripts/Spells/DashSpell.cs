using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashSpell : Spell
{
    private Rigidbody _rb;
    private PlayerMovement _playerMov;
    private Collider _playerCol;
    private Camera _cam;

    [Header("Dash settings")]
    [SerializeField] private float _dashForce = 3;
    [SerializeField] private SpellVfxManager _speedLines;
    [SerializeField] private float _dashFovAdder = 10;
    [SerializeField] private float _fovTweenTime = 0.25f;
    private float _speedLinesOffsetZ;
    private float _startingCamFov;
    private bool _dashing = false;
    private bool _canCheckDash = false;

    public override IEnumerator CastSpell()
    {
        Vector3 dir = _playerMov.MovementDir;

        if (_playerMov.MovementInput.magnitude <= 0.1f)
        {
            dir = _rb.transform.forward;
            dir.y = 0;
        }

        Vector3 vel = _rb.velocity;
        vel.y = 0;

        if (vel.magnitude < _playerMov.MoveSpeed)
        {
            _rb.velocity = dir * _playerMov.MoveSpeed;
        }

        _rb.AddForce(dir * _dashForce, ForceMode.Impulse);
        _playerMov.UseGravity = false;

        _speedLines.PlayEffect(modifiers.Count > 0);
        _speedLines.transform.position = _cam.transform.position + dir * _speedLinesOffsetZ;
        _speedLines.transform.LookAt(_speedLines.transform.position - (_cam.transform.position - _speedLines.transform.position));
        _cam.DOFieldOfView(_startingCamFov + _dashFovAdder, _fovTweenTime);

        _playerCol.gameObject.layer = LayerMask.NameToLayer("Dashing");

        _dashing = true;

        yield return new WaitForSeconds(0.1f);

        _canCheckDash = true;
    }

    protected override void FireSpellEffect(SpellEffect effect, float amount)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerMov = FindObjectOfType<PlayerMovement>();
        _rb = _playerMov.GetComponent<Rigidbody>();
        _playerCol = _rb.GetComponentInChildren<Collider>();

        _speedLinesOffsetZ = _speedLines.transform.localPosition.z;
        _cam = Camera.main;
        _startingCamFov = _cam.fieldOfView;

        List<Elements> elements = new List<Elements>();

        foreach (var mod in modifiers)
        {
            if (mod.Element != Elements.Unaspected)
            {
                elements.Add(mod.Element);
            }
        }

        OnApplyModifiers.AddListener(() => _speedLines.ModifyParticleSystems(castAmount, elements.ToArray()));
    }

    private void Update()
    {
        CheckDash();
    }

    private void CheckDash()
    {
        if (!_dashing || !_canCheckDash)
            return;

        Vector3 vel = _rb.velocity;
        vel.y = 0;

        if (vel.magnitude - _playerMov.MoveSpeed < 0.5f)
        {
            _dashing = false;
            _playerMov.UseGravity = true;
            _canCheckDash = false;
            _speedLines.StopEffect();
            _cam.DOFieldOfView(_startingCamFov, _fovTweenTime);

            _playerCol.gameObject.layer = _rb.gameObject.layer;
        }
    }
}
