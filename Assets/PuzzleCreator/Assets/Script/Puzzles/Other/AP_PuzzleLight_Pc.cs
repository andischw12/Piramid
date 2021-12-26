//Description: AP_PuzzleLight_Pc: Use On Lever_Light_Feedback inside the module Grp_PuzzleState
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_PuzzleLight_Pc : MonoBehaviour {

    public Texture texOn;
    public Texture texOff;
    private MeshRenderer meshRender;
    public bool b_On = false;

	// Use this for initialization
	void Start () {
        meshRender = GetComponent<MeshRenderer>();
        if (b_On) AP_Btn_On();
        else AP_Btn_Off();
	}
	
    public void AP_Btn_On(){
        meshRender.material.SetTexture("_EmissionMap", texOn);
    }

    public void AP_Btn_Off()
    {
        meshRender.material.SetTexture("_EmissionMap", texOff);
    }
}
