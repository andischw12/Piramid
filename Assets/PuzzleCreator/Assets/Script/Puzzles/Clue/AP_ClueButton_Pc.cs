// Description : AP_ClueButton_Pc : script attached to Clue in the Hierarchy.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_ClueButton_Pc : MonoBehaviour
{

    public void AP_DisplayClueUI()
    {
        #region
       /*

        ingameGlobalManager gManager = ingameGlobalManager.instance;

        gManager.onlyPauseGame();


        gManager.currentPuzzle.GetComponent<conditionsToAccessThePuzzle>().objClueBox.displayClueWithItsNumber(0);         // Display the first clue;

        //gManager.lastUIButtonSelected = null;

        gManager.canvasMainMenu.GoToOtherPageWithHisNumber(12);                 // Clue Window
        gManager.navigationList.Add("Clue");

        gManager.StartCoroutine(gManager.changeLockStateConfined(true));
        gManager.b_AllowCharacterMovment = false;
        if (gManager.reticule && gManager.reticule.activeSelf && gManager.b_DesktopInputs)
            gManager.reticule.SetActive(false);

        ingameGlobalManager.instance._joystickReticule.newPosition(Screen.width / 2, Screen.height / 2);

        if (gManager.reticuleJoystickImage && gManager.b_Joystick)
            gManager.reticuleJoystickImage.gameObject.SetActive(true);


        //ingameGlobalManager.instance.canvasPlayerInfos.gameObject.SetActive(false);
        //ingameGlobalManager.instance.canvasPlayerInfos.obj_Grp_InfoPuzzle.SetActive(false);


        gManager.canvasPlayerInfos.deactivateIcons(false);

        ingameGlobalManager.instance.canvasMainMenu.GetComponent<iconsInfoInputs>().displayAvailableActionOnScreen(false, false);
        gManager.audioMenuClips.playASound(5);                          // Play sound (Hierarchy : ingameGlobalManager -> audioMenu)


        ingameGlobalManager.instance.canvasMainMenu.GetComponent<iconsInfoInputs>().displayAvailableActionOnScreen(false, true);
        */       
        #endregion
    }

    public void AP_HideClueUI()
    {

    }

  
}
