using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amboolance : MonoBehaviour
{
    public GameManager gameManager;
    public ConversationStarter conversationStarter;
    public AudioSource soundEffect;
    bool stopper = false;
    // Start is called before the first frame update
    void Start()
    {
        //soundEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.CPRtimer > 80 && !stopper)
        {
            soundEffect.Play();
            stopper = true;
        }

        if(GameManager.ugh)
        {
            soundEffect.Stop();
        }
    }
}
