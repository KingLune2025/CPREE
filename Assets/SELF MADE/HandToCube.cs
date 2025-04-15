using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HandToCube : MonoBehaviour
{
    public Transform Cube, rHand, lHand;
    private float updateTimer = 0f;
    private float updateInterval = 0.1f;

    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;

        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;

            float lHandtoCube = Mathf.Round(Vector3.Distance(lHand.position, Cube.position) * 100) / 100;
            float rHandtoCube = Mathf.Round(Vector3.Distance(rHand.position, Cube.position) * 100) / 100;
            float zLDistance = (lHand.position - Cube.position).z;
            float zRDistance = (rHand.position - Cube.position).z;

            GameManager.Instance.setHandCubeDist(Vector3.Distance(rHand.position, Cube.position));
            GameManager.Instance.setVertDist((rHand.position - Cube.position).y);
        }
    }
}
