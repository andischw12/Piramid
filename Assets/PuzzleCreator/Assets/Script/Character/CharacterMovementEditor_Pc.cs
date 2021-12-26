//Description : CharacterMovementEditor_Pc. Work in association with characterMovement_Pc. Allow to manage player foostep sound depending the surface
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(characterMovement_Pc))]
public class CharacterMovementEditor_Pc : Editor {
	SerializedProperty	SeeInspector;											// use to draw default Inspector

	SerializedProperty	rbBodyCharacter;
	SerializedProperty 	objCamera;
	SerializedProperty 	addForceObj;
	SerializedProperty	minimum;
	SerializedProperty 	maximum;
	SerializedProperty 	characterSpeed;
	SerializedProperty 	sensibilityMouse;
	SerializedProperty 	sensibilityJoystick;
	SerializedProperty	animationCurveJoystick;
	SerializedProperty	mobileToystickController;
	SerializedProperty	sensibilityMobile;
	SerializedProperty	animationCurveMobile;

	SerializedProperty 	forwardKeyboard;
	SerializedProperty 	backwardKeyboard;
	SerializedProperty 	leftKeyboard;
	SerializedProperty 	rightKeyboard;

    SerializedProperty b_MobileMovement_Stick;
    SerializedProperty b_MobileCamRotation_Stick;

    SerializedProperty mobileSpeedRotation;
   
	SerializedProperty 			VerticalAxisBody;
	SerializedProperty 			HorizontalAxisBody;
	SerializedProperty 			JoystickVerticalAxisCam;
	SerializedProperty 			JoystickHorizontalAxisCam;

	SerializedProperty 			mouseInvertYAxisCam;
	SerializedProperty 			joystickInvertYAxisCam;

	public List<string> s_inputListJoystickAxis = new List<string> ();
	public List<string> s_inputListJoystickButton = new List<string> ();
	public List<string> s_inputListKeyboardAxis = new List<string> ();
	public List<string> s_inputListKeyboardButton = new List<string> ();

	public List<string>  s_inputListJoystickBool = new List<string> ();
	public List<string> s_inputListKeyboardBool= new List<string> ();

	public GameObject objCanvasInput;
   
	private Texture2D MakeTex(int width, int height, Color col) {						// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	private Texture2D 		Tex_01;														// 
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	public string selectedTag = "";

	public string newTagName = "";

	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 				= serializedObject.FindProperty ("SeeInspector");

		rbBodyCharacter				= serializedObject.FindProperty ("rbBodyCharacter");
		objCamera					= serializedObject.FindProperty ("objCamera");
		addForceObj					= serializedObject.FindProperty ("addForceObj");
		minimum						= serializedObject.FindProperty ("minimum");
		maximum						= serializedObject.FindProperty ("maximum");
		characterSpeed				= serializedObject.FindProperty ("characterSpeed");
		sensibilityMouse			= serializedObject.FindProperty ("sensibilityMouse");
		sensibilityJoystick			= serializedObject.FindProperty ("sensibilityJoystick");
		animationCurveJoystick		= serializedObject.FindProperty ("animationCurveJoystick");
		mobileToystickController	= serializedObject.FindProperty ("mobileToystickController");
		sensibilityMobile			= serializedObject.FindProperty ("sensibilityMobile");
		animationCurveMobile		= serializedObject.FindProperty ("animationCurveMobile");

		forwardKeyboard				= serializedObject.FindProperty ("forwardKeyboard");
		backwardKeyboard			= serializedObject.FindProperty ("backwardKeyboard");
		leftKeyboard				= serializedObject.FindProperty ("leftKeyboard");
		rightKeyboard				= serializedObject.FindProperty ("rightKeyboard");

        b_MobileMovement_Stick = serializedObject.FindProperty("b_MobileMovement_Stick");
        b_MobileCamRotation_Stick = serializedObject.FindProperty("b_MobileCamRotation_Stick");

        mobileSpeedRotation = serializedObject.FindProperty("mobileSpeedRotation");
  

		VerticalAxisBody			= serializedObject.FindProperty ("VerticalAxisBody");
		HorizontalAxisBody			= serializedObject.FindProperty ("HorizontalAxisBody");
		JoystickVerticalAxisCam		= serializedObject.FindProperty ("JoystickVerticalAxisCam");
		JoystickHorizontalAxisCam	= serializedObject.FindProperty ("JoystickHorizontalAxisCam");

		mouseInvertYAxisCam			= serializedObject.FindProperty ("mouseInvertYAxisCam");
		joystickInvertYAxisCam		= serializedObject.FindProperty ("joystickInvertYAxisCam");

		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(1,.3f,1,.3f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 

		
	}


    public override void OnInspectorGUI()
    {
        if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
            DrawDefaultInspector();

        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("See Inspector :", GUILayout.Width(85));
        EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;


        //GUILayout.Label("");
        characterMovement_Pc myScript = (characterMovement_Pc)target;


        EditorGUILayout.HelpBox("This script allow to setup character movement on desktop and Mobile", MessageType.Info);
        EditorGUILayout.LabelField("");

        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.HelpBox("The next 4 fields need to be connected", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Character Rigidbody :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(rbBodyCharacter, new GUIContent(""), GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Character Camera :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(objCamera, new GUIContent(""), GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mobile Stick controller :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(mobileToystickController, new GUIContent(""), GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Add Force Position :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(addForceObj, new GUIContent(""), GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("");

        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Character speed", MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Character speed :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(characterSpeed, new GUIContent(""), GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        //	EditorGUILayout.LabelField ("");


        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Camera Options", MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Minimum Camera Angle :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(minimum, new GUIContent(""), GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Maximum Camera Angle :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(maximum, new GUIContent(""), GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("");


        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Desktop Mouse and Keyboard Options", MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mouse sensibility :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(sensibilityMouse, new GUIContent(""), GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        if (objCanvasInput)
        {
            //-> Mouse Invert Y Cam Axis
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Invert Y Look: ", GUILayout.Width(160));
            mouseInvertYAxisCam.intValue = EditorGUILayout.Popup(mouseInvertYAxisCam.intValue, s_inputListKeyboardBool.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> forward Keyboard
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Forward : ", GUILayout.Width(160));
            forwardKeyboard.intValue = EditorGUILayout.Popup(forwardKeyboard.intValue, s_inputListKeyboardButton.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> backward Keyboard
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Backward : ", GUILayout.Width(160));
            backwardKeyboard.intValue = EditorGUILayout.Popup(backwardKeyboard.intValue, s_inputListKeyboardButton.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> left Keyboard
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Left : ", GUILayout.Width(160));
            leftKeyboard.intValue = EditorGUILayout.Popup(leftKeyboard.intValue, s_inputListKeyboardButton.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> right Keyboard
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Right : ", GUILayout.Width(160));
            rightKeyboard.intValue = EditorGUILayout.Popup(rightKeyboard.intValue, s_inputListKeyboardButton.ToArray());
            EditorGUILayout.EndHorizontal();

        }


        EditorGUILayout.EndVertical();

        //EditorGUILayout.LabelField ("");

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Desktop Joystick Options", MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Joystick sensibility :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(sensibilityJoystick, new GUIContent(""), GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Joystick sensibility curve :", GUILayout.Width(160));
        EditorGUILayout.PropertyField(animationCurveJoystick, new GUIContent(""), GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        if (objCanvasInput)
        {
            //-> Joystick Input to move  Horizontaly
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Horizontal Body: ", GUILayout.Width(160));
            HorizontalAxisBody.intValue = EditorGUILayout.Popup(HorizontalAxisBody.intValue, s_inputListJoystickAxis.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> Joystick Input to move  verticaly
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Vertical Body : ", GUILayout.Width(160));
            VerticalAxisBody.intValue = EditorGUILayout.Popup(VerticalAxisBody.intValue, s_inputListJoystickAxis.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> Joystick Input to move  Horizontaly
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Horizontal Cam : ", GUILayout.Width(160));
            JoystickHorizontalAxisCam.intValue = EditorGUILayout.Popup(JoystickHorizontalAxisCam.intValue, s_inputListJoystickAxis.ToArray());
            EditorGUILayout.EndHorizontal();

            //-> Joystick Input to move  verticaly
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Vertical Cam : ", GUILayout.Width(160));
            JoystickVerticalAxisCam.intValue = EditorGUILayout.Popup(JoystickVerticalAxisCam.intValue, s_inputListJoystickAxis.ToArray());
            EditorGUILayout.EndHorizontal();


            //-> Joystick Input Invert Y Cam Axis
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Invert Y Look: ", GUILayout.Width(160));
            joystickInvertYAxisCam.intValue = EditorGUILayout.Popup(joystickInvertYAxisCam.intValue, s_inputListJoystickBool.ToArray());
            EditorGUILayout.EndHorizontal();

        }

        EditorGUILayout.EndVertical();

        //EditorGUILayout.LabelField ("");

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Mobile Joystick Options", MessageType.Info);


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Player Movement use :", GUILayout.Width(160));


        //-> Left Stick
        string newtitle = "Left Stick";
        if (!b_MobileMovement_Stick.boolValue)
            newtitle = "Left Buttons";

        if (GUILayout.Button(newtitle, GUILayout.Width(200)))
        {
            b_MobileMovement_Stick.boolValue = !b_MobileMovement_Stick.boolValue;

            GameObject Grp_Canvas = GameObject.Find("Grp_Canvas");

            Transform[] allChildren = Grp_Canvas.GetComponentsInChildren<Transform>(true);
            canvasMobileConnect_Pc mobileCanvas = null;

            foreach (Transform child in allChildren)
            {
                if (child.name == "mobileCanvas")
                {
                    mobileCanvas = child.GetComponent<canvasMobileConnect_Pc>();
                    break;
                }
            }
            Undo.RegisterFullObjectHierarchyUndo(mobileCanvas.grp_LeftButtonsMove, mobileCanvas.grp_LeftButtonsMove.name);
            Undo.RegisterFullObjectHierarchyUndo(mobileCanvas.virtualJoystickLeftStickToMove, mobileCanvas.virtualJoystickLeftStickToMove.name);

            if (b_MobileMovement_Stick.boolValue)
            {
                mobileCanvas.grp_LeftButtonsMove.SetActive(false);
                mobileCanvas.virtualJoystickLeftStickToMove.gameObject.SetActive(true);
            }
            else
            {
                mobileCanvas.grp_LeftButtonsMove.SetActive(true);
                mobileCanvas.virtualJoystickLeftStickToMove.gameObject.SetActive(false);
            }

        }
        EditorGUILayout.EndHorizontal();


        //-> Right Stick
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Player Camera Rotation use :", GUILayout.Width(160));

        newtitle = "Right Stick";
        if (!b_MobileCamRotation_Stick.boolValue)
            newtitle = "No Stick";


        if (GUILayout.Button(newtitle, GUILayout.Width(200)))
        {
            b_MobileCamRotation_Stick.boolValue = !b_MobileCamRotation_Stick.boolValue;

            GameObject Grp_Canvas = GameObject.Find("Grp_Canvas");

            Transform[] allChildren = Grp_Canvas.GetComponentsInChildren<Transform>(true);
            canvasMobileConnect_Pc mobileCanvas = null;

            foreach (Transform child in allChildren)
            {
                if (child.name == "mobileCanvas")
                {
                    mobileCanvas = child.GetComponent<canvasMobileConnect_Pc>();
                    break;
                }
            }
            Undo.RegisterFullObjectHierarchyUndo(mobileCanvas.virtualJoystick, mobileCanvas.virtualJoystick.name);
            if (!b_MobileCamRotation_Stick.boolValue)
            {
                mobileCanvas.virtualJoystick.gameObject.SetActive(false);
            }
            else
            {
                mobileCanvas.virtualJoystick.gameObject.SetActive(true);
            }



        }
        EditorGUILayout.EndHorizontal();

        // Right Stick Options 
        if (b_MobileCamRotation_Stick.boolValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mobile stick sensibility :", GUILayout.Width(160));
            EditorGUILayout.PropertyField(sensibilityMobile, new GUIContent(""), GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mobile stick sensibility curve :", GUILayout.Width(160));
            EditorGUILayout.PropertyField(animationCurveMobile, new GUIContent(""), GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();


        }
        // No Stick Options
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("sensibility :", GUILayout.Width(160));
            EditorGUILayout.PropertyField(mobileSpeedRotation, new GUIContent(""), GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
        }

		EditorGUILayout.EndVertical ();

		EditorGUILayout.LabelField ("");
		EditorGUILayout.LabelField ("");

		serializedObject.ApplyModifiedProperties ();
	}



	void OnSceneGUI( )
	{
	}
}
#endif