using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thud : MonoBehaviour
{
    public AudioSource thud;
    float targetTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetTime += Time.deltaTime;
        if (targetTime > 10 && targetTime < 10.5)
        {
            thud.Play();
        }
    }
}
