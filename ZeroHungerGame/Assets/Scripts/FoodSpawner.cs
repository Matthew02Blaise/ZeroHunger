using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script spawns food items at regular intervals in random lanes at the top of the screen
public class FoodSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] foodPrefabs;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Lane Settings")]
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private float spawnY = 6f;

    private float timer;

    private void Update()
    {
        if (foodPrefabs == null || foodPrefabs.Length == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnFood();
            timer = 0f;
        }
    }

    // Spawns a food item in a random lane at the top of the screen
    private void SpawnFood()
    {
        int lane = Random.Range(0, 3);
        float spawnX = (lane - 1) * laneDistance;

        int foodIndex = Random.Range(0, foodPrefabs.Length);
        GameObject selectedFood = foodPrefabs[foodIndex];

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Instantiate(selectedFood, spawnPosition, Quaternion.identity);

        // ** added for testing*** can only be seen in unity editor
        Debug.Log("Spawned food index: " + foodIndex);
    }
}
