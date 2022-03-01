//Description: AP_Clue_Pc: Manage Clue system ingame
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_Clue_Pc : MonoBehaviour {
    public bool SeeInspector = false;

    [System.Serializable]
    public class clueParams
    {
        public List<string> txt_Clue = new List<string>();
        public List<Sprite> spriteClue = new List<Sprite>();    // entry Unique ID
        public bool b_Lock = false; 
    }

    public List<clueParams> clueList = new List<clueParams>(1) { new clueParams() };
    public int currentClue = 0; 

    public void displayClueWithItsNumber(int clueNumber)
    {
        #region
        AP_ClueUIManager_Pc clueUIManager = GameObject.Find("ClueManager").GetComponent<AP_ClueUIManager_Pc>();

        if(clueUIManager){
            if(clueList.Count == 1){
                clueUIManager.previousClue.gameObject.SetActive(false); 
                clueUIManager.nextClue.gameObject.SetActive(false); 
                clueUIManager.obj_Txt_HowManyClues.gameObject.SetActive(false); 
            }


            if(clueList[clueNumber].b_Lock){
                clueUIManager.obj_padLock.gameObject.SetActive(true);
                clueUIManager.obj_Txt_Clue.gameObject.SetActive(false);
                clueUIManager.obj_Sprite_Clue.gameObject.SetActive(false);
                clueUIManager.obj_Faketxt.gameObject.SetActive(false);

                if (clueNumber != 0 && clueList[clueNumber - 1].b_Lock)
                    clueUIManager.obj_padLock.sprite = clueUIManager.spriteNoneAvailable;
                else
                    clueUIManager.obj_padLock.sprite = clueUIManager.spriteLock;
            }
            else{
                clueUIManager.obj_padLock.gameObject.SetActive(false);  
                clueUIManager.obj_Txt_Clue.gameObject.SetActive(true);

                int sLanguage = AP_GlobalPuzzleManager_Pc.instance.currentPuzzleLanguage;
                if (clueList[clueNumber].txt_Clue.Count - 1 < sLanguage)
                    sLanguage = 0;

                clueUIManager.obj_Txt_Clue.text = clueList[clueNumber].txt_Clue[sLanguage];

                if (clueList[clueNumber].spriteClue[sLanguage] != null){
                    clueUIManager.obj_Sprite_Clue.gameObject.SetActive(true);
                    clueUIManager.obj_Faketxt.gameObject.SetActive(false); 
                    clueUIManager.obj_Sprite_Clue.sprite = clueList[clueNumber].spriteClue[sLanguage];
                }
                else{
                    clueUIManager.obj_Sprite_Clue.gameObject.SetActive(false);
                    clueUIManager.obj_Faketxt.gameObject.SetActive(true);
                }
            }
            clueUIManager.obj_Txt_HowManyClues.text = (clueNumber + 1).ToString() + "/" + clueList.Count;  
        }
        #endregion
    }

    public void AP_InitClue(string s_ObjectDatas){
        #region
        Debug.Log("Load Clue: " + s_ObjectDatas);

        string[] codes = s_ObjectDatas.Split('_');              // Split data in an array.
       

        //--> Actions to do for this puzzle ----> BEGIN <----
        if (s_ObjectDatas == "")
        {                               // Save Doesn't exist
           
        }
        else
        {                                                   // Save exist
            int startValue = 2;
            //if (!GetComponent<VoiceProperties>())
            //    startValue = 1;
            
            Debug.Log("startValue: " + startValue);

            for (var i = 0; i < clueList.Count; i++)
            {
                if (codes[i+startValue] == "T")
                    clueList[i].b_Lock = true;
                else
                    clueList[i].b_Lock = false;
            }
        }
        #endregion
    }

    public string AP_SaveData()
    {
        #region
        Debug.Log("Save Clue: ");
        string valuesToSave = "";

        for (var i = 0; i < clueList.Count;i++){
            valuesToSave += r_TrueFalse(clueList[i].b_Lock);
            valuesToSave += "_";
        }

        return valuesToSave;
        #endregion
    }

    public void AP_PreviousClue()
    {
        #region
        currentClue--;
        if(currentClue < 0) currentClue =  clueList.Count - 1;
        displayClueWithItsNumber(currentClue);
        #endregion
    }

    public void AP_NextClue()
    {
        #region
        currentClue++;
        currentClue %= clueList.Count;
        displayClueWithItsNumber(currentClue);
        #endregion
    }

    public void AP_UnlockClue(){
        #region
        clueList[currentClue].b_Lock = false;       // Clue is available
        displayClueWithItsNumber(currentClue);
        #endregion
    }

    //--> Convert bool to T or F string
    private string r_TrueFalse(bool s_Ref)
    {
        if (s_Ref) return "T";
        else return "F";
    }

}
