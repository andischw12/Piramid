// Description: AP_GlobalPuzzleManager_Pc: Manage Global Puzzle parameters for all the project
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AP_GlobalPuzzleManager_Pc : MonoBehaviour {
    public bool             SeeInspector = false;
    public datasWindowReadyToUse_Pc _dataGlobal;

    public Color color_01 = new Color(1, .8f, 0.2F, .4f);
    public Color color_02 = new Color(1, .8f, 0.2F, 1f);
    public Color color_03 = new Color(.3F, .9f, 1, .5f);
    public Color color_04 = new Color(.3F, .9f, 1, 1f);
    public Color color_05 = new Color(1, .5f, 0.3F, .4f);

    public static AP_GlobalPuzzleManager_Pc instance = null;                    // Static instance of GameManager which allows it to be accessed by any other script.

    public bool             b_Reticule = true;
    public bool             b_iconPuzzle = true;
    public bool             b_iconPuzzleMobile = false;

    public Image            reticule;
    public Image            iconPuzzle;
    public bool             b_DesktopInputs = true;
    public bool             b_Joystick = false;

    public Transform        reticuleJoystickImage;
    public JoystickReticule_Pc _joystickReticule;

    public bool             b_AlwaysFindReticule = true;
    public bool             b_AlwaysFindiconPuzzle = true;
    public bool             b_AlwaysFindreticuleJoystickImage = true;
    public bool             b_AlwaysFind_joystickReticule = true;

    public string           HorizontalAxisJoystickLeft = "Horizontal"; 
    public string           verticalAxisJoystickLeft = "Vertical"; 
    public KeyCode          validationButtonJoystick = KeyCode.JoystickButton16;
    public KeyCode          backButtonJoystick = KeyCode.JoystickButton17;

    public KeyCode          validationButtonKeyboard = KeyCode.Mouse0;
    public KeyCode          backButtonKeyboard = KeyCode.Mouse1;

    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsListExitPuzzle      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();


    public List<EditorMethodsList_Pc.MethodsList> methodsListEnterPuzzleNoFocus      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsListExitPuzzleNoFocus      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsListEnterPuzzleNoFocusReticule      // Create a list of Custom Methods that could be edit in the Inspector
  = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsListExitPuzzleNoFocusReticule      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();


    public List<EditorMethodsList_Pc.MethodsList> methodsListVRValidationDown      // Create a list of Custom Methods that could be edit in the Inspector
  = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsListVRValidationUp      // Create a list of Custom Methods that could be edit in the Inspector
 = new List<EditorMethodsList_Pc.MethodsList>();

    public List<EditorMethodsList_Pc.MethodsList> methodsListVRBackDown      // Create a list of Custom Methods that could be edit in the Inspector
= new List<EditorMethodsList_Pc.MethodsList>();


    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.

    public bool mouseWaitUntilFirstMouseMove = true;// Prevent bug with the mouse when CursorLockMode is changed
    public bool b_LockStateWhenStart = true;

    public bool t_Inputs = false;

    public Menu_Manager_Pc canvasMainMenu;
    public bool b_IsClueActivated = false;

    public AP_PuzzleRaycast_Pc charaPuzzleRaycast;

    public AP_PuzzleDetector_Pc currentPuzzle;
    public AP_PuzzleDetector_Pc currentPuzzleWithNoFocus;

    public int currentPuzzleLanguage = 0;


    public Transform dragAndDropParent;
    public AP_DragAndDropParent_Pc aP_DragAndDropParent;

    public int editorModeDisplayed = 0;

    public bool b_AutoLockCursorWhenGameStarts = true;

    public bool b_Pause = false;

    public Camera ap_MainCamera;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        if (b_LockStateWhenStart && b_AutoLockCursorWhenGameStarts)
            StartCoroutine(changeLockStateLock());
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

        if (dragAndDropParent != null)
            aP_DragAndDropParent = dragAndDropParent.GetComponent<AP_DragAndDropParent_Pc>();
            
    }
   
    public bool bool_changeLockStateLock()
    {
        #region
        StartCoroutine(changeLockStateLock());
        return true;
        #endregion
    }

    public IEnumerator changeLockStateLock()
    {
        #region
        if (b_DesktopInputs)
        {    // True for Desktop | False for Mobile
            mouseWaitUntilFirstMouseMove = false;
            yield return new WaitForEndOfFrame();
            if (b_AutoLockCursorWhenGameStarts)
            {
                Cursor.lockState = CursorLockMode.None;
                yield return new WaitForEndOfFrame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }

            if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;
            
            if (reticuleJoystickImage && reticuleJoystickImage.gameObject.activeSelf)
            {
                reticuleJoystickImage.gameObject.SetActive(false);
            }

            //b_currentCursorVisibility = Cursor.visible;
            //b_currentCursorState = true;
        }
        #endregion
    }

    public bool bool_changeLockStateConfined()
    {
        #region
        StartCoroutine(changeLockStateConfined(true));
        return true;
        #endregion
    }

    public IEnumerator changeLockStateConfined(bool b_showCursor)
    {
        #region
        mouseWaitUntilFirstMouseMove = false;

        if (b_DesktopInputs)    // True for Desktop | False for Mobile
        {
            yield return new WaitForEndOfFrame();
            //if (Application.platform != RuntimePlatform.WindowsEditor)
            Cursor.lockState = CursorLockMode.None;
            yield return new WaitForEndOfFrame();
            Cursor.lockState = CursorLockMode.Confined;

            if (b_Joystick)
            {
                if (_joystickReticule == null && b_AlwaysFind_joystickReticule)
                    _joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;
                if (_joystickReticule.transform)
                {
                    if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                        reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;


                    if (b_showCursor)
                    {
                        _joystickReticule.newPosition(Screen.width / 2, Screen.height / 2);
                        yield return new WaitForEndOfFrame();
                        reticuleJoystickImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        reticuleJoystickImage.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (b_showCursor)
                    Cursor.visible = true;
                else
                    Cursor.visible = false;

               
            }
           // Debug.Log("Confined");
        }
        #endregion
    }

    public void AP_SwitchPuzzleInputsToKeyboardAndMouse(bool b_showCursor)
    {
        StartCoroutine(AP_I_SwitchPuzzleInputsToKeyboardAndMouse(b_showCursor));
    }

    public IEnumerator AP_I_SwitchPuzzleInputsToKeyboardAndMouse(bool b_showCursor)
    {
        #region
        //Debug.Log("Keyboard");
        b_DesktopInputs = true;
        b_Joystick = false;
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            if (b_showCursor)
                Cursor.visible = true;
            else
                Cursor.visible = false;

            if (_joystickReticule == null && b_AlwaysFind_joystickReticule)
                _joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;
            if (_joystickReticule.transform){
                if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                    reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;

                reticuleJoystickImage.gameObject.SetActive(false);
            }
                


            mouseWaitUntilFirstMouseMove = true;
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.visible = false;
            if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;

            if (reticuleJoystickImage && reticuleJoystickImage.gameObject.activeSelf)
                reticuleJoystickImage.gameObject.SetActive(false);
        }
        yield return null;
        #endregion
    }

    public void AP_SwitchPuzzleInputsToGamepad(bool b_showCursor)
    {
        StartCoroutine(AP_I_SwitchPuzzleInputsToGamepad(b_showCursor));
    }

    public IEnumerator AP_I_SwitchPuzzleInputsToGamepad(bool b_showCursor)
    {
        #region
        b_DesktopInputs = true;
        b_Joystick = true;
        //Debug.Log("Mouse");
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.visible = false;
            if (_joystickReticule == null && b_AlwaysFind_joystickReticule)
                _joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;
            if (_joystickReticule.transform)
            {
                if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                    reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;
                if (b_showCursor)
                {
                    _joystickReticule.newPosition(Screen.width / 2, Screen.height / 2);
                    yield return new WaitForEndOfFrame();
                    reticuleJoystickImage.gameObject.SetActive(true);
                }
                else
                {
                    reticuleJoystickImage.gameObject.SetActive(false);
                }
            }


        }
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.visible = false;
            if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;
            
            if (reticuleJoystickImage && reticuleJoystickImage.gameObject.activeSelf)
                reticuleJoystickImage.gameObject.SetActive(false);
        }

        yield return null;
        #endregion
    }

    public void AP_SwitchPuzzleInputsToMobile()
    {
        StartCoroutine(AP_I_SwitchPuzzleInputsToMobile());
    }

    public IEnumerator AP_I_SwitchPuzzleInputsToMobile()
    {
        #region
        //Debug.Log("Mobile");
        b_DesktopInputs = false;
        b_Joystick = false;
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.visible = false;
            if (_joystickReticule == null && b_AlwaysFind_joystickReticule)
                _joystickReticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>()._joystickReticule;
            if (_joystickReticule.transform){
                if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                    reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;

                reticuleJoystickImage.gameObject.SetActive(false);
            }
               
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.visible = false;
            if (reticuleJoystickImage == null && b_AlwaysFindreticuleJoystickImage)
                reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;

            if (reticuleJoystickImage && reticuleJoystickImage.gameObject.activeSelf)
                reticuleJoystickImage.gameObject.SetActive(false);
        }
        mouseWaitUntilFirstMouseMove = true;
        yield return null;
        #endregion
    }

    public IEnumerator CallAllTheMethodsOneByOneWhenFocusStartsOnAPuzzle()
    {
        for (var i = 0; i < methodsList.Count; i++)
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsList, i) == true);
        yield return null;
    }

    public IEnumerator CallAllTheMethodsOneByOneWhenFocusEndedOnAPuzzle()
    {
        for (var i = 0; i < methodsListExitPuzzle.Count; i++)
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsListExitPuzzle, i) == true);
        yield return null;
    }


    public IEnumerator CallAllTheMethodsOneByOneWhenNoFocusStartsOnAPuzzle()
    {
        for (var i = 0; i < methodsListEnterPuzzleNoFocus.Count; i++)
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsListEnterPuzzleNoFocus, i) == true);
        yield return null;
    }

    public IEnumerator CallAllTheMethodsOneByOneWhenNoFocusEndedOnAPuzzle()
    {
        for (var i = 0; i < methodsListExitPuzzleNoFocus.Count; i++)
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsListExitPuzzleNoFocus, i) == true);
        yield return null;
    }

    public IEnumerator CallAllTheMethodsOneByOneWhenNoFocusStartsOnAPuzzleReticule()
    {
        for (var i = 0; i < methodsListEnterPuzzleNoFocusReticule.Count; i++)
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsListEnterPuzzleNoFocusReticule, i) == true);
        yield return null;
    }

    public IEnumerator CallAllTheMethodsOneByOneWhenNoFocusEndedOnAPuzzleReticule()
    {
        for (var i = 0; i < methodsListExitPuzzleNoFocusReticule.Count; i++)
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsListExitPuzzleNoFocusReticule, i) == true);
        yield return null;
    }

   

    //--> Prevent Mac bug with the cursor lockstate (drop movement)
    public IEnumerator waitToInitMouseMovement()
    {
        yield return new WaitForEndOfFrame();
        mouseWaitUntilFirstMouseMove = true;
    }

    public void AP_DisplayClue(Transform T_Puzzle){
        if(canvasMainMenu == null)
            canvasMainMenu = GameObject.Find("Canvas_Clue").GetComponent<Menu_Manager_Pc>(); 
        
        if (canvasMainMenu != null && !b_DesktopInputs ){
            if (currentPuzzle != null){
                AP_Clue_Pc aP_Clue = currentPuzzle.accessPuzzle.GetComponent<conditionsToAccessThePuzzle_Pc>().objClueBox;
                aP_Clue.displayClueWithItsNumber(aP_Clue.currentClue);  
            }
            canvasMainMenu.GoToOtherPageWithHisNumber(1);
            b_IsClueActivated = true;



        }
        else{
            Debug.Log("Clue works only on Mobile. More information in the documentation.");
        }
    }


    public void changeCurrentPuzzle(AP_PuzzleDetector_Pc newPuzzleSelected){
        currentPuzzle = newPuzzleSelected;

    }

   public Camera returnMainCamera()
    {
        if (!ap_MainCamera)
           return Camera.main;
        else
            return ap_MainCamera;
    }

}
