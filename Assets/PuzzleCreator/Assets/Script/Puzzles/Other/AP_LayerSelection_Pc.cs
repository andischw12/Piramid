//Description: AP_LayerSelection_Pc: Choose a specific layer for gameObject with this script attached to it
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_LayerSelection_Pc : MonoBehaviour
{
    public string selectedLayer = "puzzleDragAndDrop";

    private void Start()
    {
        if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal)
        {
            gameObject.layer = returnLayerUsed();
        }
    }


    private int returnLayerUsed()
    {
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;
        if (selectedLayer == "Puzzle")
            return aP_GlobalPuzzle._dataGlobal.currentLayerPuzzle;
        if (selectedLayer == "PuzzleFeedBackCam")
            return aP_GlobalPuzzle._dataGlobal.currentLayerPuzzleFeedbackCam;
        if (selectedLayer == "puzzleRay")
            return aP_GlobalPuzzle._dataGlobal.currentLayerPuzzleRay;
        if (selectedLayer == "puzzleDragAndDrop")
            return aP_GlobalPuzzle._dataGlobal.currentLayerPuzzleDragAndDrop;
        else
            return 0;
    }
}
