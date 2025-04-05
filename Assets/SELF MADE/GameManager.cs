using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton Instance (without private set)
    public static GameManager Instance;
    public TextMeshProUGUI text;

    public int score = 0;
    public float timer = 0.0f;
    public List<string> mistakes = new List<string>();
    public bool isAlive = true;

    public BreathingState breathingState = BreathingState.None;
    public float handDist = 0.0f;
    public float handToCubeDist = 0.0f;
    public float handToCubeVerticalDist = 0.0f;

    static float maxDepth = 0.35f;
    static float leeway = maxDepth * 0.5f;
    static float minAllowedDepth = maxDepth - leeway;
    static float maxAllowedDepth = maxDepth + leeway;

    static float prevDepth = 0.0f;
    static bool isCompressing = false;

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI BreathingText;
    public Button endGameButton;

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
        endGameButton.gameObject.SetActive(false);
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
        timer += Time.deltaTime;

        //CPR 
        if (handToCubeDist < 0.4 && handDist < 0.1)
        {
            //text.text = ("Hand Distance: " + handDist + "\nvert dist" + handToCubeVerticalDist + "reg dist: " + handToCubeDist + "score: " + score);

            if (handToCubeVerticalDist < prevDepth) // moving down
            {
                isCompressing = true;
            }
            else if (handToCubeVerticalDist > prevDepth && isCompressing) // moving up after compression
            {
                if (handToCubeVerticalDist >= minAllowedDepth && handToCubeVerticalDist <= maxAllowedDepth)
                {
                    Debug.Log("Compression Successful!");
                    score++;
                }
                else if (handToCubeVerticalDist > maxAllowedDepth)
                {
                    Debug.Log("Compression too deep!");
                    score--;
                }
                else
                {
                    Debug.Log("Compression too shallow!");
                    score--;
                }

                isCompressing = false;
            }

            // Update prevDepth *after* checking conditions
            prevDepth = handToCubeVerticalDist;
            }


        //Tutorial Text
        if (timer > 10)
        {
            tutorialText.enabled = false;
            endGameButton.gameObject.SetActive(true);
        }
          


        //Breathing
        if (breathingState == BreathingState.None)
            BreathingText.enabled = false;
        else
        {
            BreathingText.enabled = true;
            BreathingText.text = (breathingState.ToString());
        }

    }

}

public enum BreathingState
{
    None,
    Breathing,
    AbnormalBreathing,
    NotBreathing
}




