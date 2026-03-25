using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings")]
    public float laneDistance = 2f;   // Distance between lanes
    public float laneChangeSpeed = 10f;
    private int currentLane = 1;        // 0 = left, 1 = middle, 2 = right

    private BoxCollider2D boxCollider;

    private Vector3 targetPosition;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        HandleLaneMovement();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(-1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveLane(1);
        }
    }

    void MoveLane(int direction)
    {
        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, 0, 2);

        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    void HandleLaneMovement()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetPosition.x, laneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
    }
}
