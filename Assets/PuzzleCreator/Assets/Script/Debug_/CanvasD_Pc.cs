// Description : CanvasD_Pc : Use in canvas debug Mode : Unlock everything and complete all puzzles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasD_Pc : MonoBehaviour {
    public bool SeeInspector = false;

    public KeyCode obj = KeyCode.F;
    public KeyCode puzzle = KeyCode.G;
    public Text     txtFeedback;


    public static CanvasD_Pc instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public bool b_MobileButtons;
    public GameObject btn_Puzzle;
    public GameObject btn_Objects;


    public bool _P = false;
    public bool _D = false;

    void Awake()
    {
        if (instance == null)           //Check if instance already exists
            instance = this;            //if not, set instance to this

        else if (instance != this)      //If instance already exists and it's not this:
            Destroy(gameObject);        //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(obj))
            debugObjects();

        if (Input.GetKeyDown(puzzle))
            debugPuzzle();

        if(txtFeedback){
            string tmpString = "";

            if (_P)
                tmpString += "Puzzle Automatically Solved || ";

            if (_D)
                tmpString += "Everything is unlocked";

            txtFeedback.text = tmpString;
        }
	}

    public void debugPuzzle(){
        if (_P)
            _P = false;
        else
            _P = true;
    }

    public void debugObjects()
    {
        if (_D)
            _D = false;
        else
            _D = true;
    }
}
