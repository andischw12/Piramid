//Desciption: AP_PlayerInfos_Pc: This script is used to access easily to various variables during the game (gameObject Canvas_UIPuzzle).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AP_PlayerInfos_Pc : MonoBehaviour
{
    public Image                reticule;
    public Image                iconPuzzle;
    public Transform            reticuleJoystickImage;
    public JoystickReticule_Pc _joystickReticule;
    public Image                Image_ObjDetected;
    public GameObject           btn_PuzzleMobile;
    public GameObject           btn_GrabObjectMobile;
    public bool                 b_GrabActivated = false;

    private AP_PuzzleRaycast_Pc objPuzzleRaycast;

    public AP_Reticule_Pc s_reticule;

    public void Ap_BtnActivatePuzzle(){
        if (objPuzzleRaycast == null)
            objPuzzleRaycast = GameObject.Find("puzzleRaycast").GetComponent<AP_PuzzleRaycast_Pc>();

        if (objPuzzleRaycast != null)
            objPuzzleRaycast.AP_ActivatePuzzle();
    }


    public void Ap_BtnGrabObjectPuzzle()
    {
        AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.accessPuzzle.GetComponent<AP_.DragAndDrop_Pc>().AP_MobileSelectObjectToGrab(b_GrabActivated);
        b_GrabActivated = !b_GrabActivated;
    }
}
