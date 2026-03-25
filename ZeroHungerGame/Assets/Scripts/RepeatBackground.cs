using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;

    private Vector3 startPos;
    private float repeatHeight;

    private SpriteRenderer _sr;

    private void Start()
    {
        startPos = transform.position;
        _sr = GetComponent<SpriteRenderer>();

        repeatHeight = _sr.bounds.size.y; // Use height instead of width
    }

    private void Update()
    {
        // Move background DOWN
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Reset when it goes too far down
        if (transform.position.y < startPos.y - repeatHeight)
        {
            transform.position = startPos;
        }
    }
}
