using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Person : MonoBehaviour
{
    private Transform xrCamera; // Reference to the XR Camera (or XR Rig)
    public GameObject player;  // Reference to the player object that contains the XR Camera and controller
    private InputData InputData;

    public float heightAboveObject = 2f; // Height above the object to set the camera to

    private bool isHovered = false;

    // Reference to the movement script to disable the movement controls
    private MonoBehaviour movementScript;

    void Start()
    {
        GameObject myXROrigin = GameObject.Find("XR Origin");
        if (myXROrigin != null)
        {
            InputData = myXROrigin.GetComponent<InputData>();
            xrCamera = myXROrigin.transform.Find("Camera Offset/Main Camera"); // Adjust based on your XR Rig structure
            Debug.Log("Found XR Camera: " + xrCamera.name);
            movementScript = myXROrigin.GetComponent<CharacterControllerDriver>();
            Debug.Log("Found movement script: " + movementScript.name);
        }
    }

    void Update()
    {
        bool triggerPressed = false;
        InputData.RController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed);

        if (isHovered && triggerPressed)
        {
            OnClick();
        }
    }

    public void enter()
    {
        isHovered = true;
    }

    public void exit()
    {
        isHovered = false;
    }

    public void OnClick()
    {
        Debug.Log("Object clicked!");

        if (movementScript != null)
        {
            movementScript.enabled = false;
            Debug.Log("Movement disabled!");
        }

        if (xrCamera != null)
        {
            // Calculate offset to place camera above the object
            Vector3 offset = transform.position + Vector3.up * heightAboveObject;

            // Move the entire XR rig, not the camera!
            xrCamera.transform.position = offset;
            Debug.Log("XR Rig moved to: " + offset);

            // Optionally, rotate XR Rig to look at the object
            Vector3 lookAtDirection = transform.position - xrCamera.transform.position;
            lookAtDirection.y = 0; // Prevent tilt
            xrCamera.transform.forward = lookAtDirection.normalized;
            Debug.Log("XR Rig now facing object.");
        }
    }


    public void OnRelease()  // When the interaction ends, restore things
    {
        Debug.Log("Object released!");

        // Re-enable movement
        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
    }
}
