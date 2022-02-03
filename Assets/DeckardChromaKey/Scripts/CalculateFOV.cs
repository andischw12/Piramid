using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateFOV : MonoBehaviour
{


    public Quaternion angle1;
    public Quaternion angle2;
    public float FOV;
    public KeyCode key;
    public KeyCode keyEnd;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(key))
        {
            angle1 = gameObject.transform.rotation;
        }
        if (Input.GetKeyDown(keyEnd))
        {
            angle2 = gameObject.transform.rotation;
        }

        FOV = Quaternion.Angle ( angle1, angle2);
    }


}
