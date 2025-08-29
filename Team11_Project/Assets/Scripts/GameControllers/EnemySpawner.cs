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
    [SerializeField] private bool stopNormalSpawnsWhenBossAppears = true;

    [Header("Boss Timing")]
    [Tooltip("When to start boss loop (seconds from game start). Set to 240 for 4 minutes.")]
    [SerializeField] private float level3EndAt = 240f;
    [Tooltip("Interval between boss spawns once the boss phase starts.")]
    [SerializeField] private float bossSpawnInterval = 10f;

    [Header("Level Times (seconds from game start)")]
    [SerializeField] private float level1EndAt = 60f;
    [SerializeField] private float level2EndAt = 150f;
    // level3EndAt is reused as the end of level 3

    private float spawnCounter = 0.5f;
    private bool bossSpawned = false;      // true after first boss (for music + optional stop)
    private bool bossLoopStarted = false;  // ensures we start the loop once

    void OnDisable()
    {
        CancelInvoke(nameof(SpawnBoss));
    }

    void Update()
    {
        if (GameController.Instance != null && GameController.Instance.IsGameOver())
            return;

        // Start the repeating boss spawns once level 3 has ended (e.g., at 240s)
        if (!bossLoopStarted && Now() >= level3EndAt)
        {
            StartBossLoop();
        }

        // Stop normal enemy spawns after the first boss appears, if desired
        if (stopNormalSpawnsWhenBossAppears && bossSpawned)
            return;

        // Normal enemy spawn loop
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

    private void StartBossLoop()
    {
        if (bossPrefab == null) return;
        bossLoopStarted = true;
        // Start immediately, then repeat every bossSpawnInterval seconds
        InvokeRepeating(nameof(SpawnBoss), 0f, bossSpawnInterval);
    }

    private void SpawnBoss()
    {
        if (GameController.Instance != null && GameController.Instance.IsGameOver())
            return;

        if (!bossSpawned)
        {
            AudioManager.Instance.PlayMusic(AudioManager.AudioType.Background_Boss_Music);
            bossSpawned = true;
        }

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
        else return level3Enemy; // keep your fallback the same
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        spawnPoint.y = 1;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.z = Random.Range(minSpawn.position.z, maxSpawn.position.z);
            spawnPoint.x = (Random.Range(0f, 1f) > .5f) ? maxSpawn.position.x : minSpawn.position.x;
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);
            spawnPoint.z = (Random.Range(0f, 1f) > .5f) ? maxSpawn.position.z : minSpawn.position.z;
        }
        return spawnPoint;
    }
}
