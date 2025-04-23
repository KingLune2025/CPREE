using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amboolance : MonoBehaviour
{
    public GameManager gameManager;
    public ConversationStarter conversationStarter;
    public AudioSource soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.CPRtimer > 60 && conversationStarter.isPaused)
        {
            soundEffect.Play();
        }
    }
}
