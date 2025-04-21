using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DialogueEditor;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    // Singleton Instance (without private set)
    public static GameManager Instance;
    public TextMeshProUGUI text;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI speedText;

    // score
    public int score = 0;
    public int compressions = 0;
    public int trueDepths = 0;
    public float accuracyPos = 0.0f;

    public float timer = 0.0f;
    public float CPRtimer = 0.0f; 
    public List<string> mistakes = new List<string>();
    public bool isAlive = true;

    public BreathingState breathingState = BreathingState.None;
    public float handDist = 0.0f;
    public float handToCubeDist = 0.0f;
    public float handToCubeVerticalDist = 0.0f;

    static float lowerBound = -0.06f; // ADJUSTABLE: max depth of CPR
    static float upperBound = 0.06f; // ADJUSTABLE: min depth of CPR

    static bool isCompressing = false;
    static float movementThreshold = 0.01f;
    static float prevDepth = 0f;
    static float compressionStartTime = 0f;


    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI BreathingText;

    public bool CPRMeasuringStarted = false;


    public TextMeshProUGUI scoreText;
    public AudioSource soundEffect;
    private InputData InputData;
    public XROrigin player;
    bool tutorialActive = true;
    public bool inEndGame = false;
    public Transform camera;
    private float compressionTimer = 0f; 
    public Canvas Conversation;
    public UnityEngine.Object endGameButton;
    public TextMeshProUGUI endScreenText;
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

        if (player != null)
        {
            InputData = player.GetComponent<InputData>();
        }

    } // Sets singleton
    #region setter
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
        if (CPRMeasuringStarted) {
            CPRtimer += Time.deltaTime;
        }
        Debug.Log("CPRTimer: " + CPRtimer);
        timer += Time.deltaTime;
        compressionTimer += Time.deltaTime;
        if (CPRMeasuringStarted && handToCubeDist < 0.4f && handDist < 0.1f)
        {
            text.text = "CPR Active! motion: " + (isCompressing ? "down" : "up");

            if (handToCubeVerticalDist < prevDepth) // Moving Down
            {
                if (!isCompressing) { // Moving down AFTER reaching peak up
                    if (compressionTimer <= 0.7f && compressionTimer >= 0.4f) speedText.text = "Compression good speed!";
                    else if (compressionTimer < 0.4f) speedText.text = "Too Fast!!!!";
                    else speedText.text = "Too Slow!";

                    compressionTimer = 0f;
                }
                isCompressing = true;
            }
            else if (handToCubeVerticalDist > prevDepth && isCompressing) // Moving Up
            {
                // accuracy = accuracy from center
                accuracyPos += 1 - Mathf.Abs((handToCubeDist-Mathf.Abs(handToCubeVerticalDist))/handToCubeDist);
                Debug.Log("AccuracyPos: " + accuracyPos);
                Debug.Log("Accuracy: " + (accuracyPos/compressions * 100));
                Debug.Log("Compressions: " + compressions);

                // depth
                if (handToCubeVerticalDist >= lowerBound && handToCubeVerticalDist <= upperBound)
                {
                    Debug.Log("Compression Successful!");
                    statusText.text = "Compression Successful";
                    trueDepths++;
                }
                else if (handToCubeVerticalDist > upperBound)
                {
                    Debug.Log("Compression too shallow!");
                    statusText.text = "Compression too shallow!";
                }
                else
                {
                    Debug.Log("Compression too deep!");
                    statusText.text = "Compression too deep!";
                }
                
                compressions++;
                isCompressing = false;
            }

            prevDepth = handToCubeVerticalDist;
        }

        //Tutorial Text
        if (timer > 10 && tutorialActive) 
        {
            tutorialText.enabled = false;
            timer = 0;
            soundEffect.Play();
            tutorialActive = false;
        }

        //Breathing
        if (breathingState == BreathingState.None)
            BreathingText.enabled = false;
        else
        {
            BreathingText.enabled = true;
            BreathingText.text = (breathingState.ToString());
        }

        
        bool leftTriggerPressed = false;
        InputData.LController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTriggerPressed);
        if (leftTriggerPressed || inEndGame)
        {
            inEndGame = true;
        }
        Debug.Log("left trigger pressed: " + leftTriggerPressed);

        if (inEndGame)
        {
            endGame();
            inEndGame = false;
        }
    }


    public void endGame()
    {
        player.transform.position = new Vector3(-30, 0.975f, 2);
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        camera.transform.rotation = Quaternion.Euler(0, 180, 0);
        Conversation.enabled = false;
        endGameButton.IsDestroyed();
        endScreenText.text = "Game Complete \n Mistakes- \n Conversation Mistakes: \n";
        int a = 1;
        foreach (String mistake in mistakes)
        {
            endScreenText.text = endScreenText.text + a + ". " + mistake + "\n";
        }
        endScreenText.text = endScreenText.text + "CPR Mistakes: \n";
        if (accuracyPos/compressions < 0.85)
        {
            endScreenText.text = endScreenText.text + "1. Position of hands on chest was incorrect \n";
        }
        endScreenText.text = endScreenText.text + "Score: " + score + "\n";
        endScreenText.text = endScreenText.text + "Accuracy: " + ((accuracyPos/compressions)*100) + "\n";
        endScreenText.text = endScreenText.text + "Depth: " + ((trueDepths/compressions)*100) + "\n";
        endScreenText.text = endScreenText.text + "BPM: " + ((compressions/CPRtimer)*60) + "\n";
    }
}

public enum BreathingState
{
    None,
    Breathing,
    AbnormalBreathing,
    NotBreathing
}





