using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_TutoExtendSave : MonoBehaviour
{
    public bool b_IsDoorUnlocked = false;
    public bool b_IsDoorOpened = false;



    public string AP_sExtendSave()
    {
        string s_ObjectDatas = "";
        s_ObjectDatas += b_IsDoorUnlocked.ToString();   // Save b_IsDoorUnlocked state. Save a string: True or False. 
        s_ObjectDatas += "_";                           // Separate each elements that need to be saved with a _
        s_ObjectDatas += b_IsDoorOpened.ToString();   // Save b_IsDoorOpened state. Save a string: True or False. 

        return s_ObjectDatas;
    }


    public void AP_UpdateThisObject(string s_ObjectDatas)
    {
        // Split data in an array.
        string[] codes = s_ObjectDatas.Split('_');              

        // Save Doesn't exist
        if (s_ObjectDatas == "")
        {}
        // Save exist
        else
        {                       
            // Your elements saved with the save Extention start on the elements 1. Element 0 check if the object need to be activated/deactivated                           
            int startValue = 1;

            // Update b_IsDoorUnlocked state. 
            if (codes[startValue] == "True")                
                b_IsDoorUnlocked = true;
            else
                b_IsDoorUnlocked = false;
                
            // Update b_IsDoorOpened state. 
            if (codes[startValue + 1] == "True")
                b_IsDoorOpened = true;
            else
                b_IsDoorOpened = false;
        }
    }
}
