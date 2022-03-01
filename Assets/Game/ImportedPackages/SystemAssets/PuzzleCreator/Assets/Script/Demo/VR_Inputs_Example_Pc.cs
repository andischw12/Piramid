using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Inputs_Example_Pc : MonoBehaviour
{
    public bool TestIfVRValidationButtonIsPressed_Down()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Button S is pressed Down");
            return true;
        }
        return false;
    }

    public bool TestIfVRValidationButtonIsPressed_Up()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            //Debug.Log("Button S is pressed Up");
            return true;
        }
        return false;
    }

    public bool TestIfVRBackButtonIsPressed_Down()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("Button D is pressed Down");
            return true;
        }
        return false;
    }
}
