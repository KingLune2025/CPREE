using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HandDistance : MonoBehaviour
{

    public Transform rHand, lHand;
    private float updateTimer = 0f;
    private float updateInterval = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;

        if (updateTimer >= updateInterval)
        {
            GameManager.Instance.setHandsDist(Vector3.Distance(rHand.position, lHand.position));
        }
    }
}