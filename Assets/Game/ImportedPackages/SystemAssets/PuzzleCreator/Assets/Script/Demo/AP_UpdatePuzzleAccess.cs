using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_UpdatePuzzleAccess : MonoBehaviour
{
    public conditionsToAccessThePuzzle_Pc _condition;   // Reference to the puzzle you want to check its access

    public GameObject ObjectToActivate;                 // Reference to the object we want to activate in the Hierarchy

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))            // When key Y is pressed
        {
            UpdatePuzzleAccess();
        }

    }


    public void UpdatePuzzleAccess()
    {
        if (ObjectToActivate.GetComponent<Renderer>())
            ObjectToActivate.GetComponent<Renderer>().enabled = true;
        else
            ObjectToActivate.SetActive(true);       // Activate in the hierarchy ObjectToActivate

        _condition.checkAccessAllowed();            // Check if the selected puzzle access can be allowed 
    }
}
