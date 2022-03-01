using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ap_PlayASound_Pc : MonoBehaviour
{
    private AudioSource aSource;
    private void Start()
    {
        aSource =  gameObject.AddComponent<AudioSource>();
    }

    public void AP_playASoundOneShot(AudioClip _aclip)
    {
        aSource.PlayOneShot(_aclip);
    }
}
