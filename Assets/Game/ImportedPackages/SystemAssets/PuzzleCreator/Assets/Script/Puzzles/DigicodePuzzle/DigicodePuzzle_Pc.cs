// Description : DigicodePuzzle_Pc : Manage the digicode puzzle type
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigicodePuzzle_Pc : MonoBehaviour {
    public bool                         SeeInspector = false;
    public bool                         helpBoxEditor = true;
    public GameObject                   defaultTile;
    public int                          _Raw = 3;


    public int                          _Column = 3;
    public int                          _NumberOfKey = 10;

    public int                          toolbarCurrentValue = 0;
    public int                          SquareSize = 80;
    public int                          currentSelectedSprite = 0;
    public List<GameObject>             tilesList = new List<GameObject>();

    public List<string>                 keyStringList = new List<string>();
    public string                       resultCode = "";

    public List<int>                    positionList = new List<int>();


    public List<int>                    refPositionList = new List<int>();
    public int                          randomNumber = 200;

    public bool                         b_PuzzleSolved = false;         // Know if the puzzle is solved
    public LayerMask                    myLayer;                        // Raycast is done only on layer 15 : Puzzle
    public bool                         b_UsePuzzleFocus = true;        // Use the puzzle focus. If false : focus step is bypass
    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component


    private detectPuzzleClick_Pc _detectClick;

    public int                          validationButtonJoystick = 4;


    public AudioClip                    a_KeyPressed;
    public float                        a_KeyPressedVolume = 1;
    public AudioClip                    a_Reset;
    public float                        a_ResetVolume = 1;
    public AudioClip                    a_WrongCode;
    public float                        a_WrongCodeVolume = 1;
    private AudioSource                 a_Source;

    public bool                         VisualizeSprite = true;

    public Text                         txt_result;
    public GameObject                   iconPosition;

    [System.Serializable]
    public class idList
    {
        public int ID = 0;          // entry ID in the window tab
        public int uniqueID = 0;    // entry Unique ID
    }

    public bool                         b_UIFeedback = true;
    public bool                         b_feedbackActivated = false;                               // Lock Section : if true text is displayed
    public List<idList>                 feedbackIDList = new List<idList>() { new idList() };      // Lock Section : Check if an Object is in the player inventory using his ID


    public Camera                       cameraUseForFocus;

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
       // camManager = GetComponent<focusCamEffect>();
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

        for (var i = 0; i < positionList.Count; i++)            // Save the default Mix. Use when puzzle is reset.
            refPositionList.Insert(0, 0);

        for (var i = 0; i < refPositionList.Count; i++)
            refPositionList[i] = positionList[i];

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
        //if (ingameGlobalManager.instance.b_InputIsActivated)
        //{
        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated /*&& !ingameGlobalManager.instance.b_Ingame_Pause*/)
        {
            if (b_OnlyTheFirstTime)
            {
                b_OnlyTheFirstTime = false;
                callMethods.Call_A_Method(methodsList);
            }
            PuzzleBehaviour();
        }
        /*if (!iconPosition.activeInHierarchy && ingameGlobalManager.instance.currentPuzzle == null)
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
            aP_PuzzleMoveType.puzzleStateButtons(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, GetComponent<DigicodePuzzle_Pc>(), gameObject, b_PuzzleSolved);
        }
        //}
        #endregion
    }
    //------> BEGIN <------ Next 6 methods are always needed in a puzzle script 


    //--> Reset Puzzle when button iconResetPuzzle in Canvas_PlayerInfos is pressed
    public void F_ResetPuzzle()
    {
        #region
        if (a_Source && a_Reset)
        {
            a_Source.clip = a_Reset;
            a_Source.volume = a_ResetVolume;
            a_Source.Play();
        }
        txt_result.text = "";
        #endregion
    }

    //--> Return if the puzzle is Solved
    public bool returnIfPuzzleIsSolved()
    {
        #region
        return b_PuzzleSolved;
        #endregion
    }


    //--> Actions when puzzle is solved
    private void puzzleSolved()
    {
        #region
        //-> Actions done for all type of puzzle
        if (!b_PuzzleSolved || CanvasD_Pc.instance && CanvasD_Pc.instance._P)
        {         // Call script actionsWhenPuzzleIsSolved. Do actions when the puzzle is solved the first time.
            _actionsWhenPuzzleIsSolved.F_PuzzleSolved();
        }
        else
        {
            _actionsWhenPuzzleIsSolved.b_actionsWhenPuzzleIsSolved = true; // Use when focus is called. The variable b_actionsWhenPuzzleIsSolved in script puzzleSolved equal True
        }

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

        //--> Actions to do for this puzzle ----> BEGIN <----
        if (s_ObjectDatas == "")
        {                               // Save Doesn't exist
            txt_result.text = "";
        }
        else
        {                                                   // Save exist
            txt_result.text = codes[1];

            /* if (codes[2] == "T")
                 b_popUpDone = true;
             else
                 b_popUpDone = false; 

             //Debug.Log("Number : " + number);

             if (codes[3] == "T")
                 b_UIFeedback = true;
             else
                 b_UIFeedback = false; 
                 */
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

        //if(!b_PuzzleSolved)
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
        value += txt_result.text + "_";                            // save the current text display on screen

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
            //if(aP_PuzzleDetector.b_FocusActivated) objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance,validationButtonJoystick,cameraUseForFocus);
            //else objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, Camera.main);

            if (objClicked != null)
            {                                                     // Player press validation button on a puzzle object
                //MoveTile(objClicked.transform, int.Parse(objClicked.transform.name)); 
                foreach (GameObject obj in tilesList)
                {
                    if (obj.transform.GetChild(0).transform == objClicked.transform)
                        AddToResultScreen(objClicked.transform, int.Parse(objClicked.transform.name));
                }

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

    //--> Update the tile array list
    private void AddToResultScreen(Transform obj, int selectedObj)
    {
        #region
        if (a_Source && a_KeyPressed)
        {
            a_Source.clip = a_KeyPressed;
            a_Source.volume = a_KeyPressedVolume;
            a_Source.Play();

        }


        if (txt_result.text.Length < resultCode.Length)
        {
            int selectedTile = 0;
            for (var i = 0; i < tilesList.Count; i++)
            {
                if (i == selectedObj)
                {
                    selectedTile = i;
                    break;
                }
            }

            txt_result.text += keyStringList[selectedTile];

            if (txt_result.text == resultCode)                          // code is true. Puzzle is Solved
                puzzleSolved();
            else if (CanvasD_Pc.instance && CanvasD_Pc.instance._P)        // Debug Mode
                puzzleSolved();
            else if (txt_result.text.Length >= resultCode.Length)
            {       // Wrong code
                StartCoroutine(ap_ActionIfPuzzleIsWrong());
            }
        }
        #endregion
    }

    private float timer = 0;
    public float targetTime = .1f;
    private IEnumerator ap_ActionIfPuzzleIsWrong()
    {
        #region
        bool processEnded = false;
        string currentCode = txt_result.text;


        while (!processEnded)
        {
            // if (!ingameGlobalManager.instance.b_Ingame_Pause){
            yield return new WaitUntil(() => ap_Timer());
            if (a_Source && a_WrongCode)
            {
                a_Source.clip = a_WrongCode;
                a_Source.volume = a_WrongCodeVolume;
                a_Source.Play();
            }

            txt_result.text = "";
            yield return new WaitUntil(() => ap_Timer());
            txt_result.text = currentCode;
            yield return new WaitUntil(() => ap_Timer());
            txt_result.text = "";

            processEnded = true;
            // }

            yield return null;
        }



        yield return null;
        #endregion
    }

    private bool ap_Timer()
    {
        #region
        timer = Mathf.MoveTowards(timer, targetTime, Time.deltaTime);
        if (timer == targetTime)
        {
            timer = 0;
            return true;
        }

        return false;
        #endregion
    }


    public void VRTouch(GameObject objClicked,bool bGameplay)
    {
        //-> Gameplay part
        if (bGameplay)
        {
            foreach (GameObject obj in tilesList)
            {
                if (obj.transform.GetChild(0).transform == objClicked.transform)
                    AddToResultScreen(objClicked.transform, int.Parse(objClicked.transform.name));
            }
        }
        //-> Reset, Exit, Clue
        else
        {
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
        }





    }
}
