using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class AP_ScenePuzzleManager_Pc : MonoBehaviour {
    public bool                                 SeeInspector = false; 
    public Color color_01 = new Color(1, .8f, 0.2F, .4f);
    public Color color_02 = new Color(1, .8f, 0.2F, 1f);
    public Color color_03 = new Color(.3F, .9f, 1, .5f);
    public Color color_04 = new Color(.3F, .9f, 1, 1f);
    public Color color_05 = new Color(1, .5f, 0.3F, .4f);

    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.

    public bool                                 b_Auto = true;                      // If True: Save is auto load when scene starts.

    public List<SaveData_Pc>                       listOfPuzzles = new List<SaveData_Pc>();
    public List<bool>                           listState = new List<bool>();

    public bool                                 b_DeletePuzzleSaveForThisScene = false;

    public int                                  currentSelectedSlot = 0;
    private string                              sNameOfTheSave = "Puzzles_";

    public bool                                 b_InputsForTest = false;
    public KeyCode                              saveKey = KeyCode.B;
    public KeyCode                              loadKey = KeyCode.N;

    public bool                                 b_InitDone = false;

    //public bool                                 b_PlayerPrefs = false;      // Choose Save Type .Dat or PlayePrefs

    public bool                                 saveProcessOff = true;

    [Serializable]
    public class DataClass
    {
        public string data;
    }

    // Use this for initialization
    void Start () {
        #region
        if (b_Auto)
            StartCoroutine(CallAllTheMethodsOneByOne());


        if (b_DeletePuzzleSaveForThisScene)
            PlayerPrefs.DeleteKey(currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString());

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

    //-> Call all the methods in the list 
    IEnumerator CallAllTheMethodsOneByOne()
    {
        #region
        for (var i = 0; i < methodsList.Count; i++)
        {
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(methodsList, i) == true);
        }

        yield return null;
        #endregion
    }

    //--> Initialized all the puzzles
    public bool Bool_PuzzlesInitialisation()
    {
        StartCoroutine(I_PuzzlesInitialisation());
        return true;
    }

    private IEnumerator I_PuzzlesInitialisation()
    {
        #region
        b_InitDone = false;
        yield return new WaitForEndOfFrame();

        List<conditionsToAccessThePuzzle_Pc> tmpPuzzleList = new List<conditionsToAccessThePuzzle_Pc>();

        string puzzleDatas = "";

        if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal.SaveAsPlayerPrefs)  
        {       // PlayerPrefs are used

            if (PlayerPrefs.HasKey(currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString()))
            {
                puzzleDatas = PlayerPrefs.GetString(currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString());
            }
            else
            {
                for (var i = 0; i < listOfPuzzles.Count; i++)
                {
                    puzzleDatas += ":";
                    PlayerPrefs.SetString(currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString(), puzzleDatas);
                }
            }

    
                
            if (puzzleDatas != "")
            {
                string[] codes = puzzleDatas.Split(':');
                for (var i = 0; i < listOfPuzzles.Count; i++)
                    if (listOfPuzzles[i] != null)
                    {
                        listOfPuzzles[i].LoadData(codes[i]);
                        if (listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>())
                            tmpPuzzleList.Add(listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>());
                    }

            }
            else{
                for (var i = 0; i < listOfPuzzles.Count; i++)
                    if (listOfPuzzles[i] != null)
                    {
                        listOfPuzzles[i].LoadData("");
                        if (listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>())
                            tmpPuzzleList.Add(listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>());
                    }
               
            }

        }
        else
        {                                                                               // .dat
            if (LoadDAT() != "")
            {

                string[] codes = LoadDAT().Split(':');
                for (var i = 0; i < listOfPuzzles.Count; i++)
                {
                    if (listOfPuzzles[i] != null)
                    {
                        if (i >= codes.Length)
                            Debug.Log("INFO Puzzles: 1-Check if each of the scene used a unique name. " +
                            	"2-If you have change the scene order in Build Settings: Scene in Build -> Reset all the save datas in w_PuzzleCreator" +
                            	"");

                        listOfPuzzles[i].LoadData(codes[i]);
                        if (listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>())
                            tmpPuzzleList.Add(listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>());
                    }

                }
            }
            else{

                for (var i = 0; i < listOfPuzzles.Count; i++)
                {
                    if (listOfPuzzles[i] != null)
                    {
                        listOfPuzzles[i].LoadData("");
                        if (listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>())
                            tmpPuzzleList.Add(listOfPuzzles[i].GetComponent<conditionsToAccessThePuzzle_Pc>());
                    }

                }
            }
        }


        Debug.Log("Load Done");


        yield return new WaitForEndOfFrame();
        for (var i = 0; i< tmpPuzzleList.Count; i++){
            if(tmpPuzzleList[i].gameObject.activeInHierarchy)tmpPuzzleList[i].checkAccessAllowed();
        }

        //Debug.Log("Check Access Done: " + tmpPuzzleList.Count);

        b_InitDone = true;
        #endregion
    }

    //-> Check if all the puzzles are initialized
    public bool Bool_CheckIfAllPuzzlesInitialized()
    {
        return b_InitDone;
    }

    //-> Save all the puzzles contains in listOfPuzzles
    public void SaveAllPuzzles()
    {
        #region
        saveProcessOff = false;
        string sceneName = currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex;
        //--> Delete all save Parts
        //-> PlayerPrefs Case
        if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal.SaveAsPlayerPrefs)
        {
            if (PlayerPrefs.HasKey(sceneName))
                PlayerPrefs.DeleteKey(sceneName);
        }
        //-> .dat Case
        else
        {
            if (File.Exists(Application.persistentDataPath + "/" + sceneName + ".dat"))
                File.Delete(Application.persistentDataPath + "/" + sceneName + ".dat");
        }

        F_Save_SlotInformation(true);
        #endregion
    }

    //-> Save a specific puzzle manually in a playerPrefs
    public void SaveASpecificPuzzle(SaveData_Pc ThePuzzle, string UniqueName)
    {
        #region
        PlayerPrefs.SetString(UniqueName, ThePuzzle.R_SaveData());      // Save puzzle datas in PlayerPrefs name UniqueName
        Debug.Log("Save:" + ThePuzzle.R_SaveData());
        #endregion
    }

    //-> Initialize a specific puzzle manualy
    public void LoadASpecificPuzzle(SaveData_Pc ThePuzzle, string UniqueName)
    {
        #region
        StartCoroutine(I_LoadASpecificPuzzle(ThePuzzle, UniqueName));
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

    //--> Save Slot Information
    public void F_Save_SlotInformation( bool b_createNewData)
    {
        #region
        string s_refDatas = "";
        for (var i = 0; i < listOfPuzzles.Count; i++)
        {
            if (listOfPuzzles[i] != null)
                s_refDatas += listOfPuzzles[i].R_SaveData() + ":";
        }


        if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal.SaveAsPlayerPrefs)  // PlayerPrefs are used
        {
            Debug.Log("here Save PlayerPrefs: "+ currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString());
            PlayerPrefs.SetString(currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString(), s_refDatas);
            StartCoroutine(I_WaitTheEndOfSaveProcess());
        }

        else                                                                            // .dat
        {

            saveDAT(s_refDatas);
        }

        Debug.Log("Save:" + s_refDatas);
        #endregion
    }


    // prevent bug during the save
    private IEnumerator I_WaitTheEndOfSaveProcess()
    {
        #region
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        saveProcessOff = true;
        #endregion
    }


    //--> Save Datas in a .dat file
    private void saveDAT(string s_ObjectsDatas)
    {
        #region
        /*BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + currentSelectedSlot + "_" +  sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString() + ".dat");

        ObjectsDatasInSceneTwo data = new ObjectsDatasInSceneTwo();
        data.s_ObjectsDatas = s_ObjectsDatas;
        //Debug.Log ("In : " + data.s_ObjectsDatas);

        bf.Serialize(file, data);
        file.Close();
        StartCoroutine(I_WaitTheEndOfSaveProcess());
        */

        // JSON
        DataClass dataObject = new DataClass();
        dataObject.data = s_ObjectsDatas;
        string sName = currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString();

        string json = JsonUtility.ToJson(dataObject);
        File.WriteAllText(Application.persistentDataPath + "/" + sName + ".dat", json);
        StartCoroutine(I_WaitTheEndOfSaveProcess());


        #endregion
    }

    //--> Load datas from a .dat file
    string LoadDAT()
    {
        #region
        //Debug.Log(Application.persistentDataPath + "/" + currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString() + ".dat");
        //Scene scene = SceneManager.GetActiveScene();
        //Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);

        if (File.Exists(Application.persistentDataPath + "/" + currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString() + ".dat"))
        {
            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString() + ".dat", FileMode.Open);

            ObjectsDatasInSceneTwo data = (ObjectsDatasInSceneTwo)bf.Deserialize(file);
            //Debug.Log ("Out : " + data.s_ObjectsDatas);
            string result = data.s_ObjectsDatas;
            file.Close();
            */

            // JSON
            string result;
            string sName = currentSelectedSlot + "_" + sNameOfTheSave + SceneManager.GetActiveScene().buildIndex.ToString();
            string dat = File.ReadAllText(Application.persistentDataPath + "/" + sName + ".dat");
            DataClass dataObject = JsonUtility.FromJson<DataClass>(dat);
            result = dataObject.data;
            


            return result;
        }
        else
            return "";
        #endregion
    }

    public bool bool_changeLockStateLock()
    {
        AP_GlobalPuzzleManager_Pc.instance.bool_changeLockStateLock();
        return true;
    }

    public bool bool_changeLockStateConfined()
    {
        AP_GlobalPuzzleManager_Pc.instance.bool_changeLockStateConfined();
        return true;
    }

    public bool bool_PausePuzzle()
    {
        AP_GlobalPuzzleManager_Pc.instance.b_Pause = true;
        return true;
    }

    public bool bool_UnPausePuzzle()
    {
        AP_GlobalPuzzleManager_Pc.instance.b_Pause = false;
        return true;
    }



    public bool bool_returnSaveProcessState()
    {
        return saveProcessOff;
    }
}

[System.Serializable]
class ObjectsDatasInSceneTwo
{
    public string s_ObjectsDatas;
}
