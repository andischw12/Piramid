// Description : ToolMenu_Pc : Correspond to the Menu Tools -> AP -> ... in the Unity Menu bar
#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToolMenu_Pc : MonoBehaviour {

    //--> custom Collider from the Hierarchy
    [MenuItem ("GameObject/3D Object/Puzzle Creator/Collider Sphere")]
	public static void  createColliderSphere()
	{
        #region

        string objectPath = "Assets/PuzzleCreator/Assets/Prefabs/PC_System/Colliders/COLLIDER_Sphere.prefab";
		GameObject refButton = AssetDatabase.LoadAssetAtPath (objectPath, typeof(UnityEngine.Object)) as GameObject;

		if (Selection.activeGameObject == null) {
			if (EditorUtility.DisplayDialog ("Info : This action is not possible."
				, "You need to select an Object in the Hierarchy"
				, "Continue")) {}
		}
		else{
			
			GameObject newButton = Instantiate (refButton, Selection.activeGameObject.transform);
			Undo.RegisterCreatedObjectUndo (newButton, newButton.name);
            newButton.name = "COLLIDER_Sphere";
            Selection.activeGameObject = newButton;
		}
        #endregion
    }

    //--> custom Collider from the Hierarchy
    [MenuItem("GameObject/3D Object/Puzzle Creator/Collider Box")]
    public static void createColliderBox()
    {
        #region

        string objectPath = "Assets/PuzzleCreator/Assets/Prefabs/PC_System/Colliders/COLLIDER_Box.prefab";
        GameObject refButton = AssetDatabase.LoadAssetAtPath(objectPath, typeof(UnityEngine.Object)) as GameObject;

        if (Selection.activeGameObject == null)
        {
            if (EditorUtility.DisplayDialog("Info : This action is not possible."
                , "You need to select an Object in the Hierarchy"
                , "Continue")) { }
        }
        else
        {

            GameObject newButton = Instantiate(refButton, Selection.activeGameObject.transform);
            Undo.RegisterCreatedObjectUndo(newButton, newButton.name);
            newButton.name = "COLLIDER_Box";
            Selection.activeGameObject = newButton;
        }
        #endregion
    }

    //--> custom Collider from the Hierarchy
    [MenuItem("GameObject/3D Object/Puzzle Creator/Collider Capsule")]
    public static void createColliderCapsule()
    {
        #region

        string objectPath = "Assets/PuzzleCreator/Assets/Prefabs/PC_System/Colliders/COLLIDER_Capsule.prefab";
        GameObject refButton = AssetDatabase.LoadAssetAtPath(objectPath, typeof(UnityEngine.Object)) as GameObject;

        if (Selection.activeGameObject == null)
        {
            if (EditorUtility.DisplayDialog("Info : This action is not possible."
                , "You need to select an Object in the Hierarchy"
                , "Continue")) { }
        }
        else
        {

            GameObject newButton = Instantiate(refButton, Selection.activeGameObject.transform);
            Undo.RegisterCreatedObjectUndo(newButton, newButton.name);
            newButton.name = "COLLIDER_Capsule";
            Selection.activeGameObject = newButton;
        }
        #endregion
    }


}
#endif