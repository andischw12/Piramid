//Description : LeverPuzzle_Pc : Manage the Lever Puzzle behaviour
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverPuzzle_Pc : MonoBehaviour {
    public bool                         SeeInspector = false;
    public bool                         helpBoxEditor = true;

    public int                          HowManyLeverPosition = 2;
    public int                          startAngle = 50;
    public int                          totalMovement = 100;                            // Rotation from start position to end position in degres 
    public List<int>                    LeverPositionList = new List<int>();
    public List<int>                    LeverSolutionList = new List<int>();

    public List<bool>                   LeverDirectionUpList = new List<bool>();
    public List<bool>                   LeverDirectionUpSolutionList = new List<bool>();

    public List<int>                    inGameLeverPositionList = new List<int>();
    public List<bool>                   inGameLeverDirectionUpList = new List<bool>();

    public List<Renderer>               lightList = new List<Renderer>();

    [System.Serializable]
    public class LinkLever
    {
        public List<int> _leverList;
    }

    [SerializeField]
    public List<LinkLever>              linkLever;

    public bool                         b_LinkMode = false;
    public bool                         b_SelectLeversToLink = false;


    public Color                        Emission_Off = new Color(0, 0, 0);
    public Color                        Emission_On = new Color(1, 1, 1);


    public int                          selectDefaultTile = 0;
    public List<GameObject>             defaultTileList = new List<GameObject>();
    public GameObject                   defaultTile;
  

    public int                          _Column = 3;
    public int                          _NumberOfKey = 10;

    public int                          toolbarCurrentValue = 0;
    public int                          SquareSize = 80;
    public int                          currentSelectedSprite = 0;
    public List<GameObject>             tilesList = new List<GameObject>();


    public string                       resultCode = "";

    public List<int>                    positionList = new List<int>();

    public bool                         b_PuzzleSolved = false;         // Know if the puzzle is solved
    public LayerMask                    myLayer;                        // Raycast is done only on layer 15 : Puzzle
    public bool                         b_UsePuzzleFocus = true;        // Use the puzzle focus. If false : focus step is bypass
    //public focusCamEffect               camManager;                     // access focusCamEffect component
    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component


    private detectPuzzleClick_Pc _detectClick;

    public int                          validationButtonJoystick = 4;


    public AudioClip                    a_KeyPressed;
    public float                        a_KeyPressedVolume = 1;
    public AudioClip                    a_Reset;
    public float                        a_ResetVolume = 1;

    private AudioSource                 a_Source;


    public bool                         VisualizeSprite = true;


    public Text                         txt_result;

    public GameObject                   iconPosition;

    public bool                         b_popUpDone = false;
    public GameObject                   popUpObject;
    public float                        popupSpeed = 3;


    [System.Serializable]
    public class idList
    {
        public int ID = 0;          // entry ID in the window tab
        public int uniqueID = 0;    // entry Unique ID
    }

    public bool                         b_UIFeedback = true;
    public bool                         b_feedbackActivated = false;                               // Lock Section : if true text is displayed
    public List<idList>                 feedbackIDList = new List<idList>() { new idList() };      // Lock Section : Check if an Object is in the player inventory using his ID

    public Camera cameraUseForFocus;

    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
     = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.
    public bool b_OnlyTheFirstTime = true;


    public AP_PuzzleDetector_Pc aP_PuzzleDetector;

    public AP_PuzzleMoveType_Pc aP_PuzzleMoveType;

    // Use this for initialization
    void Start()
    {
        #region
        //--> Every Puzzle  ----> BEGIN <----
        //camManager = GetComponent<focusCamEffect>();
        _conditionsToAccessThePuzzle = GetComponent<conditionsToAccessThePuzzle_Pc>();
        _actionsWhenPuzzleIsSolved = GetComponent<actionsWhenPuzzleIsSolved_Pc>();

        /*Transform[] children = gameObject.transform.parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == "PuzzleDetector")
                aP_PuzzleDetector = child.GetComponent<AP_PuzzleDetector>();
        }*/

        aP_PuzzleMoveType = GetComponent<AP_PuzzleMoveType_Pc>();
        //----> END <----


        //--> Common for all puzzle ----> BEGIN <----
        _detectClick = new detectPuzzleClick_Pc();                 // Access Class that allow to detect click (Mouse, Joystick, Mobile) 


        for (var i = 0; i < lightList.Count;i++){
            if(lightList[i] != null)
                lightList[i] = lightList[i].GetComponent<Renderer>();      
        }
         


        a_Source = GetComponent<AudioSource>();

        // Update layer Mask using the data wObjCreator.
        if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal)
        {
            AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;
            myLayer = LayerMask.GetMask(LayerMask.LayerToName(aP_GlobalPuzzle._dataGlobal.currentLayerPuzzle));
        }
        //----> END <----
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region
       // if (ingameGlobalManager.instance.b_InputIsActivated)
        //{
            if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated/* && !ingameGlobalManager.instance.b_Ingame_Pause*/)
            {
                if (b_OnlyTheFirstTime)
                {
                    b_OnlyTheFirstTime = false;
                    callMethods.Call_A_Method(methodsList);
                }


                PuzzleBehaviour();
            }
           /* if (!iconPosition.activeInHierarchy && ingameGlobalManager.instance.currentPuzzle == null)
            {
                iconPosition.SetActive(true);
            }*/


        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated && // All Case puzzle is not solved
        _conditionsToAccessThePuzzle.b_PuzzleStateButtons

        ||

           b_PuzzleSolved &&                                    // Case Focus: Puzzle is already solved
        _conditionsToAccessThePuzzle.b_PuzzleStateButtons &&
          aP_PuzzleDetector.b_FocusActivated)
        {
            aP_PuzzleMoveType.feedbackPuzzleState(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, b_PuzzleSolved);
            aP_PuzzleMoveType.puzzleStateButtons(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, GetComponent<LeverPuzzle_Pc>(), gameObject, b_PuzzleSolved);
        }
        //}
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
        initCurrentPosition("");                             // init Lever position
        #endregion
    }

   
//--> Actions when puzzle is solved
    private void puzzleSolved(){
        #region
        //-> Actions done for all type of puzzle
        if(!b_PuzzleSolved || CanvasD_Pc.instance && CanvasD_Pc.instance._P)
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
        aP_PuzzleMoveType.AP_InitAfterAPuzzleSolved();   
        #endregion
    }

//--> Use to load object state. Initialize the puzzle  (T = True or F = False)
    public void saveSystemInitGameObject(string s_ObjectDatas)
    {
        #region
        string[] codes = s_ObjectDatas.Split('_');              // Split data in an array.
        int number2 = 0;

        //--> Actions to do for this puzzle ----> BEGIN <----
        if (s_ObjectDatas == ""){                               // Save Doesn't exist
            //txt_result.text = "";
            initCurrentPosition("SaveDoesntExist");
        }
        else{                                                   // Save exist
            //txt_result.text = codes[1];
            if (inGameLeverPositionList.Count == 0)                          // Load saved value in inGameLeverPositionList and inGameLeverDirectionUpList
            {
                for (var i = 0; i < LeverPositionList.Count; i++)
                {
                    inGameLeverPositionList.Add(0);
                    inGameLeverDirectionUpList.Add(true);
                    number2+= 2;

                }
            }

            int number = 1;
            for (var i = 0; i < inGameLeverPositionList.Count; i++)
            {
               
                inGameLeverPositionList[i] = int.Parse(codes[number]);

                if (codes[number+1] == "F")
                    inGameLeverDirectionUpList[i] = false;
                else if (codes[number + 1] == "T")
                    inGameLeverDirectionUpList[i] = true;

                number += 2;
            }

          


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
        for (var i = 0; i < inGameLeverPositionList.Count; i++)
        {
            value += inGameLeverPositionList[i] + "_";          // Save the current lever position
            value += r_TrueFalse(inGameLeverDirectionUpList[i]) + "_";          // Save the current lever direction
        }

     

        //----> END <---- 


        return value;
        #endregion
    }

//--> Convert bool to T or F string
    private string r_TrueFalse(bool s_Ref)
    {
        #region
        if (s_Ref) return "T";
        else return "F";
        #endregion
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
                objClicked = aP_PuzzleMoveType.returnObjectSelectedObject(_detectClick, aP_PuzzleDetector, myLayer, validationButtonJoystick, cameraUseForFocus);
            else
                objClicked = null;
            //if (aP_PuzzleDetector.b_FocusActivated) objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, cameraUseForFocus);
            //else objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, Camera.main);
           
            if(objClicked != null){                                                     // Player press validation button on a puzzle object
                foreach (GameObject obj in tilesList)
                {
                    if (obj.transform == objClicked.transform)
                    {
                        if (a_Source && a_KeyPressed)
                        {
                            a_Source.clip = a_KeyPressed;
                            a_Source.volume = a_KeyPressedVolume;
                            a_Source.Play();
                        }

                        moveLever(objClicked.transform, int.Parse(objClicked.transform.parent.name));

                        for (var i = 0; i < linkLever[int.Parse(objClicked.transform.parent.name)]._leverList.Count; i++)
                        {
                            moveLever(tilesList[linkLever[int.Parse(objClicked.transform.parent.name)]._leverList[i]].transform,
                                      linkLever[int.Parse(objClicked.transform.parent.name)]._leverList[i]);
                        }
                    }
                }
               

                CheckIfPuzzleSolved();
            }
        }
        #endregion
    }

    public void puzzleStateButtons()
    {
        #region
        Transform objClicked;

        if (aP_PuzzleDetector.b_FocusActivated) objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, cameraUseForFocus);
        else objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());


        if (objClicked != null && objClicked.transform.name == "btnPuzzleReset" && !b_PuzzleSolved) // Player press button reset 
        {
            F_ResetPuzzle();

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
        }

        #endregion
    }

    private void initCurrentPosition(string s_SaveState){
        #region
        if(inGameLeverPositionList.Count == 0){
            for (var i = 0; i < LeverPositionList.Count; i++)
            {
                inGameLeverPositionList.Add(LeverPositionList[i]);
                inGameLeverDirectionUpList.Add(LeverDirectionUpList[i]);
            }  
        }

        for (var i = 0; i < LeverPositionList.Count; i++)
        {
            float step = totalMovement / (HowManyLeverPosition-1);

            float newAngle = 0f;
            if (s_SaveState == "SaveDoesntExist" || s_SaveState == "")      // Save slot doesn't exist or Reset button is pressed
                newAngle = startAngle - step * (LeverPositionList[i]);
            if (s_SaveState == "SaveExist")                                 // Save slot exist
                newAngle = startAngle - step * (inGameLeverPositionList[i]);
            

            //Debug.Log(i + " : " + newAngle);
            tilesList[i].transform.localEulerAngles = new Vector3(newAngle, 0, 0);


            if (s_SaveState == "SaveDoesntExist" || s_SaveState == ""){      // Save slot doesn't exist or Reset button is pressed
                inGameLeverPositionList[i] = LeverPositionList[i];
                inGameLeverDirectionUpList[i] = LeverDirectionUpList[i];
            }

            checkSwitchOnLight(i);
        }
        #endregion
    }

    private void moveLever(Transform objPIVOT, int number)
    {
        #region
        float step = totalMovement / (HowManyLeverPosition - 1);

        int position = inGameLeverPositionList[number];
        bool directionUp = inGameLeverDirectionUpList[number];

        if (directionUp)
        {

            if (position == 1)
            {
                directionUp = false;
            }

            position--;
        }
        else
        {

            if (position == HowManyLeverPosition - 2)
            {
                directionUp = true;
            }

            position++;
        }

        float newAngle = startAngle - step * position;

        //Debug.Log(position + " : " + newAngle);
        tilesList[number].transform.localEulerAngles = new Vector3(newAngle, 0, 0);


        inGameLeverPositionList[number] = position;
        inGameLeverDirectionUpList[number] = directionUp;

        checkSwitchOnLight(number);

        //--> Check Light
        #endregion
    }

    private void checkSwitchOnLight(int number){
        #region
        if (lightList[number] != null){
            if (lightList[number]
           && LeverSolutionList[number] == inGameLeverPositionList[number])
            {
                lightList[number].material.SetColor("_EmissionColor", Emission_On);
            }
            else if (lightList[number])
            {
                lightList[number].material.SetColor("_EmissionColor", Emission_Off);
            } 
        }
        #endregion
    }

    private void CheckIfPuzzleSolved()
    {
        #region
        bool result = true;

        for (var i = 0; i < inGameLeverPositionList.Count; i++)
        {
            if (LeverSolutionList[i] != inGameLeverPositionList[i])
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

}
