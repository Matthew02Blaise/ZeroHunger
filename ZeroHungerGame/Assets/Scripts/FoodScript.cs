using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
