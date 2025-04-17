using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Person : MonoBehaviour
{
    private InputData InputData;
    private bool isHovered = false;

    // Reference to the movement script to disable the movement controls

    void Start()
    {
        GameObject myXROrigin = GameObject.Find("XR Origin");
        if (myXROrigin != null)
        {
            InputData = myXROrigin.GetComponent<InputData>();
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
    }

    public void OnRelease()  // When the interaction ends, restore things
    {
        Debug.Log("Object released!");
    }
}
