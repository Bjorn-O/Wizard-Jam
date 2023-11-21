using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashSpell : Spell
{
    private Rigidbody _rb;
    private PlayerMovement _playerMov;
    private CharacterStats _characterStats;
    private Collider _playerCol;
    private Camera _cam;

    [Header("Dash settings")]
    [SerializeField] private float _dashForce = 3;
    [SerializeField] private SpellVfxManager _speedLines;
    [SerializeField] private float _dashFovAdder = 10;
    [SerializeField] private float _fovTweenTime = 0.25f;

    [Header("Spell Effect settings")]
    [SerializeField] private float _effectOffsetZ = 8.5f;
    [SerializeField] private float _effectExtraOffsetZ = 1f;
    private List<SpellEffect> pooledSpellEffects = new List<SpellEffect>();

    private float _speedLinesOffsetZ;
    private float _startingCamFov;
    private bool _dashing = false;
    private bool _canCheckDash = false;

    private int _extraCasts = 0;
    private bool _resetCastAmount = true;
    private bool _modifyParticles;

    [SerializeField] private Material material;

    public override IEnumerator CastSpell()
    {
        if (_resetCastAmount)
        {
            _extraCasts = castAmount - 1;
        }

        if (cooldown != Mathf.Infinity)
        {
            startingCooldown = cooldown;
            cooldown = Mathf.Infinity;
        }

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

        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        _playerMov.UseGravity = false;

        float offsetZ = _effectOffsetZ;

        for (int i = 0; i < effectAmount; i++)
        {
            PlayerSpellCast._audioSource.PlayOneShot(spellCastSound);
            SpellEffect effect = spellEffectPool.Get();
            effect.damage = damage;
            effect.transform.localScale = Vector3.one * effectScale;
            effect.transform.localPosition = new Vector3(0, 0, offsetZ);
            offsetZ += _effectExtraOffsetZ;

            pooledSpellEffects.Add(effect);
        }

        if (_modifyParticles)
        {
            _speedLines.ModifyParticleSystems(effectAmount, GetElementsFromMods().ToArray());
            _modifyParticles = false;
        }

        _speedLines.PlayEffect(true);
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
        Color color = Color.white;
        color.a = material.color.a;
        material.color = color;
        _playerMov = FindObjectOfType<PlayerMovement>();
        _rb = _playerMov.GetComponent<Rigidbody>();
        _characterStats = _rb.GetComponent<CharacterStats>();
        _playerCol = _rb.GetComponentInChildren<Collider>();

        _speedLinesOffsetZ = _speedLines.transform.localPosition.z;
        _cam = Camera.main;
        _startingCamFov = _cam.fieldOfView;

        OnApplyModifiers.AddListener(() => { _modifyParticles = true; });
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

            foreach (var effect in pooledSpellEffects)
            {
                spellEffectPool.Release(effect);
            }

            pooledSpellEffects.Clear();

            if (_extraCasts <= 0 || _characterStats.Mana < manaCost)
            {
                _resetCastAmount = true;
                cooldown = startingCooldown;
            }
            else
            {
                _characterStats.Mana -= manaCost;
                StartCoroutine(CastSpell());
                _extraCasts -= 1;
            }
        }
    }
}
