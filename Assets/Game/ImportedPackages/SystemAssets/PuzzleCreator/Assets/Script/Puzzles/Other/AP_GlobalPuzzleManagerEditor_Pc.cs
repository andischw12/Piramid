//Description : AP_GlobalPuzzleManagerEditor_Pc : custom editor for AP_GlobalPuzzleManager_Pc
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(AP_GlobalPuzzleManager_Pc))]
public class AP_GlobalPuzzleManagerEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector

    SerializedProperty methodsList;
    SerializedProperty methodsListExitPuzzle;

    SerializedProperty methodsListEnterPuzzleNoFocus;
    SerializedProperty methodsListExitPuzzleNoFocus;

    SerializedProperty methodsListEnterPuzzleNoFocusReticule;
    SerializedProperty methodsListExitPuzzleNoFocusReticule;

    SerializedProperty methodsListVRValidationDown;
    SerializedProperty methodsListVRValidationUp;
    SerializedProperty methodsListVRBackDown;

    SerializedProperty HorizontalAxisJoystickLeft;
    SerializedProperty verticalAxisJoystickLeft;

    SerializedProperty validationButtonJoystick;
    SerializedProperty backButtonJoystick;

    SerializedProperty validationButtonKeyboard;
    SerializedProperty backButtonKeyboard;

    SerializedProperty b_DesktopInputs;
    SerializedProperty b_Joystick;

    SerializedProperty b_Reticule;
    SerializedProperty  b_iconPuzzle;
    SerializedProperty b_iconPuzzleMobile;
    SerializedProperty reticule;
    SerializedProperty iconPuzzle;
    SerializedProperty reticuleJoystickImage;
    SerializedProperty _joystickReticule;

    SerializedProperty dragAndDropParent;
    SerializedProperty editorModeDisplayed;

    SerializedProperty b_AutoLockCursorWhenGameStarts;
    SerializedProperty b_Pause;
    SerializedProperty ap_MainCamera;

    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;

    public string[] toolbarStrings = new string[] { "Focus", "VR", "Reticule" };


    private Texture2D MakeTex(int width, int height, Color col)
    {                       // use to change the GUIStyle
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private Texture2D Tex_01;
    private Texture2D Tex_02;
    private Texture2D Tex_03;
    private Texture2D Tex_04;
    private Texture2D Tex_05;

    public Color _cGreen = Color.green;
    public Color _cGray = new Color(.9f, .9f, .9f, 1);

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        editorModeDisplayed = serializedObject.FindProperty("editorModeDisplayed");
        //b_Auto = serializedObject.FindProperty("b_Auto");
        methodsList = serializedObject.FindProperty("methodsList");
        methodsListExitPuzzle = serializedObject.FindProperty("methodsListExitPuzzle");


        methodsListEnterPuzzleNoFocus = serializedObject.FindProperty("methodsListEnterPuzzleNoFocus");
        methodsListExitPuzzleNoFocus = serializedObject.FindProperty("methodsListExitPuzzleNoFocus");

        methodsListEnterPuzzleNoFocusReticule = serializedObject.FindProperty("methodsListEnterPuzzleNoFocusReticule");
        methodsListExitPuzzleNoFocusReticule = serializedObject.FindProperty("methodsListExitPuzzleNoFocusReticule");

        methodsListVRValidationDown = serializedObject.FindProperty("methodsListVRValidationDown");
        methodsListVRValidationUp = serializedObject.FindProperty("methodsListVRValidationUp");
        methodsListVRBackDown = serializedObject.FindProperty("methodsListVRBackDown");
       
        HorizontalAxisJoystickLeft = serializedObject.FindProperty("HorizontalAxisJoystickLeft");
        verticalAxisJoystickLeft = serializedObject.FindProperty("verticalAxisJoystickLeft");
        validationButtonJoystick = serializedObject.FindProperty("validationButtonJoystick");
        backButtonJoystick = serializedObject.FindProperty("backButtonJoystick");
    
        validationButtonKeyboard = serializedObject.FindProperty("validationButtonKeyboard");
        backButtonKeyboard = serializedObject.FindProperty("backButtonKeyboard");

        b_DesktopInputs = serializedObject.FindProperty("b_DesktopInputs");
        b_Joystick = serializedObject.FindProperty("b_Joystick");

        b_Reticule = serializedObject.FindProperty("b_Reticule");
        b_iconPuzzle = serializedObject.FindProperty("b_iconPuzzle");
        b_iconPuzzleMobile = serializedObject.FindProperty("b_iconPuzzleMobile");
        reticule = serializedObject.FindProperty("reticule");
        iconPuzzle = serializedObject.FindProperty("iconPuzzle");
        reticuleJoystickImage = serializedObject.FindProperty("reticuleJoystickImage");
        _joystickReticule = serializedObject.FindProperty("_joystickReticule");
        dragAndDropParent = serializedObject.FindProperty("dragAndDropParent");

        b_AutoLockCursorWhenGameStarts = serializedObject.FindProperty("b_AutoLockCursorWhenGameStarts");
        b_Pause = serializedObject.FindProperty("b_Pause");
        ap_MainCamera = serializedObject.FindProperty("ap_MainCamera");

        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;


        Tex_01 = MakeTex(2, 2, myScript.color_01);
        Tex_02 = MakeTex(2, 2, myScript.color_02);
        Tex_03 = MakeTex(2, 2, myScript.color_03);
        Tex_04 = MakeTex(2, 2, myScript.color_04);
        Tex_05 = MakeTex(2, 2, myScript.color_05);
        #endregion
    }

    public override void OnInspectorGUI()
    {
        #region
        if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
            DrawDefaultInspector();

        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("See Inspector :", GUILayout.Width(85));
        EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Blue_Strong = new GUIStyle(GUI.skin.box); style_Blue_Strong.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;


        displayInputs(style_Blue_Strong, style_Blue);

        EditorGUILayout.LabelField("");


        //
        editorModeDisplayed.intValue = GUILayout.Toolbar(editorModeDisplayed.intValue, toolbarStrings);


        if (editorModeDisplayed.intValue == 0)
        {
            EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.LabelField("Focus Mode", EditorStyles.boldLabel);
            //EditorGUILayout.HelpBox("Those methods are called only when the player enter or exit from a puzzle with Focus Mode activated (Focus Mode).", MessageType.Info);
            displayAllTheMethods_FocusStarts(style_Yellow_Strong, style_Yellow_01);
            displayAllTheMethods_FocusEnded(style_Yellow_Strong, style_Yellow_01);
            EditorGUILayout.EndVertical();
        }
        else if (editorModeDisplayed.intValue == 1)
        {
            EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.LabelField("Mode: VR Raycast/ VR Grab", EditorStyles.boldLabel);
            //EditorGUILayout.HelpBox("Those methods are called only when the player enter or exit from a puzzle without Focus Mode activated (Mode VR Raycast/ VR Hand / Free Mode: Reticule).", MessageType.Info);
            displayAllTheMethods_NoFocusStarts(style_Yellow_Strong, style_Yellow_01);
            displayAllTheMethods_NoFocusEnded(style_Yellow_Strong, style_Yellow_01);
            EditorGUILayout.EndVertical();
        }
        else{
            EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.LabelField("Reticule Mode", EditorStyles.boldLabel);
            // EditorGUILayout.HelpBox("Those methods are called only when the player enter or exit from a puzzle without Focus Mode activated (Mode VR Raycast/ VR Hand / Free Mode: Reticule).", MessageType.Info);
            displayAllTheMethods_NoFocusStartsReticule(style_Yellow_Strong, style_Yellow_01);
            displayAllTheMethods_NoFocusEndedReticule(style_Yellow_Strong, style_Yellow_01);
            EditorGUILayout.EndVertical(); 
        }


        EditorGUILayout.LabelField("");

        displayReticuleOptions(style_Blue_Strong, style_Blue); 

        EditorGUILayout.LabelField("");

        otherOptions(style_Yellow_Strong, style_Yellow_01); 

        _Help(0);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.LabelField("");
        #endregion
    }

    //--> display a list of methods call when the Focus starts
    private void displayAllTheMethods_FocusStarts(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when enter a puzzle:",
                                       editorMethods,
                                       methodsList,
                                       myScript.methodsList,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call when the Focus Ended
    private void displayAllTheMethods_FocusEnded(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when exit the puzzle:",
                                       editorMethods,
                                       methodsListExitPuzzle,
                                       myScript.methodsListExitPuzzle,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call when the Focus starts
    private void displayAllTheMethods_NoFocusStarts(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when enter a puzzle:",
                                       editorMethods,
                                       methodsListEnterPuzzleNoFocus,
                                       myScript.methodsListEnterPuzzleNoFocus,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call when the Focus Ended
    private void displayAllTheMethods_NoFocusEnded(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when exit the puzzle:",
                                       editorMethods,
                                       methodsListExitPuzzleNoFocus,
                                       myScript.methodsListExitPuzzleNoFocus,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call when the Focus starts
    private void displayAllTheMethods_NoFocusStartsReticule(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when enter a puzzle:",
                                       editorMethods,
                                       methodsListEnterPuzzleNoFocusReticule,
                                       myScript.methodsListEnterPuzzleNoFocusReticule,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call when the Focus Ended
    private void displayAllTheMethods_NoFocusEndedReticule(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when exit the puzzle:",
                                       editorMethods,
                                       methodsListExitPuzzleNoFocusReticule,
                                       myScript.methodsListExitPuzzleNoFocusReticule,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call to check VR Validation Get key down
    private void displayAllTheMethods_VRValidationDown(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayOneMethod("Methods call to check VR Validation Button Down:",
                                       editorMethods,
                                       methodsListVRValidationDown,
                                       myScript.methodsListVRValidationDown,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods call to check VR Validation get key Up
    private void displayAllTheMethods_VRValidationUp(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayOneMethod("Methods call to check VR Validation Button Up:",
                                       editorMethods,
                                       methodsListVRValidationUp,
                                       myScript.methodsListVRValidationUp,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }


    //--> display a list of methods call to check VR Back
    private void displayAllTheMethods_VBack(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        methodModule.displayOneMethod("Methods call to check VR Back Button Down:",
                                       editorMethods,
                                       methodsListVRBackDown,
                                       myScript.methodsListVRBackDown,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    //--> display a list of methods
    private void displayInputs(GUIStyle style_Color01, GUIStyle style_Color02)
    {
        #region
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        EditorGUILayout.BeginVertical(style_Color02);
        EditorGUILayout.BeginVertical(style_Color01);
        EditorGUILayout.LabelField("Inputs List", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.HelpBox("This section display all the inputs depending the platform.", MessageType.Info);

        //-> Select Input type when the game starts
        EditorGUILayout.LabelField("Select Input type when the game starts", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        if(b_DesktopInputs.boolValue && !b_Joystick.boolValue)
            GUI.backgroundColor = _cGreen;
        if (GUILayout.Button("Keyboard + Mouse"))
        {
            b_DesktopInputs.boolValue = true;
            b_Joystick.boolValue = false;
        }
        GUI.backgroundColor = _cGray;
        if (b_DesktopInputs.boolValue && b_Joystick.boolValue)
            GUI.backgroundColor = _cGreen;
        if (GUILayout.Button("Gamepad"))
        {
            b_DesktopInputs.boolValue = true;
            b_Joystick.boolValue = true;
        }
        GUI.backgroundColor = _cGray;
        if (!b_DesktopInputs.boolValue && !b_Joystick.boolValue)
            GUI.backgroundColor = _cGreen;
        if (GUILayout.Button("Mobile"))
        {
            b_DesktopInputs.boolValue = false;
            b_Joystick.boolValue = false;
        }

        GUI.backgroundColor = _cGray;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");

       



        //-> Desktop Inputs
        EditorGUILayout.BeginVertical(style_Color01);
        EditorGUILayout.LabelField("Keyboard + Mouse Inputs:", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        //-> Reset Desktop Inputs
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Reset Inputs:", GUILayout.Width(120));
        if (myScript.validationButtonKeyboard == KeyCode.Mouse0 &&
            myScript.backButtonKeyboard == KeyCode.Mouse1 
           )
            GUI.backgroundColor = _cGreen;
        if (GUILayout.Button("Mac and PC"))
        {
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
            myScript.validationButtonKeyboard = KeyCode.Mouse0;
            myScript.backButtonKeyboard = KeyCode.Mouse1;
        }
        GUI.backgroundColor = _cGray;
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cursor:", GUILayout.Width(120));
        EditorGUILayout.LabelField("Mouse Movement", GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Validation (button):", GUILayout.Width(120));
        EditorGUILayout.PropertyField(validationButtonKeyboard, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Back (Button):", GUILayout.Width(120));
        EditorGUILayout.PropertyField(backButtonKeyboard, new GUIContent(""));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.LabelField("");

        //-> Gamepad Inputs
        EditorGUILayout.BeginVertical(style_Color01);
        EditorGUILayout.LabelField("Gamepad Inputs:", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        //-> Reset Gamepad Inputs
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Reset Inputs:", GUILayout.Width(120));
        if (myScript.validationButtonJoystick == KeyCode.JoystickButton16 &&
            myScript.backButtonJoystick == KeyCode.JoystickButton17&&
            myScript.HorizontalAxisJoystickLeft == "Horizontal" &&
            myScript.verticalAxisJoystickLeft == "Vertical"
           )
            GUI.backgroundColor = _cGreen;
        if (GUILayout.Button("Mac"))
        {
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
           // myScript.HorizontalAxisJoystickLeft = "Horizontal";
           // myScript.verticalAxisJoystickLeft = "Vertical";
            //myScript.validationButtonJoystick = KeyCode.JoystickButton16;
            //myScript.backButtonJoystick = KeyCode.JoystickButton17;

            HorizontalAxisJoystickLeft.stringValue = "Horizontal";
            verticalAxisJoystickLeft.stringValue = "Vertical";

            validationButtonJoystick.enumValueIndex = 162;
            backButtonJoystick.enumValueIndex = 163;


        }
        GUI.backgroundColor = _cGray;
        if (myScript.validationButtonJoystick == KeyCode.JoystickButton0 &&
            myScript.backButtonJoystick == KeyCode.JoystickButton1 &&
            myScript.HorizontalAxisJoystickLeft == "Horizontal" &&
            myScript.verticalAxisJoystickLeft == "Vertical"
           )
            GUI.backgroundColor = _cGreen;
        if (GUILayout.Button("PC"))
        {
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
           // myScript.HorizontalAxisJoystickLeft = "Horizontal";
           // myScript.verticalAxisJoystickLeft = "Vertical";
            // myScript.validationButtonJoystick = KeyCode.JoystickButton0;
            // myScript.backButtonJoystick = KeyCode.JoystickButton1;

            //Debug.Log(validationButtonJoystick.GetEndProperty());

            HorizontalAxisJoystickLeft.stringValue = "Horizontal";
            verticalAxisJoystickLeft.stringValue = "Vertical";

            validationButtonJoystick.enumValueIndex = 146;
            backButtonJoystick.enumValueIndex = 147;

        }
        GUI.backgroundColor = _cGray;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Horizontal Axis:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(HorizontalAxisJoystickLeft, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Vertical Axis:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(verticalAxisJoystickLeft, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Validation (button):", GUILayout.Width(120));
        EditorGUILayout.PropertyField(validationButtonJoystick, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Back (Button):", GUILayout.Width(120));
        EditorGUILayout.PropertyField(backButtonJoystick, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(style_Color02);
        EditorGUILayout.LabelField("VR Inputs", EditorStyles.boldLabel);
        // EditorGUILayout.HelpBox("Those methods are called only when the player enter or exit from a puzzle without Focus Mode activated (Mode VR Raycast/ VR Hand / Free Mode: Reticule).", MessageType.Info);
        displayAllTheMethods_VRValidationDown(style_Color02, style_Color01);
        displayAllTheMethods_VRValidationUp(style_Color02, style_Color01);
        displayAllTheMethods_VBack(style_Color02, style_Color01);
        EditorGUILayout.EndVertical();

        #endregion
    }


    //--> display a list of methods
    private void displayReticuleOptions(GUIStyle style_Color01, GUIStyle style_Color02)
    {
        #region
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        EditorGUILayout.BeginVertical(style_Color02);
        EditorGUILayout.BeginVertical(style_Color01);
        EditorGUILayout.LabelField("Icon and Reticule Options", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        //EditorGUILayout.HelpBox("This section display all the inputs depending the platform.", MessageType.Info);

        //-> Select Input type when the game starts
       
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.EndHorizontal();

        //


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grp Reticule Joystick:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(_joystickReticule, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Joystick Fake Mouse:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(reticuleJoystickImage, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Puzzle Icon Center:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(iconPuzzle, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Reticule:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(reticule, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Activate or Deactivate:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Puzzle Icon Center:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(b_iconPuzzle, new GUIContent(""), GUILayout.Width(20));
        EditorGUILayout.LabelField("(Only for puzzle with Focus)");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Btn Right Up Corner:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(b_iconPuzzleMobile, new GUIContent(""), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Reticule:", GUILayout.Width(120));
        EditorGUILayout.PropertyField(b_Reticule, new GUIContent(""), GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();

        #endregion
    }
   

    private void otherOptions(GUIStyle style_Color01, GUIStyle style_Color02){
        
#region
        AP_GlobalPuzzleManager_Pc myScript = (AP_GlobalPuzzleManager_Pc)target;

        EditorGUILayout.BeginVertical(style_Color02);
        EditorGUILayout.BeginVertical(style_Color01);
        EditorGUILayout.LabelField("Other Options", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Parent Drag And Drop:", GUILayout.Width(150));
        EditorGUILayout.PropertyField(dragAndDropParent, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

       /* EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Puzzles are paused:", GUILayout.Width(150));
        EditorGUILayout.PropertyField(b_Pause, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        */
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CursorLockMode:", GUILayout.Width(150));
        EditorGUILayout.PropertyField(b_AutoLockCursorWhenGameStarts, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("When CursorLockMode is checked cursor is locked to the center of the screen automatically", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Main Camera:", GUILayout.Width(150));
        EditorGUILayout.PropertyField(ap_MainCamera, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("By default the main camera is the camera tagged MainCamera. Drag and drop an other camera in the empty field to specify which camera is the main camera.", MessageType.Info);


        EditorGUILayout.EndVertical();

        #endregion
    } 


    void _Help(int value){
        #region
        switch (value){
            case 0:
                EditorGUILayout.HelpBox("Useful methods and variables (more info in the documentation):" + 
                                        "\n\nAP_GlobalPuzzleManager.instance.AP_SwitchPuzzleInputsToKeyboardAndMouse(true);" +
                                "\nAP_GlobalPuzzleManager.instance.AP_SwitchPuzzleInputsToGamepad(true);" +
                                "\nAP_GlobalPuzzleManager.instance.AP_SwitchPuzzleInputsToMobile();" +
                                        "\n\nHorizontalAxisJoystickLeft" +
                                        "\nVerticalAxisJoystickLeft" +
                                        "\nvalidationButtonJoystick" +
                                        "\nbackButtonJoystick" +
                                        "\n\nvalidationButtonKeyboard" +
                                        "\nbackButtonKeyboard" +
                                        "\n\nb_Pause", MessageType.Info);
                break;
        }

        #endregion
    }

    void OnSceneGUI()
    {
    }
}
#endif
