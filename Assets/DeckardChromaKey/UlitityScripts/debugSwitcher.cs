using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class debugSwitcher : MonoBehaviour
{

    public bool showTrackingGuides = true;
    public GameObject[] trackers;  
    bool activated;
    public KeyCode key;





    // Start is called before the first frame update
    void Awake()
    {
        if (showTrackingGuides)
        {
            activated = false;
            Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated != showTrackingGuides)
        {

            if (activated == true)
            {
                Deactivate();

                return;
            }

            if (activated == false)
            {
                Activate();

            }
        }

        if (Input.GetKeyDown(key) && activated == true)
        {
            showTrackingGuides = false;
            return;
        }
        if (Input.GetKeyDown(key) && activated == false)
        {
            showTrackingGuides = true;
        }
    }

    void Activate()
    {
        foreach (GameObject go in trackers)
        {
            MeshRenderer mr = go.GetComponent<MeshRenderer>();
            mr.enabled = true;
            activated = true;
            showTrackingGuides = activated;
        }
    }

    void Deactivate()
    {
        foreach (GameObject go in trackers)
        {
            MeshRenderer mr = go.GetComponent<MeshRenderer>();
            mr.enabled = false;
            activated = false;
            showTrackingGuides = activated;
        }
    }
}
