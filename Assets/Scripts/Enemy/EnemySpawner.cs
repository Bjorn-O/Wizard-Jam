using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyManager enemyManager;
    private enum Difficulty { EASY, NORMAL, HARD}

    [SerializeField] private List<EnemyReferences> _enemies = new List<EnemyReferences>();
    private List<EnemyReferences> _subbedEnemies = new List<EnemyReferences>();
    [SerializeField] private Difficulty _difficulty;

    [Header("Spawn values (values change by loop count)")]
    [SerializeField] private int _spawnCount = 3;
    [SerializeField] private int _randomExtraSpawn = 2;
    [SerializeField] private int _extraModChance = 5;

    [Header("Difficulty based values (values for normal)")]
    [SerializeField] private int _extraSpawnDifficulty = 1;
    [SerializeField] private int _modCountDifficulty = 1;

    [Header("Values change by loop and difficulty")]
    //[SerializeField] private float _loopMultiplier = 0.5f;
    [SerializeField] private float _enemyExtraLoopHealth = 20;
    [SerializeField] private float _enemyExtraDifficultyHealth = 25;
    [SerializeField] private float _wizardChance = 20;
    [SerializeField] private float _extraWizardChanceDifficulty = 5;
    [SerializeField] private float _extraWizardChanceLoop = 2;
    [SerializeField] private float _maxWizardChance = 40;

    [Header("Spawn settings")]
    [SerializeField] private Transform[] _spawnBoxes;
    [SerializeField] private float _spawnY = 1;
    [SerializeField] private int _maxEnemies = 30;
    [SerializeField] public GameObject finishCircle;

    public int _enemiesKilled = 0;

    // Start is called before the first frame update
    void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Start()
    {
        StartSpawnEnemies();
    }

    public void StartSpawnEnemies()
    {
        finishCircle.SetActive(false);
        _enemies.Clear();
        StartCoroutine(SpawnEnemies());
    }

    //private void Update()
    //{
    //    //PLACEHOLDER
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        SpawnEnemies();
    //    }
    //}

    public IEnumerator SpawnEnemies()
    {
        if (_enemiesKilled > 0)
            yield break;

        yield return new WaitForSeconds(2);

        int countToSpawn = Random.Range(_spawnCount, _spawnCount + _randomExtraSpawn) + GameManager.instance.loopCount;
        countToSpawn += _extraSpawnDifficulty * (int)_difficulty;

        float wizChance = _wizardChance + 
            (_extraWizardChanceDifficulty * (int)_difficulty) + (_extraWizardChanceLoop * GameManager.instance.loopCount);

        if (wizChance > _maxWizardChance)
            wizChance = _maxWizardChance;

        if (countToSpawn > _maxEnemies)
            countToSpawn = _maxEnemies;

        for (int i = 0; i < countToSpawn; i++)
        {
            EnemyReferences enemy;

            float rand = Random.Range(0, 100);

            if (rand < _wizardChance)
                enemy = enemyManager.GetAvailableWizard();
            else
                enemy = enemyManager.GetAvailableGoblin();

            _enemies.Add(enemy);

            if (!_subbedEnemies.Contains(enemy))
            {
                _subbedEnemies.Add(enemy);
                enemy.characterStats.OnDeath.AddListener(_ => { _enemiesKilled++; CheckOpenDoors(); });
            }

            PlaceEnemy(enemy);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PlaceEnemy(EnemyReferences enemy)
    {
        enemy.navMeshAgent.enabled = false;
        Transform enemyTransform = enemy.transform;

        int rand = Random.Range(0, _spawnBoxes.Length);

        Transform chosenBox = _spawnBoxes[rand];
        Vector3 center = chosenBox.transform.position;
        float x = chosenBox.lossyScale.x / 2;
        float z = chosenBox.lossyScale.z / 2;
        Vector3 randomPosInBox = center + new Vector3(Random.Range(-x, x), 0, Random.Range(-z, z));
        randomPosInBox.y = _spawnY;
        enemy.transform.position = randomPosInBox;

        enemy.characterStats.SetStartingStats();

        float health = enemy.characterStats.StartingHealth;
        health += _enemyExtraLoopHealth * GameManager.instance.loopCount;
        health += _enemyExtraDifficultyHealth * (int)_difficulty;

        enemy.characterStats.SetMaxStats(health, 50);

        enemy.characterStats.ResetStats();
        enemy.enemySpellcast.ClearModsFromSpells();
        enemy.enemyDamage.ResetAllSettings();

        foreach (var spell in enemy.enemySpellcast.Spells)
        {
            int modCount = _modCountDifficulty * (int)_difficulty;

            if (Random.Range(0, 100) < _extraModChance + (GameManager.instance.loopCount * 2))
                modCount++;

            if (modCount > 3)
                modCount = 3;

            for (int i = 0; i < modCount; i++)
            {
                spell.AddModifier(enemyManager.GetRandomAvailableMod());
            }
        }

        enemy.enemySpellcast.ApplyMods();

        enemy.navMeshAgent.enabled = true;
        enemy.ps.Play();
    }

    private void CheckOpenDoors()
    {
        if (_enemiesKilled >= _enemies.Count)
        {
            finishCircle.SetActive(true);
        }
    }
}
