using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles player movement between lanes and food pickup interactions
public class PlayerController : MonoBehaviour
{
    // Changable in the inspector - lane settings and swipe settings
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

    //audio sources and clips for movement and food pickup
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

    // Handle keyboard input for lane movement
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

    // Handle touch input for mobile devices
    private void HandleTouchInput()
    {
        // Only process if there's at least one touch - smooths the movement
        if (Input.touchCount == 0)
            return;

        // Get the first touch
        Touch touch = Input.GetTouch(0);

        // Check if the touch is within the player's collider
        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                isSwiping = true;
                break;

            // Only process swipe if the touch started on the player
            case TouchPhase.Ended:
                if (!isSwiping)
                    return;

                Vector2 touchEndPos = touch.position;
                Vector2 swipeDelta = touchEndPos - touchStartPos;

                // Check if the swipe is primarily horizontal and exceeds the minimum distance
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) &&
                    Mathf.Abs(swipeDelta.x) > minSwipeDistance)
                {
                    // Moves the player left and right depending on swipe direction
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

    // Move the player to the target lane and play movement sound
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

    // Smoothly move the player towards the target lane position
    void HandleLaneMovement()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetPosition.x, laneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
    }

    // Handle food pickup interactions and play corresponding sounds
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