//Description : AP_ReticuleEditor_Pc : custom editor for AP_Reticule_Pc
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(AP_Reticule_Pc))]
public class AP_ReticuleEditor_Pc : Editor {
	SerializedProperty			SeeInspector;											// use to draw default Inspector
    SerializedProperty color_01;
    SerializedProperty color_02;
    SerializedProperty color_03;

    SerializedProperty methodsListCanGrabReticule;
    SerializedProperty methodsListReticuleSelected;
    SerializedProperty methodsListReticuleNoSelection;

    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;


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


	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 		= serializedObject.FindProperty ("SeeInspector");
        color_01 = serializedObject.FindProperty("color_01");
        color_02 = serializedObject.FindProperty("color_02");
        color_03 = serializedObject.FindProperty("color_03");

        methodsListCanGrabReticule = serializedObject.FindProperty("methodsListCanGrabReticule");
        methodsListReticuleSelected = serializedObject.FindProperty("methodsListReticuleSelected");
        methodsListReticuleNoSelection = serializedObject.FindProperty("methodsListReticuleNoSelection");

        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

	
		//AP_Reticule myScript = (AP_Reticule)target; 

	
        Tex_01 = MakeTex(2, 2, color_01.colorValue);
        Tex_02 = MakeTex(2, 2, color_02.colorValue);
        Tex_03 = MakeTex(2, 2, color_03.colorValue);

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
		GUIStyle style_Blue 			= new GUIStyle(GUI.skin.box);	style_Blue.normal.background 			= Tex_03;
		GUIStyle style_Yellow_Strong 	= new GUIStyle(GUI.skin.box);	style_Yellow_Strong.normal.background 	= Tex_02;



		//AP_Reticule myScript = (AP_Reticule)target; 

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.LabelField("Mode Reticule", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        displayListCanReticule(style_Blue, style_Blue);
        displayListmethodsListReticuleSelected(style_Blue, style_Blue);
        displayListmethodsListReticuleNoSelection(style_Blue, style_Blue);
        EditorGUILayout.EndVertical();



		serializedObject.ApplyModifiedProperties ();



		EditorGUILayout.LabelField ("");
	}

    private void displayListCanReticule(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_Reticule_Pc myScript = (AP_Reticule_Pc)target;

        methodModule.displayMethodList("A Puzzle object is detected (All puzzle types):",
                                       editorMethods,
                                       methodsListCanGrabReticule,
                                       myScript.methodsListCanGrabReticule,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListReticuleSelected(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_Reticule_Pc myScript = (AP_Reticule_Pc)target;

        methodModule.displayMethodList("An Object From a Logic or a gear puzzle is selected \n(Only Logic and Gear puzzles):",
                                       editorMethods,
                                       methodsListReticuleSelected,
                                       myScript.methodsListReticuleSelected,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

    private void displayListmethodsListReticuleNoSelection(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        AP_Reticule_Pc myScript = (AP_Reticule_Pc)target;

        methodModule.displayMethodList("A Puzzle object is deselected (All puzzle types):",
                                       editorMethods,
                                       methodsListReticuleNoSelection,
                                       myScript.methodsListReticuleNoSelection,
                                       style_Blue,
                                       style_Yellow_01,
                                       "");

        #endregion
    }

	void OnSceneGUI( )
	{
	}
}
#endif