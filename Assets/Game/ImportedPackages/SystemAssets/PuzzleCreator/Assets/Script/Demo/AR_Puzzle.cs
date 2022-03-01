using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Puzzle : MonoBehaviour
{
    public float                timer = .1f;            // If the marker is disabled in the Hierarchy: The time to wait before disabled the puzzle.
    private float               currentTimer = 0f;
    private bool                refObjectState = false; // Know if the marker is active in the Hierarchy
    public GameObject           refObjectChecked;       // Ref to the cube inside the Marker
    public GameObject           refObjectToActivate;    // Ref to the puzzle
    public AP_PuzzleDetector_Pc aP_PuzzleDetector;      // Ref to the object PuzzleDetector inside the puzzle

    public bool b_PuzzleIsActivated = false;

    void Update()
    {
        #region //-> Enable the puzzle in the Hierarchy
        if (refObjectChecked.GetComponent<Renderer>().enabled && !refObjectState)     
        {
            refObjectState = true;
            refObjectToActivate.SetActive(true);
            currentTimer = 0;
            AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus = aP_PuzzleDetector;
            if (aP_PuzzleDetector != null)
            {
                b_PuzzleIsActivated = true;
                aP_PuzzleDetector.transform.parent.GetComponent<conditionsToAccessThePuzzle_Pc>().b_PuzzleIsActivated = true;
                aP_PuzzleDetector.transform.parent.GetComponent<conditionsToAccessThePuzzle_Pc>().b_PuzzleStateButtons = true; 
                Debug.Log("Activate the puzzle");
            }
        }
        #endregion

        #region //-> Disable the puzzle in the Hierarchy
        if (!refObjectChecked.GetComponent<Renderer>().enabled && currentTimer < timer)     
        {currentTimer = Mathf.MoveTowards(currentTimer, timer, Time.deltaTime);}


        if (!refObjectChecked.GetComponent<Renderer>().enabled && currentTimer == timer)   
        {
            currentTimer = Mathf.MoveTowards(currentTimer, timer, Time.deltaTime);
            refObjectState = false;
            refObjectToActivate.SetActive(false);
            currentTimer = 0;
            if(aP_PuzzleDetector != null && 
            aP_PuzzleDetector.transform.parent.GetComponent<conditionsToAccessThePuzzle_Pc>().b_PuzzleIsActivated)
            {
                b_PuzzleIsActivated = false;
                aP_PuzzleDetector.transform.parent.GetComponent<conditionsToAccessThePuzzle_Pc>().b_PuzzleIsActivated = false;
                aP_PuzzleDetector.transform.parent.GetComponent<conditionsToAccessThePuzzle_Pc>().b_PuzzleStateButtons = false;
                Debug.Log("Deactivate");
            }
        }
        #endregion
    }
}
