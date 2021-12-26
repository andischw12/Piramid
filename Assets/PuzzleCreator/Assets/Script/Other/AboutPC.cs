// Description : MCRTestingTrack.cs : This script is used to create a menu that allow to setup the global preferences of MCR Creator
#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AboutPC : EditorWindow
{

	private Vector2 scrollPosition = Vector2.zero;

	// Add menu item named "Test Mode Panel" to the Window menu
	[MenuItem("Tools/Puzzles/About")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(AboutPC));
	}

	void OnEnable () {
		
			
	}

	void OnGUI()
	{
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, true, true, GUILayout.Width (position.width), GUILayout.Height (position.height));


		EditorGUILayout.LabelField("Current version:", EditorStyles.boldLabel);
		EditorGUILayout.HelpBox("Puzzle Creator Version 1.0.1 Unity 2020.3.0f1 (LTS)" + "\n" +
		"Gamepad: Correction" + "\n" +
		"[Missing] AP_GlobalPuzzleManagerEditor_Pc", MessageType.Info);


		EditorGUILayout.LabelField("Old verion: ", EditorStyles.boldLabel);

		EditorGUILayout.HelpBox("Puzzle Creator Version 1.0 Unity 2020.3.0f1 (LTS)" + "\n" +
		"Now data is saved as JSON file", MessageType.Info);


		EditorGUILayout.HelpBox("Puzzle Creator Version 1.0 Unity 2020.1.0f1" + "\n" +
			"2020.1 Update", MessageType.Info);

		EditorGUILayout.HelpBox ("Puzzle Creator Version 1.0 Unity 2019.3.0f6 (1)" +"\n" +
            "New VR Tuto", MessageType.Info);


		GUILayout.EndScrollView();
	}

	

	void OnInspectorUpdate()
	{
        //Repaint();
	}

}
#endif