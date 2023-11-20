using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DroppedLootManager : MonoBehaviour
{
    private CharacterStats _characterStats;
    [SerializeField] private DroppedLoot _droppedLootPrefab;
    [SerializeField] private IObjectPool<DroppedLoot> _lootPool;
    [SerializeField] private int _maxPoolSize = 20;
    [SerializeField] private int _healthToGive = 20;
    [SerializeField] private int _manaToGive = 20;
    [SerializeField] private float _minHealthToGivePercentage = 0.5f;
    [SerializeField] private int _healthChance = 20;
    [SerializeField] private float _minHealthLowPercentage = 0.25f;
    [SerializeField] private int _healthChanceLow = 60;
    [SerializeField] private int _dropChance = 40;
    [SerializeField] private int _dropChanceLowStats = 60;
    [SerializeField] private float _manaHigherChancePercentage = 0.4f;

    // Start is called before the first frame update
    void Awake()
    {
        _characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        _lootPool = new ObjectPool<DroppedLoot>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, _maxPoolSize);
    }

    public void CheckDropLoot(Vector3 pos)
    {
        float rand = Random.Range(0, 100);
        int chance = _dropChance;

        if (_characterStats.Mana < _characterStats.MaxMana * _manaHigherChancePercentage || _characterStats.Health < _characterStats.MaxHealth * _minHealthLowPercentage)
            chance = _dropChanceLowStats;

        if (rand < chance)
        {
            DropLoot(pos);
        }
    }

    public void DropLoot(Vector3 pos)
    {
        float rand = Random.Range(0, 100);
        int chance = 0;

        if (_characterStats.Health < _characterStats.MaxHealth * _minHealthToGivePercentage)
            chance = _healthChance;
        else if (_characterStats.Health < _characterStats.MaxHealth * _minHealthLowPercentage)
            chance = _healthChanceLow;

        DroppedLoot loot = _lootPool.Get();
        loot.transform.position = pos;

        loot.StartMoveTowardsTarget(rand < chance ? _healthToGive : 0, rand >= chance ? _manaToGive : 0, _characterStats, _lootPool);
    }

    private DroppedLoot CreatePooledItem()
    {
        GameObject effectObject = Instantiate(_droppedLootPrefab.gameObject, transform, false);

        return effectObject.GetComponent<DroppedLoot>();
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(DroppedLoot loot)
    {
        loot.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(DroppedLoot loot)
    {
        loot.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(DroppedLoot loot)
    {
        Destroy(loot.gameObject);
    }
}
