// Description : PipesPuzzle_Pc : Manage the Pipe puzzle behaviour
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipesPuzzle_Pc : MonoBehaviour {
    public bool                         SeeInspector = false;
    public bool                         helpBoxEditor = true;

    public List<Texture2D>              pipeSprite = new List<Texture2D>();         // use for custom editor : Default Sprite

    public int                          pipeType = 0;                               // use for custom editor : Pipes type : No Move, Horizontal, Vertical, T, Elbow 
    public int                          puzzleSubType = 0;                          // use for custom editor : Horizontal/ Vertical or nested/Align
    public int                          HowManyPipesPosition = 2;                   // use for custom editor.
    public bool                         b_SelectPipessToLink = false;               // use for custom editor.
    public int                          selectDefaultTile = 0;
    public List<GameObject>             defaultTileList = new List<GameObject>();
    public GameObject                   defaultTile;
    public int                          _Column = 3;
    public int                          _NumberOfKey = 10;
    public int                          toolbarCurrentValue = 0;
    public int                          SquareSize = 80;
    public int                          currentSelectedSprite = 0;
    public List<GameObject>             tilesList = new List<GameObject>();        // use for custom editor.


    public List<int>                    PipesTypeList = new List<int>();            // Know the type of each pipe (Cross,elbow,line ...)       
    public List<int>                    PipesPositionList = new List<int>();        // Use for save and Load : Know initial pipe rotation : 0 : no rotation / 1 : 90 degres / 2 : 180 degres / 2 : 270 degres 
    public List<int>                    PipesSolutionList = new List<int>();        // Use for save and Load :Know solution pipe rotation

    public int                          startTile = 0;                              // Know the Start pipe position
    public int                          endTile = 0;                                // Know the end pipe position

    public int                          mazeStartTileX = 0;                         // use to find if the puzzle is solved. X position in maze
    public int                          mazeStartTileY = 0;                         // use to find if the puzzle is solved. Y position in maze

    public int                          mazeEndTileX = 0;                           // use to find if the puzzle is solved. X position in maze
    public int                          mazeEndTileY = 0;                           // use to find if the puzzle is solved. Y position in maze

    public List<int>                    inGamePipesPositionList = new List<int>();  // Know in-game the current pipes rotation

    [System.Serializable]
    public class LinkPipes
    {
        public List<int> _PipesList;
    }

    [SerializeField]
    public List<LinkPipes>              linkPipes;                                  // list of linked pipe

    public List<int>                    positionList = new List<int>();

    public bool                         b_PuzzleSolved = false;         // Know if the puzzle is solved
    public LayerMask                    myLayer;                        // Raycast is done only on layer 15 : Puzzle
    public bool                         b_UsePuzzleFocus = true;        // Use the puzzle focus. If false : focus step is bypass
    //public focusCamEffect               camManager;                     // access focusCamEffect component
    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component

    private detectPuzzleClick_Pc _detectClick;                   // Access component to manage click in puzzle (mobile, desktop)

    public int                          validationButtonJoystick = 4;   // Joystick button to validate action in the puzzle

    public AudioClip                    a_KeyPressed;                   // Sound when pipe is pressed
    public float                        a_KeyPressedVolume = 1;         
    public AudioClip                    a_Reset;                        // Sound when Reset button is pressed
    public float                        a_ResetVolume = 1;

    private AudioSource                 a_Source;                       // Access audioSource

    public Camera                       cameraUseForFocus;

    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
     = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.
    public bool                         b_OnlyTheFirstTime = true;


    public AP_PuzzleDetector_Pc aP_PuzzleDetector;
    public AP_PuzzleMoveType_Pc aP_PuzzleMoveType;


    // Use this for initialization
    void Start()
    {
        #region
        //--> Every Puzzle  ----> BEGIN <----
        //camManager = GetComponent<focusCamEffect>();                                   // Access focusCamEffect to zoom and dezoom on puzzle
        _conditionsToAccessThePuzzle = GetComponent<conditionsToAccessThePuzzle_Pc>();    // Access the condition to unlock the puzzle
        _actionsWhenPuzzleIsSolved = GetComponent<actionsWhenPuzzleIsSolved_Pc>();        // Access the actions done when the puzzle is solved

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

        a_Source = GetComponent<AudioSource>();

        tilesList[startTile].transform.parent.GetComponent<AP_CheckTag_Pc>()._Tag = "NoMove";
        tilesList[endTile].transform.parent.GetComponent<AP_CheckTag_Pc>()._Tag = "NoMove";


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
        /* if(!iconPosition.activeInHierarchy && ingameGlobalManager.instance.currentPuzzle == null){
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
            aP_PuzzleMoveType.puzzleStateButtons(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, GetComponent<PipesPuzzle_Pc>(), gameObject, b_PuzzleSolved);
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
        initCurrentPosition("Reset");                             // init Pipes position
        #endregion
    }

    //--> Actions when puzzle is solved
    private void puzzleSolved()
    {
        #region
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
        aP_PuzzleMoveType.AP_InitAfterAPuzzleSolved();
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
            if (inGamePipesPositionList.Count == 0)             // Load saved value in inGamePipesPositionList
            {
                for (var i = 0; i < PipesPositionList.Count; i++)
                {
                    if (i == startTile || i == endTile)
                        inGamePipesPositionList.Add(PipesSolutionList[i]);
                    else
                        inGamePipesPositionList.Add(PipesPositionList[i]);

                    number++;
                }
            }

            for (var i = 0; i < inGamePipesPositionList.Count; i++)
            {
                inGamePipesPositionList[i] = int.Parse(codes[i + 1]);
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
        for (var i = 0; i < inGamePipesPositionList.Count; i++)
        {
            value += inGamePipesPositionList[i] + "_";          // Save the current Pipes or Cercle position
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


            if (objClicked != null)
            {                                                     // Player press validation button on a puzzle object




                foreach (GameObject obj in tilesList)
                {
                    if (obj.transform.parent.transform == objClicked.transform)
                    {
                        if (a_Source && a_KeyPressed)
                        {
                            a_Source.clip = a_KeyPressed;
                            a_Source.volume = a_KeyPressedVolume;
                            a_Source.Play();
                        }

                        movePipes(objClicked.transform, int.Parse(objClicked.transform.parent.name));                       // Rotate the pipe

                        for (var i = 0; i < linkPipes[int.Parse(objClicked.transform.parent.name)]._PipesList.Count; i++)
                        {   // Rotate linked pipe if needed
                            movePipes(tilesList[linkPipes[int.Parse(objClicked.transform.parent.name)]._PipesList[i]].transform,
                                      linkPipes[int.Parse(objClicked.transform.parent.name)]._PipesList[i]);
                        }

                    }
                }

                CheckIfPuzzleSolved();
            }
        }
        #endregion
    }

    //--> Init the puzzle positions
    private void initCurrentPosition(string s_SaveState)
    {
        #region
        if (s_SaveState == "Reset")
        {
            inGamePipesPositionList.Clear();

            for (var k = 0; k < PipesPositionList.Count; k++)
            {
                if (k == startTile || k == endTile)
                {
                    inGamePipesPositionList.Add(PipesSolutionList[k]);
                }
                else
                    inGamePipesPositionList.Add(PipesPositionList[k]);
            }
        }

        if (inGamePipesPositionList.Count == 0)
        {
            for (var i = 0; i < PipesPositionList.Count; i++)
            {
                if (i == startTile || i == endTile)
                {
                    inGamePipesPositionList.Add(PipesSolutionList[i]);
                }

                else
                    inGamePipesPositionList.Add(PipesPositionList[i]);
            }
        }

        for (var i = 0; i < PipesPositionList.Count; i++)
        {
            float step = 90;
            float newAngle = 0f;

            newAngle = step * inGamePipesPositionList[i];

            tilesList[i].transform.localEulerAngles = new Vector3(0, 0, -newAngle);
        }

        #endregion
    }

    //--> Move (rotate ) specific pipe
    private void movePipes(Transform objPIVOT, int number)
    {
        #region
        if (number != startTile && number != endTile)
        {
            float step = 90;

            int position = inGamePipesPositionList[number];

            position++;
            position = position % 4;

            float newAngle = step * position;

            tilesList[number].transform.localEulerAngles = new Vector3(0, 0, -newAngle);


            inGamePipesPositionList[number] = position;

        }
        #endregion
    }

    //--> Check if puzzle is solved 
    private void CheckIfPuzzleSolved()
    {
        #region
        if (createPuzzleArray())
        {
            puzzleSolved();
        }
        if (CanvasD_Pc.instance && CanvasD_Pc.instance._P)        // Debug Mode
            puzzleSolved();
        #endregion
    }

    //--> Create an array that represent the puzzle
    private bool createPuzzleArray()
    {
        #region
        int raw = Mathf.RoundToInt(_NumberOfKey / _Column);
        int[,] arrPuzzle = new int[_Column * 3 + 2, raw * 3 + 2];
        bool[,] lastPos = new bool[_Column * 3 + 2, raw * 3 + 2];
        int[] pipeStartPosition = new int[2] { mazeStartTileX, mazeStartTileY };
        int[] pipeEndPosition = new int[2] { mazeEndTileX, mazeEndTileY };


        //-> Create the puzzle array

        // Upper border
        for (var m = 0; m <= _Column * 3 + 1; m++)
            arrPuzzle[m, 0] = 2;

        int currentRaw = 1;
        int currentColumn = 0;

        for (var f = 0; f < raw; f++)                   // How many raw
        {
            int number = 0;
            int currentSel = 0 + f * _Column;
            for (var k = 0; k < 3; k++)                 // Subdivision in a pipe
            {
                //Border Left
                arrPuzzle[0, currentRaw] = 2;

                number = 0;
                currentSel = 0 + f * _Column;

                for (var i = 0; i < _Column * 3; i++)
                {
                    // Line Up
                    if (k == 0 && number == 0 ||
                        k == 0 && number == 2 ||
                        k == 0 && number == 1 && b_returnValidExitPosition(currentSel)[0] == false)           // Check Up
                    {
                        arrPuzzle[currentColumn + 1, currentRaw] = 2;
                    }
                    // Line Mid
                    else if (k == 1 && number == 0 && b_returnValidExitPosition(currentSel)[3] == false ||    // Check Right
                             k == 1 && number == 2 && b_returnValidExitPosition(currentSel)[1] == false)      // Check Right
                    {
                        arrPuzzle[currentColumn + 1, currentRaw] = 2;
                    }
                    // Line Down
                    else if (k == 2 && number == 0 ||
                             k == 2 && number == 2 ||
                             k == 2 && number == 1 && b_returnValidExitPosition(currentSel)[2] == false)      // Check Down
                    {
                        arrPuzzle[currentColumn + 1, currentRaw] = 2;
                    }
                    else
                    {
                        arrPuzzle[currentColumn + 1, currentRaw] = 1;
                    }

                    number++;
                    number %= 3;
                    if (number == 0)
                        currentSel++;

                    currentColumn++;
                    currentColumn %= _Column * 3;

                }

                // Border Right
                arrPuzzle[_Column * 3 + 1, currentRaw] = 2;
                currentRaw++;
            }

        }

        //-> Bottom border
        for (var m = 0; m <= _Column * 3 + 1; m++)
        {
            arrPuzzle[m, raw * 3 + 1] = 2;
        }


        for (int i = 0; i < _Column * 3 + 2; i++)
        {         // Column
            for (int j = 0; j < raw * 3 + 2; j++)       // Raw
            {
                lastPos[i, j] = false;          // init array
            }
        }

        //-> Check if the puzzle is solved
        return b_checkIfPuzzleCouldbeSolved(arrPuzzle, lastPos, pipeStartPosition[0], pipeStartPosition[1], pipeEndPosition);
        #endregion
    }

    private bool b_checkIfPuzzleCouldbeSolved(int[,] arrPuzzle, bool[,] lastPos, int X_pipeStart, int Y_PipeStart, int[] _pipeEndPosition)
    {
        #region
        int raw = Mathf.RoundToInt(_NumberOfKey / _Column);

        if (X_pipeStart == _pipeEndPosition[0] && Y_PipeStart == _pipeEndPosition[1])                                                               // Puzzle is complete
            return true;

        if (arrPuzzle[X_pipeStart, Y_PipeStart] == 2 || lastPos[X_pipeStart, Y_PipeStart])                                                          // no more movement available
            return false;

        lastPos[X_pipeStart, Y_PipeStart] = true;


        if (Y_PipeStart != 0 && b_checkIfPuzzleCouldbeSolved(arrPuzzle, lastPos, X_pipeStart, Y_PipeStart - 1, _pipeEndPosition))                   // Check Up
            return true;

        if (Y_PipeStart != raw * 3 + 2 - 1 && b_checkIfPuzzleCouldbeSolved(arrPuzzle, lastPos, X_pipeStart, Y_PipeStart + 1, _pipeEndPosition))     // Check Down
            return true;

        if (X_pipeStart != 0 && b_checkIfPuzzleCouldbeSolved(arrPuzzle, lastPos, X_pipeStart - 1, Y_PipeStart, _pipeEndPosition))                   // Check Left
            return true;

        if (X_pipeStart != _Column * 3 + 2 - 1 && b_checkIfPuzzleCouldbeSolved(arrPuzzle, lastPos, X_pipeStart + 1, Y_PipeStart, _pipeEndPosition))  // Check Right
            return true;

        return false;
        #endregion
    }

    //--> Return valid exit position for a specific pipe depending his rotation and type
    private bool[] b_returnValidExitPosition(int selectedPosition)
    {
        #region
        bool[] arrExistPosition = new bool[4] { false, false, false, false };

        if (PipesTypeList[selectedPosition] == 0)           // no position
        {
        }

        if (PipesTypeList[selectedPosition] == 1)           // Vertical
        {
            if (inGamePipesPositionList[selectedPosition] == 0)
                arrExistPosition = new bool[4] { true, false, true, false };
            if (inGamePipesPositionList[selectedPosition] == 1)
                arrExistPosition = new bool[4] { false, true, false, true };
            if (inGamePipesPositionList[selectedPosition] == 2)
                arrExistPosition = new bool[4] { true, false, true, false };
            if (inGamePipesPositionList[selectedPosition] == 3)
                arrExistPosition = new bool[4] { false, true, false, true };
        }

        if (PipesTypeList[selectedPosition] == 2)           // T
        {
            if (inGamePipesPositionList[selectedPosition] == 0)
                arrExistPosition = new bool[4] { true, true, true, false };
            if (inGamePipesPositionList[selectedPosition] == 1)
                arrExistPosition = new bool[4] { false, true, true, true };
            if (inGamePipesPositionList[selectedPosition] == 2)
                arrExistPosition = new bool[4] { true, false, true, true };
            if (inGamePipesPositionList[selectedPosition] == 3)
                arrExistPosition = new bool[4] { true, true, false, true };
        }


        if (PipesTypeList[selectedPosition] == 3)           // elbow
        {
            if (inGamePipesPositionList[selectedPosition] == 0)
                arrExistPosition = new bool[4] { true, true, false, false };
            if (inGamePipesPositionList[selectedPosition] == 1)
                arrExistPosition = new bool[4] { false, true, true, false };
            if (inGamePipesPositionList[selectedPosition] == 2)
                arrExistPosition = new bool[4] { false, false, true, true };
            if (inGamePipesPositionList[selectedPosition] == 3)
                arrExistPosition = new bool[4] { true, false, false, true };
        }

        if (PipesTypeList[selectedPosition] == 4)           // Vertical
        {
            arrExistPosition = new bool[4] { true, true, true, true };
        }

        return arrExistPosition;
        #endregion
    }
}
