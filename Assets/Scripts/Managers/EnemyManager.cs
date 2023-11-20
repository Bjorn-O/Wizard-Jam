using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    //public enum EnemyTypes { GOBLIN, WIZARD, BOSS}

    [SerializeField] private GameObject _goblinPrefab;
    [SerializeField] private GameObject _wizardPrefab;
    [SerializeField] private GameObject _bossPrefab;

    private IObjectPool<EnemyReferences> _goblinPool;
    private IObjectPool<EnemyReferences> _wizardPool;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private Modifier[] _availableMods;

    // Start is called before the first frame update
    void Awake()
    {
        _goblinPool = new ObjectPool<EnemyReferences>(() => CreatePooledItem(_goblinPrefab, _goblinPool), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, _poolSize);
        _wizardPool = new ObjectPool<EnemyReferences>(() => CreatePooledItem(_wizardPrefab, _wizardPool), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, _poolSize);
    }

    private EnemyReferences CreatePooledItem(GameObject prefab, IObjectPool<EnemyReferences> pool)
    {
        GameObject enemy = Instantiate(prefab, transform, false);
        EnemyReferences enemyRefs = enemy.GetComponent<EnemyReferences>();
        enemy.GetComponent<EnemyDamage>().FadedOut.AddListener(() => pool.Release(enemyRefs));

        return enemyRefs;
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(EnemyReferences enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(EnemyReferences enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(EnemyReferences enemy)
    {
        Destroy(enemy.gameObject);
    }

    public EnemyReferences GetAvailableGoblin()
    {
        return _goblinPool.Get();
    }

    public EnemyReferences GetAvailableWizard()
    {
        return _wizardPool.Get();
    }

    //public void ReturnGoblin(EnemyReferences enemy)
    //{
    //    _goblinPool.Release(enemy);
    //}

    public Modifier GetRandomAvailableMod()
    {
        if (_availableMods.Length == 0)
            return null;

        return _availableMods[Random.Range(0, _availableMods.Length)];
    }
}
