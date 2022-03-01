// Description : Move the fake mouse with the joystick
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickReticule_Pc : MonoBehaviour {
	public bool 				SeeInspector = false;

	public float 				sensibilityJoystick = .1f;	// Joystick sensibility

	public Image 				joyReticule;

	//public AnimationCurve		animationCurveJoystick;

	public RectTransform 		joyReticule2;

    Vector3                     joyInput = Vector3.zero;
    public Image                Image_ObjDetected;

    public bool                 b_CanGrab = false;
    public bool                 b_Selected = false;

    public Color                cNoSelection = Color.white;
    public Color                cSelection = Color.red;


	private void Awake()
	{
        if (joyReticule2) joyReticule2.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
        MoveJoystickReticule ();  
	}

	public void MoveJoystickReticule(){
        #region
        float joyVertical = Input.GetAxis (AP_GlobalPuzzleManager_Pc.instance.verticalAxisJoystickLeft);
        float joyHorizontal = Input.GetAxis (AP_GlobalPuzzleManager_Pc.instance.HorizontalAxisJoystickLeft);


        joyInput = new Vector2(joyHorizontal,-joyVertical);

        if (joyInput.sqrMagnitude > 1.0f)
            joyInput = joyInput.normalized;



        if(joyInput.sqrMagnitude > .005f)
        joyReticule2.position -= joyInput * sensibilityJoystick * Time.deltaTime * 5;

        joyReticule2.position = new Vector3(Mathf.Clamp(joyReticule2.position.x, 0, Screen.width * .97f),
                                            Mathf.Clamp(joyReticule2.position.y,Screen.height * 0.07f, Screen.height),
                                            0);
        

        joyReticule.rectTransform.pivot = new Vector2 (joyReticule2.position.x / Screen.width, joyReticule2.position.y / Screen.height);
        #endregion
    }

	public void newPosition(float newPosX, float newPosY){
        #region
        joyReticule2.position = new Vector3(newPosX, newPosY,0);
        joyReticule.rectTransform.pivot = new Vector2(joyReticule2.position.x / Screen.width, joyReticule2.position.y / Screen.height);
        #endregion
    }

    public void AP_FakeHand_CanGrab(){
        #region
        Image_ObjDetected.color = cSelection; 
        b_CanGrab = true;
        #endregion
    }
    public void AP_FakeHand_Selected()
    {
        #region
        Image_ObjDetected.color = cNoSelection;
        b_Selected = true;
        #endregion
    }
    public void AP_FakeHand_NoSelection()
    {
        #region
        Image_ObjDetected.color = cNoSelection; 
        b_CanGrab = false;
        b_Selected = false;
        #endregion
    }
}
