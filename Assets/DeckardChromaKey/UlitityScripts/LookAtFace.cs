using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtFace : MonoBehaviour
{


    public Transform lookAt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.LookAt(lookAt);
    }
}
