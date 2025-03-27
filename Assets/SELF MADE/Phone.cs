using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Phone : MonoBehaviour
{
    public XRRayInteractor interactor;
    private bool isInteractable;
    private InputData InputData;
    private bool timedOut = false;
    int timer = 0;
    public Transform phone;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        interactor.hoverEntered.AddListener(OnHoverEnter);
        interactor.hoverExited.AddListener(OnHoverExit);
        GameObject myXROrigin = GameObject.Find("XR Origin");
        InputData = myXROrigin.GetComponent<InputData>();
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        isInteractable = true;
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        isInteractable = false;
    }
        // Update is called once per frame
        void Update()
    {
        bool test;
        InputData.RController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out test);
            
        if (isInteractable && test && !timedOut)
        {
            text.text = "Calling...";
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
