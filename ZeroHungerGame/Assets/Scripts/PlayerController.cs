using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings")]
    public float laneDistance = 2f;   // Distance between lanes
    public float laneChangeSpeed = 10f;
    private int currentLane = 1;      // 0 = left, 1 = middle, 2 = right

    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f;

    private BoxCollider2D boxCollider;
    private Vector3 targetPosition;

    private Vector2 touchStartPos;
    private bool isSwiping = false;

    [SerializeField] private AudioSource moveAudioSource;
    [SerializeField] private AudioClip moveSound;

    [SerializeField] private AudioSource pickupAudioSource;
    [SerializeField] private AudioClip goodFoodSound;
    [SerializeField] private AudioClip badFoodSound;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        HandleTouchInput();
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

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                isSwiping = true;
                break;

            case TouchPhase.Ended:
                if (!isSwiping)
                    return;

                Vector2 touchEndPos = touch.position;
                Vector2 swipeDelta = touchEndPos - touchStartPos;

                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) &&
                    Mathf.Abs(swipeDelta.x) > minSwipeDistance)
                {
                    if (swipeDelta.x > 0)
                    {
                        MoveLane(1);
                    }
                    else
                    {
                        MoveLane(-1);
                    }
                }

                isSwiping = false;
                break;
        }
    }

    void MoveLane(int direction)
    {
        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, 0, 2);

        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        if (moveAudioSource != null && moveSound != null)
        {
            moveAudioSource.PlayOneShot(moveSound);
        }
    }

    void HandleLaneMovement()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetPosition.x, laneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FoodScript food = other.GetComponent<FoodScript>();

        if (food != null)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.AddFood(food.ScoreValue, food.HungerValue);
            }

            if (pickupAudioSource != null)
            {
                if (food.HungerValue < 0)
                {
                    pickupAudioSource.PlayOneShot(badFoodSound);
                }
                else
                {
                    pickupAudioSource.PlayOneShot(goodFoodSound);
                }
            }

            Destroy(other.gameObject);
        }
    }
}