//Description : conditionsToAccessThePuzzleEditor_Pc : Custom Editor for conditionsToAccessThePuzzle_Pc.cs
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(conditionsToAccessThePuzzle_Pc))]
public class conditionsToAccessThePuzzleEditor_Pc : Editor {
	SerializedProperty			SeeInspector;											// use to draw default Inspector
    SerializedProperty          onlyFocusMode;

    SerializedProperty          methodsListFeedback;

    SerializedProperty          puzzleSprite;
    SerializedProperty          ledPuzzleSolved;
    SerializedProperty          methodsList;
    public EditorMethods_Pc     editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc   methodModule;


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
	private Texture2D 		Tex_04;


	void OnEnable () {
        #region
        // Setup the SerializedProperties.
        SeeInspector 			= serializedObject.FindProperty ("SeeInspector");
        onlyFocusMode           = serializedObject.FindProperty("onlyFocusMode");

        conditionsToAccessThePuzzle_Pc myScript = (conditionsToAccessThePuzzle_Pc)target; 

        methodsList             = serializedObject.FindProperty("methodsList");
        editorMethods           = new EditorMethods_Pc();
        methodModule            = new AP_MethodModule_Pc();


        methodsListFeedback     = serializedObject.FindProperty("methodsListFeedback");

        puzzleSprite            = serializedObject.FindProperty("puzzleSprite");
        ledPuzzleSolved = serializedObject.FindProperty("ledPuzzleSolved");

		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
        Tex_04 = MakeTex(2, 2, new Color(1,.8f,0.2F,.2f));
        #endregion
    }

	public override void OnInspectorGUI()
	{
        #region
        if(SeeInspector.boolValue)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();

		serializedObject.Update ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("See Inspector :", GUILayout.Width (85));
		EditorGUILayout.PropertyField(SeeInspector, new GUIContent (""), GUILayout.Width (30));
		EditorGUILayout.EndHorizontal ();

		GUIStyle style_Yellow_01 		= new GUIStyle(GUI.skin.box);	style_Yellow_01.normal.background 		= Tex_01; 
        GUIStyle style_Blue 			= new GUIStyle(GUI.skin.box);	style_Blue.normal.background 			= Tex_01;
		GUIStyle style_Purple 			= new GUIStyle(GUI.skin.box);	style_Purple.normal.background 			= Tex_04;
        GUIStyle style_Orange 			= new GUIStyle(GUI.skin.box);	style_Orange.normal.background 			= Tex_01; 
		GUIStyle style_Yellow_Strong 	= new GUIStyle(GUI.skin.box);	style_Yellow_Strong.normal.background 	= Tex_02;

        if (!onlyFocusMode.boolValue)
        {
            conditionsToAccessThePuzzle_Pc myScript = (conditionsToAccessThePuzzle_Pc)target;

            //-> Show Custom Methods
            EditorGUILayout.BeginVertical(style_Orange);
            showCustomMethods(style_Purple, style_Blue);
            EditorGUILayout.EndVertical();

            //-> Display feedback ID used when the puzzle is not available
            displayFeedbackWhenPuzzleIsLocked(style_Yellow_01,style_Blue);

            //-> Display section more Options
            //displayMoreOptions(style_Yellow_01);

        }

		serializedObject.ApplyModifiedProperties ();

        EditorGUILayout.HelpBox("Useful methods or variables:" +
        	"\nb_PuzzleIsActivated" +
            "\nb_PuzzleStateButtons", MessageType.Info);



        EditorGUILayout.LabelField ("");
        #endregion
    }

    //--> Display custom methods used to know if the puzzle could be activated
    private void showCustomMethods(GUIStyle style_Purple, GUIStyle style_Blue)
    {
        #region
        conditionsToAccessThePuzzle_Pc myScript = (conditionsToAccessThePuzzle_Pc)target;
     
        methodModule.displayMethodList("Methods checked to activate the puzzle",
                                       editorMethods,
                                       methodsList, 
                                       myScript.methodsList, 
                                       style_Purple,
                                       style_Blue, 
                                       "Only bool methods are allowed. Other methods are ignored." +
                                       "Puzzle is activated if all the methods return true.");
        #endregion
    }

    //--> display Feedback When Puzzle Is Locked
    private void displayFeedbackWhenPuzzleIsLocked(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        conditionsToAccessThePuzzle_Pc myScript = (conditionsToAccessThePuzzle_Pc)target;

        methodModule.displayMethodList("Call methods if puzzle access is denied: ",
                                       editorMethods,
                                       methodsListFeedback, 
                                       myScript.methodsListFeedback,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Methods allowed: void without argument and void with one argument (int, gameObject, string, float or AudioClip).");

        #endregion
    }

    void displayMoreOptions(GUIStyle style_Yellow_01){
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.LabelField("More Options: ", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sprite (Puzzle State):", GUILayout.Width(120));
            EditorGUILayout.PropertyField(puzzleSprite, new GUIContent(""), GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Led (Puzzle Solved):", GUILayout.Width(120));
            EditorGUILayout.PropertyField(ledPuzzleSolved, new GUIContent(""), GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();
        #endregion
    }

	void OnSceneGUI( )
	{
	}
}
#endif