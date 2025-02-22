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

    public Door door;
    public XRRayInteractor interactor;
    public TextMeshProUGUI text;
    public bool canOpen = false;
    public InputActionProperty gripInput;

    void Start()
    {
        interactor.hoverEntered.AddListener(OnHoverEnter);
        interactor.hoverEntered.AddListener(OnHoverExit);
        text.text = "Added lisener";
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        text.text = door.open + " - DETECTED DOOR";
        canOpen = true;
    }

    private void OnHoverExit(HoverEnterEventArgs args)
    {
        text.text = door.open + " - DETECTED DOOR";
        canOpen = false;
    }

    void Update()
    {
        float grip = gripInput.action.ReadValue<float>();
        if(grip > 0 && canOpen)
        {
            door.OpenDoor();
        }
    }
}
