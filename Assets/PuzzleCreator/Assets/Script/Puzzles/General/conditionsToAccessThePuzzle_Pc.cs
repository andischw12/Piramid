// Description : conditionsToAccessThePuzzle_Pc : This script is use to check if the puzzle could be unlock or not
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conditionsToAccessThePuzzle_Pc : MonoBehaviour {
    public bool                         SeeInspector = false;
    public bool                         onlyFocusMode = false;

    public bool                         b_PuzzleIsActivated = false;        // return if the puzzle is currently activated


    public AP_Clue_Pc                   objClueBox;                         // know if the puzzle has a hint


    public List<EditorMethodsList_Pc.MethodsList> methodsList               // Create a list of Custom Methods that could be edit in the Inspector
     = new List<EditorMethodsList_Pc.MethodsList>();


    public List<EditorMethodsList_Pc.MethodsList> methodsListFeedback      // Create a list of Custom Methods that could be edit in the Inspector
     = new List<EditorMethodsList_Pc.MethodsList>();
    
    public CallMethods_Pc               callMethods;                        // Access script taht allow to call public function in this script.

    private actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;        // Access component actionsWhenPuzzleIsSolved


  
    public bool                         b_feedbackActivated = false;        // Lock Section : if true text is displayed


    public bool                         b_ActivateDoubleTapIcon = true;    // True : ACtivated the double tap UI icon if game is played on mobile platform

    public AP_PuzzleSpriteState_Pc      puzzleSprite;
    public AP_PuzzleLight_Pc            ledPuzzleSolved;
    public bool                         b_CheckSpriteAndLed = false;        // If true the script has check if the puzzle contains puzzleSprite and/or ledPuzzleSolved

    public bool                         b_PuzzleStateButtons = false;       // Know if the buttons reset, clue and exit puzzle are activated on the puzzle

    private void Start()
    {
        #region
        if (puzzleSprite) puzzleSprite = puzzleSprite.GetComponent<AP_PuzzleSpriteState_Pc>();

        _actionsWhenPuzzleIsSolved = GetComponent<actionsWhenPuzzleIsSolved_Pc>();                                   // Access actionsWhenPuzzleIsSolved component

        //puzzleSprite = checkForAP_PuzzleSpriteState();
        //ledPuzzleSolved = checkForAP_PuzzleLight();


        //Find if a ClueBox is attached to the puzzle
        Transform[] allTransform = GetComponentsInChildren<Transform>(true);
        foreach (Transform obj in allTransform)
        {
            if (obj.name == "ClueBox")
            {
                objClueBox = obj.gameObject.GetComponent<AP_Clue_Pc>();   // Access to the AP_Clue script
                break;
            }
        }
        #endregion
    }

    public GameObject puzzleCamera;

    //--> Check if the puzzle is locked or not
    public void checkIfPuzzleIsAvailable()
    {
        #region
        if (callMethods.Call_A_Method_Only_Boolean(methodsList) &&     // all the custom method return true
            !_actionsWhenPuzzleIsSolved.returnactionsWhenPuzzleIsSolved()
             
            ||

            CanvasD_Pc.instance && CanvasD_Pc.instance._D)                           // Debug Mode Activated
        {
          
            //b_PuzzleIsActivated = true;
            StartCoroutine(WaitBeforeStartingPuzzle());
        }
        else if (!callMethods.Call_A_Method_Only_Boolean(methodsList)     // A custom method return false     
                /* && ingameGlobalManager.instance.b_focusModeIsActivated*/)
        {
            b_PuzzleIsActivated = false;

            callMethods.Call_A_Method(methodsListFeedback);
            //b_PuzzleStateButtons = true;
        }

        //b_PuzzleStateButtons = true;

        #endregion
    }

    // Prevent bug when focus is used. Puzzle part is not moving just after entered the puzzle.
    private IEnumerator WaitBeforeStartingPuzzle()  
    {
        #region
        yield return new WaitForEndOfFrame();
        b_PuzzleIsActivated = true;
        //b_PuzzleStateButtons = true;
        yield return null;
        #endregion
    }

    public void checkAccessAllowed()
    {
        #region
        bool b_AccessAllowed = false;


        if (callMethods.Call_A_Method_Only_Boolean(methodsList) &&     // all the custom method return true
            !_actionsWhenPuzzleIsSolved.returnactionsWhenPuzzleIsSolved()

            ||

            CanvasD_Pc.instance && CanvasD_Pc.instance._D)                           // Debug Mode Activated
        {
            b_AccessAllowed = true;


        }

         //Debug.Log(b_AccessAllowed);
        if(puzzleSprite == null)
            puzzleSprite = checkForAP_PuzzleSpriteState();
        if (ledPuzzleSolved == null)
            ledPuzzleSolved = checkForAP_PuzzleLight();


        //-> Choose the sprite corresponding to the puzzle state(Available or not Available)
        if (b_AccessAllowed){
            if (puzzleSprite && puzzleSprite.gameObject.activeInHierarchy) puzzleSprite.AP_ChangeSprite(1); 
        }
        else{
            if (puzzleSprite && puzzleSprite.gameObject.activeInHierarchy) puzzleSprite.AP_ChangeSprite(0);  

        }
        #endregion
    }


    private AP_PuzzleSpriteState_Pc checkForAP_PuzzleSpriteState()
    {
        #region
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.name == "Sprite_PuzzleState")
            {
                return child.GetComponent<AP_PuzzleSpriteState_Pc>();
            }
        }
       
        return null;
        #endregion
    }

    private AP_PuzzleLight_Pc checkForAP_PuzzleLight()
    {
        #region
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.name == "Lever_Light_Feedback")
            {
                return child.GetComponent<AP_PuzzleLight_Pc>();
            }
        }

        return null;
        #endregion
    }
   

    public void changeSpriteAndLedWhenPuzzleSolved()
    {
        puzzleSprite = checkForAP_PuzzleSpriteState();
        ledPuzzleSolved = checkForAP_PuzzleLight();

        if (ledPuzzleSolved && ledPuzzleSolved.gameObject.activeInHierarchy) ledPuzzleSolved.AP_Btn_On();       // Led switch On
        if (puzzleSprite && puzzleSprite.gameObject.activeInHierarchy) puzzleSprite.AP_ChangeSprite(2);   // Sprite: Solved
    }

    public bool ReturnIfAllTheConditionsReturnTrue()
    {
       return callMethods.Call_A_Method_Only_Boolean(methodsList);
    }

    public void CallFeedbackMethods()
    {
        callMethods.Call_A_Method(methodsListFeedback);
    }

    public bool Bool_checkAccessAllowed()
    {
        checkAccessAllowed();
        return true;
    }

  
}
