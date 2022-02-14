// Description : actionsWhenPuzzleIsSolved_Pc : use inpuzzle to do actions when the puzzle is solved
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsWhenPuzzleIsSolved_Pc : MonoBehaviour {
    public bool                     SeeInspector = false;
    public bool                     onlyFocusMode = false;
    public bool IsPuzzleSolved { get; set; }
    public GameObject               playerCamera;
    public GameObject               feedbackCamera;
 
    public bool                     b_actionsWhenPuzzleIsSolved = false;


    [System.Serializable]
    public class ListOfEvent
    {
        public float                duration = 0;
        public GameObject           feedbackCamera;
        public GameObject           objChangeScale;
        public AnimationCurve       animCurve = new AnimationCurve();
        public Vector3              objScale = new Vector3(0,0,0);
    }

    public List<ListOfEvent>        listOfEvent = new List<ListOfEvent>() { new ListOfEvent() };    // List of Event when the puzzle is solved

    public List<EditorMethodsList_Pc.MethodsList> methodsList                                          // Create a list of Custom Methods that could be edit in the Inspector
        = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                                                    // Access script taht allow to call public function in this script.

    public AudioClip                a_puzzleSolved;
    public float                    a_puzzleSolvedVolume = .25f;
    private AudioSource             a_Source;

    public GameObject               objectActivatedWhenPuzzleIsSolved;                              // If the gameobject is activated, the puzzle is solved

    public AP_PuzzleDetector_Pc aP_PuzzleDetector;

	private void Start()
	{
        objectActivatedWhenPuzzleIsSolved = null;
        #region
        a_Source = GetComponent<AudioSource>();
        playerCamera = AP_GlobalPuzzleManager_Pc.instance.returnMainCamera().gameObject;

        Transform[] children = gameObject.transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == "PuzzleDetector")
                aP_PuzzleDetector = child.GetComponent<AP_PuzzleDetector_Pc>();
        }
        #endregion
    }

	public void F_PuzzleSolved(){
        StartCoroutine(I_PuzzleSolved());
    }

    private IEnumerator I_PuzzleSolved()
    {
        #region
        if (a_Source && a_puzzleSolved)
        {
            a_Source.clip = a_puzzleSolved;
            a_Source.volume = a_puzzleSolvedVolume;
            a_Source.Play();
        }


        // Activate this object when the puzzle is solved. This object can't use to know if a door, drawer or wardrobe can't be open 
        if (objectActivatedWhenPuzzleIsSolved)
            objectActivatedWhenPuzzleIsSolved.SetActive(true);


        //--> Display available actions on screen
    //    ingameGlobalManager.instance.canvasMainMenu.GetComponent<iconsInfoInputs>().displayAvailableActionOnScreen(false, false);
        b_actionsWhenPuzzleIsSolved = true;
        Cursor.visible = false;


        if (AP_GlobalPuzzleManager_Pc.instance.reticuleJoystickImage == null && AP_GlobalPuzzleManager_Pc.instance.b_AlwaysFindreticuleJoystickImage)
            AP_GlobalPuzzleManager_Pc.instance.reticuleJoystickImage = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticuleJoystickImage;

        if(AP_GlobalPuzzleManager_Pc.instance.reticuleJoystickImage && AP_GlobalPuzzleManager_Pc.instance.reticuleJoystickImage.gameObject.activeSelf){
            AP_GlobalPuzzleManager_Pc.instance.reticuleJoystickImage.gameObject.SetActive(false);
        }


        bool b_FeedbackCamera = false; // Check if a feedback camera has been activated

        for (var i = 0; i < listOfEvent.Count;i++){

            //-> Display feedback camera
            if (listOfEvent[i].feedbackCamera || i > 0 && listOfEvent[i-1].feedbackCamera)
            {
                if(i == 0 ||  b_FeedbackCamera == false && playerCamera.activeSelf){
                }
                else if(listOfEvent[i - 1].feedbackCamera)
                    listOfEvent[i-1].feedbackCamera.SetActive(false);  

                if(listOfEvent[i].feedbackCamera)
                listOfEvent[i].feedbackCamera.SetActive(true);
                b_FeedbackCamera = true;
            } 
         
            //-> Custom Method
            if (methodsList[i].obj != null)
            {
                callMethods.Call_A_Specific_Method(methodsList,i);

            } 

            yield return new WaitForSeconds(listOfEvent[i].duration);
        }


       
        if(listOfEvent.Count>0 && listOfEvent[listOfEvent.Count - 1].feedbackCamera)
            listOfEvent[listOfEvent.Count-1].feedbackCamera.SetActive(false);

        // Deactivate FocusCamera
        aP_PuzzleDetector.puzzleCamera.gameObject.SetActive(false);

        if (aP_PuzzleDetector.b_FocusActivated)
            aP_PuzzleDetector.Ap_DeactivatePuzzle();

        #endregion
    }


    public bool returnactionsWhenPuzzleIsSolved()
    {
        IsPuzzleSolved = true;
        return b_actionsWhenPuzzleIsSolved;
    }
 
   
}
