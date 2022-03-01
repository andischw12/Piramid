//Description : Sliding Puzzle. Manage the sliding puzzle behaviour
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidingPuzzle_Pc : MonoBehaviour {
    public bool                         SeeInspector = false;           // variable for the custom editor
    public bool                         helpBoxEditor = true;           
    public GameObject                   defaultTile;
    public int                          _Raw = 3;
    public int                          _Column = 3;
    public int                          toolbarCurrentValue = 0;
    public int                          SquareSize = 80;
    public int                          currentSelectedSprite = 0;
    public int                          randomNumber = 1000;

    public List<GameObject>             tilesList = new List<GameObject>(); // list of tiles 
    public List<int>                    positionList = new List<int>();     // During the game know the position of each Tile
    public List<int>                    refPositionList = new List<int>();  // Know the default position for each tile

    public bool                         b_PuzzleSolved = false;         // Know if the puzzle is solved
    public LayerMask                    myLayer;                        // Raycast is done only on layer 15 : Puzzle
    public bool                         b_UsePuzzleFocus = true;        // Use the puzzle focus. If false : focus step is bypass
    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component


    private detectPuzzleClick_Pc _detectClick;                   // Access the script that detect click (mobile, gaepad and keyboard)

    public int                          validationButtonJoystick = 4;   // The button use by the gamepad to validate an action in this puzzle


    public AudioClip                    a_TileMove;                     // Audio played when a tile is moved
    public float                        a_TileMoveVolume = 1;           // a_TileMove volume
    public AudioClip                    a_Reset;                        // Sound when Reset button is pressed
    public float                        a_ResetVolume = 1;
    private bool                        b_PlaySound = true;             // prevent bug with a_Reset sound when the puzzle starts
    private AudioSource                 a_Source;                       // use to access the audio source

    public Camera                       cameraUseForFocus;
   
    public List<EditorMethodsList_Pc.MethodsList> methodsList              // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                   // Access script taht allow to call public function in this script.
    public bool                         b_OnlyTheFirstTime = true;


    public AP_PuzzleDetector_Pc aP_PuzzleDetector;
    public AP_PuzzleMoveType_Pc aP_PuzzleMoveType;

    // Use this for initialization
    void Start()
    {
        #region
        //--> Every Puzzle  ----> BEGIN <----
        //camManager = GetComponent<focusCamEffect>();                                    // Access focusCamEffect to zoom and dezoom on puzzle
        _conditionsToAccessThePuzzle = GetComponent<conditionsToAccessThePuzzle_Pc>();     // Access the condition to unlock the puzzle
        _actionsWhenPuzzleIsSolved = GetComponent<actionsWhenPuzzleIsSolved_Pc>();         // Access the actions done when the puzzle is solved

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

        for (var i = 0; i < refPositionList.Count; i++)         // init the positionList
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
        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated/* && !ingameGlobalManager.instance.b_Ingame_Pause*/)
        {
            if (b_OnlyTheFirstTime)
            {
                b_OnlyTheFirstTime = false;
                callMethods.Call_A_Method(methodsList);
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

            aP_PuzzleMoveType.feedbackPuzzleState(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, b_PuzzleSolved);
            aP_PuzzleMoveType.puzzleStateButtons(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, GetComponent<SlidingPuzzle_Pc>(), gameObject, b_PuzzleSolved);
        }
       
        #endregion
    }

    //------> BEGIN <------ Next 6 methods are always needed in a puzzle script 


    //--> Reset Puzzle when button iconResetPuzzle in Canvas_PlayerInfos is pressed
    public void F_ResetPuzzle()
    {
        #region
        if (a_Source && a_Reset && b_PlaySound)
        {
            a_Source.clip = a_Reset;
            a_Source.volume = a_ResetVolume;
            a_Source.Play();
        }

        b_PlaySound = true;

        InitMixPosition();
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
            b_PlaySound = false;
            F_ResetPuzzle();
            //Debug.Log("Puzzle Init");
        }
        else
        {                                                   // Save exist
            //Debug.Log("Puzzle Init Exist");
            for (var i = 0; i < positionList.Count; i++)
            {
                positionList[i] = int.Parse(codes[i + 1]);      // load Each tile position in PostionList List
                number++;
            }


            InitPositionAfterLoading();
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
        //  _actionsWhenPuzzleIsSolved.initSolvedSection();    // Init Popup object in script actionsWhenPuzzleIsSolved.cs
        //----> END <----
        #endregion
    }

    //--> Use to save Object state
    public string ReturnSaveData()
    {
        #region
        //-> Common for all puzzle ----> BEGIN <----
        string value = "";

        value += r_TrueFalse(b_PuzzleSolved) + "_";         // b_PuzzleSolved : Save if the puzzle is solved or not
        //----> END <----


        //-> Specific for this puzzle ----> BEGIN <----
        for (var i = 0; i < positionList.Count; i++)
        {       // Save Each tile position
            value += positionList[i] + "_";
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

        //if (!b_PuzzleSolved)
        //{
        Transform objClicked;

        if (!b_PuzzleSolved)
            objClicked = aP_PuzzleMoveType.returnObjectSelectedObject(_detectClick, aP_PuzzleDetector, myLayer, validationButtonJoystick, cameraUseForFocus);
        else
            objClicked = null;
        //if (aP_PuzzleDetector.b_FocusActivated) objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, cameraUseForFocus);
        //else objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, validationButtonJoystick, Camera.main);

        /*if(objClicked)
            Debug.Log(objClicked.name);
        else
            Debug.Log("Null");
            */
        if (objClicked != null)
        {                                                     // Player press validation button on a puzzle object

            foreach (GameObject obj in tilesList)
            {
                if (obj.transform.GetChild(0).transform == objClicked.transform)
                    MoveTile(objClicked.transform, int.Parse(objClicked.transform.name));
            }
        }
        //}
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
    private void MoveTile(Transform obj, int selectedObj)
    {
        #region
        int selectedTile = 0;
        for (var i = 0; i < positionList.Count; i++)
        {
            if (positionList[i] == selectedObj)
            {
                selectedTile = i;
                break;
            }
        }

        int numRaw = selectedTile / _Column;
        int numColumn = selectedTile % _Column;

        string result = "Raw : " + numRaw.ToString() + " : Column : " + numColumn.ToString();


        //-> Move if it is Possible
        ///--> Check Up position
        if (numRaw > 0)
        {
            if (positionList[selectedTile - _Column] == -1)
            {
                result += " : Could move Up";
                positionList[selectedTile - _Column] = positionList[selectedTile];
                positionList[selectedTile] = -1;
                MoveTileInSceneView(obj, "Up");
            }
        }

        //--> Check Down position
        if (numRaw < _Raw - 1)
        {
            result += " : Down Ok";
            if (positionList[selectedTile + _Column] == -1)
            {
                result += " : Could move Down";
                positionList[selectedTile + _Column] = positionList[selectedTile];
                positionList[selectedTile] = -1;
                MoveTileInSceneView(obj, "Down");
            }
        }
        //--> Check Right position
        if (numColumn < _Column - 1)
        {
            result += " : Right Ok";
            if (positionList[selectedTile + 1] == -1)
            {
                result += " : Could move Right";
                positionList[selectedTile + 1] = positionList[selectedTile];
                positionList[selectedTile] = -1;
                MoveTileInSceneView(obj, "Right");
            }
        }

        //--> Check Left position
        if (numColumn > 0)
        {
            result += " : Left Ok";
            if (positionList[selectedTile - 1] == -1)
            {
                result += " : Could move Left";
                positionList[selectedTile - 1] = positionList[selectedTile];
                positionList[selectedTile] = -1;
                MoveTileInSceneView(obj, "Left");
            }
        }
        #endregion
    }

    //--> Move a selected tile in the scene view
    private void MoveTileInSceneView(Transform obj, string direction)
    {
        #region
        if (a_Source && a_TileMove)
        {
            a_Source.clip = a_TileMove;
            a_Source.volume = a_TileMoveVolume;
            a_Source.Play();
        }

        if (direction == "Down")
            obj.parent.transform.localPosition = new Vector3(obj.parent.transform.localPosition.x, obj.parent.transform.localPosition.y - .25f, 0);
        if (direction == "Up")
            obj.parent.transform.localPosition = new Vector3(obj.parent.transform.localPosition.x, obj.parent.transform.localPosition.y + .25f, 0);
        if (direction == "Left")
            obj.parent.transform.localPosition = new Vector3(obj.parent.transform.localPosition.x - .25f, obj.parent.transform.localPosition.y, 0);
        if (direction == "Right")
            obj.parent.transform.localPosition = new Vector3(obj.parent.transform.localPosition.x + .25f, obj.parent.transform.localPosition.y, 0);

        CheckIfactionsWhenPuzzleIsSolved();
        #endregion
    }

    //--> Check if the the puzzle is solved after having moving a tile
    private void CheckIfactionsWhenPuzzleIsSolved()
    {
        #region
        bool b_solved = true;
        for (var i = 0; i < positionList.Count - 1; i++)
        {
            if (i != positionList[i])
            {
                b_solved = false;
            }
        }

        if (b_solved)
        {
            //Debug.Log("Puzzle is Solved");
            puzzleSolved();
        }

        if (CanvasD_Pc.instance && CanvasD_Pc.instance._P)        // Debug Mode
        {
            puzzleSolved();
        }
        #endregion
    }

    //--> Init the tiles position in scene view after pressing button reset the puzzle
    private void InitMixPosition()
    {
        #region
        for (var i = 0; i < refPositionList.Count; i++)
        {
            positionList[i] = refPositionList[i];
        }

        for (var i = 0; i < refPositionList.Count; i++)
        {
            if (refPositionList[i] != -1)
            {
                int numRaw = i / _Column;
                int numColumn = i % _Column;

                tilesList[refPositionList[i]].transform.localPosition = new Vector3(.25f * numColumn, -.25f * numRaw, 0);
            }
        }
        #endregion
    }

    //--> Init the tiles position in scene view after a save slot is loading
    private void InitPositionAfterLoading()
    {
        #region
        for (var i = 0; i < positionList.Count; i++)
        {
            if (positionList[i] != -1)
            {
                int numRaw = i / _Column;
                int numColumn = i % _Column;

                tilesList[positionList[i]].transform.localPosition = new Vector3(.25f * numColumn, -.25f * numRaw, 0);
            }
        }
        #endregion
    }
}
