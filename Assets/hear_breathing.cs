using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class hear_breathing : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform SK_military_;
    public TextMeshPro distanceText;

    public hear_breathing(Transform sK_military_)
    {
        Transform sK_military_1 = sK_military_;
        SK_military_ = sK_military_1;
    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { if (xrOrigin == null || SK_military_ == null || distanceText == null)
        {
          
            
        }
        float distance = 
            Vector3.Distance(xrOrigin.position, SK_military_.position);
        string v = "Distance:" + distance.ToString("F2") + "meters";
        distanceText.text = v;
    }
}
