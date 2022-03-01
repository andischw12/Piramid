using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetector : MonoBehaviour
{
    public int Damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>()) 
        {
            PlayerManager.instance.ChangeHealth(-Damage);
        }
    }
}
