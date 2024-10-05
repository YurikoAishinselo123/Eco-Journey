using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishBehaviour : MonoBehaviour
{
    [Header("Rubbish Settings")]
    [SerializeField] private GameObject rubbishPrefab; // Assign rubbish prefab
    [SerializeField] private float spawnInterval = 3f; // Interval between spawns
    [SerializeField] private float xOffset = -1f; // Offset for the X axis
    public int currentRubbish;


    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnRubbish());
        currentRubbish = 1;
    }

    IEnumerator SpawnRubbish()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Calculate the spawn position with X offset
            Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, 1, transform.position.z);
            
            // Spawn rubbish at the calculated position
            Instantiate(rubbishPrefab, spawnPosition, Quaternion.identity); 
            currentRubbish = currentRubbish + 1;

        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}
