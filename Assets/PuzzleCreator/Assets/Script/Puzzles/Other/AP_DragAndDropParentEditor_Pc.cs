//Description : AP_DragAndDropParentEditor_Pc : AP_DragAndDropParent_Pc custom editor
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

[CustomEditor(typeof(AP_DragAndDropParent_Pc))]
public class AP_DragAndDropParentEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector
    SerializedProperty b_FocusActivated;
    SerializedProperty color_01;
    SerializedProperty color_02;
    SerializedProperty color_03;

    SerializedProperty methodsListCanGrabLogicOrGear;
    SerializedProperty methodsListLogicOrGearSelected;
    SerializedProperty methodsListLogicOrGearNoSelection;

    SerializedProperty methodsListCanGrabLogicOrGearModeRaycast;
    SerializedProperty methodsListLogicOrGearSelectedModeRaycast;
    SerializedProperty methodsListLogicOrGearNoSelectionModeRaycast;

    SerializedProperty methodsLineModeRaycast;
    SerializedProperty methodsLineModeRaycastReset;

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

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector                                = serializedObject.FindProperty("SeeInspector");
        color_01                                    = serializedObject.FindProperty("color_01");
        color_02                                    = serializedObject.FindProperty("color_02");
        color_03                                    = serializedObject.FindProperty("color_03");

        methodsListCanGrabLogicOrGear               = serializedObject.FindProperty("methodsListCanGrabLogicOrGear");
        methodsListLogicOrGearSelected              = serializedObject.FindProperty("methodsListLogicOrGearSelected");
        methodsListLogicOrGearNoSelection           = serializedObject.FindProperty("methodsListLogicOrGearNoSelection");

        methodsListCanGrabLogicOrGearModeRaycast    = serializedObject.FindProperty("methodsListCanGrabLogicOrGearModeRaycast");
        methodsListLogicOrGearSelectedModeRaycast   = serializedObject.FindProperty("methodsListLogicOrGearSelectedModeRaycast");
        methodsListLogicOrGearNoSelectionModeRaycast = serializedObject.FindProperty("methodsListLogicOrGearNoSelectionModeRaycast");

        methodsLineModeRaycast                      = serializedObject.FindProperty("methodsLineModeRaycast");
        methodsLineModeRaycastReset                 = serializedObject.FindProperty("methodsLineModeRaycastReset");

        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

        Tex_01 = MakeTex(2, 2, color_01.colorValue);
        Tex_02 = MakeTex(2, 2, color_02.colorValue); 
        Tex_03 = MakeTex(2, 2, color_03.colorValue);
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
        GUIStyle style_Blue           = new GUIStyle(GUI.skin.box);   style_Blue.normal.background            = Tex_02;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_03;

        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.LabelField("Mode: VR Raycast", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        //EditorGUILayout.LabelField("");
        displayListmethodsListLineModeRaycast(style_Orange, style_Orange);
        displayListmethodsListLineModeRaycastReset(style_Orange, style_Orange);
        EditorGUILayout.LabelField("");
        displayListCanGrabLogicOrGearModeRaycast(style_Yellow_01, style_Yellow_01);
        displayListmethodsListLogicOrGearSelectedModeRaycast(style_Yellow_01, style_Yellow_01);
        displayListmethodsListLogicOrGearNoSelectionModeRaycast(style_Yellow_01, style_Yellow_01);
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("");

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.LabelField("Mode: VR Grab", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        displayListCanGrabLogicOrGear(style_Blue, style_Blue);
        displayListmethodsListLogicOrGearSelected(style_Blue, style_Blue);
        displayListmethodsListLogicOrGearNoSelection(style_Blue, style_Blue);
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        #endregion
    }

    private void displayListCanGrabLogicOrGear(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("A Puzzle object is detected (All puzzle types):",
                                       editorMethods,
                                       methodsListCanGrabLogicOrGear,
                                       myScript.methodsListCanGrabLogicOrGear,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListLogicOrGearSelected(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("An Object From a Logic or a gear puzzle is selected \n(Only Logic and Gear puzzles):",
                                       editorMethods,
                                       methodsListLogicOrGearSelected,
                                       myScript.methodsListLogicOrGearSelected,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListLogicOrGearNoSelection(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("A Puzzle object is deselected (All puzzle types):",
                                       editorMethods,
                                       methodsListLogicOrGearNoSelection,
                                       myScript.methodsListLogicOrGearNoSelection,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListCanGrabLogicOrGearModeRaycast(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("A Puzzle object is detected (All puzzle types):",
                                       editorMethods,
                                       methodsListCanGrabLogicOrGearModeRaycast,
                                       myScript.methodsListCanGrabLogicOrGearModeRaycast,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListLogicOrGearSelectedModeRaycast(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("An Object From a Logic or a gear puzzle is selected \n(Only Logic and Gear puzzles):",
                                       editorMethods,
                                       methodsListLogicOrGearSelectedModeRaycast,
                                       myScript.methodsListLogicOrGearSelectedModeRaycast,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListLogicOrGearNoSelectionModeRaycast(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("A Puzzle object is deselected (All puzzle types):",
                                       editorMethods,
                                       methodsListLogicOrGearNoSelectionModeRaycast,
                                       myScript.methodsListLogicOrGearNoSelectionModeRaycast,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListLineModeRaycast(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("Raycast Line Methods:",
                                       editorMethods,
                                       methodsLineModeRaycast,
                                       myScript.methodsLineModeRaycast,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }
    private void displayListmethodsListLineModeRaycastReset(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_DragAndDropParent_Pc myScript = (AP_DragAndDropParent_Pc)target;

        methodModule.displayMethodList("Deactivate Line Methods:",
                                       editorMethods,
                                       methodsLineModeRaycastReset,
                                       myScript.methodsLineModeRaycastReset,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }


    void OnSceneGUI()
    {
    }
}
#endif