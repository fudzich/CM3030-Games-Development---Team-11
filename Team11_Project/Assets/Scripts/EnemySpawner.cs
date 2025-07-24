using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject enemy;

    [SerializeField]
    public float timeToSpawn;

    [SerializeField]
    public Transform minSpawn, maxSpawn;

    private float spawnCounter;

    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = timeToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;

        if (spawnCounter <= 0)
        {
            spawnCounter = timeToSpawn;
            Instantiate(enemy, SelectSpawnPoint(), transform.rotation);
        }
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
