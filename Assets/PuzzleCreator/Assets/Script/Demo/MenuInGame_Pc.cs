using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuInGame_Pc : MonoBehaviour
{
    public bool MainMenuScene = false;
    public KeyCode btnPause = KeyCode.Escape;
    public KeyCode btnPauseAlt = KeyCode.P;

    private Menu_Manager_Pc menuManager;

    [HideInInspector]
    public CallMethods_Pc callMethods;

    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsListOpenMenu      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsListCloseMenu      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsListConditionsToOpenTheMenu      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();

    public GameObject MobilePauseButton;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = GetComponent<Menu_Manager_Pc>();
    }

    // Update is called once per frame
    void Update()
    {
        #region
        if(!MainMenuScene && (Input.GetKeyDown(btnPause) || Input.GetKeyDown(btnPauseAlt)) && menuManager.CurrentPage != 1)
        {
            if (callMethods.Call_A_Method_Only_Boolean(methodsListConditionsToOpenTheMenu)) // All the conditions to enter the puzzle return true.
            {
                if (menuManager.CurrentPage != 0)
                    AP_DisplayMenuBackToMainMenu();      // Display menu Back to main Menu
                else
                    AP_GoBackToGame();      // Go back to game
            }
        }
        #endregion
    }

    public IEnumerator CallAllTheMethodsOneByOne(List<EditorMethodsList_Pc.MethodsList> listOfMethods)
    {
        #region
        for (var i = 0; i < listOfMethods.Count; i++)
        {
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(listOfMethods, i) == true);
        }
        yield return new WaitForEndOfFrame();
        yield return null;
        #endregion
    }


    //--> Call by TriggerManager.cs to start process to load a new scene 
    public void F_GoToAnotherLevel(int BuildInSceneIndex)
    {
        StartCoroutine(I_GoToAnotherLevel(BuildInSceneIndex));
    }

    //--> Go to another Level (use teleporter)
    IEnumerator I_GoToAnotherLevel(int BuildInSceneIndex)
    {
        #region
        //-> Display Loading black screen
        //if (!MainMenuScene)
            menuManager.GoToOtherPageWithHisNumber(3);      // Display LoadingScreen

        for(var i = 0;i<3;i++)
            yield return new WaitForEndOfFrame();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(BuildInSceneIndex, LoadSceneMode.Single);
        #endregion
    }

    public void AP_DisplayMenuBackToMainMenu()
    {
        #region
        menuManager.GoToOtherPageWithHisNumber(0);      // Display menu Back to main Menu
        StartCoroutine(CallAllTheMethodsOneByOne(methodsListOpenMenu));
        #endregion
    }

    public void AP_GoBackToGame()
    {
        #region
        menuManager.GoToOtherPageWithHisNumber(2);      // Display menu Back to main Menu
        StartCoroutine(CallAllTheMethodsOneByOne(methodsListCloseMenu));
        #endregion
    }

    public bool bool_playASound()
    {
        GetComponent<AudioSource>().Play();
        return true;
    }

    public void AP_ExitApplication()
    {
        Application.Quit();
    }

    public void AP_EndOfTheGame()
    {
        StartCoroutine(I_AP_EndOfTheGame());
    }

    private IEnumerator I_AP_EndOfTheGame()
    {
        #region
        yield return new WaitForSeconds(.5f);
        menuManager.GoToOtherPageWithHisNumber(1);      // Display End Of The Game
        StartCoroutine(CallAllTheMethodsOneByOne(methodsListOpenMenu));
        yield return null;
        #endregion
    }

   public bool Bool_EnabledPauseButton()
    {
        if (MobilePauseButton) MobilePauseButton.SetActive(true);
        return true;
    }
    public bool Bool_DisabledPauseButton()
    {
        if (MobilePauseButton) MobilePauseButton.SetActive(false);
        return true;
    }


    public void AP_ResetDemo()
    {
        #region
        PlayerPrefs.DeleteKey("Completion");
        if (PlayerPrefs.HasKey("0_Puzzles_1"))
            PlayerPrefs.DeleteKey("0_Puzzles_1");

        //Delete .Dat
        string itemPath = Application.persistentDataPath;
        if (File.Exists(Application.persistentDataPath + "/" + "0_Puzzles_1" + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + "0_Puzzles_1" + ".dat");
        }

        #endregion
    }
}
