// Description : datasWindowReadyToUse : ScriptableObject : Save data for the window w_ObjCreator
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class datasWindowReadyToUse_Pc : ScriptableObject 
{
	public bool 		    helpBoxEditor = false;							// Show helpbox in the window tab
	//public string 		currentDatasProjectFolder = "Default";			// The folder where are the datas for the game (inventory, infos, feedback,diary...)
    public int 			    currentTypeSelected = 0;
    public List<GameObject> listOfPuzzles = new List<GameObject>();
    public bool             b_SaveSystem = true;
    public bool             b_ActivatedTheFirstTime = true;

    public GameObject       clueSystem;

    public GameObject       StarterKit;

    public int currentLayerPuzzle = 15;
    public int currentLayerPuzzleFeedbackCam = 16;
    public int currentLayerPuzzleRay = 19;
    public int currentLayerPuzzleDragAndDrop = 20;

    public bool SaveAsPlayerPrefs = false;

}

