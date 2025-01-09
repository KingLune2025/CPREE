using DoorScript;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkBreathing : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            float a = Random.value;
            Debug.Log("right click was pressed");
            if (a <= 0.70)
            {
                text.color = Color.red;
                text.text = "Not Breathing";
            }
            else if (a <= 0.95)
            {
                text.color = Color.yellow;
                text.text = "Abnormal Breathing";
            }
            else
            {
                text.color = Color.green;
                text.text = "Breathing normally";
            }
        }
    }
}
