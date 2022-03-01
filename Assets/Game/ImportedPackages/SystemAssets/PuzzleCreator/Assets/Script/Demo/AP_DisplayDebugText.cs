using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_DisplayDebugText : MonoBehaviour
{
    public string sDebugText = "Test";

    public void Ap_Display_Txt()
    {
        Debug.Log(sDebugText);
    }

    public void Ap_Display_Txt_02(string txt)
    {
        Debug.Log(txt);
    }


    public bool Bool_Display_Txt_01()
    {
        Debug.Log("Activated");
        return true;
    }

    public bool Bool_Display_Txt_02()
    {
        Debug.Log("Deactivated");
        return true;
    }
}
