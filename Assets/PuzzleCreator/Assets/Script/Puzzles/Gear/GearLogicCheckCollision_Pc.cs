// Description : GearLogicCheckCollision_Pc : Use in gear and logic puzzle to know if a gear/Logic is touching an other gear/Logic
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearLogicCheckCollision_Pc : MonoBehaviour {
    public bool b_CollisionWithOtherGear = false;

  
    public void OnTriggerStay(Collider other){
        if ((other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" 
            || 
            other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "GearFixed"
            ||
            other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "LogicsFixed") &&


            gameObject.transform.localPosition != Vector3.zero &&
             other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag != "PuzzleRefPosition")
        {
           // Debug.Log(other.transform.parent.transform.parent.name + " : " + gameObject.transform.parent.transform.parent.name);
            b_CollisionWithOtherGear = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
            || 
            other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "GearFixed"
            ||
            other.transform.GetComponent<AP_CheckTag_Pc>() && other.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "LogicsFixed")
        {
            b_CollisionWithOtherGear = false;
        }
    }


    public bool returnCheckCollision(){
        return b_CollisionWithOtherGear ;
    }

  
}
