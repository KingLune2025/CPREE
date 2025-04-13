using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DialogueEditor;

public class GameManager : MonoBehaviour
{
    // Singleton Instance (without private set)
    public static GameManager Instance;
    public TextMeshProUGUI text;

    public int score = 0;
    public float timer = 0.0f;
    public float CPRtimer = 0.0f; 
    public List<string> mistakes = new List<string>();
    public bool isAlive = true;

    public BreathingState breathingState = BreathingState.None;
    public float handDist = 0.0f;
    public float handToCubeDist = 0.0f;
    public float handToCubeVerticalDist = 0.0f;

    static float minCompressionDepth = 0.02f; // 2 cm
    static float maxCompressionDepth = 0.06f; // 6 cm
    static float minCompressionTime = 0.2f;   // seconds
    static float maxCompressionTime = 1.0f;   // seconds

    static float movementThreshold = 0.0003f;

    bool isCompressing = false;
    float compressionStartTime = 0f;
    float compressionStartY = 0f;
    float prevVerticalDist = 0f;


    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI BreathingText;

    public bool CPRMeasuringStarted = false;

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


    public void addScore(int value)
    {
        score += value;
        Debug.Log("Updated score: " + score);
    }

    public void CPRStarts(bool value)
    {
        CPRMeasuringStarted = value;
    }

    private void Update()
    {
       
        timer += Time.deltaTime;
        if (CPRMeasuringStarted)
        {
            CPRtimer += Time.deltaTime;
        } 
        
        //CPR 
        if (handToCubeDist < 0.4f && handDist < 0.1f)
        {
            float verticalChange = handToCubeVerticalDist - prevVerticalDist;

            // Start of compression
            if (!isCompressing && verticalChange < -movementThreshold)
            {
                isCompressing = true;
                compressionStartTime = Time.time;
                compressionStartY = handToCubeVerticalDist; // how far above the chest you started
            }
            // Release / end of compression
            else if (isCompressing && verticalChange > movementThreshold)
            {
                float compressionTime = Time.time - compressionStartTime;
                float compressionDepth = Mathf.Abs(compressionStartY - handToCubeVerticalDist);

                Debug.Log($"StartY: {compressionStartY}, EndY: {handToCubeVerticalDist}, Depth: {compressionDepth}, Time: {compressionTime}");

                if (compressionDepth >= minCompressionDepth && compressionDepth <= maxCompressionDepth)
                {
                    if (compressionTime >= minCompressionTime && compressionTime <= maxCompressionTime)
                    {
                        Debug.Log("Compression Successful!");
                        tutorialText.text = "Compression Successful!";
                        score++;
                    }
                    else
                    {
                        Debug.Log("Compression speed not right!");
                        tutorialText.text = "Compression speed not right!";
                        score--;
                    }
                }
                else if (compressionDepth > maxCompressionDepth)
                {
                    Debug.Log("Too deep!");
                    tutorialText.text = "Compression too deep!";
                    score--;
                }
                else
                {
                    Debug.Log("Too shallow!");
                    tutorialText.text = "Compression too shallow!";
                    score--;
                }

                isCompressing = false;
            }

            // Update previous vertical distance
            prevVerticalDist = handToCubeVerticalDist;
        }
        else
        {
            // Hands aren't in the right position, reset compression state
            isCompressing = false;
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




