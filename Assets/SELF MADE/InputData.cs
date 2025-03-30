using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class InputData : MonoBehaviour
{
    public InputDevice RController;
    public InputDevice LController;


    // Update is called once per frame
    void Update()
    {
        if (!RController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref RController);
        }
        if (!LController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref LController);
        }

    }

    private void InitializeInputDevice(InputDeviceCharacteristics characteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
            Debug.Log($"Found input device: {RController.name} with characteristics: {characteristics}");
        }
    }
}
