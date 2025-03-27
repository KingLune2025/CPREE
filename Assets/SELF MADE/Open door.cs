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
    private bool isInteractable;
    private InputData InputData;
    public Door door;
    private bool timedOut = false;
    int timer = 0;
    public TextMeshPro text;

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
     
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        isInteractable = false;
    }

    void Update()
    {
        if (isInteractable)
        {
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }
        bool test;
        InputData.RController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out test);

       if (isInteractable && test && !timedOut)
        {
            door.OpenDoor();
            timedOut = true;
        }
        if (timedOut)
        {
            timer++;
            if (timer > 30)
            {
                timedOut = false;
                timer = 0;
            }
        }
    }
}
