using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class TriggerInputDetector : MonoBehaviour
{
    private InputData _inputData;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData.RController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            text.text = "" + triggerValue;
        }
    }
}
