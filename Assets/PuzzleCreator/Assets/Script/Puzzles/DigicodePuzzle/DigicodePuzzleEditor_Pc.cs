// Description : Custom Editor for DigicodePuzzle_Pc.cs
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

[CustomEditor(typeof(DigicodePuzzle_Pc))]
public class DigicodePuzzleEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector

    SerializedProperty _NumberOfKey;
    SerializedProperty _Column;
    SerializedProperty tilesList;
    SerializedProperty  toolbarCurrentValue;
    SerializedProperty SquareSize;
    SerializedProperty currentSelectedSprite;
    SerializedProperty keyStringList;
    SerializedProperty resultCode;

    SerializedProperty VisualizeSprite;

    SerializedProperty helpBoxEditor;

    SerializedProperty a_KeyPressed;
    SerializedProperty a_KeyPressedVolume;
    SerializedProperty a_Reset;
    SerializedProperty a_ResetVolume;
    SerializedProperty a_WrongCode;
    SerializedProperty a_WrongCodeVolume;

    public List<string> s_inputListJoystickButton = new List<string>();

    public Transform spriteTransform;   // Use to display key sprite
    public Text tmpText;        // use to display Key Text
    public Transform objText;

    public string[] toolbarStrings = new string[] { "Generate", "Customization", "Puzzle Solution","Game Options" };

    SerializedProperty cameraUseForFocus;
    SerializedProperty methodsList;
    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };
    //public AP_PuzzleDetector objPuzzleDetetor;
    SerializedProperty aP_PuzzleDetector;

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

    public Color _cBlue = new Color(0f, .9f, 1f, .5f);
    public Color _cRed = new Color(1f, .5f, 0f, .5f);
    public Color _cGray = new Color(.9f, .9f, .9f, 1);
    public Color _cGreen = Color.green;

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        helpBoxEditor = serializedObject.FindProperty("helpBoxEditor");

        DigicodePuzzle_Pc myScript = (DigicodePuzzle_Pc)target;

        _NumberOfKey = serializedObject.FindProperty("_NumberOfKey");

        _Column = serializedObject.FindProperty("_Column");
        toolbarCurrentValue = serializedObject.FindProperty("toolbarCurrentValue");
        SquareSize = serializedObject.FindProperty("SquareSize");

        tilesList = serializedObject.FindProperty("tilesList");

        currentSelectedSprite = serializedObject.FindProperty("currentSelectedSprite");
        keyStringList = serializedObject.FindProperty("keyStringList");
        VisualizeSprite = serializedObject.FindProperty("VisualizeSprite");

        a_KeyPressed = serializedObject.FindProperty("a_KeyPressed");
        a_KeyPressedVolume = serializedObject.FindProperty("a_KeyPressedVolume");
        a_Reset = serializedObject.FindProperty("a_Reset");
        a_ResetVolume = serializedObject.FindProperty("a_ResetVolume");
        a_WrongCode = serializedObject.FindProperty("a_WrongCode");
        a_WrongCodeVolume = serializedObject.FindProperty("a_WrongCodeVolume");

        resultCode = serializedObject.FindProperty("resultCode");

        cameraUseForFocus = serializedObject.FindProperty("cameraUseForFocus");


        methodsList = serializedObject.FindProperty("methodsList");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

        aP_PuzzleDetector = serializedObject.FindProperty("aP_PuzzleDetector");
       

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

        DigicodePuzzle_Pc myScript = (DigicodePuzzle_Pc)target;


        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Puzzle could not be edited in play mode", MessageType.Info);
        }
        else
        {
            // --> Display Tab sections in the Inspector

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

            // --> Display GeneratePuzzleSection
            if (toolbarCurrentValue.intValue == 0)
                displayGeneratePuzzleSection(myScript, style_Orange);

            // --> Display Other Section
            if (toolbarCurrentValue.intValue == 3)
                otherSection(myScript, style_Orange);


            if (tilesList.arraySize > 0)
            {

                if (b_TilesExist)
                {
                    // --> Display Select Sprites
                    if (toolbarCurrentValue.intValue == 1)
                        displaySelectSpriteSection(myScript, style_Blue);
                    // --> Display Mix Section
                    if (toolbarCurrentValue.intValue == 2)
                        displaySolutionSection(myScript, style_Yellow_01);
                }
                else
                {
                    if (toolbarCurrentValue.intValue == 1 || toolbarCurrentValue.intValue == 2)
                        puzzleNeedToBeGenerated();
                }
            }
        }


        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.LabelField("");
        #endregion
    }

    private void puzzleNeedToBeGenerated(){
        EditorGUILayout.HelpBox("You need to generate the puzzle first.", MessageType.Error);
    }

    private void otherSection(DigicodePuzzle_Pc myScript, GUIStyle style_Orange)
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
            //cameraUseForFocus.
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Puzzle Detetcor : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(aP_PuzzleDetector, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Play Audio when Key is pressed : ", GUILayout.Width(180));
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

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Play Audio when code is wrong : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(a_WrongCode, new GUIContent(""), GUILayout.Width(100));
        GUILayout.Label("Volume : ", GUILayout.Width(60));
        a_WrongCodeVolume.floatValue = EditorGUILayout.Slider(a_WrongCodeVolume.floatValue, 0, 1);
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

        DigicodePuzzle_Pc myScript = (DigicodePuzzle_Pc)target;

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

        //GUILayout.Label("");
        updatePuzlleDetectorPosition(dragAndDropMode.intValue);

        serializedObject3.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();
        #endregion
    }

    private void displayGeneratePuzzleSection(DigicodePuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Section : Generate Keys. (Minimum : 1)", MessageType.Info);
        _helpBox(0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Column :", GUILayout.Width(100));
        EditorGUILayout.PropertyField(_Column, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("How many Keys :", GUILayout.Width(100));
        EditorGUILayout.PropertyField(_NumberOfKey, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate Keys"))
        {
            GenerateKeys(myScript);
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

    private void displaySolutionSection(DigicodePuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.HelpBox("Section : Puzzle Solution.", MessageType.Info);

        _helpBox(2);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset Solution"))
        {
            resultCode.stringValue = "";
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("When you write the solution :" +
                                "\n\nCase 1 : Write directly the solution in the next field." +
                                "\nCase 2 : unselect the next field if needed then use the button below to write the solution.", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Puzzle Solution :", GUILayout.Width(100));
        EditorGUILayout.PropertyField(resultCode, new GUIContent(""));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");

        EditorGUILayout.EndVertical();

        displayKeysInTheInspector(myScript, style_Yellow_01, 1);

        EditorGUILayout.LabelField("");
        #endregion
    }

    private void displaySelectSpriteSection(DigicodePuzzle_Pc myScript, GUIStyle style_Blue)
    {
        #region
        EditorGUILayout.LabelField("");
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Section :Select a tile and change its sprite.", MessageType.Info);

        _helpBox(1);

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.LabelField("Current selected Tile : " + currentSelectedSprite.intValue, EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        if (tilesList.arraySize > currentSelectedSprite.intValue)
        {
            Transform[] ts = myScript.tilesList[currentSelectedSprite.intValue].GetComponentsInChildren<Transform>();


            bool b_SpriteExist = false;



            for (var i = 0; i < ts.Length; i++)
            {
                if (ts[i].name.Contains("Sprite"))
                {
                    spriteTransform = ts[i];
                    b_SpriteExist = true;
                    break;
                }
            }


            if (spriteTransform != null && b_SpriteExist)
            {
                GameObject objSprite = spriteTransform.gameObject;

                if (objSprite)
                {

                    EditorGUILayout.BeginHorizontal();
                    //-> Display srite thumbail 
                    Texture2D DisplayTexture = null;

                    if (spriteTransform.GetComponent<SpriteRenderer>().sprite)
                        DisplayTexture = (Texture2D)spriteTransform.GetComponent<SpriteRenderer>().sprite.texture;

                    GUILayout.Label(DisplayTexture, GUILayout.Width(20), GUILayout.Height(20));

                    //-> Display sprite slot
                    SerializedObject serializedObject3 = new UnityEditor.SerializedObject(spriteTransform.GetComponent<SpriteRenderer>());
                    SerializedProperty m_Sprite = serializedObject3.FindProperty("m_Sprite");
                    serializedObject3.Update();
                    EditorGUILayout.PropertyField(m_Sprite, new GUIContent(""), GUILayout.Width(200));
                    serializedObject3.ApplyModifiedProperties();
                    EditorGUILayout.EndHorizontal();


                    //--> Display sprite Local scale
                    EditorGUILayout.BeginHorizontal();
                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(spriteTransform.transform);
                    SerializedProperty m_localScale = serializedObject4.FindProperty("m_LocalScale");
                    serializedObject4.Update();
                    EditorGUILayout.PropertyField(m_localScale, new GUIContent(""), GUILayout.Width(200));
                    serializedObject4.ApplyModifiedProperties();

                    if (GUILayout.Button("Apply to all", GUILayout.Width(80)))
                    {
                        for (var i = 0; i < tilesList.arraySize; i++)
                        {
                            Transform[] ts2 = myScript.tilesList[i].GetComponentsInChildren<Transform>();
                            spriteTransform = null;

                            for (var k = 0; k < ts2.Length; k++)
                            {
                                if (ts2[k].name.Contains("Sprite"))
                                {
                                    spriteTransform = ts2[k];
                                }
                            }

                            if (spriteTransform)
                            {
                                SerializedObject serializedObject5 = new UnityEditor.SerializedObject(spriteTransform.transform);
                                SerializedProperty m_localScale2 = serializedObject5.FindProperty("m_LocalScale");
                                serializedObject5.Update();
                                m_localScale2.vector3Value = m_localScale.vector3Value;
                                serializedObject5.ApplyModifiedProperties();
                            }

                        }
                    }
                    EditorGUILayout.EndHorizontal();


                    //--> Display Text on Key
                    Transform[] ts3 = myScript.tilesList[currentSelectedSprite.intValue].GetComponentsInChildren<Transform>();

                    for (var i = 0; i < ts3.Length; i++)
                    {
                        if (ts3[i].name.Contains("Text"))
                        {
                            tmpText = ts3[i].GetComponent<Text>();
                        }
                    }

                    if (tmpText != null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Text displayed on KEY:", GUILayout.Width(200));

                        SerializedObject serializedObject6 = new UnityEditor.SerializedObject(tmpText);
                        SerializedProperty m_Text = serializedObject6.FindProperty("m_Text");
                        serializedObject6.Update();


                        m_Text.stringValue = EditorGUILayout.TextField(m_Text.stringValue, GUILayout.Width(80));

                        serializedObject6.ApplyModifiedProperties();

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Text : Not available (Object doesn't exist in the Hierarchy).", MessageType.Info);

                    }


                    //--> Display Text on Result Screen
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Text displayed on Result Screen :", GUILayout.Width(200));
                    EditorGUILayout.PropertyField(keyStringList.GetArrayElementAtIndex(currentSelectedSprite.intValue), new GUIContent(""), GUILayout.Width(80));
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No Sprites are available", MessageType.Info);
            }

        }

        EditorGUILayout.EndVertical();

        displayKeysInTheInspector(myScript, style_Blue, 0);
        #endregion
    }

    private void displayKeysInTheInspector(DigicodePuzzle_Pc myScript, GUIStyle style_Blue, int WhichSection)
    {
        #region
        EditorGUILayout.LabelField("");

        int number = 0;

        int raw = Mathf.RoundToInt(_NumberOfKey.intValue / _Column.intValue);

        //Debug.Log("raw : " + raw +  " : _Column :" + _Column.intValue + " : _NumberOfKey :" + _NumberOfKey.intValue);
        for (var i = 0; i <= raw; i++)
        {
            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < _Column.intValue; j++)
            {

                tmpText = null;
                if (tilesList.arraySize > number)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(SquareSize.intValue));

                    Transform[] ts3 = myScript.tilesList[number].GetComponentsInChildren<Transform>();

                    for (var k = 0; k < ts3.Length; k++)
                    {
                        if (ts3[k].name.Contains("Text"))
                        {
                            tmpText = ts3[k].GetComponent<Text>();
                        }
                    }

                    if (tmpText)
                        EditorGUILayout.LabelField("Key : " + tmpText.text as String, GUILayout.Width(SquareSize.intValue));
                    else
                        EditorGUILayout.LabelField("Key : No Text", GUILayout.Width(SquareSize.intValue));


                    EditorGUILayout.LabelField("Result : " + keyStringList.GetArrayElementAtIndex(number).stringValue as String, GUILayout.Width(SquareSize.intValue));
                    Transform[] ts = myScript.tilesList[number].GetComponentsInChildren<Transform>();
                    spriteTransform = null;

                    for (var k = 0; k < ts.Length; k++)
                    {
                        if (ts[k].name.Contains("Sprite"))
                        {
                            spriteTransform = ts[k];
                        }
                        if (ts[k].name.Contains("Text"))
                        {
                            objText = ts[k];
                        }
                    }

                    if (spriteTransform && VisualizeSprite.boolValue || objText && !VisualizeSprite.boolValue)
                    {
                        GameObject objSprite = spriteTransform.gameObject;

                        if (objSprite && VisualizeSprite.boolValue || objText && !VisualizeSprite.boolValue)
                        {


                            Texture2D DisplayTexture = null;


                            if (spriteTransform.GetComponent<SpriteRenderer>().sprite)
                                DisplayTexture = (Texture2D)spriteTransform.GetComponent<SpriteRenderer>().sprite.texture;

                            if (VisualizeSprite.boolValue)
                            {
                                if (currentSelectedSprite.intValue != number)
                                    GUI.backgroundColor = _cGray;
                                else
                                    GUI.backgroundColor = _cBlue;
                                if (GUILayout.Button(DisplayTexture, GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                                {
                                    currentSelectedSprite.intValue = number;
                                    if (WhichSection == 1)
                                    {       //-> Section : Solution 
                                        resultCode.stringValue += keyStringList.GetArrayElementAtIndex(currentSelectedSprite.intValue).stringValue;
                                    }
                                }
                            }
                            else
                            {
                                if (currentSelectedSprite.intValue != number)
                                    GUI.backgroundColor = _cGray;
                                else
                                    GUI.backgroundColor = _cBlue;
                                if (GUILayout.Button(keyStringList.GetArrayElementAtIndex(number).stringValue, GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                                {
                                    currentSelectedSprite.intValue = number;
                                    if (WhichSection == 1)
                                    {       //-> Section : Solution 
                                        resultCode.stringValue += keyStringList.GetArrayElementAtIndex(currentSelectedSprite.intValue).stringValue;
                                    }
                                }
                            }
                        }

                        number++;
                    }
                    else
                    {
                        if (GUILayout.Button("No Sprite", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                        {
                            currentSelectedSprite.intValue = number;
                            if (WhichSection == 1)          //-> Section : Solution 
                            {
                                resultCode.stringValue += keyStringList.GetArrayElementAtIndex(currentSelectedSprite.intValue).stringValue;
                            }
                        }
                        number++;
                    }

                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }

    private void GenerateKeys(DigicodePuzzle_Pc myScript)
    {
        #region
        currentSelectedSprite.intValue = 0;
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in ts)
        {
            if (child != null && child.name.Contains("Key"))
            {
                Undo.DestroyObjectImmediate(child.gameObject);
            }
        }


        tilesList.ClearArray();
        keyStringList.ClearArray();

        for (var i = 0; i < _NumberOfKey.intValue; i++)
        {
            tilesList.InsertArrayElementAtIndex(0);
            keyStringList.InsertArrayElementAtIndex(0);
        }

        int number = 0;
        int raw = 0;
        for (var i = 0; i < _NumberOfKey.intValue; i++)
        {
            GameObject newTile = Instantiate(myScript.defaultTile, myScript.gameObject.transform);

            if (raw != 0)
                newTile.transform.localPosition = new Vector3(.25f * (i % _Column.intValue), -.25f * raw, 0);
            else
                newTile.transform.localPosition = new Vector3(.25f * i, -.25f * raw, 0);

            //Debug.Log("number : " + number);
            if (number < 10)
                newTile.name = "Key_0" + number;
            else
                newTile.name = "Key_" + number;


            newTile.transform.GetChild(0).name = number.ToString();

            Undo.RegisterCreatedObjectUndo(newTile, newTile.name);

            tilesList.GetArrayElementAtIndex(number).objectReferenceValue = newTile;
            //positionList.GetArrayElementAtIndex(number).intValue = number;

            if (i % _Column.intValue == _Column.intValue - 1)
                raw++;

            number++;
        }

        #endregion
    }

    //--> display a list of methods call when the puzzle starts the first time
    private void displayFirstTimePuzzle(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        DigicodePuzzle_Pc myScript = (DigicodePuzzle_Pc)target;

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
        DigicodePuzzle_Pc myScript = (DigicodePuzzle_Pc)target;

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
        DigicodePuzzle_Pc myScript = (DigicodePuzzle_Pc)target;
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
        if (helpBoxEditor.boolValue)
        {
            switch (value)
            {
                case 0:
                    EditorGUILayout.HelpBox("1-Choose the number of Column." +
                                            "\n2-Choose the number of keys." +
                                            "\n3-Press button 'Generate' to create the puzzle.", MessageType.Info);
                    break;
                case 1:
                    EditorGUILayout.HelpBox("1-Click on a Button below to access its parameters." +
                                            "\n2-Drag and drop a sprite in the slot next to the KEY thumbnail." +
                                            "\n3-Change its scale." +
                                            "\n4-Apply the same scale to all tiles by pressing button ''Apply to All''." +
                                            "\n5-Choose the text displayed in the scene view inside the KEY." +
                                            "\n6-Choose the value displayed on the result screen in the scene view.", MessageType.Info);
                    break;
                case 2:
                    EditorGUILayout.HelpBox("1-Create the code by pressing the buttons below." +
                                            "\n" +
                                            "\nNote : Reset the solution by pressing button 'Reset Solution'.", MessageType.Info);
                    break;

            }
        }
        #endregion
    }

}
#endif