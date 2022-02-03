using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {



    [SerializeField]Transform ObjectToLookAt;

    // Use this for initialization 
    void Start()
    {
        ObjectToLookAt = FindObjectOfType<PlayerManager>().transform;

    }

    // Update is called once per frame 
    void Update()
    {
        gameObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z), new Vector3(0, -1, 0));
        gameObject.transform.Rotate(new Vector3(180, 0, 0));


    }

}
