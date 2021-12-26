// Description: AP_InitPuzzles_pc: find this script on ScenePuzzleManager in each scene. Allow to load and save puzzles data
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AP_InitPuzzles_pc : MonoBehaviour {

    public List<SaveData_Pc> listOfPuzzles = new List<SaveData_Pc>();

    public bool b_AutoLoad = true;
    public bool b_DeletePuzzleSaveForThisScene = false;

    public string sNameOfTheSave = "Puzzles_";

    public bool b_InputsForTest = false;
    public KeyCode saveKey = KeyCode.B;
    public KeyCode loadKey = KeyCode.N;

    public bool b_InitDone = false;

    // Use this for initialization
    void Start()
    {
        #region
        if (b_DeletePuzzleSaveForThisScene)
            PlayerPrefs.DeleteKey(sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString());

        if (b_AutoLoad)
            StartCoroutine(I_PuzzlesInitialisation());
        
        #endregion
    }

    private void Update()
    {
        #region
        if (b_InputsForTest)
        {
            if (Input.GetKeyDown(saveKey))
                SaveAllPuzzles();

            if (Input.GetKeyDown(loadKey))
                StartCoroutine(I_PuzzlesInitialisation());
        }
        #endregion
    }

    //--> Initialized all the puzzles
    public bool Bool_PuzzlesInitialisation(){
        StartCoroutine(I_PuzzlesInitialisation());
        return true;
    }

    private IEnumerator I_PuzzlesInitialisation()
    {
        #region
        b_InitDone = false;
        yield return new WaitForEndOfFrame();
        string puzzleDatas = "";
        if (PlayerPrefs.HasKey(sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString()))
        {
            puzzleDatas = PlayerPrefs.GetString(sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString());
        }
        else
        {
            for (var i = 0; i < listOfPuzzles.Count; i++)
            {
                puzzleDatas += ":";
                PlayerPrefs.SetString(sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString(), puzzleDatas);
            }
        }

        string[] codes = puzzleDatas.Split(':');

        for (var i = 0; i < listOfPuzzles.Count; i++)
        {
            listOfPuzzles[i].LoadData(codes[i]);
        }
        Debug.Log("Load Done");
        b_InitDone = true;
        #endregion
    }

    //-> Check if all the puzzles are initialized
    public bool Bool_CheckIfAllPuzzlesInitialized(){
        return b_InitDone;
    }

    //-> Save all the puzzles contains in listOfPuzzles
    public void SaveAllPuzzles()
    {
        #region
        string puzzleDatas = "";
        for (var i = 0; i < listOfPuzzles.Count; i++)
        {
            puzzleDatas += listOfPuzzles[i].R_SaveData() + ":";
        }
        PlayerPrefs.SetString(sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString(), puzzleDatas);
        Debug.Log("Save:" + puzzleDatas);
        #endregion
    }

    //-> Save a specific puzzle manualy in a playerPrefs
    public void SaveASpecificPuzzle(SaveData_Pc ThePuzzle,string UniqueName){
        #region
        PlayerPrefs.SetString(UniqueName, ThePuzzle.R_SaveData());      // Save puzzle datas in PlayerPrefs name UniqueName
        Debug.Log("Save:" + ThePuzzle.R_SaveData());
        #endregion
    }

    //-> Initialize a specific puzzle manualy
    public void LoadASpecificPuzzle(SaveData_Pc ThePuzzle, string UniqueName)
    {
        #region
        StartCoroutine(I_LoadASpecificPuzzle(ThePuzzle,UniqueName));
        #endregion
    }

    private IEnumerator I_LoadASpecificPuzzle(SaveData_Pc ThePuzzle, string UniqueName)
    {
        #region
        ThePuzzle.LoadData(PlayerPrefs.GetString(UniqueName));            // Load data and Initialized a specific puzzle
        Debug.Log("Load Done");
        yield return null;
        #endregion
    }


}
