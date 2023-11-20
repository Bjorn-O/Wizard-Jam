using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DroppedLoot : MonoBehaviour
{
    private float _healthToGive = 0;
    private float _manaToGive = 0;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _minTime = 0.2f;
    [SerializeField] private float _accel = 1;
    [SerializeField] private float _moveTime = 1;
    [SerializeField] private float _distance = 0.5f;
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private Color _healthColor;
    [SerializeField] private Color _manaColor;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _waitTime = 1;

    private Rigidbody _rb;
    private Collider _col;
    private CharacterStats _characterStats;
    private IObjectPool<DroppedLoot> _lootPool;
    private bool _moveTowardsTarget = false;
    private Vector3 velocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!_moveTowardsTarget)
            return;

        _moveTime -= _accel * Time.deltaTime;

        if (_moveTime < _minTime)
            _moveTime = _minTime;

        Vector3 targetPos = _characterStats.transform.position + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, _moveTime);

        if (Vector3.Distance(transform.position, targetPos) < _distance)
        {
            GiveStat();
        }
    }

    public void StartMoveTowardsTarget(int health, int mana, CharacterStats characterStats, IObjectPool<DroppedLoot> lootPool)
    {
        _lootPool = lootPool;
        _characterStats = characterStats;

        _healthToGive = health;
        _manaToGive = mana;

        foreach (var ps in _particleSystems)
        {
            var main = ps.main;
            main.startColor = _healthToGive > 0 ? _healthColor : _manaColor;
        }

        Invoke(nameof(MoveTowardsTarget), _waitTime);
    }

    private void MoveTowardsTarget()
    {
        _rb.isKinematic = true;
        _col.enabled = false;

        Vector3 pos = transform.position;
        Vector3 targetPos = _characterStats.transform.position + _offset;
        _moveTowardsTarget = true;
    }

    public void GiveStat()
    {
        _characterStats.Heal(_healthToGive);
        _characterStats.Mana += _manaToGive;
        _rb.isKinematic = false;
        _col.enabled = true;
        _lootPool.Release(this);
    }
}