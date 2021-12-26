// Description : GearsPuzzle_Pc : Manage Gear Puzzle
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearsPuzzle_Pc : MonoBehaviour {
    public bool                         SeeInspector = false;
    public bool                         helpBoxEditor = true;                   

    public List<Texture2D>              GearSprite = new List<Texture2D>();

    public int                          GearType = 0;      // Gears type : No Move, Horizontal, Vertical, T, Elbow 
    public int                          puzzleSubType = 0;  // Horizontal/ Vertical or nested/Align

    public int                          HowManyGearsPosition = 2;
    public List<int>                    GearsTypeList = new List<int>();
    public List<int>                    AxisTypeList = new List<int>();

    public List<bool>                   AxisRotationRight = new List<bool>();

    public List<bool>                   GearsUseOrFakeList = new List<bool>();
    public List<bool>                   GearsInitPositionWhenStart = new List<bool>();
    public List<bool>                   GearsAvailableWhenStart = new List<bool>();
    public List<bool>                   AxisAvailableWhenStart = new List<bool>();

    public List<int>                    GearsPositionList = new List<int>();
   
    public List<int>                    GearsSolutionList = new List<int>();

    public List<int>                    inGameGearsPositionList = new List<int>();
    public List<bool>                   inGameAxis = new List<bool>();

    public int                          selectDefaultTile = 0;
    public List<GameObject>             defaultTileList = new List<GameObject>();
    public GameObject                   defaultTile;

    public int                          _Column = 3;
    public int                          _NumberOfKey = 10;
            
    public int                          toolbarCurrentValue = 0;
    public int                          SquareSize = 80;
    public int                          currentSelectedSprite = 0;
    public List<GameObject>             pivotGearList = new List<GameObject>();
    public List<GameObject>             GearList = new List<GameObject>();
    private List<Gear_Pc>                  pGearList = new List<Gear_Pc>();
    private List<GearLogicCheckCollision_Pc>    pGearOnTriggerList = new List<GearLogicCheckCollision_Pc>();

    public List<int>                    positionList = new List<int>();

    public bool                         b_PuzzleSolved = false;         // Know if the puzzle is solved
    public LayerMask                    myLayer;                        // Raycast is done only on layer 15 : Puzzle
    public bool                         b_UsePuzzleFocus = true;        // Use the puzzle focus. If false : focus step is bypass
    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component

    private detectPuzzleClick_Pc _detectClick;

    public AudioClip                    a_KeyPressed;
    public float                        a_KeyPressedVolume = 1;
    public AudioClip                    a_Reset;
    public float                        a_ResetVolume = 1;

    private AudioSource                 a_Source;

    public bool                         VisualizeSprite = true;

    public Text                         txt_result;

    private AP_.DragAndDrop_Pc dragAndDrop;
    private List<SpriteRenderer>        listOfSelectedPuzzlePosition = new List<SpriteRenderer>();

    [System.Serializable]
    public class idList
    {
        public int ID = 0;          // entry ID in the window tab
        public int uniqueID = 0;    // entry Unique ID
    }


    public GameObject                   currentSelectedObject;

    public Camera                       cameraUseForFocus;
    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.
    public bool                         b_OnlyTheFirstTime = true;

    public AP_PuzzleDetector_Pc aP_PuzzleDetector;
    public AP_PuzzleMoveTypeGearLogic_Pc aP_PuzzleMoveTypeGearLogic;

    //private bool type2Once = false;
    //private bool type1Once = false;



    // Use this for initialization
    void Start()
    {
        #region
        dragAndDrop = GetComponent<AP_.DragAndDrop_Pc>();

        for (var i = 0; i < pivotGearList.Count;i++){
            listOfSelectedPuzzlePosition.Add(pivotGearList[i].transform.parent.GetComponent<SpriteRenderer>()); 
            pGearList.Add(GearList[i].GetComponent<Gear_Pc>());
            pGearOnTriggerList.Add(GearList[i].transform.GetChild(0).GetComponent<GearLogicCheckCollision_Pc>());
            inGameGearsPositionList.Add(-1);
            inGameAxis.Add(false);

            if (!GearsInitPositionWhenStart[i])
            {
                GearList[i].transform.GetChild(0).GetComponent<AP_CheckTag_Pc>()._Tag = "GearFixed";
                pivotGearList[i].transform.parent.GetComponent<AP_CheckTag_Pc>()._Tag = "";
                GearList[i].transform.tag = "Untagged";
            }
        }

        InitListOfHandsInDragAndDropScript();

        //--> Every Puzzle  ----> BEGIN <----

        _conditionsToAccessThePuzzle = GetComponent<conditionsToAccessThePuzzle_Pc>();
        _actionsWhenPuzzleIsSolved = GetComponent<actionsWhenPuzzleIsSolved_Pc>();


        aP_PuzzleMoveTypeGearLogic = GetComponent<AP_PuzzleMoveTypeGearLogic_Pc>();
        //----> END <----


        //--> Common for all puzzle ----> BEGIN <----
        _detectClick = new detectPuzzleClick_Pc();                 // Access Class that allow to detect click (Mouse, Joystick, Mobile) 

        a_Source = GetComponent<AudioSource>();

        // Update layer Mask using the data wObjCreator.
        if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal)
        {
            AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;
            string[] listLayer = new string[2] {
                LayerMask.LayerToName(aP_GlobalPuzzle._dataGlobal.currentLayerPuzzle),
                LayerMask.LayerToName(aP_GlobalPuzzle._dataGlobal.currentLayerPuzzleDragAndDrop) };

            myLayer = LayerMask.GetMask( listLayer); 
        }
        //----> END <----
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region
        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated/* && !ingameGlobalManager.instance.b_Ingame_Pause*/)
        {
            if (b_OnlyTheFirstTime)
            {
                b_OnlyTheFirstTime = false;
                callMethods.Call_A_Method(methodsList);
            }

            if (listOfSelectedPuzzlePosition.Count > 0 && !b_PuzzleSolved)
            {
                if (aP_PuzzleDetector.b_FocusActivated)
                    dragAndDrop.F_DragAndDrop(listOfSelectedPuzzlePosition, cameraUseForFocus);
                else
                    dragAndDrop.F_DragAndDrop(listOfSelectedPuzzlePosition, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
            }



            if (dragAndDrop.returnCurrentSelectedObject() != null)
            {
                currentSelectedObject = dragAndDrop.returnCurrentSelectedObject();
            }



            PuzzleBehaviour();
        }

        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated && // All Case puzzle is not solved
        _conditionsToAccessThePuzzle.b_PuzzleStateButtons

        ||

           b_PuzzleSolved &&                                    // Case Focus: Puzzle is already solved
        _conditionsToAccessThePuzzle.b_PuzzleStateButtons &&
          aP_PuzzleDetector.b_FocusActivated)
        {
            aP_PuzzleMoveTypeGearLogic.feedbackPuzzleState(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, b_PuzzleSolved);
            aP_PuzzleMoveTypeGearLogic.puzzleStateButtons(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, GetComponent<GearsPuzzle_Pc>(), gameObject, b_PuzzleSolved);
        }
            
        #endregion
    }

//------> BEGIN <------ Next 6 methods are always needed in a puzzle script 

 
//--> Reset Puzzle when button iconResetPuzzle in Canvas_PlayerInfos is pressed
    public void F_ResetPuzzle(){
        #region
        if (a_Source && a_Reset)
        {
            a_Source.clip = a_Reset;
            a_Source.volume = a_ResetVolume;
            a_Source.Play();
        }
        initCurrentPosition("");                             // init Gears position
        #endregion
    }


    //--> Actions when puzzle is solved
    private void puzzleSolved()
    {
        #region
        dragAndDrop.initAllSpriteWhenPuzzleIsSolved();                      // Init puzzle SPrites

        if (!b_PuzzleSolved)
            StartCoroutine(I_gearsRotations());

        //-> Actions done for all type of puzzle
        if (!b_PuzzleSolved || CanvasD_Pc.instance && CanvasD_Pc.instance._P)
            _actionsWhenPuzzleIsSolved.F_PuzzleSolved();                   // Call script actionsWhenPuzzleIsSolved. Do actions when the puzzle is solved the first time.
        else
            _actionsWhenPuzzleIsSolved.b_actionsWhenPuzzleIsSolved = true; // Use when focus is called. The variable b_actionsWhenPuzzleIsSolved in script puzzleSolved equal True


        _conditionsToAccessThePuzzle.changeSpriteAndLedWhenPuzzleSolved();

        //if (_conditionsToAccessThePuzzle.ledPuzzleSolved) _conditionsToAccessThePuzzle.ledPuzzleSolved.AP_Btn_On();       // Led switch On
        //if (_conditionsToAccessThePuzzle.puzzleSprite) _conditionsToAccessThePuzzle.puzzleSprite.AP_ChangeSprite(2);   // Sprite: Solved

        if (_actionsWhenPuzzleIsSolved.objectActivatedWhenPuzzleIsSolved)
            _actionsWhenPuzzleIsSolved.objectActivatedWhenPuzzleIsSolved.SetActive(true);

        b_PuzzleSolved = true;
       

        aP_PuzzleDetector.b_PuzzleIsSolved = true;
        aP_PuzzleMoveTypeGearLogic.AP_InitAfterAPuzzleSolved();    

        #endregion
    }

    public List<bool> gearAssociatedToAxisWhenGameSolvedNeedToRotate = new List<bool>();

    private IEnumerator I_gearsRotations()
    {
        #region
        float timer = 0;

        //Debug.Log("Rotation");

        for (var i = 0; i < GearsPositionList.Count; i++)
        {
            bool b_State = false;
            for (var j = 0; j < GearsPositionList.Count; j++)
            {
                if (pivotGearList[j].transform.position == GearList[i].transform.GetChild(0).transform.position
                    && !GearsUseOrFakeList[j])
                {
                    gearAssociatedToAxisWhenGameSolvedNeedToRotate.Add(true);
                    b_State = true;
                }

            }
            if (!b_State)
                gearAssociatedToAxisWhenGameSolvedNeedToRotate.Add(false);
        }

        while (timer != 2)
        {
            timer = Mathf.MoveTowards(timer, 2, Time.deltaTime);

            for (var i = 0; i < pivotGearList.Count; i++)
            {
                float dir = 1;
                if (AxisRotationRight[i])
                    dir = -1;

                if (!GearsUseOrFakeList[i])
                    pivotGearList[i].transform.Rotate(Vector3.forward * dir * Time.deltaTime * 100, Space.Self);

                if (gearAssociatedToAxisWhenGameSolvedNeedToRotate[i])
                    GearList[i].transform.GetChild(0).transform.Rotate(Vector3.forward * dir * Time.deltaTime * 100, Space.Self);

            }
            yield return null;
        }


        yield return null;
        #endregion
    }

    //--> Use to load object state. Initialize the puzzle  (T = True or F = False)
    public void saveSystemInitGameObject(string s_ObjectDatas)
    {
        #region
        string[] codes = s_ObjectDatas.Split('_');              // Split data in an array.
        int number = 0;

        //--> Actions to do for this puzzle ----> BEGIN <----
        if (s_ObjectDatas == "")
        {                               // Save Doesn't exist
            initCurrentPosition("SaveDoesntExist");
        }
        else
        {                                                   // Save exist
            //txt_result.text = codes[1];
            if (inGameGearsPositionList.Count == 0)                          // Load saved value in inGameGearsPositionList
            {
                inGameGearsPositionList.Add(-1);
                number++;
            }

            for (var i = 0; i < inGameGearsPositionList.Count; i++)
            {
                inGameGearsPositionList[i] = int.Parse(codes[i + 1]);
                number++;
            }
            for (var i = 0; i < inGameGearsPositionList.Count; i++)
            {
                if (codes[i + inGameGearsPositionList.Count + 1] == "T")
                    inGameAxis[i] = true;
                else
                    inGameAxis[i] = false;
                number++;
            }

            // Debug.Log("Number : " + number);
            /* number++;
             if (codes[number] == "T")
                 b_popUpDone = true;
             else
                 b_popUpDone = false;

             number++;

             if (codes[number] == "T")
                 b_UIFeedback = true;
             else
                 b_UIFeedback = false; */


            initCurrentPosition("SaveExist");
        }
        //----> END <----



        //--> Actions to do for all puzzle ----> BEGIN <----
        if (codes[0] == "T")
        {                                                       // Element 0 : Check if the puzzle is solved                           
            b_PuzzleSolved = true;
            puzzleSolved();
        }
        else
        {
            GetComponent<conditionsToAccessThePuzzle_Pc>().checkAccessAllowed();
            //if (_actionsWhenPuzzleIsSolved.objectActivatedWhenPuzzleIsSolved)
            //  _actionsWhenPuzzleIsSolved.objectActivatedWhenPuzzleIsSolved.SetActive(false);
        }

        // if(!b_PuzzleSolved)
        //   _actionsWhenPuzzleIsSolved.initSolvedSection();    // Init Popup object in script actionsWhenPuzzleIsSolved.cs
        //----> END <----

        #endregion
    }

    //--> Use to save Object state
    public string ReturnSaveData()
    {
        #region
        //-> Common for all puzzle ----> BEGIN <----
        string value = "";

        value += r_TrueFalse(b_PuzzleSolved) + "_";     // b_PuzzleSolved : Save if the puzzle is solved or not
        //----> END <----


        //-> Specific for this puzzle ----> BEGIN <----
        for (var i = 0; i < inGameGearsPositionList.Count; i++)
        {
            value += inGameGearsPositionList[i] + "_";          // Save the current Gears or Cercle position
        }
        for (var i = 0; i < inGameAxis.Count; i++)
        {
            value += r_TrueFalse(inGameAxis[i]) + "_";          // Save the current Gears or Cercle position
        }

        /*value += r_TrueFalse(b_popUpDone) + "_"; 
        value += r_TrueFalse(b_UIFeedback) + "_"; 
        */
        //----> END <---- 


        return value;
        #endregion
    }

//--> Convert bool to T or F string
    private string r_TrueFalse(bool s_Ref)
    {
        if (s_Ref) return "T";
        else return "F";
    }


    //------> END <------


    //--> The Puzzle Behaviour
    private void PuzzleBehaviour()
    {
        #region
        Vector3 cursorPosition = Input.mousePosition;

        if (!b_PuzzleSolved)
        {

            Transform objClicked;


            if (!b_PuzzleSolved)
                objClicked = aP_PuzzleMoveTypeGearLogic.returnObjectSelectedObject(_detectClick, aP_PuzzleDetector, myLayer, 0, cameraUseForFocus);
            else
                objClicked = null;

            //if (aP_PuzzleDetector.b_FocusActivated) objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, cameraUseForFocus);
            //else objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, Camera.main);


            if (objClicked != null && objClicked.transform.name == "btnPuzzleReset" && objClicked.parent.parent.gameObject == gameObject) // Player press button reset 
            {
                F_ResetPuzzle();
            }
            else if (objClicked != null)
            {                                                     // Player press validation button on a puzzle object

                if (a_Source && a_KeyPressed)
                {
                    a_Source.clip = a_KeyPressed;
                    a_Source.volume = a_KeyPressedVolume;
                    a_Source.Play();
                }
            }

            if (dragAndDrop.b_DeselectProcessEnded)
                CheckIfPuzzleSolved();
        }
        #endregion
    }

    /*
    // Detect if Button Reset | Exit Puzzle | Clue are selected by the player
    public void feedbackPuzzleState(){
        #region
        Transform objClicked;
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;


        //Focus Mode
        if (aP_PuzzleDetector.b_FocusActivated)
        {
            objClicked = _detectClick.DetectPuzzleStateObject(myLayer, aP_GlobalPuzzle, 0, cameraUseForFocus);
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

            objClicked = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectPuzzleState;
        }
        //Reticule Mode
        else
        {
            objClicked = _detectClick.DetectPuzzleStateObject(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
        }

       
        if (objClicked != null &&
            (objClicked.transform.name == "btnPuzzleReset" ||
             objClicked.transform.name == "ExitPuzzle" ||
             objClicked.transform.name == "Clue"))
        {

            AP_GlobalPuzzleManager_Pc aP_Global = AP_GlobalPuzzleManager_Pc.instance;

            if (aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 1 &&       // Mode Raycast
                aP_Global.aP_DragAndDropParent && 
                !type1Once)
            {
                aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGearModeRaycast);
                type1Once = true;
            }

            if (aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&       // Mode Hand
                aP_Global.aP_DragAndDropParent &&
                !type2Once)
            {
                aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGear);
                type2Once = true;
            }

            if (aP_Global.currentPuzzleWithNoFocus &&
                aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 3 &&
                aP_Global.b_DesktopInputs){                                             // Mode Reticule except for Mobile

                AP_Reticule_Pc s_Reticule = aP_Global.reticule.GetComponent<AP_Reticule_Pc>(); 

                if(s_Reticule && !s_Reticule.b_CanGrab)
                s_Reticule.callMethodsListCanGrabReticule();
            }
        }
        else{
            type2Once = false;
            type1Once = false;
        }
        #endregion
    }

    public void puzzleStateButtons()
    {
        #region
        Transform objClicked;
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;


        //Focus Mode
        if (aP_PuzzleDetector.b_FocusActivated){  
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer,aP_GlobalPuzzle, 0, cameraUseForFocus);
        } 
        //VR Raycast
        else if (!aP_PuzzleDetector.b_FocusActivated && 
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus && 
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 1){
            if (!b_PuzzleSolved)
                objClicked = _detectClick.Dec_VRRaycastCheckClick(myLayer);
            else
                objClicked = null; 
        } 
        //VR Hand
        else if (!aP_PuzzleDetector.b_FocusActivated && 
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus && 
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&
                 ((!AP_GlobalPuzzleManager_Pc.instance.b_Pause && Input.GetKeyDown(aP_GlobalPuzzle.validationButtonKeyboard) && aP_GlobalPuzzle.b_DesktopInputs && !aP_GlobalPuzzle.b_Joystick) 
                 || 
                  (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && Input.GetKeyDown(aP_GlobalPuzzle.validationButtonJoystick) && aP_GlobalPuzzle.b_DesktopInputs && aP_GlobalPuzzle.b_Joystick))){
            
            objClicked = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectPuzzleState; 
        } 
        //Reticule Mode
        else{
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera()); 
        } 


        if (objClicked != null && objClicked.transform.name == "btnPuzzleReset" && !b_PuzzleSolved) // Player press button reset 
        {
            F_ResetPuzzle();
            Debug.Log("Reset");
        }
        else if (objClicked != null && objClicked.transform.name == "ExitPuzzle") // Player press button Exit puzzle 
        {
            if (aP_PuzzleDetector.b_FocusActivated)
            {
                Debug.Log("Exit Puzzle");
                aP_PuzzleDetector.Ap_DeactivatePuzzle();
            }
            else
            {
                Debug.Log("Focus is not activate so you can delete the Object ExitPuzzle inside the puzzle.");
            }

        }
        else if (objClicked != null && objClicked.transform.name == "Clue") // Player press button Clue
        {
            Debug.Log("Display Clue");
            AP_GlobalPuzzleManager_Pc.instance.AP_DisplayClue(gameObject.transform);
        }
        #endregion
    }
    */
    private void initCurrentPosition(string s_SaveState)
    {
        #region
        //Debug.Log("Gear: " + s_SaveState);


        if (inGameGearsPositionList.Count == 0)
        {
            for (var i = 0; i < GearsPositionList.Count; i++)
            {
                inGameGearsPositionList.Add(GearsPositionList[i]);
            }
        }

        for (var i = 0; i < GearList.Count; i++)
        {
            //Debug.Log("Start");
            if (s_SaveState == "" || s_SaveState == "SaveDoesntExist")
            {
                if (GearsInitPositionWhenStart[i])
                {
                    if (GearList[i].transform.childCount > 0){
                        GearList[i].transform.GetChild(0).transform.localPosition = Vector3.zero;
                        GearList[i].transform.GetChild(0).transform.localEulerAngles = Vector3.zero;
                        inGameAxis[i] = !GearsInitPositionWhenStart[i];
                    }

                }
                else
                {
                    if (GearList[i].transform.childCount > 0)
                    {
                        GearList[i].transform.GetChild(0).transform.position = pivotGearList[i].transform.position;
                        GearList[i].transform.GetChild(0).transform.localEulerAngles = Vector3.zero;
                        inGameAxis[i] = !GearsInitPositionWhenStart[i];
                    }
                }
            }
            else
            {
                if (!inGameAxis[i])
                {
                    if (GearList[i].transform.childCount > 0)
                    {
                        GearList[i].transform.GetChild(0).transform.localPosition = Vector3.zero;
                    }
                }
                else
                {
                    if (GearList[i].transform.childCount > 0)
                    {
                        GearList[i].transform.GetChild(0).transform.position = pivotGearList[i].transform.position;
                        GearList[i].transform.GetChild(0).transform.localEulerAngles = Vector3.zero;
                        //GearList[i].transform.GetChild(0).transform.eulerAngles = pivotGearList[i].transform.eulerAngles;
                    }
                }
            }
        }
        #endregion
    }

    private void CheckIfPuzzleSolved()
    {
        #region
        bool result = true;

        for (var i = 0; i < pivotGearList.Count; i++)
        {
            for (var j = 0; j < pGearList.Count; j++)
            {
                // Check if gear is already place on Axis
                if (GearList[j].transform.childCount > 0 && GearList[j].transform.GetChild(0).transform.position == pivotGearList[i].transform.position)
                {
                    //Debug.Log("Here0");
                    if (pGearList[j].i_AxisType == AxisTypeList[i] &&
                        inGameGearsPositionList[i] != j)    // Check if gear already on this axis. New Gear on Axis position. Move Old gear on Init poisition
                    {
                       // Debug.Log("Here1");
                        if (inGameGearsPositionList[i] != -1)
                        {
                            //Debug.Log("Here2");
                            GearList[inGameGearsPositionList[i]].transform.GetChild(0).transform.localPosition = Vector3.zero;
                            GearList[inGameGearsPositionList[i]].transform.GetChild(0).transform.localEulerAngles = Vector3.zero;
                            inGameGearsPositionList[i] = j;

                            break;
                        }
                    }
                }
            }
            inGameGearsPositionList[i] = -1;                // No gear on this axis
        }

        for (var i = 0; i < pivotGearList.Count; i++)
        {
            for (var j = 0; j < pGearList.Count; j++)
            {
                // Check if the pipe Axis is compatible with the gear
                if (GearList[j].transform.childCount > 0 && GearList[j].transform.GetChild(0).transform.position == pivotGearList[i].transform.position)
                {
                    if (pGearList[j].i_AxisType != AxisTypeList[i])
                    {                                // Not compatible
                        GearList[j].transform.GetChild(0).transform.localPosition = Vector3.zero;
                    }

                    if (pGearList[j].i_AxisType == AxisTypeList[i])                                 // Compatible
                    {
                        inGameGearsPositionList[i] = j;
                        GearList[j].transform.GetChild(0).transform.eulerAngles = pivotGearList[i].transform.eulerAngles;
                    }
                }
            }
        }


        // Check if the gear is the needed gear on a specific axis
        for (var i = 0; i < pivotGearList.Count; i++)
        {
            inGameAxis[i] = false;
            for (var j = 0; j < pGearList.Count; j++)
            {
                if (GearList[j].transform.childCount > 0 && GearList[j].transform.GetChild(0).transform.position == pivotGearList[i].transform.position)
                {

                    if (pGearList[j].i_AxisType == AxisTypeList[i] &&
                        pGearList[j].i_GearType == GearsTypeList[i])
                    {
                        inGameAxis[i] = true;
                        break;
                    }
                }
            }
        }




        // Check if all axis + Gear are well associated
        for (var i = 0; i < pivotGearList.Count; i++)
        {
            if (!inGameAxis[i] && !GearsUseOrFakeList[i])
            {
                result = false;
                break;
            }
        }


        if (result)
        {
            puzzleSolved();
        }

        if (CanvasD_Pc.instance && CanvasD_Pc.instance._P)        // Debug Mode
            puzzleSolved();
        #endregion
    }

    public void InitListOfHandsInDragAndDropScript()
    {
        #region

        // Use to list a list of hand objects. Use to verify if an object is a part of ths puzzle
        List<GameObject> listFromThePuzzle = new List<GameObject>();


        foreach (GameObject obj in pivotGearList)
            listFromThePuzzle.Add(obj.transform.parent.gameObject);

        dragAndDrop.InitListOfHands(listFromThePuzzle);

        // Use to list a list of Gear objects. Use to verify if an object is a part of ths puzzle
        List<GameObject> listFromThePuzzle_02 = new List<GameObject>();
        foreach (GameObject obj in GearList)
            listFromThePuzzle_02.Add(obj.transform.GetChild(0).gameObject);

        dragAndDrop.InitListOfGearsLogics(listFromThePuzzle_02);

        #endregion
    }
}
