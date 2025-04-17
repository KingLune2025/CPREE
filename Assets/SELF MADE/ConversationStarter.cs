using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.Controls;
using System.Threading;
using TMPro;

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
    bool isPaused = false; // Variable to track if the conversation is paused
    public TextMeshProUGUI pauseConv;

    public GameManager GameManager;

    bool stopper = false;
    bool stopper2 = false;
    void Start()
    {
        if (interactor != null)
        {
            interactor.hoverEntered.AddListener(OnHoverEnter);
            interactor.hoverExited.AddListener(OnHoverExit);
        }
        else
        {
            //Debug.LogError("XRRayInteractor is not assigned to ConversationStarter.");
        }

        GameObject myXROrigin = GameObject.Find("XR Origin");
        if (myXROrigin != null)
        {
            InputData = myXROrigin.GetComponent<InputData>();
        }
    }

    public bool getConversationStarted(){
        return conversationStarted;
    }

    public void testHoverEnter()
    {
        //Debug.Log("I got hovered YAY");
        isInteractable = true;
    }

    public void testHoverExit()
    {
        //Debug.Log("I lost my hover noo");
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
        //Debug.Log("In Menu: " + inMenu);

        Vector2 vector2 = Vector2.zero;
        InputData.LController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out vector2);
        //Debug.Log(vector2);
        bool canChange = true;
        
        if (vector2.y > 0.9f && inMenu && !timedOut)
        {
            //Debug.Log("Up");
            timedOut = true;
            ConversationManager.Instance.SelectPreviousOption();
        }
        else if (vector2.y < -0.9f && inMenu && !timedOut)
        {
            //Debug.Log("Down");
            timedOut = true;
            ConversationManager.Instance.SelectNextOption();
        }
        if (triggerPressed && inMenu && !isPaused)
        {
            //Debug.Log("Select");
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
        

        //Pausing Conversation
        if (ConversationManager.Instance.DialogueText.text == "The ambulance will be here in a few seconds. Just hang in there." && !stopper)
        {
            stopper = true;
            isPaused = true;
            Debug.Log("Conversation Paused");
            pauseConv.text = "Conversation Paused.";

        }
        //Debug.Log(GameManager.CPRtimer);
        if (GameManager.CPRtimer > 300)
        {
            isPaused = false;
            Debug.Log("Conversation Unpaused");
            pauseConv.text = "Conversation Unpaused.";
        }


        if (GameManager.timer <= 15 && inMenu && !stopper2)
        {
            GameManager.score += 1;
            stopper2 = true;
        }
    }
}