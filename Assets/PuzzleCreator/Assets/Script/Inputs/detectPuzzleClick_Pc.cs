// Descition : detectPuzzleClick_Pc : Detect if the player click (mouse, gamepad and Mobile) on screen when a puzzle is activated
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectPuzzleClick_Pc {
   
    private float raycastDistance = 5;

    public Transform Dec_F_detectPuzzleClick(LayerMask myLayer, AP_GlobalPuzzleManager_Pc _globalPuzzleManager, int validationButtonJoystick, Camera objCamera)
    {
        #region
        //-> Joystick Case
        if (_globalPuzzleManager._joystickReticule == null && _globalPuzzleManager.b_AlwaysFind_joystickReticule)
            _globalPuzzleManager._joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;

        if (_globalPuzzleManager._joystickReticule &&
            _globalPuzzleManager._joystickReticule.transform &&
            _globalPuzzleManager.b_Joystick &&
            _globalPuzzleManager.b_DesktopInputs)
            return joystickCheckClick(myLayer, _globalPuzzleManager, objCamera);
        //-> Keyboard Case
        else if (!_globalPuzzleManager.b_Joystick && _globalPuzzleManager.b_DesktopInputs)
            return Dec_keyboardCheckClick(myLayer, objCamera);
        //-> Mobile case
        else
            return MobileCheckClick(myLayer, objCamera);
        #endregion
    }

    //--> Check mouse click
    public Transform Dec_keyboardCheckClick(LayerMask myLayer, Camera objCamera)
    {
        #region
        if(objCamera.gameObject != null){
            Ray ray = objCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
            {
                if (hit.transform.GetComponent<AP_CheckTag_Pc>() && 
                    (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" 
                    || 
                    hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject")     //Buttons: Reset puzzle / Clue / Exit 
                   )
                {
                    if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                        return hit.transform;
                }
            } 
        }
       

        return null;
        #endregion
    }

    //--> Check Joystick Click
    public Transform joystickCheckClick(LayerMask myLayer, AP_GlobalPuzzleManager_Pc _globalPuzzleManager, Camera objCamera)
    {
        #region
        if (_globalPuzzleManager._joystickReticule == null && _globalPuzzleManager.b_AlwaysFind_joystickReticule)
            _globalPuzzleManager._joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;

        Ray ray;
        // Mode Focus
        if(_globalPuzzleManager._joystickReticule && 
           _globalPuzzleManager.currentPuzzle && 
           _globalPuzzleManager.currentPuzzle.puzlleIntearctionType == 0){
            ray = objCamera.ScreenPointToRay(new Vector3(_globalPuzzleManager._joystickReticule.transform.position.x, _globalPuzzleManager._joystickReticule.transform.position.y, 0));
        }
        // Mode Reticule
        else {
            ray = objCamera.ScreenPointToRay(Input.mousePosition); 
        }
           

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
            {
                if (hit.transform.GetComponent<AP_CheckTag_Pc>() && 
                (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                    ||
                    hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject")     //Buttons: Reset puzzle / Clue / Exit 
                   )
                {
                    if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(_globalPuzzleManager.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                    {
                        return hit.transform;
                    }
                }
            }  



        return null;
        #endregion
    }
 
    //--> Check mouse click
    public Transform MobileCheckClick(LayerMask myLayer, Camera objCamera)
    {
        #region
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Ray ray = objCamera.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
                {
                    if (hit.transform.GetComponent<AP_CheckTag_Pc>() && 
                        (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                        ||
                         hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject") &&
                        !AP_GlobalPuzzleManager_Pc.instance.b_IsClueActivated)
                    {
                        return hit.transform;
                    }
                }
            }
        }

        return null;
        #endregion
    }

    //--> Detect if if a puzzle object is under the fake joystick reticule
    public bool F_detectPuzzleObject(LayerMask myLayer, AP_GlobalPuzzleManager_Pc _globalPuzzleManager)
    {
        #region
        //-> Joystick Case
        if (_globalPuzzleManager._joystickReticule == null && _globalPuzzleManager.b_AlwaysFind_joystickReticule)
            _globalPuzzleManager._joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;
        if (_globalPuzzleManager._joystickReticule &&
            _globalPuzzleManager._joystickReticule.transform && 
            _globalPuzzleManager.b_Joystick && 
            _globalPuzzleManager.b_DesktopInputs /*&&
            _ingameManager.navigationList.Count > 0 &&
            _ingameManager.navigationList[ingameGlobalManager.instance.navigationList.Count - 1] == "Focus"*/)
            return joystickCheckPuzzleObject(myLayer, _globalPuzzleManager._joystickReticule.transform, _globalPuzzleManager);

        return false;
        #endregion
    }

    public bool joystickCheckPuzzleObject(LayerMask myLayer,Transform ReticuleJoystick, AP_GlobalPuzzleManager_Pc _globalPuzzleManager)
    {
        #region
        if(AP_GlobalPuzzleManager_Pc.instance.returnMainCamera() && ReticuleJoystick){
            Ray ray = AP_GlobalPuzzleManager_Pc.instance.returnMainCamera().ScreenPointToRay(new Vector3(ReticuleJoystick.position.x, ReticuleJoystick.position.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
            {
                if (/*hit.transform.gameObject.CompareTag("PuzzleObject")*/ hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject")
                {
                    return true;
                }
            }
        }


        return false;
        #endregion
    }
     
    public Transform Dec_VRRaycastCheckClick(LayerMask myLayer)
    {
        #region
        AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
        if(refTrans){
            RaycastHit hit;
            //refTrans.AP_methodsLineModeRaycast();

            if (Physics.Raycast(refTrans.transform.position, refTrans.transform.forward, out hit, raycastDistance, myLayer))
            {
                if (hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                    (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                    ||
                    hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject")     //Buttons: Reset puzzle / Clue / Exit 
                   )
                {
                    if (AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs && 
                        !AP_GlobalPuzzleManager_Pc.instance.b_Joystick &&
                        !AP_GlobalPuzzleManager_Pc.instance.b_Pause &&
                        (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                        return hit.transform;
                    else if (AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs &&
                            AP_GlobalPuzzleManager_Pc.instance.b_Joystick &&
                            !AP_GlobalPuzzleManager_Pc.instance.b_Pause &&
                             (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                        return hit.transform;

                }
            
            }
        }

        return null;
        #endregion
    }

    public Transform DetectPuzzleStateObject(LayerMask myLayer, AP_GlobalPuzzleManager_Pc _globalPuzzleManager, int validationButtonJoystick, Camera objCamera)
    {
        #region
        //-> Joystick Case
        if (_globalPuzzleManager._joystickReticule == null && _globalPuzzleManager.b_AlwaysFind_joystickReticule)
            _globalPuzzleManager._joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;

        if (_globalPuzzleManager._joystickReticule &&
            _globalPuzzleManager._joystickReticule.transform &&
            _globalPuzzleManager.b_Joystick &&
            _globalPuzzleManager.b_DesktopInputs)
            return DetectPuzzleStateObjectJoystick(myLayer, _globalPuzzleManager, objCamera);
        //-> Keyboard Case
        else if (!_globalPuzzleManager.b_Joystick && _globalPuzzleManager.b_DesktopInputs)
            return DetectPuzzleStateObjectsKeyboard(myLayer, objCamera);
        //-> Mobile case
        else
            return MobileCheckClick(myLayer, objCamera);
        #endregion
    }

    public Transform DetectPuzzleStateObjectsRaycast(LayerMask myLayer,bool b_LineRaycastMode)
    {
        #region
        AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
        if (refTrans)
        {
            RaycastHit hit;

            if(b_LineRaycastMode)
            refTrans.AP_methodsLineModeRaycast();

            if (Physics.Raycast(refTrans.transform.position, refTrans.transform.forward, out hit, raycastDistance, myLayer))
            {
                if (hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                    (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                    ||
                    hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject")     //Buttons: Reset puzzle / Clue / Exit 
                   )
                {
                        return hit.transform;
                }

            }
        }

        return null;
        #endregion
    }

    public Transform DetectPuzzleStateObjectsKeyboard(LayerMask myLayer, Camera objCamera)
    {
        #region
        if (objCamera.gameObject != null)
        {
            Ray ray = objCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
            {
                if (hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                    (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                    ||
                     hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject"))     //Buttons: Reset puzzle / Clue / Exit 
                {
                        return hit.transform;
                }
            }
        }


        return null;
        #endregion
    }

    //--> Check Joystick Click
    public Transform DetectPuzzleStateObjectJoystick(LayerMask myLayer, AP_GlobalPuzzleManager_Pc _globalPuzzleManager, Camera objCamera)
    {
        #region
        if (objCamera.gameObject != null)
        {
            if (_globalPuzzleManager._joystickReticule == null && _globalPuzzleManager.b_AlwaysFind_joystickReticule)
                _globalPuzzleManager._joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;

            Ray ray;
            // Mode Focus
            if (_globalPuzzleManager._joystickReticule &&
               _globalPuzzleManager.currentPuzzle &&
               _globalPuzzleManager.currentPuzzle.puzlleIntearctionType == 0)
            {
                ray = objCamera.ScreenPointToRay(new Vector3(_globalPuzzleManager._joystickReticule.transform.position.x, _globalPuzzleManager._joystickReticule.transform.position.y, 0));
            }
            // Mode Reticule
            else
            {
                ray = objCamera.ScreenPointToRay(Input.mousePosition);
            }

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
            {
                if (hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                    (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                    ||
                            hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject"))     //Buttons: Reset puzzle / Clue / Exit 
                {
                    return hit.transform;
                }
            }
        }


        return null;
        #endregion
    }

    //--> Check mouse click
    public Transform DetectPuzzleStateObjectMobile(LayerMask myLayer, Camera objCamera)
    {
        #region
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Ray ray = objCamera.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
                {
                    if (hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                        (hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"
                        ||
                        hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject") && 
                        !AP_GlobalPuzzleManager_Pc.instance.b_IsClueActivated)
                    {
                        return hit.transform;
                    }
                }
            }
        }

        return null;
        #endregion
    }
}
