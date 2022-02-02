using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {



    Transform ObjectToLookAt;

    // Use this for initialization 
    void Start()
    {
        ObjectToLookAt = FindObjectOfType<PlayerManager>().transform;

    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.LookAt(new Vector3(ObjectToLookAt.transform.position.x, transform.position.y,transform.position.z));
         

    }

}
