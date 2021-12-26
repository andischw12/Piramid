//Description: AP_PuzzleDetector_Pc: Use on object PuzzleDetector that can be find in each puzzle
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_PuzzleDetector_Pc : MonoBehaviour {
    public bool SeeInspector = false;
    public Color color_01 = new Color(1, .8f, 0.2F, .4f);


    public conditionsToAccessThePuzzle_Pc accessPuzzle;
    public Camera                       puzzleCamera;
    public Camera                       _mainCam;
    private GameObject                  puzzleRaycast;
    private Ap_VariousMethods_Pc variousMethods;
    public GameObject                   btn_MobilePuzzle;
    public GameObject                   btn_GrabObjectMobile;

    public bool                         b_FocusActivated = true;
    public int                          puzlleIntearctionType = 0;
    public bool                         b_ReticuleState = true;

    public bool                         b_PuzzleIsSolved = false;

    public bool b_ChooseASpecificLayer = false;
    public int specificLayer = 0; // Default Layer

    private void Start()
    {
        variousMethods = new Ap_VariousMethods_Pc(); 

        //Puzzles: Gear or Logics
        if (accessPuzzle.GetComponent<AP_.DragAndDrop_Pc>())
            puzlleIntearctionType = accessPuzzle.GetComponent<AP_.DragAndDrop_Pc>().dragAndDropMode;
        //Other puzzles
        if (accessPuzzle.GetComponent<AP_PuzzleMoveType_Pc>())
            puzlleIntearctionType = accessPuzzle.GetComponent<AP_PuzzleMoveType_Pc>().puzzleMoveMode;
    }

    public void Ap_ActivatePuzzle(Camera objMainCamera)
    {
        #region
        accessPuzzle.checkIfPuzzleIsAvailable();

        AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus = gameObject.GetComponent<AP_PuzzleDetector_Pc>();
        accessPuzzle.b_PuzzleStateButtons = true;

        if (b_FocusActivated)
        {
            puzzleCamera.gameObject.SetActive(true);
            _mainCam = objMainCamera;

            objMainCamera.enabled = false;
            Transform[] allChildren = objMainCamera.gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name == "puzzleCam_Infos")
                {
                    child.gameObject.SetActive(false);
                    break;
                }
            }

            AP_GlobalPuzzleManager_Pc.instance.StartCoroutine(AP_GlobalPuzzleManager_Pc.instance.CallAllTheMethodsOneByOneWhenFocusStartsOnAPuzzle());
            AP_GlobalPuzzleManager_Pc.instance.StartCoroutine(AP_GlobalPuzzleManager_Pc.instance.changeLockStateConfined(true));
        }
        else
        {
            AP_GlobalPuzzleManager_Pc GlobalPuzzleManager = AP_GlobalPuzzleManager_Pc.instance;

            if (btn_MobilePuzzle == null)
                btn_MobilePuzzle = variousMethods.FindAnObjectUsingItsParent("Canvas_UIPuzzle", "btn_PuzzleMobile");

            if (btn_MobilePuzzle && GlobalPuzzleManager.b_iconPuzzleMobile)
            {
                btn_MobilePuzzle.SetActive(true);
            }

            if (btn_GrabObjectMobile == null)
                btn_GrabObjectMobile = variousMethods.FindAnObjectUsingItsParent("Canvas_UIPuzzle", "btn_GrabObjectMobile");


            GlobalPuzzleManager.StartCoroutine(GlobalPuzzleManager.CallAllTheMethodsOneByOneWhenNoFocusStartsOnAPuzzle());
            GlobalPuzzleManager.StartCoroutine(GlobalPuzzleManager.CallAllTheMethodsOneByOneWhenNoFocusStartsOnAPuzzleReticule());

        }
        #endregion
    }

    public void Ap_DeactivatePuzzle()
    {
        StartCoroutine(WaitAFewSeconds());
    }

    private IEnumerator WaitAFewSeconds()
    {
        #region
        if (b_FocusActivated)
            _mainCam.enabled = true;

        yield return new WaitForEndOfFrame();
        AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus = null;
        AP_GlobalPuzzleManager_Pc.instance.currentPuzzle = null;

        accessPuzzle.b_PuzzleIsActivated = false;
        accessPuzzle.b_PuzzleStateButtons = false;

        if (b_FocusActivated)
        {
            if (accessPuzzle.gameObject.GetComponent<AP_.DragAndDrop_Pc>())
            {      // Deactivate Hands on logic and gear puzzles
                accessPuzzle.gameObject.GetComponent<AP_.DragAndDrop_Pc>().InitHandsWhenThePlayerLeavesPuzzleFocusMode();
                StartCoroutine(transform.parent.gameObject.GetComponent<AP_.DragAndDrop_Pc>().waitDeselectObject(false));
            }

            puzzleRaycast = GameObject.Find("puzzleRaycast");
            if (puzzleRaycast) puzzleRaycast.GetComponent<AP_PuzzleRaycast_Pc>().PreventBugWhenPlayerExitAPuzzle();

            puzzleCamera.gameObject.SetActive(false);

            //_mainCam.enabled = true;
            Transform[] allChildren = _mainCam.gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name == "puzzleCam_Infos")
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }

          
                AP_GlobalPuzzleManager_Pc GlobalPuzzleManager = AP_GlobalPuzzleManager_Pc.instance;

            GlobalPuzzleManager.StartCoroutine(GlobalPuzzleManager.changeLockStateLock());
            GlobalPuzzleManager.StartCoroutine(GlobalPuzzleManager.CallAllTheMethodsOneByOneWhenFocusEndedOnAPuzzle());

            if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
                GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;


            if (GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule) GlobalPuzzleManager.reticule.gameObject.SetActive(true);


            if (GlobalPuzzleManager.iconPuzzle == null && GlobalPuzzleManager.b_AlwaysFindiconPuzzle)
                GlobalPuzzleManager.iconPuzzle = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().iconPuzzle;
            if (GlobalPuzzleManager.b_iconPuzzle && GlobalPuzzleManager.iconPuzzle)
                GlobalPuzzleManager.iconPuzzle.gameObject.SetActive(true);
        }
        else
        {
            AP_GlobalPuzzleManager_Pc GlobalPuzzleManager = AP_GlobalPuzzleManager_Pc.instance;
            if (btn_MobilePuzzle == null)
                btn_MobilePuzzle = variousMethods.FindAnObjectUsingItsParent("Canvas_UIPuzzle", "btn_PuzzleMobile");

            if (btn_MobilePuzzle && GlobalPuzzleManager.b_iconPuzzleMobile)
                btn_MobilePuzzle.SetActive(false);

            if (btn_GrabObjectMobile == null)
                btn_GrabObjectMobile = variousMethods.FindAnObjectUsingItsParent("Canvas_UIPuzzle", "btn_GrabObjectMobile");

            if (btn_GrabObjectMobile)
                btn_GrabObjectMobile.SetActive(false);

            AP_PlayerInfos_Pc Canvas_UIPuzzle = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>();
            if (Canvas_UIPuzzle)
                Canvas_UIPuzzle.b_GrabActivated = false;

            // Puzzles: Gear and Logics
            if (transform.parent.gameObject.GetComponent<AP_.DragAndDrop_Pc>())
            {
                StartCoroutine(transform.parent.gameObject.GetComponent<AP_.DragAndDrop_Pc>().waitDeselectObject(false));

                //Mode Raycast: Deactivate Red Cube and Fake Ray
                if (GlobalPuzzleManager.aP_DragAndDropParent)
                    GlobalPuzzleManager.aP_DragAndDropParent.AP_methodsLineModeRaycastReset();
            }
            //Other Puzzles
            if (transform.parent.gameObject.GetComponent<AP_PuzzleMoveType_Pc>())
            {
                //Mode Raycast: Deactivate Red Cube and Fake Ray
                if (GlobalPuzzleManager.aP_DragAndDropParent)
                    GlobalPuzzleManager.aP_DragAndDropParent.AP_methodsLineModeRaycastReset();
            }


            GlobalPuzzleManager.StartCoroutine(GlobalPuzzleManager.CallAllTheMethodsOneByOneWhenNoFocusEndedOnAPuzzle());
            GlobalPuzzleManager.StartCoroutine(GlobalPuzzleManager.CallAllTheMethodsOneByOneWhenNoFocusEndedOnAPuzzleReticule());

        }
        #endregion
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!b_FocusActivated && !b_ChooseASpecificLayer
        ||
            !b_FocusActivated && b_ChooseASpecificLayer && specificLayer == other.gameObject.layer)
        {
            Ap_ActivatePuzzle(null);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!b_FocusActivated && !b_ChooseASpecificLayer
        ||
            !b_FocusActivated && b_ChooseASpecificLayer && specificLayer == other.gameObject.layer)
        {
            Ap_DeactivatePuzzle();
        }
    }

}
