// Description : Custom Editor for GearsPuzzle_Pc.cs
#if (UNITY_EDITOR)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(GearsPuzzle_Pc))]
public class GearsPuzzleEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector

    SerializedProperty GearType;
    SerializedProperty GearsUseOrFakeList;

    SerializedProperty GearsInitPositionWhenStart;
    SerializedProperty GearsAvailableWhenStart;
    SerializedProperty AxisAvailableWhenStart;

    SerializedProperty GearsSolutionList; 

    SerializedProperty _Column;
    SerializedProperty pivotGearList;
    SerializedProperty GearList;
    SerializedProperty toolbarCurrentValue;
    SerializedProperty SquareSize;
    SerializedProperty currentSelectedSprite;
    SerializedProperty GearsPositionList;

    SerializedProperty GearsTypeList;
    SerializedProperty AxisTypeList;

    SerializedProperty AxisRotationRight;

    SerializedProperty selectDefaultTile;
 
    SerializedProperty helpBoxEditor;

    SerializedProperty a_KeyPressed;
    SerializedProperty a_KeyPressedVolume;
    SerializedProperty a_Reset;
    SerializedProperty a_ResetVolume;


    SerializedObject serializedObject3;

    SerializedProperty aP_PuzzleDetector;

    SerializedProperty cameraUseForFocus;
    SerializedProperty methodsList;
    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;


    public Color _cBlue = new Color(0f, .9f, 1f, .5f);
    public Color _cRed = new Color(1f, .5f, 0f, .5f);
    public Color _cGray = new Color(.9f, .9f, .9f, 1);
    public Color _cGreen = Color.green;


    public Transform spriteTransform;   // Use to display key sprite
    public Text tmpText;        // use to display Key Text

    public string[] toolbarStrings = new string[] { "Puzzle Creation", "Puzzle Init Position", "Game Options" };

    public string[] GearsTypeStrings             = new string[] { "Empty", "Vertical","T","Elbow","No Move"};

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };


    public AP_PuzzleDetector_Pc objPuzzleDetetor;

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

    public EditorManipulate2DTexture_Pc manipulate2DTex;

    void OnEnable()
    {
        #region
        manipulate2DTex             = new EditorManipulate2DTexture_Pc();
        // Setup the SerializedProperties.
        SeeInspector                = serializedObject.FindProperty("SeeInspector");
        helpBoxEditor               = serializedObject.FindProperty("helpBoxEditor");

        GearsPuzzle_Pc myScript = (GearsPuzzle_Pc)target;

        GearType                    = serializedObject.FindProperty("GearType");
        GearsUseOrFakeList          = serializedObject.FindProperty("GearsUseOrFakeList");

        GearsInitPositionWhenStart  = serializedObject.FindProperty("GearsInitPositionWhenStart");
        GearsAvailableWhenStart     = serializedObject.FindProperty("GearsAvailableWhenStart");
        AxisAvailableWhenStart      = serializedObject.FindProperty("AxisAvailableWhenStart");

        GearsSolutionList           = serializedObject.FindProperty("GearsSolutionList");
        _Column                     = serializedObject.FindProperty("_Column");
        toolbarCurrentValue         = serializedObject.FindProperty("toolbarCurrentValue");
        SquareSize                  = serializedObject.FindProperty("SquareSize");

        pivotGearList               = serializedObject.FindProperty("pivotGearList");
        GearList                    = serializedObject.FindProperty("GearList");

        currentSelectedSprite       = serializedObject.FindProperty("currentSelectedSprite");
        GearsPositionList           = serializedObject.FindProperty("GearsPositionList");

        GearsTypeList               = serializedObject.FindProperty("GearsTypeList");
        AxisTypeList                = serializedObject.FindProperty("AxisTypeList");

        AxisRotationRight           = serializedObject.FindProperty("AxisRotationRight");

     
        selectDefaultTile           = serializedObject.FindProperty("selectDefaultTile");

        a_KeyPressed                = serializedObject.FindProperty("a_KeyPressed");
        a_KeyPressedVolume          = serializedObject.FindProperty("a_KeyPressedVolume");
        a_Reset                     = serializedObject.FindProperty("a_Reset");
        a_ResetVolume               = serializedObject.FindProperty("a_ResetVolume");

        aP_PuzzleDetector           = serializedObject.FindProperty("aP_PuzzleDetector");

        cameraUseForFocus = serializedObject.FindProperty("cameraUseForFocus");
        methodsList = serializedObject.FindProperty("methodsList");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();


        /*Transform[] children = myScript.gameObject.transform.parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == "PuzzleDetector")
                objPuzzleDetetor = child.GetComponent<AP_PuzzleDetector>();
        }
    */
   

        Tex_01 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_02 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_03 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_04 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_05 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
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
        EditorGUILayout.LabelField("See Help Boxes :", GUILayout.Width(85));
        EditorGUILayout.PropertyField(helpBoxEditor, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

        GearsPuzzle_Pc myScript = (GearsPuzzle_Pc)target;


        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Puzzle could not be edited in play mode", MessageType.Info);
        }
        else
        {
            // --> Display Tab sections in the Inspector
            EditorGUI.BeginChangeCheck();
            toolbarCurrentValue.intValue = GUILayout.Toolbar(toolbarCurrentValue.intValue, toolbarStrings);

            bool b_TilesExist = true;
            if (pivotGearList.arraySize > 0)
            {

                for (var i = 0; i < pivotGearList.arraySize; i++)
                {
                    if (pivotGearList.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    {
                        b_TilesExist = false;
                        break;
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (toolbarCurrentValue.intValue == 0){
                    loadGearsPosition(myScript,  0);
                }
                  
                if (toolbarCurrentValue.intValue ==  1){
                    loadGearsPosition(myScript, 1);
                }
            }
        
            // --> Display GeneratePuzzleSection
            if (toolbarCurrentValue.intValue == 0 && GearsTypeList.arraySize == 0)
                displayGeneratePuzzleSection(myScript, style_Orange);

            // --> Display Other Section
            if (toolbarCurrentValue.intValue == 2)
                otherSection(myScript, style_Orange);


            if (pivotGearList.arraySize > 0)
            {

                if (b_TilesExist)
                {
                    // --> Display Select Sprites
                    if (toolbarCurrentValue.intValue == 1)
                        displaySelectSpriteSection(myScript, style_Blue);
                    // --> Display Mix Section
                    if (toolbarCurrentValue.intValue == 0)
                        displaySolutionSection(myScript, style_Yellow_01);
                }
                else
                {
                    if (toolbarCurrentValue.intValue == 0 || toolbarCurrentValue.intValue == 1)
                        puzzleNeedToBeGenerated();
                }


            }

        }

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.LabelField("");
        #endregion
    }

    private void puzzleNeedToBeGenerated()
    {
        EditorGUILayout.HelpBox("You need to generate the puzzle first.", MessageType.Error);
    }

    private void otherSection(GearsPuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Section : Other Options.", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Camera Use for focus : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(cameraUseForFocus, new GUIContent(""));
        string camState = "Test";
        Camera refObj = (Camera)cameraUseForFocus.objectReferenceValue;
        if (refObj.gameObject.activeInHierarchy)
        {
            camState = "Stop";
        }

        if (GUILayout.Button(camState, GUILayout.Width(70)))
        {
            SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.cameraUseForFocus.gameObject);
            SerializedProperty m_IsActive = serializedObject4.FindProperty("m_IsActive");
            serializedObject4.Update();
            if (camState == "Test")
            {
                camState = "Stop";
                m_IsActive.boolValue = true;
                Selection.activeTransform = myScript.cameraUseForFocus.transform.parent;
            }
            else if (camState == "Stop")
            {
                camState = "Test";
                m_IsActive.boolValue = false;
            }
           
            serializedObject4.ApplyModifiedProperties();
        }
            EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Puzzle Detetcor : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(aP_PuzzleDetector, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Play Audio when Gears is pressed : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(a_KeyPressed, new GUIContent(""), GUILayout.Width(100));
        GUILayout.Label("Volume : ", GUILayout.Width(60));
        a_KeyPressedVolume.floatValue = EditorGUILayout.Slider(a_KeyPressedVolume.floatValue, 0, 1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Play Audio when puzzle is Reset : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(a_Reset, new GUIContent(""), GUILayout.Width(100));
        GUILayout.Label("Volume : ", GUILayout.Width(60));
        a_ResetVolume.floatValue = EditorGUILayout.Slider(a_ResetVolume.floatValue, 0, 1);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("");
        PuzzleDetection(style_Orange, style_Orange);
        GUILayout.Label("");

        displayFirstTimePuzzle(style_Orange, style_Orange);

        EditorGUILayout.EndVertical();
        #endregion
    }

    private void PuzzleDetection(GUIStyle _color_01, GUIStyle _color_02)
    {
        #region
        EditorGUILayout.BeginVertical(_color_01);

        GearsPuzzle_Pc myScript = (GearsPuzzle_Pc)target;

        serializedObject3 = new UnityEditor.SerializedObject(myScript.gameObject.GetComponent<AP_.DragAndDrop_Pc>());

        SerializedProperty dragAndDropMode = serializedObject3.FindProperty("dragAndDropMode");
        SerializedProperty distanceFromTheCamera = serializedObject3.FindProperty("distanceFromTheCamera");
        SerializedProperty a_TakeObject = serializedObject3.FindProperty("a_TakeObject");

        serializedObject3.Update();

        GUILayout.Label("Puzzle Detection Options:", EditorStyles.boldLabel);
        /*if (helpBoxEditor.boolValue)
        {
            EditorGUILayout.HelpBox("Desktop: (0) Focus Mode and (3) Free Mode: Reticule" +
                                    "\nVR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand", MessageType.Info);

            EditorGUILayout.HelpBox("VR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand doesn't work on Mobile", MessageType.Warning);
        }*/


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Drag and Drop Mode: ", GUILayout.Width(170));
        dragAndDropMode.intValue = EditorGUILayout.Popup(dragAndDropMode.intValue, listDragAndDropMode);
        EditorGUILayout.EndHorizontal();


        if (dragAndDropMode.intValue == 3)
        {
            Transform[] allChildren = myScript.gameObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<AP_PuzzleDetector_Pc>())
                {
                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(child.GetComponent<AP_PuzzleDetector_Pc>());
                    SerializedProperty b_ReticuleState = serializedObject4.FindProperty("b_ReticuleState");
                    serializedObject4.Update();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Reticule : ", GUILayout.Width(170));
                    EditorGUILayout.PropertyField(b_ReticuleState, new GUIContent(""), GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();

                    serializedObject4.ApplyModifiedProperties();
                    break;
                }
            }
        }


        if (aP_PuzzleDetector.objectReferenceValue)
        {
            SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.GetComponent<AP_PuzzleDetector_Pc>());
            SerializedProperty b_FocusActivated = serializedObject4.FindProperty("b_FocusActivated");

            serializedObject4.Update();

            if (dragAndDropMode.intValue == 0 && !b_FocusActivated.boolValue)
                b_FocusActivated.boolValue = true;
            else if ((dragAndDropMode.intValue != 0) && b_FocusActivated.boolValue)
                b_FocusActivated.boolValue = false;

            if (b_FocusActivated.boolValue)
                GUILayout.Label("(This puzzle use focus.)");
            else
                GUILayout.Label("(This puzzle do not use focus.)");


            serializedObject4.ApplyModifiedProperties();
        }

        if (dragAndDropMode.intValue == 0)
        {
            EditorGUILayout.BeginVertical(_color_01);
            EditorGUILayout.HelpBox("Select local Z distance between the selected object and the camera.", MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Distance from the camera: ", GUILayout.Width(170));
            EditorGUILayout.PropertyField(distanceFromTheCamera, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();


            EditorGUILayout.HelpBox("Play a sound when an Object is selected or deselected.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("AudioSource : ", GUILayout.Width(170));
            EditorGUILayout.PropertyField(a_TakeObject, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("");
        }

        updatePuzlleDetectorPosition(dragAndDropMode.intValue);

        serializedObject3.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
        #endregion
    }

    private void displayGeneratePuzzleSection(GearsPuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.HelpBox("Section : Puzzle Creation." +
                                "\n\n-Press Button ''Generate the first Gear'' to start creating the puzzle.", MessageType.Info);

        EditorGUILayout.BeginVertical(style_Orange);
      
      
        if (GUILayout.Button("Generate the first Gear"))
        {
            GenerateKeys(myScript, 1, false);
        }
       
        EditorGUILayout.EndVertical();
        #endregion
    }

    private void displaySolutionSection(GearsPuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);

        EditorGUILayout.HelpBox("Section : Puzzle Creation. " +
                                "\n\n1-Select a Gear Type in the list by clicking on it." +
                                "\n2-Click on a tile in the table to apply it." +
                                "\n" +
                                "\n3-If you want to use the gear Axis in the solution the button bellow the gear button need to display ''Use''" +
                                "\n4-If you want to use the gear Axis as a fake Axis the button bellow the gear button need to display ''Fake''" +
                                "\n" 
                                , MessageType.Info);

        //_helpBox(2);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Gear"))
        {
            GenerateKeys(myScript, 1, false);
        }
       
        if (GUILayout.Button("Delete all Gears"))
        {
            DeleteGear(myScript, 0);
        }

        EditorGUILayout.EndHorizontal();

        //-> Diameter 1
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pivot Size ", GUILayout.Width(60));
        EditorGUILayout.LabelField("| Gear Size ", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("1 ->", GUILayout.Width(30));
        GUILayout.Label(myScript.GearSprite[0], GUILayout.Width(30), GUILayout.Height(30));
        GUI.backgroundColor = returnBackgroundColor(5);
        if (GUILayout.Button(myScript.GearSprite[5], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 5;
        }
        GUI.backgroundColor = returnBackgroundColor(6);
        if (GUILayout.Button(myScript.GearSprite[6], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 6;
        }
        GUI.backgroundColor = returnBackgroundColor(7);
        if (GUILayout.Button(myScript.GearSprite[7], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 7;
        }

       
        GUI.backgroundColor = _cGray;
        EditorGUILayout.EndHorizontal();

        //-> Diameter 2

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("2 ->", GUILayout.Width(30));
        GUILayout.Label(myScript.GearSprite[1], GUILayout.Width(30), GUILayout.Height(30));
        GUI.backgroundColor = returnBackgroundColor(8);
        if (GUILayout.Button(myScript.GearSprite[8], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 8;
        }
        GUI.backgroundColor = returnBackgroundColor(9);
        if (GUILayout.Button(myScript.GearSprite[9], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 9;
        }
        GUI.backgroundColor = returnBackgroundColor(10);
        if (GUILayout.Button(myScript.GearSprite[10], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 10;
        }


        GUI.backgroundColor = _cGray;
        EditorGUILayout.EndHorizontal();

        //-> Diameter 3

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("3 ->", GUILayout.Width(30));
        GUILayout.Label(myScript.GearSprite[2], GUILayout.Width(30), GUILayout.Height(30));
        EditorGUILayout.LabelField("", GUILayout.Width(30));
        GUI.backgroundColor = returnBackgroundColor(11);
        if (GUILayout.Button(myScript.GearSprite[11], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 11;
        }
        GUI.backgroundColor = returnBackgroundColor(12);
        if (GUILayout.Button(myScript.GearSprite[12], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 12;
        }
        GUI.backgroundColor = returnBackgroundColor(13);
        if (GUILayout.Button(myScript.GearSprite[13], GUILayout.Width(30), GUILayout.Height(30)))
        {
            GearType.intValue = 13;
        }
        EditorGUILayout.EndHorizontal();

            //-> Diameter 4

            EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("4 ->", GUILayout.Width(30));
            GUILayout.Label(myScript.GearSprite[3], GUILayout.Width(30), GUILayout.Height(30));
            EditorGUILayout.LabelField("", GUILayout.Width(64));
            GUI.backgroundColor = returnBackgroundColor(14);
        if (GUILayout.Button(myScript.GearSprite[14], GUILayout.Width(30), GUILayout.Height(30)))
            {
                GearType.intValue = 14;
            }
            GUI.backgroundColor = returnBackgroundColor(15);
        if (GUILayout.Button(myScript.GearSprite[15], GUILayout.Width(30), GUILayout.Height(30)))
            {
                GearType.intValue = 15;
            }
            EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = _cGray;

        EditorGUILayout.EndVertical();

        displayKeysInTheInspector(myScript, style_Yellow_01, 1);
        #endregion
    }

    private Color returnBackgroundColor(int value)
    {
        #region
        if (GearType.intValue == value)
            return _cBlue;
        else
            return _cGray;
        #endregion
    }

    //--> return puzzle is solved
    private void returnIfPuzzleIsSolved(){
        //Debug.Log("Here");
    }


    private void displaySelectSpriteSection(GearsPuzzle_Pc myScript, GUIStyle style_Blue)
    {
        #region
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Section : Puzzle Initial Position. " +
                                "\n\n1-Choose the gear rotation by clicking the button ''Left Rot/Right Rot''." +
                                "\n2-Choose if the gear position is the same as his axis by clicking the button ''Axis Pos/Init Pos''." +
                                "\n3-Deactivate/Activate a Axis by clicking the button ''Axis on/Axis Off''." +
                                "\n4-Deactivate/Activate a Gear by clicking the button ''Gear on/Gear Off''." +
                                "\n", MessageType.Info);
        
        EditorGUILayout.EndVertical();
       
        displayKeysInTheInspector(myScript, style_Blue, 0);
        #endregion
    }

    //--> Display square that represent puzzle object
    public void displayKeysInTheInspector(GearsPuzzle_Pc myScript, GUIStyle style_Blue, int WhichSection)
    {
        #region
        EditorGUILayout.LabelField("");

        int number = 0;

            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < GearsPositionList.arraySize; j++)
            {
                tmpText = null;
            if (pivotGearList.arraySize > number)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(SquareSize.intValue));
                EditorGUILayout.BeginVertical();

                if (GUILayout.Button(myScript.GearSprite[GearsTypeList.GetArrayElementAtIndex(number).intValue], GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                //if (GUILayout.Button(myScript.GearSprite[GearsTypeList.GetArrayElementAtIndex(number).intValue], GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                {
                    Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();
                    foreach (Transform child in ts)
                    {
                        if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == number)
                        {
                            //objPIVOT = child;
                        }
                    }
                    currentSelectedSprite.intValue = number;

                    if (WhichSection == 1)          //-> Section : Puzzle Creation
                    {
                        GearsTypeList.GetArrayElementAtIndex(number).intValue = GearType.intValue;
                        ReplaceGrear(myScript, number, GearsTypeList.GetArrayElementAtIndex(number).intValue);
                    }

                    if (WhichSection == 0)          //-> Section : Puzzle Init Position
                    {
                        
                    }
                }


                if (WhichSection == 1)          //-> Section : Puzzle Creation
                {
                   // EditorGUILayout.BeginHorizontal();
                        string s_GearsUseOrFakeListState = "Fake";
                        if (!GearsUseOrFakeList.GetArrayElementAtIndex(number).boolValue)
                            s_GearsUseOrFakeListState = "Use";

                        if (GUILayout.Button(s_GearsUseOrFakeListState, GUILayout.Width(SquareSize.intValue), GUILayout.Height(20)))
                        {
                            if (GearsUseOrFakeList.GetArrayElementAtIndex(number).boolValue){           // Fake
                                GearsUseOrFakeList.GetArrayElementAtIndex(number).boolValue = false;
                                AxisAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = true;
                                GearsAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = true;
                            }
                            else{                                                                       // use
                                GearsUseOrFakeList.GetArrayElementAtIndex(number).boolValue = true;
                                AxisAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = true;
                                GearsAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = true;
                            }
                            activateDeactivateGear(myScript, number);
                            activateDeactivateAxis(myScript, number);
                        }
                }
                if (WhichSection == 0)          //-> Section : Puzzle Init Position
                {

                    string tmpSting = "";

                    if (AxisRotationRight.GetArrayElementAtIndex(number).boolValue)
                        tmpSting = "Right Rot";
                    else
                        tmpSting = "Left Rot";

                    if (GUILayout.Button(tmpSting, GUILayout.Width(SquareSize.intValue), GUILayout.Height(20)))
                    {
                        if (AxisRotationRight.GetArrayElementAtIndex(number).boolValue)
                        {
                            AxisRotationRight.GetArrayElementAtIndex(number).boolValue = false;

                        }
                        else
                        {
                            AxisRotationRight.GetArrayElementAtIndex(number).boolValue = true;

                        }
                    }

                    tmpSting = "";

                        if (GearsInitPositionWhenStart.GetArrayElementAtIndex(number).boolValue)
                            tmpSting = "Init Pos";
                        else
                            tmpSting = "Axis Pos";

                        if (GUILayout.Button(tmpSting, GUILayout.Width(SquareSize.intValue), GUILayout.Height(20)))
                        {
                        if (GearsInitPositionWhenStart.GetArrayElementAtIndex(number).boolValue){
                            GearsInitPositionWhenStart.GetArrayElementAtIndex(number).boolValue = false; 

                        }
                        else{
                            GearsInitPositionWhenStart.GetArrayElementAtIndex(number).boolValue = true;
  
                        }

                            loadGearsPosition(myScript, 1);
                        }


                    if (GearsUseOrFakeList.GetArrayElementAtIndex(number).boolValue)
                    {
                        if (AxisAvailableWhenStart.GetArrayElementAtIndex(number).boolValue)
                            tmpSting = "Axis On";
                        else
                            tmpSting = "Axis Off";
                        if (GUILayout.Button(tmpSting, GUILayout.Width(SquareSize.intValue), GUILayout.Height(20)))
                        {
                            if (AxisAvailableWhenStart.GetArrayElementAtIndex(number).boolValue)
                                AxisAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = false;
                            else
                                AxisAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = true;

                            activateDeactivateAxis(myScript, number);

                        }

                        if (GearsAvailableWhenStart.GetArrayElementAtIndex(number).boolValue)
                            tmpSting = "Gear On";
                        else
                            tmpSting = "Gear Off";
                        if (GUILayout.Button(tmpSting, GUILayout.Width(SquareSize.intValue), GUILayout.Height(20)))
                        {
                            if (GearsAvailableWhenStart.GetArrayElementAtIndex(number).boolValue)
                                GearsAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = false;
                            else
                                GearsAvailableWhenStart.GetArrayElementAtIndex(number).boolValue = true;

                            activateDeactivateGear(myScript, number);
                        }
                    }
                }

                    EditorGUILayout.EndVertical();

                    number++;

                    EditorGUILayout.EndVertical();
                }
            }
        

            EditorGUILayout.EndHorizontal();


        #endregion
    }

    //--> display a list of methods call when the puzzle starts the first time
    private void displayFirstTimePuzzle(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        GearsPuzzle_Pc myScript = (GearsPuzzle_Pc)target;

        methodModule.displayMethodList("Actions when the puzzle starts the first time:",
                                       editorMethods,
                                       methodsList,
                                       myScript.methodsList,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Read docmentation for more info the methods allowed.");

        #endregion
    }

    private void activateDeactivateAxis(GearsPuzzle_Pc myScript, int selectedObj)
    {
        #region
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);


        foreach (Transform child in ts)
        {
            if (child != null && child.name == selectedObj.ToString() && child.parent.name.Contains("Axis") && child != myScript.transform)
            {
                Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
                if (AxisAvailableWhenStart.GetArrayElementAtIndex(selectedObj).boolValue)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false); 
            }
        }
        #endregion
    }

    private void activateDeactivateGear(GearsPuzzle_Pc myScript, int selectedObj)
    {
        #region
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in ts)
        {
            if (child != null && child.name == selectedObj.ToString() && child.parent.name.Contains("Gears") && child != myScript.transform)
            {
                Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
                if (GearsAvailableWhenStart.GetArrayElementAtIndex(selectedObj).boolValue)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);
            }
        }
        #endregion
    }

    private void ReplaceGrear(GearsPuzzle_Pc myScript,int selectedGear,int newGear){
        #region

        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);

        GameObject tmpGear = null;
        GameObject tmpAxis = null;

        foreach (Transform child in ts)
        {
            if (child != null && child.name == selectedGear.ToString() && child.parent.name.Contains("Gears") && child != myScript.transform){
                GameObject newTile = Instantiate(myScript.defaultTileList[newGear], child.parent.gameObject.transform);
                newTile.name = selectedGear.ToString();
                Undo.RegisterCreatedObjectUndo(newTile, newTile.name);
                Undo.DestroyObjectImmediate(child.gameObject);
                tmpGear = newTile;

                GearList.GetArrayElementAtIndex(selectedGear).objectReferenceValue = newTile.transform.gameObject;
            }
             
            if (child != null && child.name == selectedGear.ToString() && child.parent.name.Contains("Axis") && child != myScript.transform)
            {
                int gearDiamNumber = 0;                                     // Gear Pivot Size 1
                if (newGear == 8 || newGear == 9 || newGear == 10)          // Gear Pivot Size 2
                {    
                    gearDiamNumber = 1;
                    AxisTypeList.GetArrayElementAtIndex(selectedGear).intValue = 2;
                }
                else if (newGear == 11 || newGear == 12 || newGear == 13){  // Gear Pivot Size 3
                    gearDiamNumber = 2;
                    AxisTypeList.GetArrayElementAtIndex(selectedGear).intValue = 3;
                }   
                else if (newGear == 14 || newGear == 15){                   // Gear Pivot Size 4
                    gearDiamNumber = 3;
                    AxisTypeList.GetArrayElementAtIndex(selectedGear).intValue = 4;
                }
                else{
                    AxisTypeList.GetArrayElementAtIndex(selectedGear).intValue = 1;
                }
                    

                GameObject newTile = Instantiate(myScript.defaultTileList[gearDiamNumber], child.parent.gameObject.transform);
                newTile.name = selectedGear.ToString();
                Undo.RegisterCreatedObjectUndo(newTile, newTile.name);
                Undo.DestroyObjectImmediate(child.gameObject);

                tmpAxis = newTile;

               
                newTile.transform.GetChild(0).GetChild(0).name = newTile.transform.GetChild(0).GetChild(0).name + "_" + selectedGear;

                pivotGearList.GetArrayElementAtIndex(selectedGear).objectReferenceValue = newTile.transform.GetChild(0).GetChild(0).gameObject;


                GearsUseOrFakeList.GetArrayElementAtIndex(selectedGear).boolValue = false;
                GearsInitPositionWhenStart.GetArrayElementAtIndex(selectedGear).boolValue = true;
                GearsAvailableWhenStart.GetArrayElementAtIndex(selectedGear).boolValue = true;
                AxisAvailableWhenStart.GetArrayElementAtIndex(selectedGear).boolValue = true;
            }
        }
        tmpGear.transform.GetChild(0).transform.position = tmpAxis.transform.position;
        #endregion
    }

    private void GenerateKeys(GearsPuzzle_Pc myScript,int howManyGear, bool b_MultipleGears)
    {
        #region
        currentSelectedSprite.intValue = 0;
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();
       
        int intListPosition = 0;
        if (pivotGearList.arraySize != 0)
            intListPosition = pivotGearList.arraySize - 1;

        pivotGearList.InsertArrayElementAtIndex(intListPosition);
        GearList.InsertArrayElementAtIndex(intListPosition);

        GearsTypeList.InsertArrayElementAtIndex(intListPosition);
        AxisTypeList.InsertArrayElementAtIndex(intListPosition);

        AxisRotationRight.InsertArrayElementAtIndex(intListPosition);

        GearsUseOrFakeList.InsertArrayElementAtIndex(intListPosition);

        GearsInitPositionWhenStart.InsertArrayElementAtIndex(intListPosition);
        GearsAvailableWhenStart.InsertArrayElementAtIndex(intListPosition);
        AxisAvailableWhenStart.InsertArrayElementAtIndex(intListPosition);

        GearsPositionList.InsertArrayElementAtIndex(intListPosition);
        GearsSolutionList.InsertArrayElementAtIndex(intListPosition);

        string objName = "";
        GameObject tmpGear = null;
        GameObject tmpAxis = null;

        for (var k = 0; k < 2; k++)
        {
            if(k == 0){
                objName = "Gears";
                selectDefaultTile.intValue = 16; // Default Gear
            }
            if (k == 1){
                objName = "Axis";
                selectDefaultTile.intValue = 17; // Default Gear
            }

            

            int number = 0;
            int raw = 0;
            for (var i = 0; i < howManyGear; i++)
            {
                GameObject newTile = Instantiate(myScript.defaultTileList[selectDefaultTile.intValue], myScript.gameObject.transform);

                if (k == 0){
                    tmpGear = newTile;
                    newTile.transform.localPosition = new Vector3(.18f * (GearsTypeList.arraySize - 1), .2f, 0);
                }
                else{
                    tmpAxis = newTile;
                    newTile.transform.localPosition = new Vector3(.18f * (GearsTypeList.arraySize - 1), 0, 0);
                }
                       

                    if (GearsTypeList.arraySize <= 10)
                        newTile.name = objName + "_0" + (GearsTypeList.arraySize - 1);
                    else
                        newTile.name = objName + "_" + (GearsTypeList.arraySize - 1);

                    newTile.transform.GetChild(0).name = (GearsTypeList.arraySize - 1).ToString();

                    Undo.RegisterCreatedObjectUndo(newTile, newTile.name);
                    ts = newTile.GetComponentsInChildren<Transform>();

                    foreach (Transform child in ts)
                    {
                        if (child != null && child.name.Contains("PIVOT_Gear"))
                        {
                            child.name += "_" + (GearsTypeList.arraySize - 1).ToString();
                            pivotGearList.GetArrayElementAtIndex(GearsTypeList.arraySize - 1).objectReferenceValue = child.gameObject;
                        }

                        if (child != null && child.name.Contains("Gears_"))
                        {

                        GearList.GetArrayElementAtIndex(GearsTypeList.arraySize - 1).objectReferenceValue = child.transform.GetChild(0).gameObject;
                        }
                    }

                if (pivotGearList.arraySize != 1)
                    intListPosition = pivotGearList.arraySize-1;
                else
                    intListPosition = 0;
                
                GearsTypeList.GetArrayElementAtIndex(intListPosition).intValue = 15;     // Gig Gear
                AxisTypeList.GetArrayElementAtIndex(intListPosition).intValue = 4;

                if (i % _Column.intValue == _Column.intValue - 1)
                    raw++;
                number++;
            }
        }

        tmpGear.transform.GetChild(0).transform.GetChild(0).transform.position = tmpAxis.transform.position;
        #endregion
    }

    public GameObject tmpDestroyGear;

    private void DeleteGear(GearsPuzzle_Pc myScript, int gearNumber)
    {
        #region
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);

        pivotGearList.ClearArray();
        pivotGearList.ClearArray();

        GearList.ClearArray();
        GearList.ClearArray();

        GearsTypeList.ClearArray();
        AxisTypeList.ClearArray();
        AxisRotationRight.ClearArray();

        GearsUseOrFakeList.ClearArray();

        GearsInitPositionWhenStart.ClearArray();
        GearsAvailableWhenStart.ClearArray();
        AxisAvailableWhenStart.ClearArray();

        GearsPositionList.ClearArray();
        GearsSolutionList.ClearArray();

        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in ts)
            {
            if (child != null && child.name.Contains("Gears") && child != myScript.transform )
                    Undo.DestroyObjectImmediate(child.gameObject);
            if (child != null && child.name.Contains("Axis") && child != myScript.transform )
                    Undo.DestroyObjectImmediate(child.gameObject);
            }
        #endregion
    }

    private void ResetPosition(GearsPuzzle_Pc myScript,SerializedProperty _PositionList, SerializedProperty _DirectionList){
        #region
        for (var i = 0; i < _PositionList.arraySize;i++){
            _PositionList.GetArrayElementAtIndex(i).intValue = 0;
        }

        for (var i = 0; i < GearsPositionList.arraySize; i++)
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == i)
                {
                   // objPIVOT = child;
                }
            }
        }
        #endregion
    }

    private void loadGearsPosition(GearsPuzzle_Pc myScript, int WhichSection)
    {
        #region
        for (var i = 0; i < GearsPositionList.arraySize; i++)
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("Gears_") && child != myScript.transform && int.Parse(child.GetChild(0).name) == i)
                {
                    // objPIVOT = child;
                    Undo.RegisterFullObjectHierarchyUndo(child.transform.GetChild(0).gameObject, child.transform.GetChild(0).name);
                    if (WhichSection == 0)
                    {
                        child.GetChild(0).transform.GetChild(0).transform.position = myScript.pivotGearList[i].transform.position;
                    }
                    else if (WhichSection == 1)
                    {
                            if(GearsInitPositionWhenStart.GetArrayElementAtIndex(i).boolValue)
                                child.GetChild(0).transform.GetChild(0).transform.localPosition = Vector3.zero;
                            else
                                child.GetChild(0).transform.GetChild(0).transform.position = myScript.pivotGearList[i].transform.position;

                        }
                    }
                }
            }
        #endregion
    }

    private void updatePuzlleDetectorPosition(int dragAndDropMode)
    {
        #region
        GearsPuzzle_Pc myScript = (GearsPuzzle_Pc)target;

        if (dragAndDropMode == 0)      // Focus
        {
            if (GUILayout.Button("Update Puzzle Detector Position (Focus)"))
            {
                if (FindGameObject("refFocus"))
                {
                    SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.transform);
                    SerializedProperty m_LocalScale = serializedObject3.FindProperty("m_LocalScale");
                    SerializedProperty m_LocalPosition = serializedObject3.FindProperty("m_LocalPosition");

                    serializedObject3.Update();

                    m_LocalScale.vector3Value = FindGameObject("refFocus").transform.localScale;
                    m_LocalPosition.vector3Value = FindGameObject("refFocus").transform.localPosition;

                    serializedObject3.ApplyModifiedProperties();

                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.GetComponent<MeshRenderer>());
                    SerializedProperty m_Enabled = serializedObject4.FindProperty("m_Enabled");
                    serializedObject4.Update();
                    m_Enabled.boolValue = false;
                    serializedObject4.ApplyModifiedProperties();
                }
            }
        }
        else                                    // Other Modes
        {
            if (GUILayout.Button("Update Puzzle Detector Position (No Focus)"))
            {
                if (FindGameObject("refNoFocus"))
                {
                    SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.transform);
                    SerializedProperty m_LocalScale = serializedObject3.FindProperty("m_LocalScale");
                    SerializedProperty m_LocalPosition = serializedObject3.FindProperty("m_LocalPosition");


                    serializedObject3.Update();

                    m_LocalScale.vector3Value = FindGameObject("refNoFocus").transform.localScale;
                    m_LocalPosition.vector3Value = FindGameObject("refNoFocus").transform.localPosition;

                    serializedObject3.ApplyModifiedProperties();


                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.GetComponent<MeshRenderer>());
                    SerializedProperty m_Enabled = serializedObject4.FindProperty("m_Enabled");
                    serializedObject4.Update();
                    m_Enabled.boolValue = true;
                    serializedObject4.ApplyModifiedProperties();
                }
            }
        }
        #endregion
    }

    private GameObject FindGameObject(string sName)
    {
        #region
        GearsPuzzle_Pc myScript = (GearsPuzzle_Pc)target;
        Transform[] children = myScript.gameObject.transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == sName)
                return child.gameObject;
        }

        return null;
        #endregion
    }

    void OnSceneGUI()
    {
    }

    public void _helpBox(int value)
    {
        #region
        string sType = "";
       // if (puzzleType.intValue == 1)
            sType = "Gear";
        //else
          //  sType = "Cercle";

        if (helpBoxEditor.boolValue)
        {
            switch (value)
            {
                case 0:
                    EditorGUILayout.HelpBox("1-Choose the number of " + sType +"s." +
                                            "\n2- Choose the number of column." +
                                            "\n3-Press button 'Generate " + sType + "' to create the puzzle.", MessageType.Info);
                    break;
                case 1:
                    EditorGUILayout.HelpBox("1-Click on a Button below to access his parameters." +
                                            "\n2-Drag and drop a sprite in the slot next to the KEY thumbnail." +
                                            "\n3-Change his scale." +
                                            "\n4-Apply the same scale to all tiles by pressing button ''Apply to All''." + 
                                            "\n5-Choose the text displayed in the scene view inside the KEY." +
                                            "\n6-Choose the value displayed on the result screen in the scene view.", MessageType.Info);
                    break;
                case 2:
                    EditorGUILayout.HelpBox("1-Press button 'Activate Mode' next to Choose Parent " + sType + ". Then choose the " + sType + " you want to link." +
                                            "\n(Press one of the square button to select a " + sType + ")" +
                                            "\n\n2-Press button 'Activate Mode' next to Choose Linked " + sType + ". Then choose the " + sType + " you want to link to the parent." +
                                            "\n (Press one of the square button to select a " + sType + ")." , MessageType.Info);
                    break;
            }
        }
        #endregion
    }

   /* void OnInspectorUpdate()
    {
        Repaint();
    }
*/

}
#endif