using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        text.text = "Right hand distance: " + Mathf.Round(Vector3.Distance(rHand.position, Cube.position) * 100) / 100 + ((Vector3.Distance(rHand.position, Cube.position))) + "Left hand Distance: " + Mathf.Round(Vector3.Distance(lHand.position, Cube.position) * 100) / 100 + ((Vector3.Distance(lHand.position, Cube.position)));
    }
}
