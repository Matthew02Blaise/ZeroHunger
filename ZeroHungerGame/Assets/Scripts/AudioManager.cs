using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the background music for the game, ensuring it persists across scenes and only one instance exists
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
