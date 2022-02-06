using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    // Start is called before the first frame update
    [SerializeField] AudioSource EffetsAudioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }


    public void PlaySound(AudioClip c) 
    {
        EffetsAudioSource.PlayOneShot(c);
    }
}
