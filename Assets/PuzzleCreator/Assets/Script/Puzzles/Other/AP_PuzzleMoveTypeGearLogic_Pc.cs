// AP_PuzzleMoveTypeGearLogic_Pc: This script is used on puzzles Gear and Logic puzzles only. AP_PuzzleMoveType.cs is used for the other puzlle types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AP_PuzzleMoveTypeGearLogic_Pc : MonoBehaviour
{

    private int              puzzleMoveMode = 0;
    //private bool            type2Once = false;
    private bool            type1Once = false;
    private AP_Reticule_Pc s_Reticule;

    bool b_OnlyOneTimeNoSelection = false;
    bool b_OnlyOneTimeSelected = false;

    public Transform returnObjectSelectedObject(detectPuzzleClick_Pc _detectClick,
                                                AP_PuzzleDetector_Pc aP_PuzzleDetector,
                                                LayerMask myLayer,
                                                int validationButtonJoystick,
                                                Camera cameraUseForFocus)
    {
        #region
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;
        puzzleMoveMode = AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.puzlleIntearctionType;
        Transform objClicked;
        // Mode Focus
        if (puzzleMoveMode == 0)
        {
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, aP_GlobalPuzzle, validationButtonJoystick, cameraUseForFocus);
        }
        //VR Raycast
        else if (puzzleMoveMode == 1)
        {

            objClicked = _detectClick.Dec_VRRaycastCheckClick(myLayer);


            //if (_detectClick.DetectPuzzleStateObjectsRaycast(myLayer, false)) { aP_GlobalPuzzle.aP_DragAndDropParent.AP_CanGrabLogicOrGearMethods(); }
            //else { aP_GlobalPuzzle.aP_DragAndDropParent.AP_LogicOrGearNoSelectionMethods(); }
        }
        //VR Hand
        else if (puzzleMoveMode == 2 &&
                 ((!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(aP_GlobalPuzzle.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && aP_GlobalPuzzle.b_DesktopInputs && !aP_GlobalPuzzle.b_Joystick)
                 ||
                  (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(aP_GlobalPuzzle.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && aP_GlobalPuzzle.b_DesktopInputs && aP_GlobalPuzzle.b_Joystick)))
        {

            objClicked = aP_GlobalPuzzle.aP_DragAndDropParent.SelectedObjectLogicOrGear;//SelectedObjectPuzzleState;

        }
        // Mode Reticule
        else //if (puzzleMoveMode == 3)
        {
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, aP_GlobalPuzzle, validationButtonJoystick, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
        }


        return objClicked;
        #endregion
    }

    public void AP_InitAfterAPuzzleSolved()
    {
        #region
        AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
        if (puzzleMoveMode == 1)                   // Mode Raycast
        {
            refTrans.AP_LogicOrGearNoSelectionMethods();
            refTrans.AP_methodsLineModeRaycastReset();
        }
        #endregion
    }

    // Detect if Button Reset | Exit Puzzle | Clue are selected by the player
    public void feedbackPuzzleState(detectPuzzleClick_Pc _detectClick,
                                    AP_PuzzleDetector_Pc aP_PuzzleDetector,
                                    LayerMask myLayer,
                                    Camera cameraUseForFocus,
                                    bool b_PuzzleSolved)
    {
        #region
        Transform objClicked;
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;


        //Focus Mode
        if (aP_PuzzleDetector.b_FocusActivated)
        {
            objClicked = _detectClick.DetectPuzzleStateObject(myLayer, aP_GlobalPuzzle, 0, cameraUseForFocus);

            if (aP_GlobalPuzzle._joystickReticule &&
                aP_GlobalPuzzle.currentPuzzle &&
                aP_GlobalPuzzle.currentPuzzle.puzlleIntearctionType == 0)
            {
                if (objClicked &&
                AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)         // Can Grab Object
                    AP_FakeHandState(0);
                else
                    AP_FakeHandState(2);   // No Selection
                                       
            }
        }
        //VR Raycast
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 1)
        {
            if (!b_PuzzleSolved)
                objClicked = _detectClick.DetectPuzzleStateObjectsRaycast(myLayer,true);
            else
                objClicked = null;


        }
        //VR Hand
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 2)
        {
            if (!b_PuzzleSolved)
                objClicked = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectPuzzleState;
            else
                objClicked = null;
        }
        //Reticule Mode
        else
        {
            if (!b_PuzzleSolved)
            {
                objClicked = _detectClick.DetectPuzzleStateObject(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
            }
            else
            {
                objClicked = null;
            }

            if (objClicked &&
                AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)         // Can Grab Object
                reticuleState(0);
            else
                reticuleState(2);   // No Selection
        }


        if (objClicked != null &&
             objClicked.transform.parent.parent.gameObject == gameObject &&
            (objClicked.transform.name == "btnPuzzleReset" ||
             objClicked.transform.name == "ExitPuzzle" ||
             objClicked.transform.name == "Clue"))
        {

            AP_GlobalPuzzleManager_Pc aP_Global = AP_GlobalPuzzleManager_Pc.instance;

            if (aP_Global.currentPuzzleWithNoFocus &&
                aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 1 &&       // Mode Raycast
                aP_Global.aP_DragAndDropParent &&
                !type1Once)
            {
                aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGearModeRaycast);
                type1Once = true;
            }

            if (aP_Global.currentPuzzleWithNoFocus && 
                aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&       // Mode Hand
                aP_Global.aP_DragAndDropParent/* &&
                !type2Once*/)
            {
                //Debug.Log("Red");
                //aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGear);
                //type2Once = true;

                if (!b_OnlyOneTimeNoSelection)
                {
                    b_OnlyOneTimeSelected = false;
                    b_OnlyOneTimeNoSelection = true;
                    aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGear);
                }
            }

        }
        else
        {
            AP_GlobalPuzzleManager_Pc aP_Global = AP_GlobalPuzzleManager_Pc.instance;

            if (!b_OnlyOneTimeSelected &&
                 aP_Global.currentPuzzleWithNoFocus &&
                aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&       // Mode Hand
                aP_Global.aP_DragAndDropParent)
            {
                b_OnlyOneTimeSelected = true;
                b_OnlyOneTimeNoSelection = false;
                aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListLogicOrGearNoSelection);
            }

            //type2Once = false;
            type1Once = false;
        }
        #endregion
    }

    // Change the reticule color
    public void reticuleState(int reticuleState)
    {
        #region
        if (s_Reticule == null)
            s_Reticule = AP_GlobalPuzzleManager_Pc.instance.reticule.GetComponent<AP_Reticule_Pc>();

        if (s_Reticule)
        {
            if (!s_Reticule.b_CanGrab && reticuleState == 0)        // Grab is allowed
            { s_Reticule.callMethodsListCanGrabReticule(); }
            else if (!s_Reticule.b_Selected && reticuleState == 1)    // Selected
            { s_Reticule.callMethodsListReticuleSelected(); }
            else if (s_Reticule.b_CanGrab && reticuleState == 2)     // No Selection
            { s_Reticule.callMethodsReticuleNoSelection(); }
        }
        #endregion
    }


    // Change the reticule color
    public void AP_FakeHandState(int fakeHandState)
    {
        #region
        //Debug.Log("Here: " + fakeHandState);
        JoystickReticule_Pc joystickReticule = AP_GlobalPuzzleManager_Pc.instance._joystickReticule;
        if (joystickReticule)
        {
            if (!joystickReticule.b_CanGrab && fakeHandState == 0)        // Grab is allowed
            { AP_GlobalPuzzleManager_Pc.instance._joystickReticule.AP_FakeHand_CanGrab(); }
            else if (!joystickReticule.b_Selected && fakeHandState == 1)    // Selected
            { AP_GlobalPuzzleManager_Pc.instance._joystickReticule.AP_FakeHand_Selected(); }
            else if (joystickReticule.b_CanGrab && fakeHandState == 2)     // No Selection
            { AP_GlobalPuzzleManager_Pc.instance._joystickReticule.AP_FakeHand_NoSelection(); }
        }
        #endregion
    }

    public void puzzleStateButtons(detectPuzzleClick_Pc _detectClick,
                                    AP_PuzzleDetector_Pc aP_PuzzleDetector,
                                    LayerMask myLayer,
                                    Camera cameraUseForFocus,
                                    Component c_puzzle,
                                    GameObject puzzle,
                                    bool b_PuzzleSolved
                                  )
    {
        #region
        Transform objClicked;
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;


        //Focus Mode
        if (aP_PuzzleDetector.b_FocusActivated)
        {
            //Debug.Log("Here" + gameObject.name);
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, aP_GlobalPuzzle, 0, cameraUseForFocus);
        }
        //VR Raycast
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 1)
        {
            if (!b_PuzzleSolved)
                objClicked = _detectClick.Dec_VRRaycastCheckClick(myLayer);
            else
                objClicked = null;
        }
        //VR Hand
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&
                 ((!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(aP_GlobalPuzzle.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && aP_GlobalPuzzle.b_DesktopInputs && !aP_GlobalPuzzle.b_Joystick)
                ||
                  (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(aP_GlobalPuzzle.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && aP_GlobalPuzzle.b_DesktopInputs && aP_GlobalPuzzle.b_Joystick)))
        {
            if (!b_PuzzleSolved)
                objClicked = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectPuzzleState;
            else
                objClicked = null;
        }
        //Reticule Mode
        else
        {
            if (!b_PuzzleSolved)
                objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
            else
                objClicked = null;
        }


        if (objClicked != null && objClicked.transform.name == "btnPuzzleReset" && !b_PuzzleSolved && objClicked.transform.parent.parent.gameObject == gameObject) // Player press button reset 
        {
            //F_ResetPuzzle();
            Debug.Log("Reset");
            returnPuzzleType(c_puzzle, puzzle);

        }
        else if (objClicked != null && objClicked.transform.name == "ExitPuzzle" && objClicked.transform.parent.parent.gameObject == gameObject) // Player press button Exit puzzle 
        {
            if (aP_PuzzleDetector.b_FocusActivated)
            {
                Debug.Log("Exit Puzzle");
                aP_PuzzleDetector.Ap_DeactivatePuzzle();
            }
            else
            {
                Debug.Log("Focus is not activate so you can delete the Object ExitPuzzle inside the puzzle.: " + objClicked.name);
            }

        }
        else if (objClicked != null && objClicked.transform.name == "Clue" && objClicked.transform.parent.parent.gameObject == gameObject) // Player press button Clue
        {
            Debug.Log("Display Clue");
            AP_GlobalPuzzleManager_Pc.instance.AP_DisplayClue(gameObject.transform);
        }
        #endregion
    }

    void returnPuzzleType(Component newComponent, GameObject puzzle)
    {
        #region
        if (newComponent.GetType() == typeof(GearsPuzzle_Pc))
            puzzle.GetComponent<GearsPuzzle_Pc>().F_ResetPuzzle();

        if (newComponent.GetType() == typeof(LogicsPuzzle_Pc))
            puzzle.GetComponent<LogicsPuzzle_Pc>().F_ResetPuzzle();
        #endregion
    }


}
