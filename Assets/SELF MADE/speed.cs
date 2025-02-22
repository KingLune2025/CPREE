using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class speed : MonoBehaviour
{
    public Rigidbody rHand, lHand;
    public TextMeshProUGUI text;
    float lV, rV;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        lV = Vector3.Magnitude(lHand.velocity);
        rV = Vector3.Magnitude(rHand.velocity);
        text.text = lV + " a " + rV;
    }
}
