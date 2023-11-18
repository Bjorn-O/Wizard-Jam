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
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private Modifier[] _availableMods;

    // Start is called before the first frame update
    void Awake()
    {
        _goblinPool = new ObjectPool<EnemyReferences>(() => CreatePooledItem(_goblinPrefab), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, _poolSize);
    }

    private EnemyReferences CreatePooledItem(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab, transform, false);
        EnemyReferences enemyRefs = enemy.GetComponent<EnemyReferences>();
        enemy.GetComponent<EnemyDamage>().FadedOut.AddListener(() => _goblinPool.Release(enemyRefs));

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

    public EnemyReferences GetAvailableEnemy()
    {
        return _goblinPool.Get();
    }

    public void ReturnEnemy(EnemyReferences enemy)
    {
        _goblinPool.Release(enemy);
    }

    public Modifier GetRandomAvailableMod()
    {
        if (_availableMods.Length == 0)
            return null;

        return _availableMods[Random.Range(0, _availableMods.Length)];
    }
}
