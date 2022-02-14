using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>()) 
        {
            PlayerManager.instance.Die();
        }
    }
}
