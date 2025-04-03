using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HandToCube : MonoBehaviour
{
    public Transform Cube, rHand, lHand;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float lHandtoCube = Mathf.Round(Vector3.Distance(lHand.position, Cube.position) * 100) / 100;
        float rHandtoCube = Mathf.Round(Vector3.Distance(rHand.position, Cube.position) * 100) / 100;
        float zLDistance = (lHand.position - Cube.position).z; 
        float zRDistance = (rHand.position - Cube.position).z;

        GameManager.Instance.setHandCubeDist(Mathf.Min(Vector3.Distance(rHand.position, Cube.position), Vector3.Distance(lHand.position, Cube.position)));
        GameManager.Instance.setVertDist(Mathf.Min((rHand.position - Cube.position).z, (lHand.position - Cube.position).z));
    }
}
