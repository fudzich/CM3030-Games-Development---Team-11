using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoDamage : MonoBehaviour
{
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private GameObject tornadoFirePrefab;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float damageInterval = 0.5f;
    private float damageIncreasePerFireball = 5f;
    private Vector3 scaleIncreasePerFireball = Vector3.one * 0.1f;


    private List<EnemyCollision> enemiesInRange = new List<EnemyCollision>();

    void Start()
    {
        StartCoroutine(ApplyDamageRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyCollision enemy = other.GetComponent<EnemyCollision>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }

        if (other.CompareTag("FireBall"))
        {
            if (tornadoFirePrefab != null && !tornadoFirePrefab.activeSelf)
            {
                tornadoPrefab.SetActive(false);
                tornadoFirePrefab.SetActive(true);
            }
            else
            {
                upgradeTornado();
            }
        }
    }
    private void upgradeTornado()
    {
        damagePerSecond += damageIncreasePerFireball;
        transform.localScale += scaleIncreasePerFireball;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyCollision enemy = other.GetComponent<EnemyCollision>();
            if (enemy != null)
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    private IEnumerator ApplyDamageRoutine()
    {
        while (true)
        {
            foreach (EnemyCollision enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    enemy.receiveDamge(damagePerSecond * damageInterval);
                    Debug.Log("Tornado damaged enemy! Applied damage: " + (damagePerSecond * damageInterval));
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        enemiesInRange.Clear();
    }
}