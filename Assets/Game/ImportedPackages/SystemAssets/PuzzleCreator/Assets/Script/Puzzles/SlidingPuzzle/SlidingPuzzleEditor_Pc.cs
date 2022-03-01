// Description : Custom Editor for SlidingPuzzle_Pc.cs
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(SlidingPuzzle_Pc))]
public class SlidingPuzzleEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector

    SerializedProperty _Raw;
    SerializedProperty _Column;
    SerializedProperty tilesList;
    SerializedProperty  toolbarCurrentValue;
    SerializedProperty SquareSize;
    SerializedProperty currentSelectedSprite;
    SerializedProperty positionList;
    SerializedProperty randomNumber;
    SerializedProperty helpBoxEditor;

    SerializedProperty a_TileMove;
    SerializedProperty a_TileMoveVolume;
    SerializedProperty a_Reset;
    SerializedProperty a_ResetVolume;

    SerializedProperty cameraUseForFocus;
    SerializedProperty methodsList;
    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;

    bool b_Reset = false;
  
    public string[] toolbarStrings = new string[] { "Generate", "Customization", "Init Position","Game Options" };

    public Transform spriteTransform;


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

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };
    SerializedProperty aP_PuzzleDetector;

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        helpBoxEditor = serializedObject.FindProperty("helpBoxEditor");
        aP_PuzzleDetector = serializedObject.FindProperty("aP_PuzzleDetector");

        SlidingPuzzle_Pc myScript = (SlidingPuzzle_Pc)target;

        _Raw = serializedObject.FindProperty("_Raw");
        _Column = serializedObject.FindProperty("_Column");
        toolbarCurrentValue = serializedObject.FindProperty("toolbarCurrentValue");
        SquareSize = serializedObject.FindProperty("SquareSize");

        tilesList = serializedObject.FindProperty("tilesList");

        currentSelectedSprite = serializedObject.FindProperty("currentSelectedSprite");
        positionList = serializedObject.FindProperty("positionList");
        randomNumber = serializedObject.FindProperty("randomNumber");

        a_TileMove = serializedObject.FindProperty("a_TileMove");
        a_TileMoveVolume = serializedObject.FindProperty("a_TileMoveVolume");
        a_Reset = serializedObject.FindProperty("a_Reset");
        a_ResetVolume = serializedObject.FindProperty("a_ResetVolume");

        cameraUseForFocus = serializedObject.FindProperty("cameraUseForFocus");


        methodsList = serializedObject.FindProperty("methodsList");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

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

        SlidingPuzzle_Pc myScript = (SlidingPuzzle_Pc)target;


        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Puzzle could not be edited in play mode", MessageType.Info);
        }
        else
        {
            //--> Display Tab sections in the Inspector
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

            //--> Update the Inspector view
            if (EditorGUI.EndChangeCheck())
            {
                if ((toolbarCurrentValue.intValue == 1 && b_TilesExist) || (toolbarCurrentValue.intValue == 0 && b_TilesExist))
                    InitSelectSpritesPosition(myScript);
                if (toolbarCurrentValue.intValue == 2 && b_TilesExist)
                    InitMixPosition(myScript);
            }



            // --> Display Generate Section
            if (toolbarCurrentValue.intValue == 0)
                displayGeneratePuzzleSection(myScript, style_Orange);

            // --> Display Other Section
            if (toolbarCurrentValue.intValue == 3)
                otherSection(myScript, style_Orange);

            if (tilesList.arraySize > 0)
            {
                if (b_TilesExist)
                {
                    // --> Display Customization Section
                    if (toolbarCurrentValue.intValue == 1)
                        displaySelectSpriteSection(myScript, style_Blue);
                    // --> Display Init Position Section
                    if (toolbarCurrentValue.intValue == 2)
                        displayMixSection(myScript, style_Yellow_01);
                }
                //--> Puzzle coun't be displayed
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

    //--> Puzzle coun't be displayed
    private void puzzleNeedToBeGenerated()
    {
        #region
        EditorGUILayout.HelpBox("You need to generate the puzzle first.", MessageType.Error);
        #endregion
    }

    //--> Section Other Options
    private void otherSection(SlidingPuzzle_Pc myScript, GUIStyle style_Orange)
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
        GUILayout.Label("Play Audio when tile move : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(a_TileMove, new GUIContent(""), GUILayout.Width(100));
        GUILayout.Label("Volume : ", GUILayout.Width(60));
        a_TileMoveVolume.floatValue = EditorGUILayout.Slider(a_TileMoveVolume.floatValue, 0, 1);
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

        SlidingPuzzle_Pc myScript = (SlidingPuzzle_Pc)target;

        SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.gameObject.GetComponent<AP_PuzzleMoveType_Pc>());

        SerializedProperty dragAndDropMode = serializedObject3.FindProperty("puzzleMoveMode");

        serializedObject3.Update();

        GUILayout.Label("Puzzle Detection Options:", EditorStyles.boldLabel);
        /*if (helpBoxEditor.boolValue)
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

    //--> Section Generate
    private void displayGeneratePuzzleSection(SlidingPuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Section : Generate Tiles. (Minimum : Raw * Column >= 4)", MessageType.Info);
        _helpBox(0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Column :", GUILayout.Width(85));
        EditorGUILayout.PropertyField(_Column, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Row :", GUILayout.Width(85));
        EditorGUILayout.PropertyField(_Raw, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate Tiles"))
        {
            GenerateTile(myScript);
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

    //--> Section Init Position
    private void displayMixSection(SlidingPuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.HelpBox("Section : Mix Tiles.", MessageType.Info);

        _helpBox(2);

        //-> Mixing Parameters
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Random Mixing"))
        { RandomMix(myScript); }

        EditorGUILayout.PropertyField(randomNumber, new GUIContent(""), GUILayout.Width(45));

        if (GUILayout.Button("Update Scene View"))
        { InitMixPosition(myScript); }

        if (GUILayout.Button("Reset Mixing"))
        { ResetPosition(myScript); }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");
        EditorGUILayout.EndVertical();


        EditorGUILayout.LabelField("");

        //-> Display tiles and sprites in the Inspector
        if (!b_Reset)
        {
            int number = 0;
            for (var i = 0; i < _Raw.intValue; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < _Column.intValue; j++)
                {
                    if (myScript.positionList[number] != -1)
                    {
                        if (tilesList.arraySize >= myScript.positionList[number])
                        {
                            Transform[] ts = myScript.tilesList[myScript.positionList[number]].GetComponentsInChildren<Transform>();

                            spriteTransform = null;
                            bool b_Sprite = false;
                            for (var k = 0; k < ts.Length; k++)
                            {
                                if (ts[k].name.Contains("Sprite"))
                                {
                                    spriteTransform = ts[k];
                                    b_Sprite = true;
                                }
                            }

                            if (myScript.tilesList[myScript.positionList[number]].transform.GetChild(0).transform.childCount > 0 && b_Sprite)
                            {
                                GameObject objSprite = spriteTransform.gameObject;

                                if (objSprite)
                                {
                                    Texture2D DisplayTexture = (Texture2D)spriteTransform.GetComponent<SpriteRenderer>().sprite.texture;
                                    if (GUILayout.Button(DisplayTexture, GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                                    {
                                        currentSelectedSprite.intValue = number;
                                        MoveTile(myScript, currentSelectedSprite.intValue, true);
                                    }
                                }
                                number++;
                            }
                            else
                            {
                                if (GUILayout.Button("No Sprite", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                                {
                                    currentSelectedSprite.intValue = number;
                                    MoveTile(myScript, currentSelectedSprite.intValue, true);
                                }
                                number++;
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue));
                        number++;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.LabelField("");
        #endregion
    }

    //--> Reset Tile Mixing
    private void ResetPosition(SlidingPuzzle_Pc myScript)
    {
        #region
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        myScript.currentSelectedSprite = -2;
        for (var i = 0; i < myScript.positionList.Count; i++)
        {
            myScript.positionList[i] = i;
        }
        myScript.positionList[myScript.positionList.Count - 1] = -1;
        myScript.currentSelectedSprite = 0;

        int number = 0;
        for (var i = 0; i < _Raw.intValue; i++)
        {
            for (var j = 0; j < _Column.intValue; j++)
            {
                if (i == _Raw.intValue - 1 && j == _Column.intValue - 1) { }
                else
                { myScript.tilesList[number].transform.localPosition = new Vector3(.25f * j, -.25f * i, 0); }

                number++;
            }
        }
        #endregion
    }

    //--> Update Tile position in scene view
    private void InitMixPosition(SlidingPuzzle_Pc myScript)
    {
        #region
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        for (var i = 0; i < myScript.positionList.Count; i++)
        {
            if (myScript.positionList[i] != -1)
            {
                int numRaw = i / _Column.intValue;
                int numColumn = i % _Column.intValue;

                myScript.tilesList[myScript.positionList[i]].transform.localPosition = new Vector3(.25f * numColumn, -.25f * numRaw, 0);
            }
        }
        #endregion
    }

    //--> Init tile position when tab is clicked
    private void InitSelectSpritesPosition(SlidingPuzzle_Pc myScript)
    {
        #region
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        for (var i = 0; i < myScript.tilesList.Count; i++)
        {
            int numRaw = i / _Column.intValue;
            int numColumn = i % _Column.intValue;

            myScript.tilesList[i].transform.localPosition = new Vector3(.25f * numColumn, -.25f * numRaw, 0);
        }
        #endregion
    }

    //--> Random Mixing method
    private void RandomMix(SlidingPuzzle_Pc myScript)
    {
        #region
        for (var i = 0; i < randomNumber.intValue; i++)
        {
            int value = UnityEngine.Random.Range(0, tilesList.arraySize + 1);
            MoveTile(myScript, value, false);
        }
        InitMixPosition(myScript);
        #endregion
    }

    //--> Move Tile : Scene view and Inspector
    private string MoveTile(SlidingPuzzle_Pc myScript, int selectedTile, bool b_Move)
    {
        #region
        int numRaw = selectedTile / _Column.intValue;
        int numColumn = selectedTile % _Column.intValue;

        string result = "Raw : " + numRaw.ToString() + " : Column : " + numColumn.ToString();

        //-> Move if it is Possible
        ///--> Check Up position
        if (numRaw > 0)
        {
            result += " : Up Ok";
            if (positionList.GetArrayElementAtIndex(selectedTile - _Column.intValue).intValue == -1)
            {
                result += " : Could move Up";
                positionList.GetArrayElementAtIndex(selectedTile - _Column.intValue).intValue = positionList.GetArrayElementAtIndex(selectedTile).intValue;
                positionList.GetArrayElementAtIndex(selectedTile).intValue = -1;
                if (b_Move) MoveTileInSceneView(selectedTile, "Up", myScript);
            }
        }
        else
        {
            result += " : Up No";
        }
        //--> Check Down position
        if (numRaw < _Raw.intValue - 1)
        {
            result += " : Down Ok";
            if (positionList.GetArrayElementAtIndex(selectedTile + _Column.intValue).intValue == -1)
            {
                result += " : Could move Down";
                positionList.GetArrayElementAtIndex(selectedTile + _Column.intValue).intValue = positionList.GetArrayElementAtIndex(selectedTile).intValue;
                positionList.GetArrayElementAtIndex(selectedTile).intValue = -1;
                if (b_Move) MoveTileInSceneView(selectedTile, "Down", myScript);
            }
        }
        else
        {
            result += " : Down No";
        }
        //--> Check Right position
        if (numColumn < _Column.intValue - 1)
        {
            result += " : Right Ok";
            if (positionList.GetArrayElementAtIndex(selectedTile + 1).intValue == -1)
            {
                result += " : Could move Right";
                positionList.GetArrayElementAtIndex(selectedTile + 1).intValue = positionList.GetArrayElementAtIndex(selectedTile).intValue;
                positionList.GetArrayElementAtIndex(selectedTile).intValue = -1;
                if (b_Move) MoveTileInSceneView(selectedTile, "Right", myScript);
            }
        }
        else
        {
            result += " : Right No";
        }
        //--> Check Left position
        if (numColumn > 0)
        {
            result += " : Left Ok";
            if (positionList.GetArrayElementAtIndex(selectedTile - 1).intValue == -1)
            {
                result += " : Could move Left";
                positionList.GetArrayElementAtIndex(selectedTile - 1).intValue = positionList.GetArrayElementAtIndex(selectedTile).intValue;
                positionList.GetArrayElementAtIndex(selectedTile).intValue = -1;
                if (b_Move) MoveTileInSceneView(selectedTile, "Left", myScript);
            }
        }
        else
        {
            result += " : Left No";
        }

        return result;
        #endregion
    }

    //--> Move tile in scene view
    private void MoveTileInSceneView(int oldPosition, string direction, SlidingPuzzle_Pc myScript)
    {
        #region
        if (oldPosition != -1)
        {
            // Debug.Log("oldPosition : " + oldPosition);
            SerializedObject serializedObject2 = new UnityEditor.SerializedObject(myScript.tilesList[myScript.positionList[oldPosition]].GetComponent<Transform>());
            serializedObject2.Update();
            SerializedProperty m_Position = serializedObject2.FindProperty("m_LocalPosition");

            if (direction == "Down")
                m_Position.vector3Value = new Vector3(m_Position.vector3Value.x, m_Position.vector3Value.y - .25f, 0);
            if (direction == "Up")
                m_Position.vector3Value = new Vector3(m_Position.vector3Value.x, m_Position.vector3Value.y + .25f, 0);
            if (direction == "Left")
                m_Position.vector3Value = new Vector3(m_Position.vector3Value.x - .25f, m_Position.vector3Value.y, 0);
            if (direction == "Right")
                m_Position.vector3Value = new Vector3(m_Position.vector3Value.x + .25f, m_Position.vector3Value.y, 0);

            serializedObject2.ApplyModifiedProperties();
        }
        #endregion
    }

    //--> Section : Customization
    private void displaySelectSpriteSection(SlidingPuzzle_Pc myScript, GUIStyle style_Blue)
    {
        #region
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Section :Select a tile and change his sprite.", MessageType.Info);
        _helpBox(1);

        EditorGUILayout.BeginVertical(style_Blue);

        EditorGUILayout.LabelField("Current selected Tile : " + currentSelectedSprite.intValue, EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        if (tilesList.arraySize > currentSelectedSprite.intValue)
        {

            Transform[] ts = myScript.tilesList[currentSelectedSprite.intValue].GetComponentsInChildren<Transform>();

            spriteTransform = null;
            bool b_Sprite = false;
            for (var k = 0; k < ts.Length; k++)
            {
                if (ts[k].name.Contains("Sprite"))
                {
                    spriteTransform = ts[k];
                    b_Sprite = true;
                }
            }

            //-> Display current selected tile parameters
            if (myScript.tilesList[currentSelectedSprite.intValue].transform.GetChild(0).transform.childCount > 0 && b_Sprite)
            {
                GameObject objSprite = spriteTransform.gameObject;

                if (objSprite)
                {
                    EditorGUILayout.BeginHorizontal();
                    Texture2D DisplayTexture = null;

                    if (spriteTransform.GetComponent<SpriteRenderer>().sprite)
                        DisplayTexture = (Texture2D)spriteTransform.GetComponent<SpriteRenderer>().sprite.texture;

                    GUILayout.Label(DisplayTexture, GUILayout.Width(20), GUILayout.Height(20));

                    SerializedObject serializedObject3 = new UnityEditor.SerializedObject(spriteTransform.GetComponent<SpriteRenderer>());
                    SerializedProperty m_Sprite = serializedObject3.FindProperty("m_Sprite");
                    serializedObject3.Update();
                    EditorGUILayout.PropertyField(m_Sprite, new GUIContent(""), GUILayout.Width(200));
                    serializedObject3.ApplyModifiedProperties();

                    EditorGUILayout.EndHorizontal();

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
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No Sprites are available", MessageType.Info);
            }
        }


        EditorGUILayout.EndVertical();


        EditorGUILayout.LabelField("");

        //-> Display all tile and sprite in the Inspector
        int number = 0;
        for (var i = 0; i < _Raw.intValue; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (var j = 0; j < _Column.intValue; j++)
            {
                if (i == _Raw.intValue - 1 && j == _Column.intValue - 1)
                {
                }
                else if (tilesList.arraySize > number)
                {
                    Transform[] ts = myScript.tilesList[number].GetComponentsInChildren<Transform>();

                    spriteTransform = null;
                    bool b_Sprite = false;
                    for (var k = 0; k < ts.Length; k++)
                    {
                        if (ts[k].name.Contains("Sprite"))
                        {
                            spriteTransform = ts[k];
                            b_Sprite = true;
                        }
                    }

                    if (myScript.tilesList[number].transform.GetChild(0).transform.childCount > 0 && b_Sprite)
                    {
                        GameObject objSprite = spriteTransform.gameObject;

                        if (objSprite)
                        {

                            Texture2D DisplayTexture = null;

                            if (spriteTransform.GetComponent<SpriteRenderer>().sprite)
                                DisplayTexture = (Texture2D)spriteTransform.GetComponent<SpriteRenderer>().sprite.texture;


                            if (currentSelectedSprite.intValue != number)
                                GUI.backgroundColor = _cGray;
                            else
                                GUI.backgroundColor = _cBlue;

                            if (GUILayout.Button(DisplayTexture, GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                            {
                                currentSelectedSprite.intValue = number;
                            }

                        }

                        number++;
                    }
                    else
                    {
                        if (currentSelectedSprite.intValue != number)
                            GUI.backgroundColor = _cGray;
                        else
                            GUI.backgroundColor = _cBlue;
                        if (GUILayout.Button("No Sprite", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                        {
                            currentSelectedSprite.intValue = number;
                        }
                        number++;
                    }
                }

            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }

    //--> Generate Puzzle
    private void GenerateTile(SlidingPuzzle_Pc myScript)
    {
        #region
        currentSelectedSprite.intValue = 0;
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in ts)
        {
            if (child != null && child.name.Contains("Tile"))
            {
                Undo.DestroyObjectImmediate(child.gameObject);
            }
        }

        tilesList.ClearArray();
        positionList.ClearArray();

        for (var i = 0; i < _Raw.intValue; i++)
        {
            for (var j = 0; j < _Column.intValue; j++)
            {
                if (i == _Raw.intValue - 1 && j == _Column.intValue - 1)
                {
                }
                else
                {
                    tilesList.InsertArrayElementAtIndex(0);

                }
                positionList.InsertArrayElementAtIndex(0);
            }
        }

        int number = 0;
        for (var i = 0; i < _Raw.intValue; i++)
        {
            for (var j = 0; j < _Column.intValue; j++)
            {
                //Debug.Log("i : " + i + " j : " + j);
                if (i == _Raw.intValue - 1 && j == _Column.intValue - 1)
                {
                }
                else
                {
                    GameObject newTile = Instantiate(myScript.defaultTile, myScript.gameObject.transform);

                    newTile.transform.localPosition = new Vector3(.25f * j, -.25f * i, 0);


                    //Debug.Log("number : " + number);
                    if (number < 10)
                        newTile.name = "Tile_0" + number;
                    else
                        newTile.name = "Tile_" + number;


                    newTile.transform.GetChild(0).name = number.ToString();

                    Undo.RegisterCreatedObjectUndo(newTile, newTile.name);

                    tilesList.GetArrayElementAtIndex(number).objectReferenceValue = newTile;
                    positionList.GetArrayElementAtIndex(number).intValue = number;
                    number++;
                }
            }
        }
        positionList.GetArrayElementAtIndex(number).intValue = -1;
        #endregion
    }

    private void updatePuzlleDetectorPosition(int dragAndDropMode)
    {
        #region
        SlidingPuzzle_Pc myScript = (SlidingPuzzle_Pc)target;

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
        SlidingPuzzle_Pc myScript = (SlidingPuzzle_Pc)target;
        Transform[] children = myScript.gameObject.transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == sName)
                return child.gameObject;
        }

        return null;
        #endregion
    }

    //--> display a list of methods call when the puzzle starts the first time
    private void displayFirstTimePuzzle(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        SlidingPuzzle_Pc myScript = (SlidingPuzzle_Pc)target;

        methodModule.displayMethodList("Actions when the puzzle starts the first time:",
                                       editorMethods,
                                       methodsList,
                                       myScript.methodsList,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Read docmentation for more info the methods allowed.");

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
                    EditorGUILayout.HelpBox("1-Choose the number of column." +
                                            "\n2-Choose the number of row." +
                                            "\n3-Press button Generate to create the puzzle.", MessageType.Info);
                    break;
                case 1:
                    EditorGUILayout.HelpBox("1-Click a tile in the inspector to access its parameters." +
                                            "\n2-Drag and drop a sprite in the slot next to the tile thumbnail." +
                                            "\n3-Change its scale." +
                                            "\n4-Apply the same scale to all tiles by pressing button ''Apply to All''.", MessageType.Info);
                    break;
                case 2:
                    EditorGUILayout.HelpBox("1-Press button 'Random Mixing' to mix the tiles." +
                                            "\n2-Press button 'Update Scene View' to update the scene view visualization." +
                                            "\n3-Press a tile to move it manually." +
                                            "\n\n4-Press button 'Reset Mixing' to initialized the puzzle.", MessageType.Info);
                    break;

            }
        }
        #endregion
    }

}
#endif