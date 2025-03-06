using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class InputData : MonoBehaviour
{
    public InputDevice RController;
    public InputDevice LController;
    
    // Start is called before the first frame update
    void Start()
    {

    }

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
            RController = devices[0];
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", RController.name, RController.characteristics.ToString()));
        }
    }
}
