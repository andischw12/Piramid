//Description : isObjectActivatedEditor : isObjectActivated_Pc editor
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

[CustomEditor(typeof(isObjectActivated_Pc))]
public class isObjectActivatedEditor_Pc : Editor {
    SerializedProperty methodsList;
    SerializedProperty methodsListObjDeactivated;
    SerializedProperty methodsListSaveExtend;
    SerializedProperty methodsListSaveExtendLoadProcess;

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
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	void OnEnable () {
        #region
        methodsList = serializedObject.FindProperty("methodsList");
        methodsListObjDeactivated = serializedObject.FindProperty("methodsListObjDeactivated");
        methodsListSaveExtend = serializedObject.FindProperty("methodsListSaveExtend");
        methodsListSaveExtendLoadProcess = serializedObject.FindProperty("methodsListSaveExtendLoadProcess");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();


		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
        Tex_04 = MakeTex(2, 2, new Color(.88F, .88f, .88f, 1f));
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f));
        #endregion
    }

    public override void OnInspectorGUI()
	{
        #region
        DrawDefaultInspector();

		serializedObject.Update ();

        isObjectActivated_Pc myScript = (isObjectActivated_Pc)target;

	
		GUIStyle style_Yellow_01 		= new GUIStyle(GUI.skin.box);	style_Yellow_01.normal.background 		= Tex_01; 
		GUIStyle style_Blue 			= new GUIStyle(GUI.skin.box);	style_Blue.normal.background 			= Tex_03;
		GUIStyle style_Purple 			= new GUIStyle(GUI.skin.box);	style_Purple.normal.background 			= Tex_04;
		GUIStyle style_Orange 			= new GUIStyle(GUI.skin.box);	style_Orange.normal.background 			= Tex_05; 
		GUIStyle style_Yellow_Strong 	= new GUIStyle(GUI.skin.box);	style_Yellow_Strong.normal.background 	= Tex_02;
       
        EditorGUILayout.LabelField("");
        EditorGUILayout.BeginVertical(style_Yellow_01);
        EditorGUILayout.LabelField("Call Methods during loading Process",EditorStyles.boldLabel);

        displayAllTheMethods_ObjIsActivated(style_Yellow_01, style_Yellow_01);

        displayAllTheMethods_ObjIsDeactivated(style_Yellow_01, style_Yellow_01);
        EditorGUILayout.EndVertical();


        EditorGUILayout.LabelField(""); 

        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.LabelField("Save Extension", EditorStyles.boldLabel);
        displayAllTheMethods_SaveExtend(style_Blue, style_Blue);
        displayAllTheMethods_SaveExtend_LoadProcess(style_Blue, style_Blue);
        EditorGUILayout.EndVertical();
       
		serializedObject.ApplyModifiedProperties ();
        #endregion
    }

    //--> display a list of methods call during the loading Process if the object need to be activated
    private void displayAllTheMethods_ObjIsActivated(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        isObjectActivated_Pc myScript = (isObjectActivated_Pc)target;

        methodModule.displayMethodList("Methods call when the object needs to be activated:",
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

    //--> display a list of methods call during the loading Process if the object need to be deactivated
    private void displayAllTheMethods_ObjIsDeactivated(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        isObjectActivated_Pc myScript = (isObjectActivated_Pc)target;

        methodModule.displayMethodList("Methods call when the object needs to be deactivated:",
                                       editorMethods,
                                       methodsListObjDeactivated,
                                       myScript.methodsListObjDeactivated,
                                       style_Blue,
                                       style_Yellow_01,
                                       "The methods are called in the same order as the list. " +
                                       "\nAll methods must be boolean methods. " +
                                       "\nOther methods will be ignored.");

        #endregion
    }


    //--> display a Method call when game is saved
    private void displayAllTheMethods_SaveExtend(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        isObjectActivated_Pc myScript = (isObjectActivated_Pc)target;

        methodModule.displayOneMethod("Method call when game is saved:",
                                       editorMethods,
                                       methodsListSaveExtend,
                                       myScript.methodsListSaveExtend,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Method must be string method. " +
                                       "\nOther method will be ignored.");

        #endregion
    }

    //--> display a Method call when game is saved
    private void displayAllTheMethods_SaveExtend_LoadProcess(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        isObjectActivated_Pc myScript = (isObjectActivated_Pc)target;

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


	void OnSceneGUI( )
	{
	}
}
#endif