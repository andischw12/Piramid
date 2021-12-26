// Description : Character_Pc. Global Manager for the player character
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Pc : MonoBehaviour {
    private characterMovement_Pc charaMovement;				// Access to the characterMovement component

    public bool b_DesktopInputs = true;                     // True: Player use keyboard + Mouse or Gamepad | False: Mobile Device
    public bool b_Joystick = false;                         // True: Gamepad is used | False: keyboard + Mouse or Gamepad used
    public bool mouseWaitUnitlFirstMouseMove = true;        // Prevent bug on Mac when lockstate is changed 
    public bool b_IsActivated = true;

	void Start(){
        #region
        if (b_DesktopInputs) {	// Init all axis
			Input.ResetInputAxes();}

		if (gameObject.GetComponent<characterMovement_Pc> ())	// Access to the characterMovement component
			charaMovement = gameObject.GetComponent<characterMovement_Pc> ();
        #endregion
    }

    #region Update
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            b_Joystick = !b_Joystick;
        }
        if(Input.GetKeyDown(KeyCode.W)){
            b_DesktopInputs = !b_DesktopInputs;
        }
    }*/
    #endregion

    void FixedUpdate () {
        if (charaMovement && b_IsActivated)
            charaMovement.charaGeneralMovementController();
	}

    public bool bool_ActivateCharacter(){
        #region
        b_IsActivated = true;
        if(!AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs){   // Mobile case: Deactivate Mobile Inputs
            charaMovement.mobileToystickController.transform.parent.gameObject.SetActive(true);
        }
        return true;
        #endregion
    }
    public bool bool_DeactivateCharacter()
    {
        #region
        b_IsActivated = false;
        if (!AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)
        {   // Mobile case: Activate Mobile Inputs
            charaMovement.mobileToystickController.transform.parent.gameObject.SetActive(false);
        }
        return true;
        #endregion
    }
}
