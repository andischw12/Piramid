using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_ExampleAR_Pc : MonoBehaviour
{
    public conditionsToAccessThePuzzle_Pc puzzleCondition;

    public void ActivateThePuzzleManually()
    {
        puzzleCondition.b_PuzzleIsActivated = true;
    }
}
