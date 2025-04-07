using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.Controls;
using System.Threading;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor interactor;
    private bool isInteractable;
    private InputData InputData;
    private bool conversationStarted = false;
    double timer = 0;
    bool timedOut = false;
    bool inMenu = false;
    void Start()
    {
        if (interactor != null)
        {
            interactor.hoverEntered.AddListener(OnHoverEnter);
            interactor.hoverExited.AddListener(OnHoverExit);
        }
        else
        {
            Debug.LogError("XRRayInteractor is not assigned to ConversationStarter.");
        }

        GameObject myXROrigin = GameObject.Find("XR Origin");
        if (myXROrigin != null)
        {
            InputData = myXROrigin.GetComponent<InputData>();
        }
    }

    public void testHoverEnter()
    {
        Debug.Log("I got hovered YAY");
        isInteractable = true;
    }

    public void testHoverExit()
    {
        Debug.Log("I lost my hover noo");
        isInteractable = false;
        conversationStarted = false; // Reset when leaving
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {

    }

    private void OnHoverExit(HoverExitEventArgs args)
    {

    }

    void Update()
    {
        if (InputData == null || InputData.RController == null) return;

        bool triggerPressed = false;
        InputData.RController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed);

        if (isInteractable && triggerPressed && !conversationStarted)
        {
            ConversationManager.Instance.StartConversation(myConversation);
            conversationStarted = true; // Prevents immediate retriggering
            inMenu = true; // Set inMenu to true when conversation starts
        }
       
        
        //Conversation Choosing Options
        Vector2 vector2 = Vector2.zero;
        InputData.LController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out vector2);
        
        if (vector2.y > 0.9f && inMenu && !timedOut)
        {
            Debug.Log("Up");
            timedOut = true;
            ConversationManager.Instance.SelectPreviousOption();
        }
        else if (vector2.y < -0.9f && inMenu && !timedOut)
        {
            Debug.Log("Down");
            timedOut = true;
            ConversationManager.Instance.SelectNextOption();
        }
        if (triggerPressed && inMenu)
        {
            Debug.Log("Select");
            timedOut = true;
            ConversationManager.Instance.PressSelectedOption();
        }
        if (timedOut)
        {
            timer++;
            if (timer > 8)
            {
                timedOut = false;
                timer = 0;
            }
        }
    }
}