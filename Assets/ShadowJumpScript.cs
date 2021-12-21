using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowJumpScript : MonoBehaviour
{
    bool InJump;
    float ShadowOpacity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !InJump) 
        {
            GetComponent<MeshRenderer>().material.color=  new Color (GetComponent<MeshRenderer>().material.color.r, GetComponent<MeshRenderer>().material.color.b, GetComponent<MeshRenderer>().material.color.g, GetComponent<MeshRenderer>().material.color.a -0.001f);
        
        }

    }
}
