//Descritpion : windowMethods_Pc : A collection of methods use in multiple window tab.
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine.SceneManagement;

public class windowMethods_Pc {

    public bool bool_UpdateProcessDone = false;         // use to launch game in the editor mode (Button : Update + play)


    public void saveLevelInfos()
    {
        if (EditorUtility.DisplayDialog("Info: Save System",
                                        ".Dat and PlayersPrefs have been delete for this scene. ", "Continue"))
        {
            List<GameObject> listGameObject = new List<GameObject>();
            List<string> listString = new List<string>();

            int numberTotal = 0;
            int number = 0;
            GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject go in allObjects)
            {
                Transform[] Children = go.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in Children)
                {
                    if (child.GetComponent<SaveData_Pc>())
                    {
                        listGameObject.Add(child.gameObject);
                        listString.Add(child.GetComponent<SaveData_Pc>().R_SaveData());

                        number++;
                    }
                    numberTotal++;
                }
            }
            GameObject tmp = GameObject.Find("ScenePuzzleManager");
            string tmpString = "";
            if (tmp)
            {
                Undo.RegisterFullObjectHierarchyUndo(tmp, tmp.name);

                AP_ScenePuzzleManager_Pc levelManager = tmp.GetComponent<AP_ScenePuzzleManager_Pc>();

                levelManager.listOfPuzzles.Clear();
                levelManager.listState.Clear();

                for (var i = 0; i < listGameObject.Count; i++)
                {
                    levelManager.listOfPuzzles.Add(listGameObject[i].GetComponent<SaveData_Pc>());
                    levelManager.listState.Add(false);
                    tmpString += listString[i];
                }

            }
            else
            {
                tmp = GameObject.Find("MM_NoLevelManager");

                if (!tmp)
                    //Debug.Log ("Info : You need a LevelManager in your scene to be allowed to save data for this level");
                    if (EditorUtility.DisplayDialog("Info : This action is not possible."
                        , "You need an object LevelManager in your scene to record data for this level. LevelManager need to have LevelManager.cs attached to it."
                        , "Continue")) { }
            }
           EraseCurrentSceneSaveData();
        }
    }

    public void EraseCurrentSceneSaveData(){
        for (var i = 0; i < 30; i++)
        {    // Check all the Slots
             //Delete .Dat
            string itemPath = Application.persistentDataPath;
            itemPath = itemPath.TrimEnd(new[] { '\\', '/' });
            FileUtil.DeleteFileOrDirectory(itemPath + "/" + i + "_" + "Puzzles_" + SceneManager.GetActiveScene().buildIndex.ToString() + ".dat");

            //Delete PlayerPrefs
            if (PlayerPrefs.HasKey(i + "_" + "Puzzles_" + SceneManager.GetActiveScene().buildIndex.ToString()))
                PlayerPrefs.DeleteKey(i + "_" + "Puzzles_" + SceneManager.GetActiveScene().buildIndex.ToString());
        }
        Debug.Log("Save System Updated"); 
    }

   
}
#endif