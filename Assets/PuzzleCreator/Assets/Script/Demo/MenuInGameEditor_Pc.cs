//Description : MenuInGameEditor_Pc : MenuInGame_Pc editor
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

[CustomEditor(typeof(MenuInGame_Pc))]
public class MenuInGameEditor_Pc : Editor
{
    SerializedProperty methodsListOpenMenu;
    SerializedProperty methodsListCloseMenu;
    SerializedProperty methodsListConditionsToOpenTheMenu;
    //SerializedProperty methodsListSaveExtendLoadProcess;

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
        methodsListOpenMenu = serializedObject.FindProperty("methodsListOpenMenu");
        methodsListCloseMenu = serializedObject.FindProperty("methodsListCloseMenu");
        methodsListConditionsToOpenTheMenu = serializedObject.FindProperty("methodsListConditionsToOpenTheMenu");
        //methodsListSaveExtendLoadProcess = serializedObject.FindProperty("methodsListSaveExtendLoadProcess");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();


        Tex_01 = MakeTex(2, 2, new Color(1, .8f, 0.2F, .4f));
        Tex_02 = MakeTex(2, 2, new Color(1, .8f, 0.2F, .4f));
        Tex_03 = MakeTex(2, 2, new Color(.3F, .9f, 1, .5f));
        Tex_04 = MakeTex(2, 2, new Color(.88F, .88f, .88f, 1f));
        Tex_05 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        #endregion
    }

    public override void OnInspectorGUI()
    {
        #region
        DrawDefaultInspector();

        serializedObject.Update();

        MenuInGame_Pc myScript = (MenuInGame_Pc)target;


        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

        EditorGUILayout.LabelField("");

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.LabelField("Conditions to open the Menu", EditorStyles.boldLabel);
        displayAllTheMethods_ConditionsToOpenTheMenu(style_Blue, style_Blue);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.LabelField("Actions During Open and Close Menu", EditorStyles.boldLabel);

        displayAllTheMethods_OpenTheMenu(style_Yellow_01, style_Yellow_01);

        displayAllTheMethods_CloseTheMenu(style_Yellow_01, style_Yellow_01);
        EditorGUILayout.EndVertical();


       /* EditorGUILayout.LabelField("");

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.LabelField("Save Extention", EditorStyles.boldLabel);
        displayAllTheMethods_SaveExtend(style_Blue, style_Blue);
        displayAllTheMethods_SaveExtend_LoadProcess(style_Blue, style_Blue);
        EditorGUILayout.EndVertical();
        */
        serializedObject.ApplyModifiedProperties();
        #endregion
    }

    //--> display a list of methods call during the loading Process if the object need to be activated
    private void displayAllTheMethods_OpenTheMenu(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        MenuInGame_Pc myScript = (MenuInGame_Pc)target;

        methodModule.displayMethodList("Actions when the menu is opened:",
                                       editorMethods,
                                       methodsListOpenMenu,
                                       myScript.methodsListOpenMenu,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }



    //--> display a list of methods call during the loading Process if the object need to be deactivated
    private void displayAllTheMethods_CloseTheMenu(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        MenuInGame_Pc myScript = (MenuInGame_Pc)target;

        methodModule.displayMethodList("Actions when the menu is closed:",
                                       editorMethods,
                                       methodsListCloseMenu,
                                       myScript.methodsListCloseMenu,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }


    //--> display a Method call when game is saved
    private void displayAllTheMethods_ConditionsToOpenTheMenu(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        MenuInGame_Pc myScript = (MenuInGame_Pc)target;

        methodModule.displayMethodList("",
                                       editorMethods,
                                       methodsListConditionsToOpenTheMenu,
                                       myScript.methodsListConditionsToOpenTheMenu,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }

    /*
    //--> display a Method call when game is saved
    private void displayAllTheMethods_SaveExtend_LoadProcess(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        MenuInGame myScript = (MenuInGame)target;

        methodModule.displayOneMethod("Method call during Loading Process:",
                                       editorMethods,
                                      methodsListSaveExtendLoadProcess,
                                      myScript.methodsListSaveExtendLoadProcess,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Methods must be void method with a string argument. " +
                                       "\nOther method will be ignored.");

        #endregion
    }
    */

    void OnSceneGUI()
    {
    }
}
#endif