using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Completion_Pc : MonoBehaviour
{
    public Text txtCompetion;
    public int puzzleTotal = 70;
    public string completionName = "Completion";
    public AP_ScenePuzzleManager_Pc scenePuzzleManager;

    public MenuInGame_Pc menuInGame;

    public void AP_Completion()
    {
        #region
        PlayerPrefs.SetInt(completionName, PlayerPrefs.GetInt(completionName) +1);
        InitComplettion();
        #endregion
    }

    public void InitComplettion()
    {
        #region
        scenePuzzleManager.SaveAllPuzzles();
        int currentCompletion = 0;
        if (PlayerPrefs.HasKey(completionName)) currentCompletion = PlayerPrefs.GetInt(completionName);

        if (txtCompetion) txtCompetion.text = "Completion: " + currentCompletion + "/" + puzzleTotal;

        AP_CheckIfAllPuzzleComplete();
        #endregion
    }

    public void AP_CheckIfAllPuzzleComplete()
    {
        #region
        if (PlayerPrefs.GetInt(completionName) == puzzleTotal)
            menuInGame.AP_EndOfTheGame();
        #endregion
    }

    public bool Bool_InitCompletion()
    {
        #region
        InitComplettion();
        return true;
        #endregion
    }

    public void AP_ResetDemo()
    {
        #region
        PlayerPrefs.DeleteKey(completionName);
        if (PlayerPrefs.HasKey("0_Puzzles_1"))
            PlayerPrefs.DeleteKey("0_Puzzles_1");

        //Delete .Dat
        string itemPath = Application.persistentDataPath;
        //itemPath = itemPath.TrimEnd(new[] { '\\', '/' });
        //FileUtil.DeleteFileOrDirectory(itemPath + "/" + j + "_" + "Puzzles_" + (i).ToString() + ".dat");
        if (File.Exists(Application.persistentDataPath + "/" + "0_Puzzles_1" + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + "0_Puzzles_1" + ".dat");
        }

        menuInGame.F_GoToAnotherLevel(0);
        #endregion
    }

    public bool Bool_AP_CheckIfAllPuzzleComplete()
    {
        #region
        if (PlayerPrefs.GetInt(completionName) == puzzleTotal)
            menuInGame.AP_EndOfTheGame();

        return true;
        #endregion
    }

    public bool Bool_AP_FirstTimeInitComplettion()
    {
        #region
        int currentCompletion = 0;
        if (PlayerPrefs.HasKey(completionName)) currentCompletion = PlayerPrefs.GetInt(completionName);

        if (txtCompetion) txtCompetion.text = "Completion: " + currentCompletion + "/" + puzzleTotal;

        AP_CheckIfAllPuzzleComplete();

        return true;
        #endregion
    }
}
