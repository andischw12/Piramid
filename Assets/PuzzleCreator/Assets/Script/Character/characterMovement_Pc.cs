// Description : characterMovement_Pc : use to controller where the character is look to.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class characterMovement_Pc : MonoBehaviour {
	public 	bool 				SeeInspector = true;

	public Rigidbody			rbBodyCharacter;			// Reference to the character body
	public Transform 			tangentStartPosition;
	public Transform			objCamera;					// Reference to the camera
	public GameObject 			addForceObj;				// Position where forces is add to the character
    public Transform            refHead;                    // use for focus camera in inGameGloabalManager in the Hierarchy

	private string 				s_mouseAxisX 		= "Mouse X";				// Default Mouse Inputs
	private string 				s_mouseAxisY 		= "Mouse Y";

	public float 				minimum 	= -60f;			// Limit camera Y movement
	public float 				maximum		= 60f;

	public float 				characterSpeed = 2;			// Character speed when moving left right or forward backward

	public float 				sensibilityMouse 	= 2;	// Mouse sensibility
	//public AnimationCurve		animationCurveMouse;
	public float 				sensibilityJoystick = 2;	// Joystick sensibility
	public AnimationCurve		animationCurveJoystick;

	private float				minimumAxisMovement = .2f;

	public float 				mouseY 		= 0;			// current X camera Rotation

	private float 				tmpXAxis 	= 0;			// temporary values
	private float 				tmpYAxis 	= 0;	

	public VirtualController_Pc mobileToystickController;
	public float 				sensibilityMobile = 2;
	public AnimationCurve		animationCurveMobile;

	private bool 				b_MoveForward = false;		// Use on Mobile
	private bool 				b_MoveBackward = false;
	private bool 				b_MoveLeft = false;		// Use on Mobile
	private bool 				b_MoveRight = false;

    // Mobile Move Camera Second system
    public bool                 b_MobileCamRotation_Stick = true;
    public float                mobileSpeedRotation = 1f;
    private Vector2[]           arrStartPos = new Vector2[10]; 
    // Mobile Move Player Second system
    public bool                 b_MobileMovement_Stick = true;
    public VirtualController_Pc mobileLeftJoystickToMove;
    public float                LeftStickSensibility = 5;

	private float				smoothStart = 0;
	public AnimationCurve		animationCurveMobileSmoothMove;

	public LayerMask 			myLayerMask;
    public LayerMask            CheckGroundLayerMask;

    private float               joyHorizontal = 0;
    private float               joyVertical = 0;

    private float               axisHorizontal = 0;
    private float               AxisVertical = 0;

    private float               mouseVertical = 0;

    private float               mouseInputX = 0;

    private float               yRot;
    private float               mouseHorizontal = 0;

    public float                BrakeForce = 35f;
    public float                Coeff = .15f;
    public float                MaxSpeed = 1f;

    public scPreventClimbing_Pc preventClimbing;

    public Character_Pc character;

    public float                mouseYLastValue = 0;

    Vector3                     joyInput = Vector3.zero;


    public string               HorizontalAxis = "Horizontal";
    public string               verticalAxis = "Vertical";

    public string               HorizontalAxisGamepadHead = "JoyHorizontalHead";
    public string               verticalAxisGamepadHead = "JoyVerticalHead";


    public void charaGeneralMovementController () {
        #region
        // Left right forward backward
        if (!b_MobileMovement_Stick || character.b_DesktopInputs)
                bodyMovement();
            else   // Mobile system 2: Left joystick
                AP_Mobile_bodyMovement_LeftStick();
        #endregion
    }

    void Update()
    {
        #region
        joyVertical = Input.GetAxis(verticalAxisGamepadHead);
        joyHorizontal = Input.GetAxis(HorizontalAxisGamepadHead);


        joyInput = new Vector3(joyVertical,joyHorizontal,0);

        if (joyInput.sqrMagnitude > 1.0f)
            joyInput = joyInput.normalized;


        mouseHorizontal = Input.GetAxis(s_mouseAxisX);
        mouseVertical = Input.GetAxis(s_mouseAxisY);

        axisHorizontal = Input.GetAxis(HorizontalAxis);
        AxisVertical = Input.GetAxis(verticalAxis);

        mouseInputX = Input.GetAxis("Mouse X");

        if(character.b_IsActivated){
            //-> Desktop Case
            if (character.b_DesktopInputs)
            {
                bodyRotation();
                cameraRotation();
            }
            //-> Mobile case
            else
            {
                bodyRotationMobile();
                CamRotationMobile();
            } 
        }
        #endregion
    }

    //--> Desktop Case : Body rotation
    private void bodyRotation(){
        #region
        if (!character.b_Joystick && AP_GlobalPuzzleManager_Pc.instance.mouseWaitUntilFirstMouseMove) {

            if (!character.b_Joystick && mouseHorizontal != 0) {
                tmpXAxis = mouseInputX * 1.1f;
                tmpXAxis *= sensibilityMouse;
			} else {
				//Debug.Log ("here 1");
				tmpXAxis = 0;
			}
				
           
            objCamera.transform.Rotate(0, tmpXAxis, 0);


		}
        else if (character.b_Joystick) {     // Caculate body and camera X rotation at the same time

            objCamera.localEulerAngles += joyInput * animationCurveJoystick.Evaluate(joyInput.magnitude) * 100f  * Time.deltaTime;

            objCamera.localEulerAngles = new Vector3( ClampAngle(objCamera.localEulerAngles.x, minimum, maximum),
                                                     objCamera.localEulerAngles.y,
                                                     0);
         
		}
        #endregion
    }

    float ClampAngle(float angle, float from, float to)
    {
        #region
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
        #endregion
    }

    //--> Desktop case : camera rotation X Axis
    private void cameraRotation(){
        #region
        if (!character.b_Joystick && AP_GlobalPuzzleManager_Pc.instance.mouseWaitUntilFirstMouseMove) {
            
			// Mouse Case
            if (!character.b_Joystick && mouseVertical != 0) {
				tmpYAxis = mouseVertical;

                tmpYAxis = Mathf.Clamp(tmpYAxis, -3f, 3f);
                tmpYAxis *= 1.5f;

                mouseY -= tmpYAxis * sensibilityMouse;
			}
								
			mouseY = Mathf.Clamp (mouseY, minimum, maximum);
				
			objCamera.localEulerAngles = new Vector3 (
				mouseY,
				objCamera.localEulerAngles.y,
				0);
		} 
		// Joystick Case
        else if (character.b_Joystick) {
            // camera X rotation is calculated in section bodyRotation() for gamepad inputs

		}

        //-> Prevent Mac bug with cursor lockstate
        else if(!AP_GlobalPuzzleManager_Pc.instance.mouseWaitUntilFirstMouseMove && mouseYLastValue != Input.GetAxis(s_mouseAxisY)/* && b_Once*/)
        {
            StartCoroutine(AP_GlobalPuzzleManager_Pc.instance.waitToInitMouseMovement());
        }
		mouseYLastValue = Input.GetAxis (s_mouseAxisY);
        #endregion
    }

    // --> Desktop case : Move the character left right forward backward
    void bodyMovement(){
        #region

        addForceObj.transform.localEulerAngles 
		= new Vector3 (addForceObj.transform.localEulerAngles.x,
			objCamera.transform.localEulerAngles.y,
			addForceObj.transform.localEulerAngles.z);

        Vector3 Direction = new Vector3 (0,0, 0);

        if (character.b_Joystick) {
			// --> Left and Right Movement	Joystick
			if (axisHorizontal > minimumAxisMovement) {
				Direction += FindTangentX () * -axisHorizontal;
			} else if (axisHorizontal < -minimumAxisMovement) {
				Direction -= FindTangentX () * axisHorizontal;
			}
		} 
		// --> Left and Right Movement Keyboard
		else {
            Direction += FindTangentX () * Input.GetAxis(HorizontalAxis);
		}

        if (character.b_Joystick) {
		// --> Forward backward movement Joystick
            if (AxisVertical > minimumAxisMovement) {
                Direction += FindTangentZ () * AxisVertical;
                } else if (AxisVertical < -minimumAxisMovement) {
                    Direction -= FindTangentZ () * -AxisVertical;
            }  
 
		}
		// --> Forward backward movement Keyboard
		else {
            Direction += FindTangentZ () * Input.GetAxis(verticalAxis);
		}

		if(b_MoveRight || b_MoveLeft || b_MoveBackward || b_MoveForward){
			smoothStart = Mathf.MoveTowards (smoothStart, 1, Time.deltaTime*2);
		}

		if (b_MoveRight) {
			Direction += FindTangentX () * animationCurveMobileSmoothMove.Evaluate(smoothStart);
		} else if (b_MoveLeft) {
			Direction -= FindTangentX () * animationCurveMobileSmoothMove.Evaluate(smoothStart);
		}

    		
        if (b_MoveForward) {
            Direction += FindTangentZ () * animationCurveMobileSmoothMove.Evaluate(smoothStart);
        } else if (b_MoveBackward) {
            Direction -= FindTangentZ () * animationCurveMobileSmoothMove.Evaluate(smoothStart);
        }


        if (preventClimbing.b_preventClimbing){
            Direction.y = 0;
        }
          

        rbBodyCharacter.AddForceAtPosition(Direction * characterSpeed, addForceObj.transform.position, ForceMode.Force);            // move the character

        Vector3 opposite = rbBodyCharacter.transform.InverseTransformDirection(-rbBodyCharacter.velocity);                          // Opposite force to stop the character
            rbBodyCharacter.AddRelativeForce(opposite * BrakeForce * Coeff, ForceMode.Force);  


        if (rbBodyCharacter.velocity.magnitude > MaxSpeed)
            rbBodyCharacter.velocity = rbBodyCharacter.velocity.normalized * MaxSpeed;
        #endregion
    }

    // --> Next Sections are used for mobile virtual buttons
    public void CamRotationMobile(){
        #region
        if(b_MobileCamRotation_Stick){      // using right stick to move the player
            float virtualJoyVertical = mobileToystickController.inputVector.z;

            mouseY -= animationCurveMobile.Evaluate(Mathf.Abs(virtualJoyVertical)) * virtualJoyVertical * sensibilityMobile;
            mouseY = Mathf.Clamp(mouseY, minimum, maximum);

            // Rotate Camera
            objCamera.localEulerAngles = new Vector3(
                mouseY,
                objCamera.localEulerAngles.y,
                objCamera.localEulerAngles.z);
  
        }
        else{ // moving using all screen
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(i).deltaPosition;

                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    arrStartPos[i] = Input.GetTouch(i).position;
                }

                if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    float swipe = (new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0) - new Vector3(arrStartPos[i].x, arrStartPos[i].y, 0)).magnitude;

                    Vector2 newPos = Input.GetTouch(i).position;

                    if (swipe > 50 /*&& !ingameGlobalManager.instance.mobileInputFinger.checkIfFingerTouchAUIButton(newPos)*/)
                    {
                        // Rotate camera Horizontaly
                        objCamera.transform.Rotate(Vector3.up * touchDeltaPosition.x * mobileSpeedRotation * Time.deltaTime, Space.World);

                        mouseY -= touchDeltaPosition.y * mobileSpeedRotation * Time.deltaTime;
                        mouseY = Mathf.Clamp(mouseY, minimum, maximum);

                        // Rotate Camera Verticaly
                        objCamera.localEulerAngles = new Vector3(
                            mouseY,
                            objCamera.localEulerAngles.y,
                            objCamera.localEulerAngles.z);
                    }
                }
            }
        }
        #endregion
    }

    private void bodyRotationMobile(){
        #region
        float virtualJoyVertical = mobileToystickController.inputVector.x;

		tmpXAxis = animationCurveMobile.Evaluate( Mathf.Abs( virtualJoyVertical)) * virtualJoyVertical;
		tmpXAxis *= sensibilityMobile;

		// Rotate the character using his rigidbody
		Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,tmpXAxis,0));
		rbBodyCharacter.MoveRotation(rbBodyCharacter.rotation * deltaRotation );
        #endregion
    }

    // --> First System Move the camera using the vitual joystick
    public void pointerDrag(){
        #region
        Vector2 pos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
			mobileToystickController.backgroundImage.rectTransform,
			mobileToystickController.eventData.position,
			mobileToystickController.eventData.pressEventCamera,
			out pos)){

			pos.x = pos.x / mobileToystickController.backgroundImage.rectTransform.sizeDelta.x;
			pos.y = pos.y / mobileToystickController.backgroundImage.rectTransform.sizeDelta.y;

			mobileToystickController.inputVector = new Vector3 (
				pos.x * 2,
				0,
				pos.y * 2);

			if (mobileToystickController.inputVector.magnitude > 1){
				mobileToystickController.inputVector = mobileToystickController.inputVector.normalized;
			}


			// Move joystick Image
			mobileToystickController.virtualCenter.rectTransform.anchoredPosition = 
				new Vector2(mobileToystickController.inputVector.x * (mobileToystickController.backgroundImage.rectTransform.sizeDelta.x/3),
					mobileToystickController.inputVector.z * (mobileToystickController.backgroundImage.rectTransform.sizeDelta.y/3));
		}
        #endregion
    }

    public void pointerUp(){
        #region
        if (mobileToystickController.inputVector != Vector3.zero){
			mobileToystickController.virtualCenter.rectTransform.anchoredPosition = Vector2.zero;
		}
		mobileToystickController.inputVector = Vector3.zero;
        #endregion
    }

    // --> Second System: Move the player using the vitual Left joystick
    public void pointerDrag_MoveWithLeftStick()
    {
        #region
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mobileLeftJoystickToMove.backgroundImage.rectTransform,
            mobileLeftJoystickToMove.eventData.position,
            mobileLeftJoystickToMove.eventData.pressEventCamera,
            out pos))
        {

            pos.x = pos.x / mobileLeftJoystickToMove.backgroundImage.rectTransform.sizeDelta.x;
            pos.y = pos.y / mobileLeftJoystickToMove.backgroundImage.rectTransform.sizeDelta.y;

            mobileLeftJoystickToMove.inputVector = new Vector3(
                pos.x * 2,
                0,
                pos.y * 2);

            if (mobileLeftJoystickToMove.inputVector.magnitude > 1)
            {
                mobileLeftJoystickToMove.inputVector = mobileLeftJoystickToMove.inputVector.normalized;
            }

            // Move joystick Image
            mobileLeftJoystickToMove.virtualCenter.rectTransform.anchoredPosition =
                                        new Vector2(mobileLeftJoystickToMove.inputVector.x * (mobileLeftJoystickToMove.backgroundImage.rectTransform.sizeDelta.x / 3),
                                                    mobileLeftJoystickToMove.inputVector.z * (mobileLeftJoystickToMove.backgroundImage.rectTransform.sizeDelta.y / 3));
        }
        #endregion
    }

    public void pointerUp_MoveWithLeftStick()
    {
        #region
        //  Debug.Log("Here");
        if (mobileLeftJoystickToMove.inputVector != Vector3.zero)
        {
            mobileLeftJoystickToMove.virtualCenter.rectTransform.anchoredPosition = Vector2.zero;
        }
        mobileLeftJoystickToMove.inputVector = Vector3.zero;
        #endregion
    }

    // --> Desktop case : Move the character left right forward backward
    void AP_Mobile_bodyMovement_LeftStick(/*Vector2 mobileLeftJoystickToMove*/)
    {
        #region
        if(mobileLeftJoystickToMove){
            //Debug.Log("here: " + mobileLeftJoystickToMove.inputVector.z);
            if (mobileLeftJoystickToMove.inputVector.magnitude > 1)
            {
                mobileLeftJoystickToMove.inputVector = mobileLeftJoystickToMove.inputVector.normalized;
            }


            addForceObj.transform.localEulerAngles
            = new Vector3(addForceObj.transform.localEulerAngles.x,
                objCamera.transform.localEulerAngles.y,
                addForceObj.transform.localEulerAngles.z);

            Vector3 Direction = new Vector3(0, 0, 0);


            // --> Left and Right Movement  Joystick
            if (mobileLeftJoystickToMove.inputVector.x > minimumAxisMovement)
            {
                Direction += FindTangentX() * mobileLeftJoystickToMove.inputVector.x;
            }
            else if (mobileLeftJoystickToMove.inputVector.x < -minimumAxisMovement)
            {
                Direction -= FindTangentX() * -mobileLeftJoystickToMove.inputVector.x;
            }


            // --> Forward backward movement Joystick
            if (mobileLeftJoystickToMove.inputVector.z > minimumAxisMovement)
                Direction += FindTangentZ() * mobileLeftJoystickToMove.inputVector.z;
            else if (mobileLeftJoystickToMove.inputVector.z < -minimumAxisMovement)
                Direction -= FindTangentZ() * -mobileLeftJoystickToMove.inputVector.z;



           if (b_MoveForward)
               Direction += FindTangentZ() * animationCurveMobileSmoothMove.Evaluate(smoothStart);
           else if (b_MoveBackward)
               Direction -= FindTangentZ() * animationCurveMobileSmoothMove.Evaluate(smoothStart);


            if (preventClimbing.b_preventClimbing)
            {
                Direction.y = 0;
            }


            rbBodyCharacter.AddForceAtPosition(Direction * characterSpeed, addForceObj.transform.position, ForceMode.Force);            // move the character

            Vector3 opposite = rbBodyCharacter.transform.InverseTransformDirection(-rbBodyCharacter.velocity);                          // Opposite force to stop the character
            rbBodyCharacter.AddRelativeForce(opposite * BrakeForce * Coeff, ForceMode.Force);


            if (rbBodyCharacter.velocity.magnitude > MaxSpeed)
                rbBodyCharacter.velocity = rbBodyCharacter.velocity.normalized * MaxSpeed;

        }
        #endregion
    }

    // Use for Mobile : Player move forward
    public void MoveForward(){
        #region
        b_MoveForward = true;
		b_MoveBackward 	= false;
		b_MoveLeft		= false;
		b_MoveRight 	= false;
        #endregion
    }
    // Use for Mobile : Player move backward
    public void MoveBackward(){
        #region
        b_MoveBackward = true;
		b_MoveForward 	= false;
		b_MoveLeft		= false;
		b_MoveRight 	= false;
        #endregion
    }
    // Use for Mobile : Player Stop moving when button is released
    public void StopMoving(){
        #region
        b_MoveBackward = false;
		b_MoveForward 	= false;
		b_MoveLeft		= false;
		b_MoveRight 	= false;
		smoothStart = 0;
        #endregion
    }

    // Use for Mobile : Player move Left
    public void MoveLeft(){
        #region
        b_MoveBackward = false;
		b_MoveForward 	= false;
		b_MoveLeft		= true;
		b_MoveRight 	= false;
        #endregion
    }
    // Use for Mobile : Player move Right
    public void MoveRight(){
        #region
        b_MoveBackward = false;
		b_MoveForward 	= false;
		b_MoveLeft		= false;
		b_MoveRight 	= true;
        #endregion
    }

    Vector3 FindTangentZ(){
        #region
        Vector3 newVector = Vector3.zero;
		Vector3 tangente = Vector3.zero;
		RaycastHit hit2;
		if (Physics.Raycast (tangentStartPosition.position, -Vector3.up, out hit2, 10, myLayerMask)) {						
			hit2.normal.Normalize ();

			//Debug.DrawRay(hit2.point, hit2.normal , Color.white);
			tangente = Vector3.Cross( hit2.normal, -addForceObj.transform.right );

			if( tangente.magnitude == 0 )
			{tangente = Vector3.Cross( hit2.normal, Vector3.up );}
			Debug.DrawRay(hit2.point, tangente , Color.yellow);
		}
		//Debug.Log (tangente);
		return tangente;
        #endregion
    }

    Vector3 FindTangentX(){
        #region
        Vector3 newVector = Vector3.zero;
		Vector3 tangente = Vector3.zero;
		RaycastHit hit2;
		if (Physics.Raycast (tangentStartPosition.position, -Vector3.up, out hit2, 10, myLayerMask)) {						
			hit2.normal.Normalize ();

			Vector3 myDirection = Vector3.Cross(addForceObj.transform.right, hit2.normal);

			Debug.DrawRay(hit2.point, hit2.normal , Color.white);
			tangente = Vector3.Cross( hit2.normal, myDirection );
		
			if( tangente.magnitude == 0 )
				tangente = Vector3.Cross( hit2.normal, Vector3.up );

			Debug.DrawRay(hit2.point, tangente , Color.red);
		}
		//Debug.Log (tangente);
		return tangente;
        #endregion
    }

}
