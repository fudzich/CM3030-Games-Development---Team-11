using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private Transform minSpawn;
    [SerializeField] private Transform maxSpawn;

    [Header("Enemies")]
    [SerializeField] private GameObject level1Enemy;
    [SerializeField] private GameObject level2Enemy;
    [SerializeField] private GameObject level3Enemy;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [Tooltip("Spawn the boss when remaining time is at or below this (seconds).")]
    [SerializeField] private float bossAtRemainingTime = 20f;
    [SerializeField] private bool stopNormalSpawnsWhenBossAppears = true;

    [Header("Level Times (seconds from game start)")]
    [SerializeField] private float level1EndAt = 60f;
    [SerializeField] private float level2EndAt = 150f;
    [SerializeField] private float level3EndAt = 99999f;

    private float spawnCounter = 0.5f;
    private bool bossSpawned = false;

    void Update()
    {
        if (GameController.Instance != null && GameController.Instance.IsGameOver())
            return;

        if (!bossSpawned && ShouldSpawnBoss())
        {
            SpawnBoss();
            if (stopNormalSpawnsWhenBossAppears) return;
        }

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0f)
        {
            spawnCounter = 0.5f;
            var prefab = CurrentEnemy();
            if (prefab != null)
            {
                Instantiate(prefab, SelectSpawnPoint(), Quaternion.identity);
            }
        }
    }

    private bool ShouldSpawnBoss()
    {
        return bossPrefab != null &&
               GameController.Instance != null &&
               GameController.Instance.RemainingTime <= bossAtRemainingTime;
    }

    private void SpawnBoss()
    {
        // AudioManger.Instance.Play(AudioManager.AudioType.Background_Boss_Music);
        bossSpawned = true;
        Instantiate(bossPrefab, SelectSpawnPoint(), Quaternion.identity);
    }

    private float Now()
    {
        return GameController.Instance != null
            ? GameController.Instance.ElapsedTime
            : Time.timeSinceLevelLoad;
    }

    private GameObject CurrentEnemy()
    {
        float t = Now();
        if (t < level1EndAt) return level1Enemy;
        else if (t < level2EndAt) return level2Enemy;
        else if (t < level3EndAt) return level3Enemy;
        else return level3Enemy;
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        spawnPoint.y = 1;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.z = Random.Range(minSpawn.position.z, maxSpawn.position.z);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }
            else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.z = maxSpawn.position.z;
            }
            else
            {
                spawnPoint.z = minSpawn.position.z;
            }
        }
        return spawnPoint;
    }
}
