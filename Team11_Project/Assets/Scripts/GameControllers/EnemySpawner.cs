using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;

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
    [SerializeField] private float level3EndAt = 240f;
    [SerializeField] private float bossSpawnInterval = 10f;

    [Header("Level Times (seconds from game start)")]
    [SerializeField] private float level1EndAt = 60f;
    [SerializeField] private float level2EndAt = 150f;

    // ---------- No-Spawn Area Rejection ----------
    [Header("No-Spawn Area (use Layer and/or Tag)")]
    [FormerlySerializedAs("useNonSpwaningLayer")]
    [SerializeField] private bool useNonSpawningLayer = true;

    [FormerlySerializedAs("NonSpwaningLayer")]
    [SerializeField] private LayerMask nonSpawningLayer = 0;

    [FormerlySerializedAs("useNonSpwaningTag")]
    [SerializeField] private bool useAreaTag = true;

    // keep the same field name so your Inspector value is retained
    [SerializeField] private string areaTag = "NonSpawningArea";

    [Header("Ground Probing")]
    [SerializeField] private float rayStartOffset = 25f;       // start ray this high above highest corner
    [SerializeField] private float raycastDownDistance = 200f; // how far down to look for ground
    [SerializeField] private float spawnYOffset = 0f;          // place pivot this far above ground
    [SerializeField] private float spawnCounter = 0.5f;

    [Header("Footprint")]
    [SerializeField] private float defaultFootprintRadius = 0.7f; // used if prefab has no collider we can read
    [SerializeField] private float mudEdgeClearance = 0.08f;      // buffer from edge

    [Header("Attempts / Retries")]
    [SerializeField] private int maxPreSpawnAttempts = 60;     // tries to find a clean point before spawn
    [SerializeField] private int postSpawnRetryBudget = 3;     // destroy & retry if bad

    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;
    [SerializeField] private bool debugGizmos = false;

    [Header("NavMesh")]
    [SerializeField] private bool requireOnNavMesh = true;
    [SerializeField] private float navMeshMaxSnapDistance = 2f;

    private bool bossSpawned = false;
    private bool bossLoopStarted = false;

    private Dictionary<GameObject, float> _radiusCache = new Dictionary<GameObject, float>();

    void OnDisable()
    {
        CancelInvoke("SpawnBoss");
    }

    void OnValidate()
    {
        // Try your new layer first, then fall back to "Mud" if not found
        if (nonSpawningLayer.value == 0)
            nonSpawningLayer = LayerMask.GetMask("NonSpawningArea", "Mud");

        rayStartOffset = Mathf.Max(1f, rayStartOffset);
        raycastDownDistance = Mathf.Max(5f, raycastDownDistance);
        maxPreSpawnAttempts = Mathf.Max(1, maxPreSpawnAttempts);
        postSpawnRetryBudget = Mathf.Clamp(postSpawnRetryBudget, 0, 10);
        defaultFootprintRadius = Mathf.Max(0.05f, defaultFootprintRadius);
        mudEdgeClearance = Mathf.Clamp(mudEdgeClearance, 0f, 1f);

        if (string.IsNullOrEmpty(areaTag))
            areaTag = "NonSpawningArea";
    }

    void Update()
    {
        if (GameController.Instance != null && GameController.Instance.IsGameOver())
            return;

        if (!bossLoopStarted && Now() >= level3EndAt)
            StartBossLoop();

        if (stopNormalSpawnsWhenBossAppears && bossSpawned)
            return;

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0f)
        {
            spawnCounter = 0.5f;
            GameObject prefab = CurrentEnemy();
            if (prefab != null)
                SpawnWithValidation(prefab, postSpawnRetryBudget);
        }
    }

    // ---------------- Boss ----------------

    private void StartBossLoop()
    {
        if (bossPrefab == null) return;
        bossLoopStarted = true;
        InvokeRepeating("SpawnBoss", 0f, bossSpawnInterval);
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

        if (bossPrefab != null)
            SpawnWithValidation(bossPrefab, postSpawnRetryBudget);
    }

    public void SpawnAgain(GameObject prefab)
    {
        SpawnWithValidation(prefab, postSpawnRetryBudget);
    }

    // ---------------- Core spawn pipeline ----------------

    private void SpawnWithValidation(GameObject prefab, int retriesLeft)
    {
        Vector3 pos;
        bool found = FindCleanSpawnPoint(prefab, out pos);
        if (!found)
        {
            if (debugLogs) Debug.LogWarning("[Spawner] No clean pre-spawn point found.");
            return;
        }

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        StartCoroutine(PostSpawnValidate(go, prefab, retriesLeft));
    }

    private IEnumerator PostSpawnValidate(GameObject go, GameObject prefab, int retriesLeft)
    {
        yield return null; // let physics settle one frame
        if (go == null) yield break;

        float radius = GetPrefabFootprintRadius(prefab);
        if (IsInNoSpawnAreaAtPosition(go.transform.position, radius))
        {
            if (retriesLeft > 0)
            {
                if (debugLogs) Debug.LogWarning("[Spawner] Spawn landed in a No-Spawn area. Destroying & retrying...");
                Destroy(go);
                SpawnWithValidation(prefab, retriesLeft - 1);
            }
            else
            {
                if (debugLogs) Debug.LogWarning("[Spawner] Out of retries; destroying bad spawn.");
                Destroy(go);
            }
        }
    }

    private bool FindCleanSpawnPoint(GameObject prefab, out Vector3 result)
    {
        float radius = GetPrefabFootprintRadius(prefab);

        for (int i = 0; i < maxPreSpawnAttempts; i++)
        {
            Vector3 xz = PickEdgeXZ();
            RaycastHit hit;
            if (!TryProjectToGround(xz, out hit)) continue;

            Vector3 candidate = hit.point + Vector3.up * spawnYOffset;

            if (!IsInNoSpawnAreaAtPosition(candidate, radius))
            {
                Vector3 final = candidate;
                if (!requireOnNavMesh || TrySnapToNavMesh(candidate, out final))
                {
                    result = final;
                    return true;
                }
            }
        }

        result = Vector3.zero;
        return false;
    }

    // ---------------- Time / Level helpers ----------------

    private float Now()
    {
        return (GameController.Instance != null) ? GameController.Instance.ElapsedTime : Time.timeSinceLevelLoad;
    }

    private GameObject CurrentEnemy()
    {
        float t = Now();
        if (t < level1EndAt) return level1Enemy;
        else if (t < level2EndAt) return level2Enemy;
        else if (t < level3EndAt) return level3Enemy;
        else return level3Enemy;
    }

    // ---------------- Geometry helpers ----------------

    private Vector3 PickEdgeXZ()
    {
        Vector3 p = Vector3.zero;
        bool verticalEdge = Random.value > 0.5f;

        if (verticalEdge)
        {
            p.z = Random.Range(minSpawn.position.z, maxSpawn.position.z);
            p.x = (Random.value > 0.5f) ? maxSpawn.position.x : minSpawn.position.x;
        }
        else
        {
            p.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);
            p.z = (Random.value > 0.5f) ? maxSpawn.position.z : minSpawn.position.z;
        }
        return p;
    }

    private float HighestSpawnY()
    {
        if (minSpawn == null || maxSpawn == null) return transform.position.y;
        return Mathf.Max(minSpawn.position.y, maxSpawn.position.y);
    }

    private bool TryProjectToGround(Vector3 xz, out RaycastHit hit)
    {
        Vector3 origin = new Vector3(xz.x, HighestSpawnY() + rayStartOffset, xz.z);
        return Physics.Raycast(origin, Vector3.down, out hit,
            rayStartOffset + raycastDownDistance, ~0, QueryTriggerInteraction.Collide);
    }

    // ---------------- No-Spawn detection ----------------

    private bool IsInNoSpawnAreaAtPosition(Vector3 worldPos, float footprintRadius)
    {
        RaycastHit ground;
        if (!TryGetGroundBelow(worldPos, out ground))
            return false; // no ground hit -> treat as safe (prevents lockups)

        // Centre check: is the topmost ground surface a no-spawn collider?
        if (IsNoSpawnCollider(ground.collider))
            return true;

        // Footprint check: short capsule from ground up a little bit
        float r = footprintRadius + mudEdgeClearance;
        Vector3 a = ground.point + Vector3.up * 0.02f;
        Vector3 b = a + Vector3.up * 0.5f;

        Collider[] hits = Physics.OverlapCapsule(a, b, r, ~0);
        for (int i = 0; i < hits.Length; i++)
        {
            if (IsNoSpawnCollider(hits[i])) return true;
        }

        return false;
    }

    private bool TryGetGroundBelow(Vector3 pos, out RaycastHit hit)
    {
        float up = Mathf.Max(2f, spawnYOffset + 0.5f);
        float dist = up + Mathf.Max(3f, raycastDownDistance);
        Vector3 origin = pos + Vector3.up * up;
        return Physics.Raycast(origin, Vector3.down, out hit, dist, ~0, QueryTriggerInteraction.Collide);
    }

    private bool IsNoSpawnCollider(Collider col)
    {
        if (col == null) return false;

        // Layer check
        if (useNonSpawningLayer)
        {
            int bit = 1 << col.gameObject.layer;
            if ((nonSpawningLayer.value & bit) != 0) return true;
        }

        // Tag check (walk up a few parents to be safe with child colliders)
        if (useAreaTag)
        {
            Transform t = col.transform;
            for (int i = 0; i < 4 && t != null; i++)
            {
                if (t.CompareTag(areaTag)) return true;
                t = t.parent;
            }
        }

        return false;
    }

    // ---------------- Prefab footprint radius ----------------

    private float GetPrefabFootprintRadius(GameObject prefab)
    {
        if (prefab == null) return defaultFootprintRadius;

        float cached;
        if (_radiusCache.TryGetValue(prefab, out cached))
            return cached;

        float r = defaultFootprintRadius;

        CharacterController cc = prefab.GetComponentInChildren<CharacterController>();
        if (cc != null) r = Mathf.Max(r, cc.radius);

        CapsuleCollider cap = prefab.GetComponentInChildren<CapsuleCollider>();
        if (cap != null) r = Mathf.Max(r, cap.radius);

        BoxCollider box = prefab.GetComponentInChildren<BoxCollider>();
        if (box != null) r = Mathf.Max(r, Mathf.Max(box.size.x, box.size.z) * 0.5f);

        SphereCollider sphere = prefab.GetComponentInChildren<SphereCollider>();
        if (sphere != null) r = Mathf.Max(r, sphere.radius);

        _radiusCache[prefab] = r;
        return r;
    }

    // ---------------- nav mesh helper ----------------

    private bool TrySnapToNavMesh(Vector3 pos, out Vector3 snapped)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, navMeshMaxSnapDistance, NavMesh.AllAreas))
        {
            snapped = hit.position;
            return true;
        }
        snapped = Vector3.zero;
        return false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!debugGizmos) return;

        Gizmos.color = Color.yellow;
        if (minSpawn != null && maxSpawn != null)
        {
            float y = HighestSpawnY();
            Vector3 a = new Vector3(minSpawn.position.x, y, minSpawn.position.z);
            Vector3 b = new Vector3(maxSpawn.position.x, y, minSpawn.position.z);
            Vector3 c = new Vector3(maxSpawn.position.x, y, maxSpawn.position.z);
            Vector3 d = new Vector3(minSpawn.position.x, y, maxSpawn.position.z);
            Gizmos.DrawLine(a, b); Gizmos.DrawLine(b, c); Gizmos.DrawLine(c, d); Gizmos.DrawLine(d, a);
        }
    }
#endif
}
