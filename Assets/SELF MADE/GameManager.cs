using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Instance (without private set)
    public static GameManager Instance;
    public TextMeshProUGUI text;

    public int score = 0;
    public float timer = 0.0f;
    public List<string> mistakes = new List<string>();
    public bool isAlive = true;

    public BreathingState breathingState = BreathingState.Breathing;
    public float handDist = 0.0f;
    public float handToCubeDist = 0.0f;
    public float handToCubeVerticalDist = 0.0f;

    static float maxDepth = 0.3f;
    static float leeway = maxDepth * 0.3f; // 30% leeway
    static float minAllowedDepth = maxDepth - leeway;
    static float maxAllowedDepth = maxDepth + leeway;

    static float prevDepth = 0.0f;
    static bool isCompressing = false;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    } // Sets singleton
    #region setters
    public void setScore(int value)
    {
        score = value;
        Debug.Log("Updated score: " + score);
    }
    public void setTimer(float value)
    {
        timer = value;
        Debug.Log("Updated timer: " + timer);
    }
    public void incrementMistakes(string mistake)
    {
        mistakes.Add(mistake);
    }
    public void resetMistakes()
    {
        mistakes = new List<string>();
        Debug.Log("reset mistakes");
    }
    public void SetAliveState(bool value)
    {
        isAlive = value;
        Debug.Log("Updated alive to: " + isAlive);
    }
    public void setBreathingState(BreathingState s)
    {
        breathingState = s;
    }
    public void setHandsDist(float dist)
    {
        handDist = dist;
    }
    public void setHandCubeDist(float dist)
    {
        handToCubeDist = dist;
    }
    public void setVertDist(float dist)
    {
        handToCubeVerticalDist = dist;
    }
    #endregion 

    private void Update()
    {
        if (handToCubeDist < 0.4 && handDist < 0.1)
        {
            text.text = ("Hand Distance: " + handDist + "\nvert dist" + handToCubeVerticalDist + "reg dist: " + handToCubeDist);
            Debug.Log("vert dist: " + handToCubeVerticalDist);
            Debug.Log("prev depth: " + prevDepth);

            if (handToCubeVerticalDist < prevDepth) // moving down
            {
                isCompressing = true;
                Debug.Log("Moving down!");
            }
            else if (handToCubeVerticalDist > prevDepth && isCompressing) // moving up after compression
            {
                Debug.Log("Moving up!");
                if (handToCubeVerticalDist >= minAllowedDepth && handToCubeVerticalDist <= maxAllowedDepth)
                {
                    Debug.Log("Compression Successful!");
                    score++;
                }
                else if (handToCubeVerticalDist > maxAllowedDepth)
                {
                    Debug.Log("Compression too deep!");
                }
                else
                {
                    Debug.Log("Compression too shallow!");
                }

                isCompressing = false;
            }

            // Update prevDepth *after* checking conditions
            prevDepth = handToCubeVerticalDist;
        }
    }

}

public enum BreathingState
{
    Breathing,
    AbnormalBreathing,
    NotBreathing
}

