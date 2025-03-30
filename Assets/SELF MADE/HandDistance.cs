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
        GameManager.Instance.setHandsDist(Vector3.Distance(rHand.position, lHand.position));
    }
}