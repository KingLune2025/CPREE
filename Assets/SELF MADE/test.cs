using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class test : MonoBehaviour
{
    public InputDevice RController;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!RController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller, ref RController);
        }
        float triggerValue = 0;
         RController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue);
        text.text = "" + triggerValue;
    }

    private void InitializeInputDevice(InputDeviceCharacteristics characteristics, ref InputDevice inputDevice)
    {
        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristicsR = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristicsR, rightHandedControllers);

        foreach (var device in rightHandedControllers)
        {
            RController = device;

            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        }
    }
    }
