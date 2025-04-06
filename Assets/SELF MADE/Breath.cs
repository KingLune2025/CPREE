using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random=System.Random;

public class Breath : MonoBehaviour
{
    public Transform vrHeadset;
    public Transform militarydude;
    public TextMeshProUGUI distanceText;
    private bool breathChecked = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (vrHeadset == null || militarydude == null || distanceText == null)
        {
            Debug.LogError("References are missing!");
            return;
        }
        float distance = Vector3.Distance(vrHeadset.position, militarydude.position);
        if (distance < 1.8 && !breathChecked)
        {
            var random = new Random();
            double chance = random.Next(1,10);
            if (chance < 2)
                GameManager.Instance.setBreathingState(BreathingState.Breathing);
            else if (chance < 7)
                GameManager.Instance.setBreathingState(BreathingState.AbnormalBreathing);
            else
                GameManager.Instance.setBreathingState(BreathingState.NotBreathing);

            breathChecked = true;
        }
    }
}
