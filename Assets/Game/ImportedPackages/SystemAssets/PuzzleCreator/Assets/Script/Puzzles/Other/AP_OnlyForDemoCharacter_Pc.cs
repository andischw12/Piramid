//Description: AP_OnlyForDemoCharacter_Pc: Some methods use in the demo by the character
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_OnlyForDemoCharacter_Pc : MonoBehaviour
{
    private Character_Pc chara;
    private characterMovement_Pc charaMovement;

    private Renderer VR_Hand;


    public bool bool_ActivateCharacter()
    {
        #region
        if (chara == null){
            chara = GameObject.Find("Character").GetComponent<Character_Pc>();
            charaMovement = GameObject.Find("Character").GetComponent<characterMovement_Pc>();
        }
            


        if (chara != null){
            chara.b_IsActivated = true;
            if (!AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)
            {   // Mobile case: Deactivate Mobile Inputs
                charaMovement.mobileToystickController.transform.parent.gameObject.SetActive(true);
            }  
        }

        return true;
        #endregion
    }

    public bool bool_DeactivateCharacter()
    {
        #region
        if (chara == null)
        {
            chara = GameObject.Find("Character").GetComponent<Character_Pc>();
            charaMovement = GameObject.Find("Character").GetComponent<characterMovement_Pc>();
        }

        if (chara != null)
        {
            chara.b_IsActivated = false;
            if (!AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)
            {   // Mobile case: Activate Mobile Inputs
                charaMovement.mobileToystickController.transform.parent.gameObject.SetActive(false);
            }
        }


        return true;
        #endregion
    }

    public bool bool_ActivateHand()
    {
        #region
        if(AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.puzlleIntearctionType == 1 || 
           AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.puzlleIntearctionType == 2){
            if (VR_Hand == null)
                VR_Hand = GameObject.Find("VR_Hand").GetComponent<Renderer>();

            if (VR_Hand != null)
                VR_Hand.GetComponent<Renderer>().enabled = true;

            if (AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent == null && VR_Hand)
            {
                AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent = VR_Hand.transform;
                AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent = VR_Hand.GetComponent<AP_DragAndDropParent_Pc>();
            }
        }

        return true;
        #endregion
    }

    public bool bool_DeactivateHand()
    {
        #region
        if (VR_Hand == null)
            VR_Hand = GameObject.Find("VR_Hand").GetComponent<Renderer>();

        if (VR_Hand != null)
            VR_Hand.GetComponent<Renderer>().enabled = false;
        
        return true;
        #endregion
    }

    public bool bool_ActivateReticule()
    {
        #region
        // Activate the reticule only Desktop 
        if (AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.puzlleIntearctionType == 3){
            if (AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.b_ReticuleState)
                AP_GlobalPuzzleManager_Pc.instance.reticule.gameObject.SetActive(true);
            else
                AP_GlobalPuzzleManager_Pc.instance.reticule.gameObject.SetActive(false); 
        } 
           
        
        
        return true;
        #endregion
    }

    public bool bool_DeactivateReticule()
    {
        #region

        if (!AP_GlobalPuzzleManager_Pc.instance.b_Reticule)
            AP_GlobalPuzzleManager_Pc.instance.reticule.gameObject.SetActive(false);
        else
            AP_GlobalPuzzleManager_Pc.instance.reticule.gameObject.SetActive(true);

        return true;
        #endregion
    }


}
