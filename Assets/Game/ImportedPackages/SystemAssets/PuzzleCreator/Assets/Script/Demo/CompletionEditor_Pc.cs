//Description : CompletionEditor_Pc : Completion_Pc editor
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

[CustomEditor(typeof(Completion_Pc))]
public class CompletionEditor_Pc : Editor
{
    SerializedProperty completionName;

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
        completionName = serializedObject.FindProperty("completionName");



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

        Completion_Pc myScript = (Completion_Pc)target;


        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

        EditorGUILayout.LabelField("");

        EditorGUILayout.BeginVertical(style_Blue);

        if (GUILayout.Button("Reset Completion PlayerPrefs"))
        {
            PlayerPrefs.DeleteKey(completionName.stringValue);
        }
        EditorGUILayout.EndVertical();


        serializedObject.ApplyModifiedProperties();
        #endregion
    }

  


   
    void OnSceneGUI()
    {
    }
}
#endif