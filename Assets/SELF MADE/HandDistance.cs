using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HandDistance : MonoBehaviour
{

    public Transform rHand, lHand;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = Mathf.Round(Vector3.Distance(rHand.position, lHand.position) * 100)/100 + ((Vector3.Distance(rHand.position, lHand.position)) < 0.07f ? "Yay you can do cpr" : "Clap them chee- I mean hands");
    }
}
