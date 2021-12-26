//Description : AP_PuzzleDetectorEditor_Pc : AP_PuzzleDetector_Pc custom editor
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

[CustomEditor(typeof(AP_PuzzleDetector_Pc))]
public class AP_PuzzleDetectorEditor_Pc : Editor {
	SerializedProperty			SeeInspector;											// use to draw default Inspector
    SerializedProperty 			b_FocusActivated;
    SerializedProperty          color_01;


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

	private Texture2D 		Tex_01;
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 		= serializedObject.FindProperty ("SeeInspector");
        b_FocusActivated = serializedObject.FindProperty("b_FocusActivated");
        color_01 = serializedObject.FindProperty("color_01");

        Tex_01 = MakeTex(2, 2, color_01.colorValue); 
	
	}


	public override void OnInspectorGUI()
	{
		if(SeeInspector.boolValue)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();

		serializedObject.Update ();
       
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("See Inspector :", GUILayout.Width (85));
		EditorGUILayout.PropertyField(SeeInspector, new GUIContent (""), GUILayout.Width (30));
		EditorGUILayout.EndHorizontal ();

		GUIStyle style_Yellow_01 		= new GUIStyle(GUI.skin.box);	style_Yellow_01.normal.background 		= Tex_01;
       
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.LabelField("");
        if(b_FocusActivated.boolValue)
        if (GUILayout.Button("The Focus is activated on this puzzle."))
        {
                b_FocusActivated.boolValue = false;
        }
        if (!b_FocusActivated.boolValue)
            if (GUILayout.Button("This puzzle can be played without focus."))
            {
                b_FocusActivated.boolValue = true;
            }
        EditorGUILayout.LabelField("");
        EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties ();
	}


	void OnSceneGUI( )
	{
	}
}
#endif