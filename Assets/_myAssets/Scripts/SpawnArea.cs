using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Vector3 minBounds;        
    [SerializeField] private Vector3 maxBounds;
    [SerializeField] private float sightRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private int numberOfEnemies = 10;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnInterval;

    [SerializeField] private float groundRaycastDistance = 10f;
    private bool playerInSightRange;
    

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if (playerInSightRange )
            StartCoroutine(SpawnEnemiesAfterDelay());
    }

    IEnumerator SpawnEnemiesAfterDelay()
    {
        yield return new WaitForSeconds(spawnTimer);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if(playerInSightRange)
        {
            float randomX = Random.Range(minBounds.x, maxBounds.x);
            float randomZ = Random.Range(minBounds.z, maxBounds.z);

            Vector3 spawnPosition = new Vector3(randomX, 100f, randomZ);

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundRaycastDistance))
            {
                spawnPosition.y = 0f; 
            }
            else
            {
                spawnPosition.y = 0f;  
            }
            if (IsSpawnPositionClear(spawnPosition))
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                numberOfEnemies--;
            }
        }
    }
    bool IsSpawnPositionClear(Vector3 position)
    {
        float spawnRadius = 1f;

        Collider[] colliders = Physics.OverlapSphere(position, spawnRadius);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Building") || col.gameObject.CompareTag("Obstacle"))
            {
                return false; 
            }
        }
        return true;
    }
    public void OnDrawGizmos()
    {
        // Draw a green wireframe cube to visualize the spawn area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((minBounds + maxBounds) / 2, new Vector3(maxBounds.x - minBounds.x, 0f, maxBounds.z - minBounds.z));
    }
   
}
