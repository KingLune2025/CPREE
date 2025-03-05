using DoorScript;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Opendoor : MonoBehaviour
{
    // Start is called before the first frame update
    public XRRayInteractor interactor;
    public TextMeshProUGUI text;
    private bool isInteractable;
    public UnityEngine.XR.InputDevice rightController;
    private InputData InputData;
    public TextMeshProUGUI text2;
    public Door door;
    private bool timedOut = false;
    int timer = 0;

    void Start()
    {
        interactor.hoverEntered.AddListener(OnHoverEnter);
        interactor.hoverExited.AddListener(OnHoverExit);
        GameObject myXROrigin = GameObject.Find("XR Origin");
        InputData = myXROrigin.GetComponent<InputData>();
        text.text = "Added lisener";
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        isInteractable = true;
       text.text = "" + isInteractable;
     
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        isInteractable = false;
        text.text = "" + isInteractable;

    }

    void Update()
    {
        if (InputData.RController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool test))
        {
            text2.text = "" + test;
        }

       if (isInteractable && test && !timedOut)
        {
            door.OpenDoor();
            timedOut = true;
        }
        if (timedOut)
        {
            timer++;
            if (timer > 60)
            {
                timedOut = false;
                timer = 0;
            }
        }
    }
}
