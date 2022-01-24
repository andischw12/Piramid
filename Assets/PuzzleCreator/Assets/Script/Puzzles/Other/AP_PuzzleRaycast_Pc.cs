// Description: AP_PuzzleRaycast_Pc: Use to detect puzzle. Find this script on the chararcter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AP_PuzzleRaycast_Pc : MonoBehaviour {
    public bool b_AutoInitialization = true;

    public LayerMask                myLayer;
    public float                    RayDistance = 10;
    private bool                    b_PuzzleFind = false;

    private bool                    b_isPuzzleActivated;

    private AP_PuzzleDetector_Pc currentPuzzle;

    public bool                     b_DeactivateMainCamera = true;
    private Camera                  objMainCamera;

    private Color                   reticuleInitColor = Color.white;
    public Color                    reticuleSelectedColor = Color.red;

    private GameObject               btn_MobilePuzzle;

    private AP_GlobalPuzzleManager_Pc GlobalPuzzleManager;


    void Start()
    {
        if (b_AutoInitialization)
            AP_Init();
    }

    public void AP_Init()
    {
        #region
        objMainCamera = Camera.main;
        btn_MobilePuzzle = Find_btn_MobilePuzzle();

        GlobalPuzzleManager = AP_GlobalPuzzleManager_Pc.instance;

        if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
            GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;

        if(GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule){
            reticuleInitColor = GlobalPuzzleManager.reticule.color;
            GlobalPuzzleManager.reticule.gameObject.SetActive(true);
        }
        else if(!GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule){
            GlobalPuzzleManager.reticule.gameObject.SetActive(false);
        }

        // Update layer Mask using the data wObjCreator.
        if (GlobalPuzzleManager._dataGlobal)
        {
            myLayer = LayerMask.GetMask(LayerMask.LayerToName(GlobalPuzzleManager._dataGlobal.currentLayerPuzzleRay));
        }
        #endregion
    }

    public GameObject Find_btn_MobilePuzzle(){
        #region
        GameObject tmpObj = GameObject.Find("Canvas_UIPuzzle");
        if(tmpObj){
            Transform[] allChildren = tmpObj.GetComponentsInChildren<Transform>(true);
            foreach(Transform child in allChildren){
                
                if(child.name == "btn_PuzzleMobile"){
                    Debug.Log(child.name);
                    return child.gameObject;
                   
                }
            }
        }
        return null;
        #endregion
    }

    void Update()
    {
        #region
        //if (!b_isPuzzleActivated) 
            //b_PuzzleFind = AP_DetectPuzzle();

        if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause)
        {
           
            // Desktop
            if ((Input.GetKeyDown(GlobalPuzzleManager.validationButtonKeyboard)
                || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))
                || Input.GetKeyDown(GlobalPuzzleManager.validationButtonJoystick)) &&
               b_PuzzleFind &&
               !b_isPuzzleActivated &&
               currentPuzzle != null &&
               GlobalPuzzleManager.b_DesktopInputs
               )
            {
                print("here");
                // Puzzle can be activated
                if (currentPuzzle.accessPuzzle.ReturnIfAllTheConditionsReturnTrue())
                { 
                    AP_ActivatePuzzle();
                   
                }
                // Puzzle can't be activated
                else
                {
                    currentPuzzle.accessPuzzle.CallFeedbackMethods();
                }

            }

            if ((Input.GetKeyDown(GlobalPuzzleManager.backButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRBackDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRBackDown, 0)) || Input.GetKeyDown(GlobalPuzzleManager.backButtonJoystick)) &&
                b_isPuzzleActivated &&
               GlobalPuzzleManager.b_DesktopInputs &&
                currentPuzzle &&
                currentPuzzle.puzlleIntearctionType == 0) // Focus Case
            {
                AP_DeactivatePuzzle();
            }
        }

        if (!GlobalPuzzleManager.b_DesktopInputs || GlobalPuzzleManager.b_DesktopInputs && GlobalPuzzleManager.b_iconPuzzleMobile)
        {
            if (btn_MobilePuzzle == null)
                btn_MobilePuzzle = Find_btn_MobilePuzzle();
                

            if(currentPuzzle && currentPuzzle.b_FocusActivated){
                if (btn_MobilePuzzle != null && b_PuzzleFind && btn_MobilePuzzle.activeInHierarchy == false && !b_isPuzzleActivated)
                {
                    btn_MobilePuzzle.SetActive(true);
                }

            }

            if (btn_MobilePuzzle != null && !b_PuzzleFind && btn_MobilePuzzle.activeInHierarchy == true && AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus == null)
                {
                    btn_MobilePuzzle.SetActive(false);
                } 
            
        }
        #endregion
    }

    public void PreventBugWhenPlayerExitAPuzzle(){
        #region
        b_isPuzzleActivated = false;
        currentPuzzle = null;
        if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
            GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;

        if (GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule) GlobalPuzzleManager.reticule.gameObject.SetActive(true);
        #endregion
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Puzzle" && !b_isPuzzleActivated) 
        {
           
            b_PuzzleFind = true;
            other.transform.parent.GetComponentInChildren<Outline>().enabled = true;
            currentPuzzle = other.transform.parent.GetComponentInChildren<AP_PuzzleDetector_Pc>();
            print(currentPuzzle.transform.name);

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Puzzle" && other.transform.parent.GetComponentInChildren<Outline>().enabled ==true)
        {

            b_PuzzleFind = false;
            currentPuzzle = null;
            other.transform.parent.GetComponentInChildren<Outline>().enabled = false;

        }
    }


    public bool AP_DetectPuzzle()
    {
        #region
        RaycastHit hit;
        RaycastHit hit2;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(transform.position, fwd * RayDistance, Color.green);

        bool hitLayerRay = true;
        if (Physics.Raycast(transform.position, fwd, out hit2, RayDistance))
        {
            //Debug.Log("Layer: "+ hit2.transform.gameObject.layer);
            if (hit2.transform.gameObject.layer != GlobalPuzzleManager._dataGlobal.currentLayerPuzzleRay)
                hitLayerRay = false;
        }

        if (Physics.Raycast(transform.position, fwd, out hit, RayDistance, myLayer) && hitLayerRay)
        {
            if (currentPuzzle == null)
            {

                currentPuzzle = hit.transform.gameObject.GetComponent<AP_PuzzleDetector_Pc>();
                GlobalPuzzleManager.changeCurrentPuzzle(currentPuzzle);
                if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
                    GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;

                if (GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule && currentPuzzle.b_FocusActivated) GlobalPuzzleManager.reticule.color = reticuleSelectedColor;


                if (GlobalPuzzleManager.iconPuzzle == null && GlobalPuzzleManager.b_AlwaysFindiconPuzzle)
                    GlobalPuzzleManager.iconPuzzle = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().iconPuzzle;
                if (GlobalPuzzleManager.b_iconPuzzle && GlobalPuzzleManager.iconPuzzle && currentPuzzle.b_FocusActivated)
                    GlobalPuzzleManager.iconPuzzle.gameObject.SetActive(true);
                

            }

            return true;
        }

        else
        {
            if (currentPuzzle != null)
            {
                currentPuzzle = null;
                GlobalPuzzleManager.currentPuzzle = null;
                if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
                    GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;
                if (GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule) GlobalPuzzleManager.reticule.color = reticuleInitColor;

                if (GlobalPuzzleManager.iconPuzzle == null && GlobalPuzzleManager.b_AlwaysFindiconPuzzle)
                    GlobalPuzzleManager.iconPuzzle = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().iconPuzzle;
                if(GlobalPuzzleManager.b_iconPuzzle && GlobalPuzzleManager.iconPuzzle)
                    GlobalPuzzleManager.iconPuzzle.gameObject.SetActive(false);
            }

            return false;
        }

        #endregion
    }

    public void AP_ActivatePuzzle(){
        #region
        if(currentPuzzle){
            b_isPuzzleActivated = true;
            currentPuzzle.Ap_ActivatePuzzle(objMainCamera);
             
            if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
                GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;
            if (GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule && currentPuzzle.b_FocusActivated) GlobalPuzzleManager.reticule.gameObject.SetActive(false);

            if (GlobalPuzzleManager.iconPuzzle == null && GlobalPuzzleManager.b_AlwaysFindiconPuzzle)
                GlobalPuzzleManager.iconPuzzle = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().iconPuzzle;
            if (GlobalPuzzleManager.b_iconPuzzle && GlobalPuzzleManager.iconPuzzle && currentPuzzle.b_FocusActivated)
                GlobalPuzzleManager.iconPuzzle.gameObject.SetActive(false);

            if (!GlobalPuzzleManager.b_DesktopInputs || GlobalPuzzleManager.b_DesktopInputs && GlobalPuzzleManager.b_iconPuzzleMobile)
            {
                if (btn_MobilePuzzle == null)
                    btn_MobilePuzzle = Find_btn_MobilePuzzle();

                if (btn_MobilePuzzle != null && currentPuzzle.b_FocusActivated)
                    btn_MobilePuzzle.SetActive(false);

            }  
        }
        #endregion
    }

    public void AP_DeactivatePuzzle()
    {
        #region
        b_isPuzzleActivated = false;
        currentPuzzle.Ap_DeactivatePuzzle();
        currentPuzzle = null;
        if (GlobalPuzzleManager.reticule == null && GlobalPuzzleManager.b_AlwaysFindReticule)
            GlobalPuzzleManager.reticule = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().reticule;
        if (GlobalPuzzleManager.b_Reticule && GlobalPuzzleManager.reticule) GlobalPuzzleManager.reticule.gameObject.SetActive(true);

        if (GlobalPuzzleManager.iconPuzzle == null && GlobalPuzzleManager.b_AlwaysFindiconPuzzle)
            GlobalPuzzleManager.iconPuzzle = GameObject.Find("Canvas_UIPuzzle").GetComponent<AP_PlayerInfos_Pc>().iconPuzzle;
        if (GlobalPuzzleManager.b_iconPuzzle && GlobalPuzzleManager.iconPuzzle)
            GlobalPuzzleManager.iconPuzzle.gameObject.SetActive(true);
        #endregion
    }
}
