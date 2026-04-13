using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the behavior of food items, including movement, scoring, and hunger effects
public class FoodScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float destroyY = -7f;

    [Header("Food Values")]
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float hungerValue = 0.1f;

    [Header("Food Type")]
    [SerializeField] private bool isBadFood = false;

    public int ScoreValue => isBadFood ? 0 : scoreValue;
    public float HungerValue => isBadFood ? -hungerValue : hungerValue;

    // Move the food item downwards and destroy it if it goes off screen
    private void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
