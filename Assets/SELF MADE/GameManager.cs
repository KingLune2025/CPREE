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
    public TextMeshProUGUI statusText;

    public int score = 0;
    public float timer = 0.0f;
    public List<string> mistakes = new List<string>();
    public bool isAlive = true;

    public BreathingState breathingState = BreathingState.None;
    public float handDist = 0.0f;
    public float handToCubeDist = 0.0f;
    public float handToCubeVerticalDist = 0.0f;

    static float lowerBound = -0.09f;
    static float upperBound = 0.09f;

    static float prevDepth = 0.0f;
    static bool isCompressing = false;

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI BreathingText;

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
        Debug.Log("min depth: " + lowerBound + " | max depth" + upperBound);
     
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
            text.text = ("Hand Distance: " + Mathf.Round(handDist*100)/100 + "\nvert dist: " + Mathf.Round(handToCubeVerticalDist * 100) / 100 + "\nmotion: " + (isCompressing ? "down" : "up") + "\nscore: " + score);

            if (handToCubeVerticalDist < prevDepth) // moving down
            {
                isCompressing = true;
            }
            else if (handToCubeVerticalDist > prevDepth && isCompressing) // moving up after compression
            {
                if (handToCubeVerticalDist >= lowerBound && handToCubeVerticalDist <= upperBound)
                {
                    Debug.Log("Compression Successful!");
                    statusText.text = "Compression Successful";
                    score++;
                }
                else if (handToCubeVerticalDist > upperBound)
                {
                    Debug.Log("Compression too shallow!");
                    statusText.text = "Compression too shallow";
                    score--;
                }
                else
                {
                    Debug.Log("Compression too deep!");
                    statusText.text = "Compression too deep";
                    score--;
                }

                isCompressing = false;
            }

            // Update prevDepth *after* checking conditions
            prevDepth = handToCubeVerticalDist;
        }
        else
        {
            text.text = ("Hand to hand Distance: " + Mathf.Round(handDist * 100) / 100 + "\nvert dist" + Mathf.Round(handToCubeVerticalDist * 100) / 100 + "\nreg dist: " + Mathf.Round(handToCubeDist * 100) / 100);
        }


        //Tutorial Text
        if (timer > 10)
        {
            tutorialText.enabled = false;
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




