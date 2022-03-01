// Description: AP_DragAndDropParent_Pc: Use on VR Fake Hand (001)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_DragAndDropParent_Pc : MonoBehaviour
{
    public bool         SeeInspector = false;
    public Color        color_01 = new Color(1, .8f, 0.2F, .4f);
    public Color        color_02 = new Color(.3F, .9f, 1, .5f);
    public Color        color_03 = new Color(1, .5f, 0.3F, .4f);

    public Transform    SelectedObjectAxis; 
    public Transform    SelectedObjectLogicOrGear;
    public Transform    SelectedObjectPuzzleState;

    private bool        b_SelectedObjectLogicOrGear = false;
    private bool        b_SelectedObjectAxis = false;
    private bool        b_SelectedPuzzleStateObject = false;

    public bool         b_ObjSelected = false;


    public float        sphereRaySize = .01f;
    public float        raycastDistance = 5;

    private Renderer    rend;


    public List<EditorMethodsList_Pc.MethodsList> methodsListCanGrabLogicOrGear      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();
    public List<EditorMethodsList_Pc.MethodsList> methodsListLogicOrGearSelected      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();
    public List<EditorMethodsList_Pc.MethodsList> methodsListLogicOrGearNoSelection      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();


    public List<EditorMethodsList_Pc.MethodsList> methodsListCanGrabLogicOrGearModeRaycast      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();
    public List<EditorMethodsList_Pc.MethodsList> methodsListLogicOrGearSelectedModeRaycast      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();
    public List<EditorMethodsList_Pc.MethodsList> methodsListLogicOrGearNoSelectionModeRaycast      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsLineModeRaycast      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsLineModeRaycastReset      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.

    //public float Mode = 0;      // which mode is selected for the current puzzle 0: Focus | 1: Reticule | 2: Hand 
    bool b_OnlyOneTimeCanGrab = false;
    bool b_OnlyOneTimeNoSelection = false;
    bool b_OnlyOneTimeSelected = false;

    private void Start()
    {
        if(GetComponent<Renderer>())
            rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        #region
        if (AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus &&
           AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.puzlleIntearctionType == 2) // Mode Hand
            CheckTriggerInterac();
        #endregion
    }


    void CheckTriggerInterac()
    {
        #region
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRaySize);
        b_SelectedObjectLogicOrGear = false;
        b_SelectedObjectAxis = false;
        b_SelectedPuzzleStateObject = false;
        for (var i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].transform.GetComponent<AP_CheckTag_Pc>() &&
                hitColliders[i].transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                !AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.b_PuzzleIsSolved)
            {
                // Puzzle Gear and Logics
                if (hitColliders[i].transform.GetComponent<Rigidbody>() && hitColliders[i].gameObject.layer ==  AP_GlobalPuzzleManager_Pc.instance._dataGlobal.currentLayerPuzzleDragAndDrop)                //
                    SelectedObjectLogicOrGear = hitColliders[i].transform;
                else if (hitColliders[i].transform.parent.GetComponent<Rigidbody>())
                    SelectedObjectLogicOrGear = hitColliders[i].transform.parent;
                // Other Puzzles: Pipe, Slide, Digicode,Cylinder, Lever
                else if(hitColliders[i].gameObject.layer == AP_GlobalPuzzleManager_Pc.instance._dataGlobal.currentLayerPuzzle)
                    SelectedObjectLogicOrGear = hitColliders[i].transform;

                b_SelectedObjectLogicOrGear = true;
                //Debug.Log(hitColliders[i].name);
                //AP_CanGrabLogicOrGear();
                b_OnlyOneTimeNoSelection = false;
                //     Debug.Log("2");

                if (!b_OnlyOneTimeCanGrab)
                {
                    b_OnlyOneTimeCanGrab = true;
                    b_OnlyOneTimeSelected = false;
                    callMethods.Call_A_Method(methodsListCanGrabLogicOrGear);
                }
               
            }

            if (hitColliders[i].transform.GetComponent<AP_CheckTag_Pc>() &&
                hitColliders[i].transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition" &&
                !AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.b_PuzzleIsSolved)
            {
                SelectedObjectAxis = hitColliders[i].transform;
                b_SelectedObjectAxis = true;
                //Debug.Log(hitColliders[i].name);
            }

            if (hitColliders[i].transform.GetComponent<AP_CheckTag_Pc>() &&
                hitColliders[i].transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject" &&
                !AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.b_PuzzleIsSolved)
            {
                SelectedObjectPuzzleState = hitColliders[i].transform;
                b_SelectedPuzzleStateObject = true;
            }

        }


        if (!b_SelectedObjectLogicOrGear)
        {
            SelectedObjectLogicOrGear = null;
        }

        if (!b_SelectedObjectLogicOrGear && !b_SelectedPuzzleStateObject)
        {
            if (!b_OnlyOneTimeNoSelection)
            {                   // Mode Free: Raycast
                b_OnlyOneTimeNoSelection = true;
                b_OnlyOneTimeCanGrab = false;
                b_OnlyOneTimeSelected = false;
                callMethods.Call_A_Method(methodsListLogicOrGearNoSelection);
            }
           
            SelectedObjectLogicOrGear = null;
            SelectedObjectPuzzleState = null;
        }

        if (b_ObjSelected)
        {
            if (!b_OnlyOneTimeSelected)
            {                   // Mode Free: Raycast
                b_OnlyOneTimeSelected = true;
                callMethods.Call_A_Method(methodsListLogicOrGearSelected);
            }
        }
        else
        {
            if(b_OnlyOneTimeSelected)
                b_OnlyOneTimeCanGrab = false;
        }


        if (!b_SelectedObjectAxis)
        {
            SelectedObjectAxis = null;

        }

        if (!b_SelectedPuzzleStateObject)
        {
            SelectedObjectPuzzleState = null;

        }
        #endregion
    }

    void OnDrawGizmos()
    {
        #region
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereRaySize);
        #endregion
    }

     

    public void AP_CanGrabLogicOrGear()
    {
        #region
        if (rend && rend.material.color != Color.red)
        {
            if(bVRHandTouch)
                VRHandTouch();
            rend.material.color = Color.red;
        }
        #endregion
    }

    public void AP_LogicOrGearSelected()
    {
        #region

        if (rend && rend.material.color != Color.white)
        {
            rend.material.color = Color.white;
        }
        #endregion
    }

    public void AP_LogicOrGearNoSelection()
    {
        #region

        if (rend && rend.material.color != Color.white)
        {
            rend.material.color = Color.white;
        }
        #endregion
    }

    public void AP_LineRaycast()
    {
        #region
        RaycastHit hitMode1;
        if (Physics.Raycast(transform.position, transform.forward, out hitMode1, raycastDistance))
        {
            float tmpDistance = hitMode1.distance;
            if (tmpDistance > raycastDistance*.1f)
                tmpDistance = raycastDistance * .1f;
            transform.GetChild(0).localScale = new Vector3(raycastDistance * .1f, raycastDistance * .1f, tmpDistance * 40);
            Debug.DrawRay(transform.position, transform.forward * raycastDistance * .1f, Color.white);
        }
        else
        {
            transform.GetChild(0).localScale = new Vector3(0.5f, 0.5f, raycastDistance * .1f * 40);
        }
        #endregion
    }

    public void AP_LogicOrGearSelectedMethods()
    {
        #region
        callMethods.Call_A_Method(methodsListLogicOrGearSelectedModeRaycast);
        #endregion
    }

    public void AP_LogicOrGearNoSelectionMethods()
    {
        #region
        callMethods.Call_A_Method(methodsListLogicOrGearNoSelectionModeRaycast);
        #endregion
    }

    public void AP_CanGrabLogicOrGearMethods()
    {
        #region
        callMethods.Call_A_Method(methodsListCanGrabLogicOrGearModeRaycast);
        #endregion
    }

    public void AP_methodsLineModeRaycast(){
        #region
        callMethods.Call_A_Method(methodsLineModeRaycast);
        #endregion
    }

    public void AP_methodsLineModeRaycastReset()
    {
        #region
        callMethods.Call_A_Method(methodsLineModeRaycastReset);
        #endregion
    }

    // Call when the player exit a puzzle trigger: Init the Fake Raycast and Cube
    public void AP_InitFakeRay()
    {
        #region
        transform.GetChild(0).localScale = Vector3.zero;
        AP_LogicOrGearNoSelection();
        #endregion
    }

   public bool bVRHandTouch = false;
   void VRHandTouch() {
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;
        Transform objClicked = null;

        //-> The player is selecting a puzzle button
        if (aP_GlobalPuzzle.aP_DragAndDropParent.SelectedObjectLogicOrGear != null)
        {
            objClicked = aP_GlobalPuzzle.aP_DragAndDropParent.SelectedObjectLogicOrGear;


            if (aP_GlobalPuzzle.currentPuzzleWithNoFocus != null && objClicked != null)
            {
                //-> Digicode Case
                if (aP_GlobalPuzzle.currentPuzzleWithNoFocus.transform.parent.GetComponent<DigicodePuzzle_Pc>())
                {
                    DigicodePuzzle_Pc digi = aP_GlobalPuzzle.currentPuzzleWithNoFocus.transform.parent.GetComponent<DigicodePuzzle_Pc>();

                    for (var i = 0; i < digi.tilesList.Count; i++)
                    {
                        if (digi.tilesList[i].transform.GetChild(0) == objClicked)
                        {
                            digi.VRTouch(objClicked.gameObject, true);
                            break;
                        }
                    }
                }
            }
        }
        //-> The player is selecting Reset Clue Exit
        else if (aP_GlobalPuzzle.aP_DragAndDropParent.SelectedObjectPuzzleState != null)
        {
            objClicked = aP_GlobalPuzzle.aP_DragAndDropParent.SelectedObjectPuzzleState;

            if (aP_GlobalPuzzle.currentPuzzleWithNoFocus != null && objClicked != null)
            {
                //-> Digicode Case
                if (aP_GlobalPuzzle.currentPuzzleWithNoFocus.transform.parent.GetComponent<DigicodePuzzle_Pc>())
                {
                    DigicodePuzzle_Pc digi = aP_GlobalPuzzle.currentPuzzleWithNoFocus.transform.parent.GetComponent<DigicodePuzzle_Pc>();

                    digi.VRTouch(objClicked.gameObject, false);
                }
            }
        }
    }



}
