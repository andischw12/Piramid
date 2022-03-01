//Decription : canvasMobileConnect_Pc : Use for Mobile Inputs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasMobileConnect_Pc : MonoBehaviour {
	public GameObject           canvas_Mobile;
	public characterMovement_Pc currentCharacter;
    public VirtualController_Pc virtualJoystick;                    // System 1: Right stick camera
    public GameObject           grp_LeftButtonsMove;                // System 1: Left buttons Forward/Backward/Left/Right
    public VirtualController_Pc virtualJoystickLeftStickToMove;     // System 2: Left Stick Forward/Backward/Left/Right

    public bool alwaysFindCharacter = true;


    private void Start()
    {
        initializedCanvasMobile(); 
    }

    //--> Display Main Menu
    public void displayMainMenu () {
	}

    private characterMovement_Pc FindChararcter(){
            return GameObject.Find("Character").GetComponent<characterMovement_Pc>();
    }

	public void initializedCanvasMobile(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();

        if(currentCharacter){
            currentCharacter.mobileToystickController = virtualJoystick;
            currentCharacter.mobileLeftJoystickToMove = virtualJoystickLeftStickToMove;


            currentCharacter.StopMoving();
            currentCharacter.pointerUp();
        }
	}


    // --> System 1: Move the camera using the vitual joystick
	public void F_pointerDrag(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();

        if (currentCharacter)
		currentCharacter.pointerDrag ();
	}

	public void F_pointerUp(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();

        if (currentCharacter)
		currentCharacter.pointerUp ();
	}


	// Use for Mobile : Player move forward
	public void F_MoveForward(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
		currentCharacter.MoveForward ();
	}
	// Use for Mobile : Player move backward
	public void F_MoveBackward(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
		currentCharacter.MoveBackward ();
	}

	// Use for Mobile : Player move Left
	public void F_MoveLeft(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
		currentCharacter.MoveLeft ();
	}
	// Use for Mobile : Player move right
	public void F_MoveRight(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
		currentCharacter.MoveRight ();
	}
	// Use for Mobile : Player Stop moving when button is released
	public void F_StopMoving(){
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
		currentCharacter.StopMoving ();
	}
   


    // Second System
    // --> Move the camera using the vitual joystick
    public void F_pointerDrag_MoveWithLeftStick()
    {
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
        currentCharacter.pointerDrag_MoveWithLeftStick();
    }

    public void F_pointerUp_MoveWithLeftStick()
    {
        if (currentCharacter == null && alwaysFindCharacter)
            currentCharacter = FindChararcter();
        
        if (currentCharacter)
        currentCharacter.pointerUp_MoveWithLeftStick();
    }

}
