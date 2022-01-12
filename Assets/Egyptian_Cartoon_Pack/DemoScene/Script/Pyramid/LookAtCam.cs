using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {



    Camera cameraToLookAt;

    // Use this for initialization 
    void Start()
    {
        cameraToLookAt = Camera.main;

    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.LookAt(new Vector3(cameraToLookAt.transform.position.x, transform.position.y, cameraToLookAt.transform.position.z));
         

    }

}
