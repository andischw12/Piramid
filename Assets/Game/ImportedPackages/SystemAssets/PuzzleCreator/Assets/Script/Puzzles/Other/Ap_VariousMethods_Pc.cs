using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ap_VariousMethods_Pc
{
    public GameObject FindAnObjectUsingItsParent(string parentName,string objName)
    {
        GameObject tmpObj = GameObject.Find(parentName);
        if (tmpObj)
        {
            Transform[] allChildren = tmpObj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {

                if (child.name == objName)
                {
                    Debug.Log(child.name);
                    return child.gameObject;

                }
            }
        }
        return null;
    }
 
}
