using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Breath : MonoBehaviour
{
    public Transform vrHeadset;
    public Transform militarydude;
    public TextMeshPro distanceText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vrHeadset == null || militarydude == null || distanceText == null)
        {
            Debug.LogError("References are missing!");
            return;
        }
        float distance = Vector3.Distance(vrHeadset.position, militarydude.position);
        if (distance < 0.3048)
        {
            distanceText.text = distance + "Breathing";
        }
        else
        {
            distanceText.text = distance + "Not close enough";
        }
    }
}
