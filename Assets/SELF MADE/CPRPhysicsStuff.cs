using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRPhysicsStuff : MonoBehaviour
{
    private int score = 0;
    private float timer = 0.0f;
    private List<string> mistakes = new List<string>();
    private bool isAlive = true;

    private BreathingState breathingState = BreathingState.Breathing;
    private float handDist = 0.0f;
    private float handToCubeDist = 0.0f;
    private float handToCubeVerticalDist = 0.0f;
    private float prevVerticalDist = 0.0f;


    private float verticalVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region update goofies from GameManager
        score = GameManager.Instance.score;
        timer = GameManager.Instance.timer;
        mistakes = GameManager.Instance.mistakes;
        isAlive = GameManager.Instance.isAlive;
        breathingState = GameManager.Instance.breathingState;
        handDist = GameManager.Instance.handDist;
        handToCubeDist = GameManager.Instance.handToCubeDist;
        handToCubeVerticalDist = GameManager.Instance.handToCubeVerticalDist;
        #endregion
        verticalVelocity = (handToCubeVerticalDist - prevVerticalDist)/Time.deltaTime;
        prevVerticalDist = handToCubeVerticalDist;
    }
}
