using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor interactor;
    private bool isInteractable;
    private InputData InputData;
    private bool conversationStarted = false;

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
        }
    }
}