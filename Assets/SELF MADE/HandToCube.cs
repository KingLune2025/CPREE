using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HandToCube : MonoBehaviour
{
    public Transform Cube, rHand, lHand;
    public TextMeshProUGUI text;

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
        if (lHandtoCube < rHandtoCube)
            text.text = "Left Hand to Cube: " + lHandtoCube + "Depth: " + zLDistance;

        else
            text.text = "Right Hand to Cube: " + rHandtoCube + "\n Depth: " + zRDistance;



 }
}
