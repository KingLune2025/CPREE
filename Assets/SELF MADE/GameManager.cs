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

    public TextMeshProUGUI speedText;

    // score
    public int score = 0;
    public int compressions = 0;
    public float trueDepths = 0;
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
    static List<float> lastThreeCompressionSpeeds = new List<float>();
    private List<float> lastThreeCompressionTimes = new List<float>();

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI BreathingText;

    public bool CPRMeasuringStarted = false;
    bool incrementCPR = false;
    static public bool ugh = false;


    public AudioSource soundEffect;
    private InputData InputData;
    public XROrigin player;
    bool tutorialActive = true;
    public bool inEndGame = false;

    private float compressionTimer = 0f; 
    public Canvas Conversation;

    public TextMeshProUGUI endScreenText;
    public TextMeshProUGUI depthText;
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

        if (player != null)
        {
            InputData = player.GetComponent<InputData>();
        }
        speedText.enabled = false;
        depthText.enabled = false;

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
        //Debug.Log("Updated timer: " + timer);
    }
    public void incrementMistakes(string mistake)
    {
        mistakes.Add(mistake);
    }
    public void resetMistakes()
    {
        mistakes = new List<string>();
        //Debug.Log("reset mistakes");
    }
    public void SetAliveState(bool value)
    {
        isAlive = value;
        //Debug.Log("Updated alive to: " + isAlive);
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
        if (incrementCPR) {
            CPRtimer += Time.deltaTime;
            //Debug.Log("CPR Timer: " + CPRtimer);
        }
        //Debug.Log("CPRTimer: " + CPRtimer);
        timer += Time.deltaTime;
        compressionTimer += Time.deltaTime;
        if (CPRMeasuringStarted && handToCubeDist < 0.4f && handDist < 0.1f)
        {
            incrementCPR = true;
            speedText.enabled = true;
            depthText.enabled = true;
            depthText.text = "CPR Depth: " + (Math.Round(handToCubeVerticalDist*3937.01f)/100f - 2).ToString("F2") + " in";
            if (lastThreeCompressionTimes.Count > 0)
            {
                float avgTime = 0f;
                foreach (float t in lastThreeCompressionTimes)
                {
                    avgTime += t;
                }
                avgTime /= lastThreeCompressionTimes.Count;

                float avgSpeed = 60f / avgTime;
                speedText.text = "CPR Speed: " + avgSpeed.ToString("F2") + " Avg. Compressions/Min";
            }


            if (handToCubeVerticalDist < prevDepth) // Moving Down
            {
                if (!isCompressing) { // Moving down AFTER reaching peak up
                    if (compressionTimer <= 0.7f && compressionTimer >= 0.4f) speedText.text = "Compression good speed!";
                    else if (compressionTimer < 0.4f) speedText.text = "Too Fast!";
                    else speedText.text = "Too Slow!";

                    compressionTimer = 0f;
                }
                isCompressing = true;
            }
            else if (handToCubeVerticalDist > prevDepth && isCompressing) // Moving Up
            {
                accuracyPos += 1 - Mathf.Abs((handToCubeDist - Mathf.Abs(handToCubeVerticalDist)) / handToCubeDist);

                if (handToCubeVerticalDist >= lowerBound && handToCubeVerticalDist <= upperBound)
                {
                    Debug.Log("Compression Successful!");
                    trueDepths++;
                }
                else if (handToCubeVerticalDist > upperBound)
                {
                    Debug.Log("Compression too shallow!");
                }
                else
                {
                    Debug.Log("Compression too deep!");
                }

                compressions++;

                // Record compression time
                lastThreeCompressionTimes.Add(compressionTimer*2);
                if (lastThreeCompressionTimes.Count > 2)
                {
                    lastThreeCompressionTimes.RemoveAt(0);
                }

                compressionTimer = 0f; // Reset timer for next compression
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
            ugh = true;
        }

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
        
        Conversation.enabled = false;
        
        endScreenText.text = "Game Complete \n Mistakes- \n Conversation Mistakes: \n";
        int a = 1;
        foreach (String mistake in mistakes)
        {
            endScreenText.text = endScreenText.text + a + ". " + mistake + "\n";
        }
        endScreenText.text = endScreenText.text + "CPR Mistakes: \n";
        if (accuracyPos/compressions < 0.65)
        {
            endScreenText.text = endScreenText.text + "1. Position of hands on chest was incorrect \n";
        }
        endScreenText.text = endScreenText.text + "Score: " + score + "/46\n";
        endScreenText.text = endScreenText.text + "Accuracy: " + Mathf.Round((accuracyPos/compressions)*10000)/100 + "% \n";
        endScreenText.text = endScreenText.text + "Depth: " + Mathf.Round((trueDepths/compressions)*10000)/100 + "% \n";
        endScreenText.text = endScreenText.text + "BPM: " + Mathf.Round(compressions/(CPRtimer/60)*100)/100 + "\n";
    }
}

public enum BreathingState
{
    None,
    Breathing,
    AbnormalBreathing,
    NotBreathing
}





