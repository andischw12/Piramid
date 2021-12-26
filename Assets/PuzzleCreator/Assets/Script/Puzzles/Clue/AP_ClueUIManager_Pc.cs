//Description: AP_ClueUIManager_Pc: Various methods for Clue UI interface
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AP_ClueUIManager_Pc : MonoBehaviour
{
    public Text         obj_Txt_Clue;
    public Text         obj_Faketxt;
    public Image        obj_Sprite_Clue;
    public GameObject   previousClue;
    public GameObject   nextClue;
    public Text         obj_Txt_HowManyClues;
    public Image        obj_padLock;

    public Sprite       spriteLock;
    public Sprite       spriteNoneAvailable;

    public AudioClip    a_Validate;
    public AudioClip    a_Wrong;

    public void AP_Btn_PreviousClue()
    {
        AP_Clue_Pc aP_Clue = AP_GlobalPuzzleManager_Pc.instance.currentPuzzle.accessPuzzle.GetComponent<conditionsToAccessThePuzzle_Pc>().objClueBox;
        aP_Clue.AP_PreviousClue();  
    }

    public void AP_Btn_NextClue()
    {
        AP_Clue_Pc aP_Clue = AP_GlobalPuzzleManager_Pc.instance.currentPuzzle.accessPuzzle.GetComponent<conditionsToAccessThePuzzle_Pc>().objClueBox;
        aP_Clue.AP_NextClue();  
    }

    public void AP_Btn_ShowAds()
    {
        if(obj_padLock.sprite == spriteLock){
            Debug.Log("Show Ads");
            StartCoroutine(unlockClue());
            // Here Call the method that starts your Ads
        }
        else{
            Debug.Log("Previous Clue must be unlock first");   
        }
    }

    IEnumerator unlockClue(){
        yield return new WaitForSeconds(1);
        AP_Clue_Pc aP_Clue = AP_GlobalPuzzleManager_Pc.instance.currentPuzzle.accessPuzzle.GetComponent<conditionsToAccessThePuzzle_Pc>().objClueBox;
        aP_Clue.AP_UnlockClue();  
        yield return null;
    }
}
