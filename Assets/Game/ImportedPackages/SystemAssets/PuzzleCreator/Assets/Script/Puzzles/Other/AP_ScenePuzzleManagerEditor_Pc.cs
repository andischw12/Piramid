//Description : AP_ScenePuzzleManagerEditor_Pc : custom editor for AP_ScenePuzzleManager_Pc
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(AP_ScenePuzzleManager_Pc))]
public class AP_ScenePuzzleManagerEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector
    SerializedProperty b_Auto;

    SerializedProperty methodsList;
    SerializedProperty listOfPuzzles;
    SerializedProperty listState;


    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;

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

    public windowMethods_Pc _windowMethods;

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        b_Auto = serializedObject.FindProperty("b_Auto");
        methodsList = serializedObject.FindProperty("methodsList");
        listOfPuzzles = serializedObject.FindProperty("listOfPuzzles");
        listState = serializedObject.FindProperty("listState");

        _windowMethods = new windowMethods_Pc();
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

        AP_ScenePuzzleManager_Pc myScript = (AP_ScenePuzzleManager_Pc)target;

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
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Auto:", GUILayout.Width(100));
        EditorGUILayout.PropertyField(b_Auto, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        displayAllTheMethods(style_Purple, style_Blue);

        EditorGUILayout.LabelField("");

        displayAllThePuzzlesAvailableInThisScene(style_Yellow_Strong, style_Yellow_01);

        EditorGUILayout.LabelField("");

        _Help(0);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.LabelField("");
        #endregion
    }

    //--> display a list of methods call when the scene starts
    private void displayAllTheMethods(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_ScenePuzzleManager_Pc myScript = (AP_ScenePuzzleManager_Pc)target;

        methodModule.displayMethodList("Methods call when scene starts:",
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

    private void displayAllThePuzzlesAvailableInThisScene(GUIStyle style_Color01, GUIStyle style_Color02){
        #region
        AP_ScenePuzzleManager_Pc myScript = (AP_ScenePuzzleManager_Pc)target;
        EditorGUILayout.BeginVertical(style_Color02);
        EditorGUILayout.LabelField("List of objects using Save System in the current scene",EditorStyles.boldLabel);


        EditorGUILayout.HelpBox("Find more info in the documentation about how to use multiple save slots.", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Update List"))
        {
            _windowMethods.saveLevelInfos();                        // Update save system

        }
        if (GUILayout.Button("Clear List"))
        {
            listOfPuzzles.arraySize = 0;
            listState.arraySize = 0;
            if (EditorUtility.DisplayDialog("Info: Save System",
                                        ".Dat and PlayersPrefs have been delete for this scene. ", "Continue"))
            {
                _windowMethods.EraseCurrentSceneSaveData();
            }
        }
        EditorGUILayout.EndHorizontal();

        for (var i = 0; i < listOfPuzzles.arraySize;i++){
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                listOfPuzzles.MoveArrayElement(i,listOfPuzzles.arraySize-1);
                listOfPuzzles.arraySize--;
                break;
            }
            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(20));
            EditorGUILayout.PropertyField(listOfPuzzles.GetArrayElementAtIndex(i), new GUIContent(""));

            EditorGUILayout.EndHorizontal();
        }
       
        if (GUILayout.Button("Add a new element manually"))
        {
            listOfPuzzles.arraySize++;
            if (EditorUtility.DisplayDialog("Info: Save System",
                                        ".Dat and PlayersPrefs have been delete for this scene. ", "Continue"))
            {
                _windowMethods.EraseCurrentSceneSaveData();
            }
        }

        EditorGUILayout.EndVertical();
        #endregion
    }

    void _Help(int value)
    {
        #region
        switch (value)
        {
            case 0:
                EditorGUILayout.HelpBox("Useful methods and variables (more info in the documentation):" +
                                        "\n\nSaveAllPuzzles()" +
                                        "\nStartCoroutine(I_PuzzlesInitialisation());" +
                                        "\n\nSaveASpecificPuzzle(SaveData ThePuzzle, string UniqueName)" +
                                        "\nLoadASpecificPuzzle(SaveData ThePuzzle, string UniqueName)" +
                                        "\n\nStartCoroutine(CallAllTheMethodsOneByOne());", MessageType.Info);
                break;
        }

        #endregion

    }

    void OnSceneGUI()
    {
    }
}
#endif
