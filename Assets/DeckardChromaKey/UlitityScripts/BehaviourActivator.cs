using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourActivator : MonoBehaviour
{

    
    public Transform trigger;
    [SerializeField]
    public float threshold = 0.2f;

    void Update()
    {
        float _distance = Vector3.Distance(gameObject.transform.position, trigger.position);
        if (_distance < threshold) {
            Debug.Log("contact");
        }

    }
}
