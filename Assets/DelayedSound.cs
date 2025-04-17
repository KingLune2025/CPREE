using UnityEngine;

public class DelayedSound : MonoBehaviour
{
    public AudioSource soundEffect; 

    void Start()
    {
        Invoke("PlaySound", 10f);//second counter for hs bofy
    }

    void PlaySound()
    {
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
    }
}

