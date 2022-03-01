// Description : Custom Editor for cylinderPuzzle_Pc.cs
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

[CustomEditor(typeof(cylinderPuzzle_Pc))]
public class cylinderPuzzleEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector

    SerializedProperty puzzleType;
    SerializedProperty puzzleSubType;

    SerializedProperty HowManyCylinderPosition;
    SerializedProperty totalMovement;
    SerializedProperty CylinderSolutionList;
    SerializedProperty linkCylinder;
    SerializedProperty b_SelectCylindersToLink;

    SerializedProperty rotationDirection;

    SerializedProperty _NumberOfKey;
    SerializedProperty tilesList;
    SerializedProperty toolbarCurrentValue;
    SerializedProperty SquareSize;
    SerializedProperty currentSelectedSprite;
    SerializedProperty CylinderPositionList;
 
    SerializedProperty selectDefaultTile;
 
    SerializedProperty helpBoxEditor;

    SerializedProperty a_KeyPressed;
    SerializedProperty a_KeyPressedVolume;
    SerializedProperty a_Reset;
    SerializedProperty a_ResetVolume;

    SerializedProperty cameraUseForFocus;

    SerializedProperty methodsList;
    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;

    public Color _cBlue = new Color(0f, .9f, 1f, .5f);
    public Color _cRed = new Color(1f, .5f, 0f, .5f);
    public Color _cGray = new Color(.9f, .9f, .9f, 1);

   
    public Transform spriteTransform;   // Use to display key sprite
    public Text tmpText;        // use to display Key Text
    public Transform objPIVOT;

    public string[] toolbarStrings = new string[] { "Generate","Link (Optional)", "Puzzle Solution", "Init Position", "Game Options" };

    public string[] cylinderTypeStrings             = new string[] { "Choose", "Cylinder", "Circle"};
    public string[] cylinderSubTypeCylinderStrings  = new string[] { "Vertical", "Horizontal" };
    public string[] cylinderSubTypeCircleStrings    = new string[] { "Nested", "Align" };

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

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab","Reticule Mode" };

    SerializedProperty aP_PuzzleDetector;

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector            = serializedObject.FindProperty("SeeInspector");
        helpBoxEditor           = serializedObject.FindProperty("helpBoxEditor");

        cylinderPuzzle_Pc myScript = (cylinderPuzzle_Pc)target;

        puzzleType              = serializedObject.FindProperty("puzzleType");
        puzzleSubType           = serializedObject.FindProperty("puzzleSubType");

        rotationDirection       = serializedObject.FindProperty("rotationDirection");


        HowManyCylinderPosition = serializedObject.FindProperty("HowManyCylinderPosition");

        totalMovement           = serializedObject.FindProperty("totalMovement");

        CylinderSolutionList    = serializedObject.FindProperty("CylinderSolutionList");
        linkCylinder            = serializedObject.FindProperty("linkCylinder");
        b_SelectCylindersToLink = serializedObject.FindProperty("b_SelectCylindersToLink");

        _NumberOfKey            = serializedObject.FindProperty("_NumberOfKey");

        toolbarCurrentValue     = serializedObject.FindProperty("toolbarCurrentValue");
        SquareSize              = serializedObject.FindProperty("SquareSize");

        tilesList               = serializedObject.FindProperty("tilesList");

        currentSelectedSprite   = serializedObject.FindProperty("currentSelectedSprite");
        CylinderPositionList    = serializedObject.FindProperty("CylinderPositionList");
     
        selectDefaultTile       = serializedObject.FindProperty("selectDefaultTile");

        a_KeyPressed            = serializedObject.FindProperty("a_KeyPressed");
        a_KeyPressedVolume      = serializedObject.FindProperty("a_KeyPressedVolume");
        a_Reset                 = serializedObject.FindProperty("a_Reset");
        a_ResetVolume           = serializedObject.FindProperty("a_ResetVolume");

        cameraUseForFocus       = serializedObject.FindProperty("cameraUseForFocus");

        methodsList             = serializedObject.FindProperty("methodsList");
        editorMethods           = new EditorMethods_Pc();
        methodModule            = new AP_MethodModule_Pc();

        aP_PuzzleDetector       = serializedObject.FindProperty("aP_PuzzleDetector");


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

        cylinderPuzzle_Pc myScript = (cylinderPuzzle_Pc)target;


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
            if (tilesList.arraySize > 0)
            {

                for (var i = 0; i < tilesList.arraySize; i++)
                {
                    if (tilesList.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    {
                        b_TilesExist = false;
                        break;
                    }

                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                if ((toolbarCurrentValue.intValue == 0) || (toolbarCurrentValue.intValue == 1) || (toolbarCurrentValue.intValue == 2)){
                    loadCylinderPosition(myScript, CylinderSolutionList, null);                   
                }
                   
                if (toolbarCurrentValue.intValue == 3 ){
                    loadCylinderPosition(myScript, CylinderPositionList, null);
                }
            }

            // --> Display GeneratePuzzleSection
            if (toolbarCurrentValue.intValue == 0)
                displayGeneratePuzzleSection(myScript, style_Orange);

            // --> Display Link Section
            if (toolbarCurrentValue.intValue == 1)
                LinkSection(myScript, style_Orange);

            // --> Display Other Section
            if (toolbarCurrentValue.intValue == 4)
                otherSection(myScript, style_Orange);


            if (tilesList.arraySize > 0)
            {

                if (b_TilesExist)
                {
                    // --> Display Select Sprites
                    if (toolbarCurrentValue.intValue == 3)
                        displaySelectSpriteSection(myScript, style_Blue);
                    // --> Display Mix Section
                    if (toolbarCurrentValue.intValue == 2)
                        displaySolutionSection(myScript, style_Yellow_01);
                }
                else
                {
                    if (toolbarCurrentValue.intValue == 3 || toolbarCurrentValue.intValue == 2)
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

    private void otherSection(cylinderPuzzle_Pc myScript, GUIStyle style_Orange)
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
        GUILayout.Label("Play Audio when Cylinder is pressed : ", GUILayout.Width(180));
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

        if (GUILayout.Button("Activate / Deactivate Ref Value"))
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in ts)
            {
                if (child != null && child.name == "Number" && child != myScript.transform)
                {
                    Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.gameObject.name);

                    if(child.gameObject.activeInHierarchy)
                        child.gameObject.SetActive(false);
                    else
                        child.gameObject.SetActive(true); 
                }
            }
 
        }

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

        cylinderPuzzle_Pc myScript = (cylinderPuzzle_Pc)target;

        SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.gameObject.GetComponent<AP_PuzzleMoveType_Pc>());

        SerializedProperty dragAndDropMode = serializedObject3.FindProperty("puzzleMoveMode");

        serializedObject3.Update();

        GUILayout.Label("Puzzle Detection Options:", EditorStyles.boldLabel);
       /* if (helpBoxEditor.boolValue)
        {
            EditorGUILayout.HelpBox("Desktop: (0) Focus Mode and (3) Free Mode: Reticule" +
                                    "\nVR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand", MessageType.Info);

            EditorGUILayout.HelpBox("VR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand doesn't work on Mobile", MessageType.Warning);
        }*/


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Mode: ", GUILayout.Width(170));
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


        if (aP_PuzzleDetector.objectReferenceValue && myScript.aP_PuzzleDetector)
        {
            SerializedObject serializedObject5 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector);
            SerializedProperty b_FocusActivated = serializedObject5.FindProperty("b_FocusActivated");

            serializedObject5.Update();

            if (dragAndDropMode.intValue == 0 && !b_FocusActivated.boolValue)
                b_FocusActivated.boolValue = true;
            else if ((dragAndDropMode.intValue != 0) && b_FocusActivated.boolValue)
                b_FocusActivated.boolValue = false;

            if (b_FocusActivated.boolValue)
                GUILayout.Label("(This puzzle use focus.)");
            else
                GUILayout.Label("(This puzzle do not use focus.)");


            serializedObject5.ApplyModifiedProperties();
        }

        updatePuzlleDetectorPosition(dragAndDropMode.intValue);

        serializedObject3.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();
        #endregion
    }

    private void displayGeneratePuzzleSection(cylinderPuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Section : Generate Cylinders/Circles. (Minimum 1 Cylinder/Circle / 2 Positions)", MessageType.Info);
        _helpBox(0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Puzzle Type :", GUILayout.Width(150));
        puzzleType.intValue = EditorGUILayout.Popup(puzzleType.intValue, cylinderTypeStrings);
        EditorGUILayout.EndHorizontal();
       
        string s_subType = "";
        if(puzzleType.intValue != 0){
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sub Type :", GUILayout.Width(150));
            if (puzzleType.intValue == 1){
                puzzleSubType.intValue = EditorGUILayout.Popup(puzzleSubType.intValue, cylinderSubTypeCylinderStrings);
                s_subType = "Cylinder";
            }
          
            if (puzzleType.intValue == 2){
                puzzleSubType.intValue = EditorGUILayout.Popup(puzzleSubType.intValue, cylinderSubTypeCircleStrings);
                s_subType = "Circle";
            }
               
            EditorGUILayout.EndHorizontal(); 

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("How many " + s_subType + " :", GUILayout.Width(150));
            EditorGUILayout.PropertyField(_NumberOfKey, new GUIContent(""), GUILayout.Width(30));

            if (puzzleType.intValue == 1)
            {
                EditorGUILayout.LabelField("(1 minimum)");
                if (_NumberOfKey.intValue < 1)
                    _NumberOfKey.intValue = 1;
            }

            if (puzzleType.intValue == 2){
                if(puzzleSubType.intValue == 0){
                    EditorGUILayout.LabelField("(between 1 and 5)");
                    if (_NumberOfKey.intValue < 1)
                        _NumberOfKey.intValue = 1;

                    if (_NumberOfKey.intValue > 5)
                        _NumberOfKey.intValue = 5;
                }
                if (puzzleSubType.intValue == 1)
                {
                    EditorGUILayout.LabelField("(1 minimum)");
                    if (_NumberOfKey.intValue < 1)
                        _NumberOfKey.intValue = 1;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("How Many " + s_subType + " Position :", GUILayout.Width(150));


            if (puzzleType.intValue == 1)
            {
                if (HowManyCylinderPosition.intValue != 5 && HowManyCylinderPosition.intValue != 10)
                    HowManyCylinderPosition.intValue = 5;

                if(HowManyCylinderPosition.intValue == 5){
                    if (GUILayout.Button("5 positions", GUILayout.Width(100)))
                    {
                        HowManyCylinderPosition.intValue = 10;
                    }
                }
                    

                if (HowManyCylinderPosition.intValue == 10){
                    if (GUILayout.Button("10 positions", GUILayout.Width(100)))
                    {
                        HowManyCylinderPosition.intValue = 5;
                    } 
                }
            }

            if (puzzleType.intValue == 2)
            {
                EditorGUILayout.PropertyField(HowManyCylinderPosition, new GUIContent(""), GUILayout.Width(30));
                EditorGUILayout.LabelField("(2 minimum)");
                if (HowManyCylinderPosition.intValue < 2)
                    HowManyCylinderPosition.intValue = 2;
            }

            EditorGUILayout.EndHorizontal();
        }

       
        if (GUILayout.Button("Generate " + s_subType))
        {
            GenerateKeys(myScript);
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

    private void LinkSection(cylinderPuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.HelpBox("Section : Link Cylinders/Circles.", MessageType.Info);

        _helpBox(2);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Choose Parent Cylinder/Circles :", GUILayout.Width(180));
        if(!b_SelectCylindersToLink.boolValue)
            GUI.backgroundColor = _cRed;
        else
            GUI.backgroundColor = _cGray;
        if (GUILayout.Button("Activate Mode", GUILayout.Width(120)))
        {
            b_SelectCylindersToLink.boolValue = false;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Choose Linked Cylinder/Circles :", GUILayout.Width(180));

        if (b_SelectCylindersToLink.boolValue)
            GUI.backgroundColor = _cBlue;
        else
            GUI.backgroundColor = _cGray;
        if (GUILayout.Button("Activate Mode", GUILayout.Width(120)))
        {
            b_SelectCylindersToLink.boolValue = true;
        }
        EditorGUILayout.EndHorizontal();

      
        EditorGUILayout.EndVertical();


        displayCylinderInTheInspectorSectionLink(myScript, style_Yellow_01, 0);

        EditorGUILayout.LabelField("");
        #endregion
    }

    private void displaySolutionSection(cylinderPuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.HelpBox("Section : Solution. " +
                                "\n\nCreate the solution by pressing the squares below.(Show modification in scene view).", MessageType.Info);


        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset Solution"))
        {
            ResetPosition(myScript, CylinderSolutionList, null);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        displayKeysInTheInspector(myScript, style_Yellow_01, 1);

        EditorGUILayout.LabelField("");
        #endregion

    }

    private void displaySelectSpriteSection(cylinderPuzzle_Pc myScript, GUIStyle style_Blue)
    {
        #region
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Section : Cylinders/Circles Initial Position. " +
                                "\n\nCreate the Cylinders/Circles initial position by pressing the squares below.(Show modification in scene view).", MessageType.Info);


        if (GUILayout.Button("Reset Position"))
        {
            ResetPosition(myScript,CylinderPositionList,null);
        }

        EditorGUILayout.EndVertical();

        displayKeysInTheInspector(myScript, style_Blue, 0);
        #endregion
    }


    //--> Display square that represent puzzle object
    public void displayKeysInTheInspector(cylinderPuzzle_Pc myScript, GUIStyle style_Blue, int WhichSection)
    {
        #region
        EditorGUILayout.LabelField("");

        int number = 0;

            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < _NumberOfKey.intValue; j++)
            {

                tmpText = null;
                if (tilesList.arraySize > number)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(SquareSize.intValue));

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("",GUILayout.Width((SquareSize.intValue/2) - 10));
                EditorGUILayout.LabelField(j.ToString(),GUILayout.Width(SquareSize.intValue/2));
                EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                    {
                        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();
                        foreach (Transform child in ts)
                        {
                            if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == number)
                            {
                                objPIVOT = child;
                            }
                        }

                        currentSelectedSprite.intValue = number;

                        float step = totalMovement.intValue / (HowManyCylinderPosition.intValue - 1);


                       if (WhichSection == 0)          //-> Section : Cylinder Init Position
                            {
                                Undo.RegisterFullObjectHierarchyUndo(objPIVOT, objPIVOT.name);

                                SerializedProperty position = CylinderPositionList.GetArrayElementAtIndex(currentSelectedSprite.intValue);

                                moveCylinder(myScript, position, null, step, false);        // Move Parent Cylinder
                                ap_MoveLinkedCylinder(myScript, step, 3);                          // Move Linked Cylinders
                            }

                            if (WhichSection == 1)          //-> Section : Solution 
                            {
                                Undo.RegisterFullObjectHierarchyUndo(objPIVOT, objPIVOT.name);
                                SerializedProperty position = CylinderSolutionList.GetArrayElementAtIndex(currentSelectedSprite.intValue);

                                moveCylinder(myScript, position, null, step, false);        // Move Parent Cylinder
                                ap_MoveLinkedCylinder(myScript, step, 2);                          // Move Linked Cylinders
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

    private void moveCylinder(cylinderPuzzle_Pc myScript, SerializedProperty position, SerializedProperty directionUp, float step,bool b_OnlyMoveCylinder)
    {
        #region
        if (!b_OnlyMoveCylinder) position.intValue++;
            
        position.intValue = position.intValue % HowManyCylinderPosition.intValue;

        float newAngle = (360 / HowManyCylinderPosition.intValue) * position.intValue;

        if(puzzleType.intValue == 1)    // Cylinder
            objPIVOT.localEulerAngles = new Vector3(-newAngle * rotationDirection.intValue, 0, 0);
        if (puzzleType.intValue == 2)   // Circle
            objPIVOT.localEulerAngles = new Vector3(90, 0, newAngle * rotationDirection.intValue);
#endregion
    }

    private void GenerateKeys(cylinderPuzzle_Pc myScript)
    {
        #region
        GameObject WhiteSpot = null;

        currentSelectedSprite.intValue = 0;
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in ts)
        {
            if (child != null && child.name.Contains("Cylinder") && child != myScript.transform)
            {
                Undo.DestroyObjectImmediate(child.gameObject);
            }
        }


        tilesList.ClearArray();
        CylinderPositionList.ClearArray();
        CylinderSolutionList.ClearArray();
        linkCylinder.ClearArray();
        //lightList.ClearArray();

        for (var i = 0; i < _NumberOfKey.intValue; i++)
        {
            tilesList.InsertArrayElementAtIndex(0);
            CylinderPositionList.InsertArrayElementAtIndex(0);
            CylinderSolutionList.InsertArrayElementAtIndex(0);
            linkCylinder.InsertArrayElementAtIndex(0);
            //lightList.InsertArrayElementAtIndex(0);
        }

        int number = 0;

        // 0 : Circle 0 | 1 : Circle 2 | 2 : Circle 3 | 4 : Circle 5 
        // 5 : Cylindre5xHorizontal | 6 : Cylindre5xVertical | 7 : Cylindre10xHorizontal | 8 : Cylindre10xVertical |

        if(puzzleType.intValue == 1){   // Cylinder
            if (puzzleSubType.intValue == 0 && HowManyCylinderPosition.intValue == 5)
                selectDefaultTile.intValue = 6;     // Cylinder 5x Vertical
            if (puzzleSubType.intValue == 0 && HowManyCylinderPosition.intValue == 10)
                selectDefaultTile.intValue = 8;     // Cylinder 10x Vertical

            if (puzzleSubType.intValue == 1 && HowManyCylinderPosition.intValue == 5)
                selectDefaultTile.intValue = 5;     // Cylinder 5x Horizontal
            if (puzzleSubType.intValue == 1 && HowManyCylinderPosition.intValue == 10)
                selectDefaultTile.intValue = 7;     // Cylinder 10x Vertical
        }
        if (puzzleType.intValue == 2)   // Circle
        {
            selectDefaultTile.intValue = 4;
        }

        for (var i = 0; i < _NumberOfKey.intValue; i++)
        {
            GameObject newTile;
            if (puzzleType.intValue == 2 && puzzleSubType.intValue == 0){      // nested circle
                newTile = Instantiate(myScript.defaultTileList[i], myScript.gameObject.transform); 
            }
            else{                               // other circle and cylinder case
                newTile = Instantiate(myScript.defaultTileList[selectDefaultTile.intValue], myScript.gameObject.transform);

                // Position for each cylinder or Circle
                if (puzzleType.intValue == 1)
                {   // Cylinder
                    if (puzzleSubType.intValue == 0)   // vertical
                        newTile.transform.localPosition = new Vector3(.05f * i, -.25f, 0);
                    if (puzzleSubType.intValue == 1)   // Horizontal
                        newTile.transform.localPosition = new Vector3(.19f, -.05f * i, 0);
                } 
                if (puzzleType.intValue == 2)
                {   // Circle
                    newTile.transform.localPosition = new Vector3(.20f * i, 0, 0);
                } 
            }


            if (number < 10)
                newTile.name = "Cylinder_0" + number;
            else
                newTile.name = "Cylinder_" + number;


            newTile.transform.GetChild(0).name = number.ToString();

            Undo.RegisterCreatedObjectUndo(newTile, newTile.name);


            ts = newTile.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT_Cylinder"))
                {
                    child.name += "_" + number.ToString();
                    tilesList.GetArrayElementAtIndex(number).objectReferenceValue = child.gameObject;
                   
                }

                if (child != null && child.name.Contains("Cylinder_Light_Feedback"))
                {
                    child.name += "_" + number.ToString();
                    //lightList.GetArrayElementAtIndex(number).objectReferenceValue = child.GetComponent<MeshRenderer>();
                   
                }

                if (child != null && child.name.Contains("Text_Number"))
                {
                    child.gameObject.GetComponent<Text>().text = number.ToString();
                }


                if (child != null && child.name.Contains("Spot_White"))
                {
                    WhiteSpot = child.gameObject;
                }

            }


            number++;

            //-> Create sprites Position
            if(WhiteSpot != null){
                for (var j = 0; j < HowManyCylinderPosition.intValue - 1; j++)
                {
                    GameObject newWhiteSpot;
                    newWhiteSpot = Instantiate(WhiteSpot, WhiteSpot.transform.parent.transform);

                    Undo.RegisterCreatedObjectUndo(WhiteSpot, WhiteSpot.name);

                    newWhiteSpot.name = "Spot_White_" + (j + 1).ToString();


                    newWhiteSpot.transform.localEulerAngles = new Vector3(0, 0, (360 / HowManyCylinderPosition.intValue) * (j + 1));
                }

                Undo.DestroyObjectImmediate(WhiteSpot);
            }
        }
        #endregion
    }

    private void ResetPosition(cylinderPuzzle_Pc myScript,SerializedProperty _PositionList, SerializedProperty _DirectionList){
        #region
        for (var i = 0; i < _PositionList.arraySize;i++){
            _PositionList.GetArrayElementAtIndex(i).intValue = 0;
        }


        for (var i = 0; i < CylinderPositionList.arraySize; i++)
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == i)
                {
                    objPIVOT = child;
                }
            }

            moveCylinder(myScript, _PositionList.GetArrayElementAtIndex(i), null, 0,true);
        }
        #endregion
    }

    private void loadCylinderPosition(cylinderPuzzle_Pc myScript, SerializedProperty _PositionList, SerializedProperty _DirectionList)
    {
        #region
        for (var i = 0; i < CylinderPositionList.arraySize; i++)
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == i)
                {
                    objPIVOT = child;
                }
            }

            float newAngle = (360 / HowManyCylinderPosition.intValue) * _PositionList.GetArrayElementAtIndex(i).intValue;

            if(puzzleType.intValue == 1)    // Cylinder
                objPIVOT.localEulerAngles = new Vector3(-newAngle * rotationDirection.intValue, 0, 0);
            if (puzzleType.intValue == 2)   // Circle
                objPIVOT.localEulerAngles = new Vector3(90, 0, newAngle * rotationDirection.intValue);


        }
        #endregion
    }

    public void displayCylinderInTheInspectorSectionLink(cylinderPuzzle_Pc myScript, GUIStyle style_Blue, int WhichSection)
    {
        #region
        EditorGUILayout.LabelField("");

        int number = 0;

            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < _NumberOfKey.intValue; j++)
            {

                tmpText = null;
                if (tilesList.arraySize > number)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(SquareSize.intValue));

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width((SquareSize.intValue / 2) - 10));
                EditorGUILayout.LabelField(j.ToString(), GUILayout.Width(SquareSize.intValue / 2));
                EditorGUILayout.EndHorizontal();


                    if(ap_checkIfCylinderIsLinked(myScript, number))
                        GUI.backgroundColor = _cBlue;
                    else if (currentSelectedSprite.intValue == number)
                        GUI.backgroundColor = _cRed;
                    else
                        GUI.backgroundColor = _cGray;
                    if (GUILayout.Button("", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                    {
                   
                        if (b_SelectCylindersToLink.boolValue){            // Select linked Cylinder
                            ap_linkCylinder(myScript,number);
                        }
                        else{                                           // Select Master Cylinder
                            currentSelectedSprite.intValue = number;
                        }
                    }
                    number++;

                    EditorGUILayout.EndVertical();
                }

            EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
        #endregion
    }

    private bool ap_checkIfCylinderIsLinked(cylinderPuzzle_Pc myScript, int number)
    {
        #region
        bool result = false;
        for (var i = 0; i < myScript.linkCylinder[currentSelectedSprite.intValue]._CylinderList.Count;i++){
            if(myScript.linkCylinder[currentSelectedSprite.intValue]._CylinderList[i] == number){
                result = true;
                break;
             }
        }

        return result;
        #endregion
    }

    private void ap_linkCylinder(cylinderPuzzle_Pc myScript,int number){
        #region
        if (currentSelectedSprite.intValue != number)
        {
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
            //Debug.Log("Add Linked Cylinder : " + number);

            bool result = false;
            int listPosition = 0;
            for (var i = 0; i < myScript.linkCylinder[currentSelectedSprite.intValue]._CylinderList.Count; i++)
            {
                if (myScript.linkCylinder[currentSelectedSprite.intValue]._CylinderList[i] == number)
                {
                    result = true;
                    listPosition = i;
                    break;
                }
            }

            if (result)
                myScript.linkCylinder[currentSelectedSprite.intValue]._CylinderList.RemoveAt(listPosition);
            else
                myScript.linkCylinder[currentSelectedSprite.intValue]._CylinderList.Add(number); 
        }
        #endregion
    }
   
    private void ap_MoveLinkedCylinder(cylinderPuzzle_Pc myScript,float step,int WhichSection)
    {
        #region
        SerializedProperty CylinderList = linkCylinder.GetArrayElementAtIndex(currentSelectedSprite.intValue).FindPropertyRelative("_CylinderList");



        for (var i = 0; i < CylinderList.arraySize; i++){
           // Debug.Log("CylinderList : " + CylinderList.GetArrayElementAtIndex(i).intValue);

            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") 
                    && child != myScript.transform 
                    && int.Parse(child.parent.parent.name) == CylinderList.GetArrayElementAtIndex(i).intValue)
                {
                    objPIVOT = child;
                }
            }

            if(WhichSection == 2){      // Section : Solution 
                moveCylinder(myScript,
                     CylinderSolutionList.GetArrayElementAtIndex(CylinderList.GetArrayElementAtIndex(i).intValue),
                     null,
                     step,
                     false);
            }
            if (WhichSection == 3)      // Section :Cylinder Init Position
            {
                moveCylinder(myScript,
                     CylinderPositionList.GetArrayElementAtIndex(CylinderList.GetArrayElementAtIndex(i).intValue),
                     null,
                     step,
                     false);
            }
        }
        #endregion
    }

    //--> display a list of methods call when the puzzle starts the first time
    private void displayFirstTimePuzzle(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        cylinderPuzzle_Pc myScript = (cylinderPuzzle_Pc)target;

        methodModule.displayMethodList("Actions when the puzzle starts the first time:",
                                       editorMethods,
                                       methodsList,
                                       myScript.methodsList,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Read docmentation for more info the methods allowed.");

        #endregion
    }

    private void updatePuzlleDetectorPosition(int dragAndDropMode)
    {
        #region
        cylinderPuzzle_Pc myScript = (cylinderPuzzle_Pc)target;

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
        cylinderPuzzle_Pc myScript = (cylinderPuzzle_Pc)target;
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
        if (puzzleType.intValue == 1)
            sType = "Cylinder";
        else
            sType = "Circle";

        if (helpBoxEditor.boolValue)
        {
            switch (value)
            {
                case 0:
                    EditorGUILayout.HelpBox("1-Choose the number of " + sType +"s." +
                                            "\n2-Choose the subtype" +
                                            "\n3-Choose the number of " + sType +
                                            "\n4- Choose the number of position" +
                                            "\n5-Press button 'Generate " + sType + "' to create the puzzle.", MessageType.Info);
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