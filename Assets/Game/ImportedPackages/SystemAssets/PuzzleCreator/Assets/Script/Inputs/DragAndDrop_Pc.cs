// Description : DragAndDrop_Pc : Use in puzzle to drag and drop object (mobile, keyboard and desktop)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

namespace AP_
{
    public class DragAndDrop_Pc : MonoBehaviour
    {

        public bool                 SeeInspector = false;
        public bool                 b_ValidationButtonPressed = false;              // Prevent bug. Only one click available

        public bool                 puzzleObjectIsDetected = false;                 // Know if an object from the puzzle is detected

        public List<SpriteRenderer> listOfSelectedPuzzlePosition = new List<SpriteRenderer>();      // list of the PuzzleRefPosition. The position where an object could be drop


        public Transform            ReticuleJoystick;                                // Joystick fake Mouse              

        public Color                DefaultColor = Color.white;
        public Color                SelectedColor = Color.red;
        public Image                ImageObjDetected;
        public GameObject           Grp_ImageFakeMouse;
        public AP_Reticule_Pc       s_Reticule;

        public LayerMask            myLayer;                                        // Layer : Default. Use to raycast objects that needed to move. Object with Tag : Puzzle Object 
        public LayerMask            myLayer2;                                       // Layer : PuzzleFeedbackCam. Use to detect the PuzzleRefPosition. Tag : PuzzleRefPosition
        public bool                 puzzleObjectDetectionIsActivated = false;       // Allow object detection if True    

        public GameObject           currentSelectedGameObject;                      // The current selected object
        public GameObject           lastSelectedGameObject;                         // Remember the last select object

        public GameObject           currentSelectedPuzzlePosition;                  // The current selected position
        public GameObject           lastSelectedPuzzlePosition;                     // Remember the last select position
        private int                 fingerNum = 0;                                  // Mobile : know which finger is pressed on screen

        public AudioSource          a_TakeObject;                                   // Play audio when object is taken or released
        public Vector3              currentTargetObjectPosition = Vector3.zero;

        public float                distanceFromTheCamera = .45f;                   // Tweak the distance between focus cam and puzzle Objects

        public GameObject           refDragAndDropEulerAngle;                       // When an object is drag on screen, the object use refDragAndDropEulerAngle eulerAngle

        public int                  dragAndDropMode = 0;

        public Transform            objToDragCurrentParent;

        public float                raycastDistance = 5;

        public GameObject           btn_GrabObjectMobile;

        public bool                 b_DeselectProcessEnded = true; // Prevent bug: Prevent bug with Puzzle Solved method

        bool b_OnlyOneTimeCanGrab = false;
        bool b_OnlyOneTimeNoSelection = false;
        //bool b_OnlyOneTimeSelected = false;

        public List<GameObject> listOfHandsObj = new List<GameObject>();
        public List<GameObject> listOfGearsLogicsObj = new List<GameObject>();


        void Start()
        {
            #region
            initDragAndDrop();

            foreach (Transform child in transform)
            {
                if (child.name == "a_TakeObject")
                    a_TakeObject = child.GetComponent<AudioSource>();
            }
            #endregion
        }

        //--> Script Initialisation
        private void initDragAndDrop()
        {
            #region
            GameObject Grp_canvas = GameObject.Find("Grp_Canvas");
            Transform[] allTransform = Grp_canvas.GetComponentsInChildren<Transform>(true);

            AP_PlayerInfos_Pc canvas_PlayerInfos = null;

            foreach (Transform obj in allTransform)
            {
                if (obj.name == "Canvas_UIPuzzle")
                    canvas_PlayerInfos = obj.gameObject.GetComponent<AP_PlayerInfos_Pc>();   // Access to the UIVariousFunctions script
            }

            if (canvas_PlayerInfos)
            {
                if (canvas_PlayerInfos._joystickReticule)
                    ReticuleJoystick = canvas_PlayerInfos._joystickReticule.transform;         // Access joystick reticule
                
                if (canvas_PlayerInfos.reticuleJoystickImage)
                    Grp_ImageFakeMouse = canvas_PlayerInfos.reticuleJoystickImage.gameObject;     // Access joystick fake mouse 

                if (canvas_PlayerInfos.Image_ObjDetected)
                    ImageObjDetected = canvas_PlayerInfos.Image_ObjDetected;        //  Access Joystick fake mouse 
                
                if (canvas_PlayerInfos.s_reticule)
                    s_Reticule = canvas_PlayerInfos.s_reticule;        

                if (canvas_PlayerInfos.btn_GrabObjectMobile)
                    btn_GrabObjectMobile = canvas_PlayerInfos.btn_GrabObjectMobile;  
            }

            // Update layer Mask using the data wObjCreator.
            if (AP_GlobalPuzzleManager_Pc.instance._dataGlobal)
            {
                AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;
                string[] listLayer = new string[2] {
                LayerMask.LayerToName(aP_GlobalPuzzle._dataGlobal.currentLayerPuzzle),
                LayerMask.LayerToName(aP_GlobalPuzzle._dataGlobal.currentLayerPuzzleDragAndDrop) };

                myLayer = LayerMask.GetMask(listLayer); 
                myLayer2 = LayerMask.GetMask (LayerMask.LayerToName(aP_GlobalPuzzle._dataGlobal.currentLayerPuzzleFeedbackCam));
            }

            #endregion
        }

        //--> Call from other scrit that need to use drag and drop
        public void F_DragAndDrop(List<SpriteRenderer> puzzleListOfSelectedPuzzlePosition, Camera ObjCamera)
        {
            #region
            if (listOfSelectedPuzzlePosition.Count == 0)                            // Once : Init listOfSelectedPuzzlePosition                                  
                listOfSelectedPuzzlePosition = puzzleListOfSelectedPuzzlePosition;


            // if (ingameGlobalManager.instance.b_InputIsActivated)                    // Check if input is activated
            //{
            bool b_Wait = false;
            if (puzzleObjectDetectionIsActivated)                               // Puzzle detection is activated
            {
                //-> Object stop to follow the mouse
                if (AP_GlobalPuzzleManager_Pc.instance.b_Joystick && AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)
                {    // Joystick Case
                    if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && b_ValidationButtonPressed)
                    {
                        b_ValidationButtonPressed = false;
                        //DeselectObject(true);
                        StartCoroutine(waitDeselectObject(true));
                        b_Wait = true;
                    }
                }


                if (!b_Wait)
                {
                    bool b_PuzzleRefPosition = false;

                    //-> Raycast object in the Scene View. 
                    if (ReticuleJoystick && AP_GlobalPuzzleManager_Pc.instance.b_Joystick && AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)    // Joystick Case
                        CheckObjectWithTag_Joystick(b_PuzzleRefPosition,ObjCamera);
                    else if (!AP_GlobalPuzzleManager_Pc.instance.b_Joystick && AP_GlobalPuzzleManager_Pc.instance.b_DesktopInputs)                  // Keyboard Case
                        CheckObjectWithTag_Keyboard(b_PuzzleRefPosition, b_Wait, ObjCamera);
                    else
                        CheckObjectWithTag_Mobile(b_PuzzleRefPosition, b_Wait, ObjCamera);                                                         // Mobile Case


                    //-> Update the reticule color (Joystick case)
                    //updateReticuleColor();
                }
            }

            //--> Object reach the current target position
            if (currentSelectedGameObject && b_ValidationButtonPressed)
            {

                //currentSelectedGameObject.transform.localPosition = new Vector3(currentSelectedGameObject.transform.localPosition.x, currentSelectedGameObject.transform.localPosition.y, 0);

                if (dragAndDropMode == 0)
                { // Mode Focus
                    currentSelectedGameObject.transform.position = Vector3.MoveTowards(currentSelectedGameObject.transform.position, currentTargetObjectPosition, 4 * Time.deltaTime);
                    currentSelectedGameObject.transform.eulerAngles = refDragAndDropEulerAngle.transform.eulerAngles;
                }

            }
            #endregion
        }

        //--> Update the reticule color (Joystick case)
        /*private void updateReticuleColor()
        {
            #region
            //-> Raycast UI Objects
            if (puzzleObjectIsDetected && ImageObjDetected){
                ImageObjDetected.color = SelectedColor;
            }
            else if (!puzzleObjectIsDetected && 
                     ImageObjDetected &&
                     ImageObjDetected.color != DefaultColor)
            {
                ImageObjDetected.color = DefaultColor;
            }
            #endregion
        }*/


        public IEnumerator waitDeselectObject(bool b_PlaySound)
        {
            #region
            b_DeselectProcessEnded = false;
            if (currentSelectedGameObject && currentSelectedPuzzlePosition)
            {
                Transform tmpParent = currentSelectedGameObject.transform.parent;

                currentSelectedGameObject.transform.SetParent(currentSelectedPuzzlePosition.transform.GetChild(0).transform);
                currentSelectedGameObject.transform.localPosition = Vector3.zero;
                currentSelectedGameObject.transform.localRotation = Quaternion.identity;

                if (objToDragCurrentParent != null && dragAndDropMode != 0)
                    currentSelectedGameObject.transform.SetParent(objToDragCurrentParent);
                else
                    currentSelectedGameObject.transform.SetParent(tmpParent);

            }


            GameObject refObj = currentSelectedPuzzlePosition;

            //Debug.Log("Before");
            if (refObj)
            {
                // A Gear or a Logic is already on that position
                foreach (GameObject obj in listOfGearsLogicsObj)
                {
                    if (currentSelectedGameObject && obj.transform.position == currentSelectedGameObject.transform.position && currentSelectedGameObject != obj)
                    {
                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localEulerAngles = Vector3.zero;
                    }

                }

                for (var i = 0; i < 3; i++)
                    yield return new WaitForFixedUpdate();
            }
            //yield return new WaitForSeconds(2);

            if (dragAndDropMode == 3) // Reticule become white
                AP_GlobalPuzzleManager_Pc.instance.reticule.GetComponent<AP_Reticule_Pc>().callMethodsReticuleNoSelection();



            //yield return new WaitForSeconds(1);
            //yield return new WaitForSeconds(2);



            DeselectObject(b_PlaySound, refObj);

            yield return null;
            #endregion
        }

        //--> Deselect the current selected object
        public void DeselectObject(bool b_PlaySound, GameObject reAxis)
        {
            #region
            // Debug.Log("Here");
            b_ValidationButtonPressed = false;

            if (objToDragCurrentParent != null && (dragAndDropMode == 1 || dragAndDropMode == 2 || dragAndDropMode == 3) && currentSelectedGameObject)
            {
                Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, objToDragCurrentParent, Vector3.zero, Vector3.zero);

                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                if (refTrans)
                    refTrans.b_ObjSelected = false;


                objToDragCurrentParent = null;
            }

            // if(currentSelectedGameObject)
            //Debug.Log("Pendant: " +  currentSelectedGameObject.GetComponent<GearCheckCollision>().returnCheckCollision());

            //-> Selected object go to his inital position
            if (!reAxis
                ||
                /*currentSelectedGameObject &&
                currentSelectedGameObject.GetComponent<GearCheckCollision>() &&
                currentSelectedGameObject.GetComponent<GearCheckCollision>().returnCheckCollision()*/
                checkCollisionForAllObjects(currentSelectedGameObject)  // Check collision for objects in the currentSelectedGameObject


                )
            {
                // -> Case : Gear Puzzle
                if (currentSelectedGameObject &&
                   currentSelectedGameObject.GetComponent<GearLogicCheckCollision_Pc>())
                {
                    currentSelectedGameObject.GetComponent<GearLogicCheckCollision_Pc>().b_CollisionWithOtherGear = false;
                    //Debug.Log("Here 0");
                }

                if (currentSelectedGameObject)
                {                 // init position to the currentSelectedGameObject position    
                    currentSelectedGameObject.transform.position = currentSelectedGameObject.transform.parent.position;
                    currentSelectedGameObject.transform.eulerAngles = currentSelectedGameObject.transform.parent.transform.eulerAngles;
                    //Debug.Log("Here 1");
                }
            }
            //-> The selected go to the  currentSelectedGameObject or lastSelectedGameObject
            else
            {
                if (currentSelectedGameObject)
                {
                    currentSelectedGameObject.transform.position = reAxis.transform.GetChild(0).position;
                    //Debug.Log("Here 3");
                }
            }

            if (currentSelectedGameObject)
            {
                Ap_IsKinematic(currentSelectedGameObject.transform, true, true, false);
                if (a_TakeObject && b_PlaySound) a_TakeObject.Play();              // Play a sound
            }
           

            //Debug.Log("After: ");
            currentSelectedGameObject = null;

           


            b_DeselectProcessEnded = true;
            #endregion
        }

        bool checkCollisionForAllObjects(GameObject Grp_Parent)
        {
            #region
            if (Grp_Parent)
            {
                Transform[] allChildren = Grp_Parent.GetComponentsInChildren<Transform>(true);


                if (Grp_Parent.GetComponent<GearLogicCheckCollision_Pc>() &&
                     Grp_Parent.GetComponent<GearLogicCheckCollision_Pc>().returnCheckCollision())
                {

                    return true;
                }

                foreach (Transform child in allChildren)
                {
                    if (child.GetComponent<GearLogicCheckCollision_Pc>() &&
                    child.GetComponent<GearLogicCheckCollision_Pc>().returnCheckCollision())
                    {
                        Debug.Log(child.name);
                        return true;
                    }

                }
            }
                return false;
            #endregion
        }

        // Change rigidbody properties when a gear is dragged
        private void Ap_IsKinematic(Transform objTrans, bool b_isTrigger, bool b_isConvex, bool b_isKine)
        {
            #region

            if (objTrans.GetComponent<MeshCollider>())
            {
                if (b_isKine)
                {
                    objTrans.GetComponent<MeshCollider>().isTrigger = b_isTrigger;
                    objTrans.GetComponent<MeshCollider>().convex = b_isConvex;
                }
                else
                {
                    objTrans.GetComponent<MeshCollider>().convex = b_isConvex;
                    objTrans.GetComponent<MeshCollider>().isTrigger = b_isTrigger;
                }
                if (objTrans.GetComponent<Rigidbody>())
                    objTrans.GetComponent<Rigidbody>().isKinematic = b_isKine;
            }
            else if (objTrans.GetComponent<Collider>())
            {
                if (b_isKine)
                {
                    objTrans.GetComponent<Collider>().isTrigger = b_isTrigger;
                    //objTrans.GetComponent<MeshCollider>().convex = b_isConvex;
                }
                else
                {
                    //objTrans.GetComponent<MeshCollider>().convex = b_isConvex;
                    objTrans.GetComponent<Collider>().isTrigger = b_isTrigger;
                }
                if (objTrans.GetComponent<Rigidbody>())
                    objTrans.GetComponent<Rigidbody>().isKinematic = b_isKine;
            }

            Transform[] allChildren = objTrans.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<MeshCollider>())
                {
                    if (b_isKine)
                    {
                        child.GetComponent<MeshCollider>().isTrigger = b_isTrigger;
                        child.GetComponent<MeshCollider>().convex = b_isConvex;
                    }
                    else
                    {
                        child.GetComponent<MeshCollider>().convex = b_isConvex;
                        child.GetComponent<MeshCollider>().isTrigger = b_isTrigger;
                    }
                    if(child.GetComponent<Rigidbody>())
                        child.GetComponent<Rigidbody>().isKinematic = b_isKine;
                }
                else if(child.GetComponent<Collider>())
                {
                    if (b_isKine)
                    {
                        child.GetComponent<Collider>().isTrigger = b_isTrigger;
                        //objTrans.GetComponent<MeshCollider>().convex = b_isConvex;
                    }
                    else
                    {
                        //objTrans.GetComponent<MeshCollider>().convex = b_isConvex;
                        child.GetComponent<Collider>().isTrigger = b_isTrigger;
                    }
                    if (child.GetComponent<Rigidbody>())
                        child.GetComponent<Rigidbody>().isKinematic = b_isKine;
                }
            }

           
            #endregion
        }

        // Change Gear Position
        private void Ap_VhangeParentAndPosition(Transform objTrans, Transform _parent, Vector3 newPos, Vector3 newEulerAngle)
        {
            #region
            objTrans.transform.SetParent(_parent);
            objTrans.transform.localPosition = newPos;
            objTrans.transform.localEulerAngles = newEulerAngle;
            #endregion
        }

        //--> Joystick Case : Detect object to drag
        private void CheckObjectWithTag_Joystick(bool b_PuzzleRefPosition, Camera objCamera)
        {
            #region
                Ray ray; 
                if (ReticuleJoystick && dragAndDropMode == 0)
                    ray = objCamera.ScreenPointToRay(new Vector3(ReticuleJoystick.position.x, ReticuleJoystick.position.y, 0));
                else
                    ray = objCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (dragAndDropMode == 0 || dragAndDropMode == 3)   // Mode Focus or Free Reticule
                {
                    #region
                    if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
                    {
                        if (!b_ValidationButtonPressed)
                        {
                             //Debug.Log("Here 1");
                            if (GearsLogicsInTheList((hit.transform.gameObject)) && 
                                hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                                hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                                !b_ValidationButtonPressed)
                            {
                            
                                if (dragAndDropMode == 3 && s_Reticule && !s_Reticule.b_CanGrab) // Reticule become red
                                    s_Reticule.callMethodsListCanGrabReticule();
                            
                                //Debug.Log("Here 2: ");
                                if ((!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                                    && !b_ValidationButtonPressed)
                                {
                                    b_ValidationButtonPressed = true;
                                    if (a_TakeObject) a_TakeObject.Play();

                                    if (dragAndDropMode == 3) // Reticule become white
                                        AP_GlobalPuzzleManager_Pc.instance.reticule.GetComponent<AP_Reticule_Pc>().callMethodsListReticuleSelected();
                                }

                                puzzleObjectIsDetected = true;

                                //-> An object is selected. 
                                if (b_ValidationButtonPressed &&
                                    hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                                    hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                                    !hit.transform.GetComponent<Rigidbody>().isKinematic)
                                {
                                    if (currentSelectedGameObject != hit.transform.gameObject)
                                        currentSelectedGameObject = hit.transform.gameObject;

                                    if (dragAndDropMode == 3)                           // Save the parent. Used when object is deselected
                                        objToDragCurrentParent = currentSelectedGameObject.transform.parent;



                                    Ap_IsKinematic(hit.transform, false, false, true);

                                }
                            }
                            else if (hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                                    hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject")
                            {
                                //Debug.Log("Here");
                                puzzleObjectIsDetected = true;
                            }
                            else if (puzzleObjectIsDetected)
                            {
                                puzzleObjectIsDetected = false;
                                if (currentSelectedGameObject != null)
                                {
                                    lastSelectedGameObject = currentSelectedGameObject;
                                    currentSelectedGameObject = null;
                                }
                            }
                            else
                            {
                                if (dragAndDropMode == 3 && s_Reticule && s_Reticule.b_CanGrab) // Reticule become white
                                { s_Reticule.callMethodsReticuleNoSelection(); }
                            }
                        }
                    }
                   
                    #endregion
                }
                else if (dragAndDropMode == 1)                      // Mode Raycast
                {
                #region
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                    RaycastHit hitMode1;
                    if (refTrans)
                    {
                        //refTrans.AP_methodsLineModeRaycast();
                        if (Physics.Raycast(refTrans.transform.position, refTrans.transform.forward, out hitMode1, raycastDistance, myLayer))
                        {
                            if (GearsLogicsInTheList((hitMode1.transform.gameObject)) &&
                                hitMode1.transform.GetComponent<AP_CheckTag_Pc>() &&
                                hitMode1.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                                !b_ValidationButtonPressed)
                            {
                            b_OnlyOneTimeNoSelection = false;
                            //     Debug.Log("2");

                            if (!b_OnlyOneTimeCanGrab)
                            {
                                b_OnlyOneTimeCanGrab = true;
                                refTrans.AP_CanGrabLogicOrGearMethods();
                            }
                            // Debug.Log("Joystick Validation is pressed 2");
                            if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                                {
                                    refTrans.AP_LogicOrGearSelectedMethods();
                                    b_ValidationButtonPressed = true;
                                    if (a_TakeObject) a_TakeObject.Play();
                                }

                                puzzleObjectIsDetected = true;

                                //-> An object is selected. 
                                if (b_ValidationButtonPressed &&
                                    hitMode1.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                                    !hitMode1.transform.GetComponent<Rigidbody>().isKinematic)
                                {
                                    if (currentSelectedGameObject != hitMode1.transform.gameObject)
                                    {
                                        currentTargetObjectPosition = hitMode1.transform.gameObject.transform.position + new Vector3(0, .1f, 0);
                                        currentSelectedGameObject = hitMode1.transform.gameObject;

                                        if (dragAndDropMode == 1)                           // Save the parent. Used when object is deselected
                                            objToDragCurrentParent = currentSelectedGameObject.transform.parent;
                                        Ap_IsKinematic(hitMode1.transform, false, false, true);
                                    }
                                }
                            }
                            else if(hitMode1.transform.GetComponent<AP_CheckTag_Pc>() &&
                                    hitMode1.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject"){
                            b_OnlyOneTimeNoSelection = false;
                        }
                            else
                            {
                               

                            if (dragAndDropMode == 1 && !b_OnlyOneTimeNoSelection)
                            {                   // Mode Free: Raycast
                                b_OnlyOneTimeNoSelection = true;
                                b_OnlyOneTimeCanGrab = false;
                                refTrans.AP_LogicOrGearNoSelectionMethods();
                            }
                        }
                        }
                        else
                        {
                        if (dragAndDropMode == 1 && !b_OnlyOneTimeNoSelection)
                        {                  // Mode Free: Raycast
                            b_OnlyOneTimeNoSelection = true;
                            b_OnlyOneTimeCanGrab = false;
                            refTrans.AP_LogicOrGearNoSelectionMethods();
                        }
                    }
                    }
                    #endregion
                }
                else if (dragAndDropMode == 2)                      // Mode Hand
                {
                #region
                //Debug.Log("Here 0");
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                    if (refTrans &&
                        refTrans.SelectedObjectLogicOrGear &&
                        GearsLogicsInTheList((refTrans.SelectedObjectLogicOrGear.transform.gameObject)) &&
                        refTrans.SelectedObjectLogicOrGear.transform.GetComponent<AP_CheckTag_Pc>() &&
                        refTrans.SelectedObjectLogicOrGear.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                        !b_ValidationButtonPressed)
                    {
                        if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                        {
                            // Debug.Log("Here 2");
                            b_ValidationButtonPressed = true;
                            if (a_TakeObject) a_TakeObject.Play();
                        }

                        puzzleObjectIsDetected = true;

                        //-> An object is selected. 
                        if (b_ValidationButtonPressed &&
                            refTrans &&
                            refTrans.SelectedObjectLogicOrGear &&
                            !refTrans.SelectedObjectLogicOrGear.transform.GetComponent<Rigidbody>().isKinematic)
                        {
                            // Debug.Log("Here 3");
                            if (currentSelectedGameObject != refTrans.SelectedObjectLogicOrGear.transform.gameObject)
                            {
                                //Debug.Log("Here 4");
                                currentTargetObjectPosition = refTrans.SelectedObjectLogicOrGear.transform.gameObject.transform.position + new Vector3(0, .1f, 0);
                                currentSelectedGameObject = refTrans.SelectedObjectLogicOrGear.gameObject;

                                if (dragAndDropMode == 2)                           // Save the parent. Used when object is deselected
                                    objToDragCurrentParent = currentSelectedGameObject.transform.parent;

                                refTrans.b_ObjSelected = true;

                                if (currentSelectedGameObject.transform.GetComponent<Collider>())
                                    Ap_IsKinematic(currentSelectedGameObject.transform, false, false, true);

                            }
                        }
                    }
                    #endregion
                }


                Vector3 temp;
                if (ReticuleJoystick && dragAndDropMode == 0)
                    temp = ReticuleJoystick.position;
                else
                    temp = Input.mousePosition;
            
                temp.z = distanceFromTheCamera;

                if (b_ValidationButtonPressed)
                {
                    #region
                    if (dragAndDropMode == 0) // Mode Focus
                    { currentTargetObjectPosition = objCamera.ScreenToWorldPoint(temp); }
                    else if (dragAndDropMode == 1)  // Mode Raycast
                    { Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent, Vector3.zero, Vector3.zero); }
                    else if (dragAndDropMode == 2)  // Mode Hand
                    { Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent, Vector3.zero, Vector3.zero); }
                    else if (dragAndDropMode == 3)  // Mode Reticule
                    { Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent.transform.parent, Vector3.zero, Vector3.zero); }
                    #endregion
                }

               

                //-> Check if Puzzle ref position is detected
                RaycastHit hit2;
                if (dragAndDropMode == 0 || dragAndDropMode == 3)   // Mode Focus or Free Reticule
                {
                    #region

                    if (Physics.Raycast(ray, out hit2, raycastDistance, myLayer2))
                    {
                        if (hit2.transform.GetComponent<AP_CheckTag_Pc>() &&
                       hit2.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition")
                        {
                            if (hit2.transform.gameObject.GetComponent<SpriteRenderer>() && HandInTheList((hit2.transform.gameObject)))
                            {

                                SpriteRenderer _currentHit = hit2.transform.gameObject.GetComponent<SpriteRenderer>();

                                if (_currentHit.color.a == 0 && currentSelectedGameObject != null)
                                {
                                    _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                    currentSelectedPuzzlePosition = hit2.transform.gameObject;
                                }
                                b_PuzzleRefPosition = true;
                            }
                        }
                        else
                        {
                            b_PuzzleRefPosition = false;
                        }
                    }
                    #endregion
                }
                else if (dragAndDropMode == 1)                      // Mode Raycast
                {
                #region
                // Debug.Log("Here 1");
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                    RaycastHit hitMode1Part2;
                    if (refTrans)
                    {
                        if (Physics.Raycast(refTrans.transform.position, refTrans.transform.forward, out hitMode1Part2, raycastDistance, myLayer2))
                        {
                            //   Debug.Log("Here 2");
                            if (hitMode1Part2.transform.GetComponent<AP_CheckTag_Pc>() &&
                                hitMode1Part2.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition")
                            {
                                //     Debug.Log("Here 3");
                                if (hitMode1Part2.transform.gameObject.GetComponent<SpriteRenderer>() && HandInTheList((hitMode1Part2.transform.gameObject)))
                                {
                                    SpriteRenderer _currentHit = hitMode1Part2.transform.gameObject.GetComponent<SpriteRenderer>();

                                    if (_currentHit.color.a == 0)
                                    {
                                        _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                        currentSelectedPuzzlePosition = hitMode1Part2.transform.gameObject;
                                    }
                                    b_PuzzleRefPosition = true;
                                }
                            }
                            else
                            {
                                b_PuzzleRefPosition = false;
                            }
                        }


                    }

                    #endregion
                }
                else if (dragAndDropMode == 2)                      // Mode: Hand
                {
                #region
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                    if (refTrans)
                    {
                        if (refTrans.SelectedObjectAxis)
                        {
                            SpriteRenderer _currentHit = refTrans.SelectedObjectAxis.transform.gameObject.GetComponent<SpriteRenderer>();
                            if (_currentHit.color.a == 0)
                            {
                                _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                currentSelectedPuzzlePosition = refTrans.SelectedObjectAxis.gameObject;
                            }
                            b_PuzzleRefPosition = true;
                        }
                        else
                        {
                            b_PuzzleRefPosition = false;
                        }
                    }
                    #endregion
                }


            saveTheLastSelectedPuzzlePosition(b_PuzzleRefPosition);
            initSelectedPuzzlePositionSPriteColor();
            #endregion
        }



        //--> Keyboard Case : Detect object to drag
        private void CheckObjectWithTag_Keyboard(bool b_PuzzleRefPosition, bool b_Wait, Camera objCamera)
        {
            #region

            Ray ray = objCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (dragAndDropMode == 0 || dragAndDropMode == 3)   // Mode Focus or Free Reticule
            {
                #region
                if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
                {
                    if (GearsLogicsInTheList((hit.transform.gameObject)) && 
                        hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                        hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                        !b_ValidationButtonPressed)
                    {
                        if(dragAndDropMode == 3 && s_Reticule && !s_Reticule.b_CanGrab){    // Reticule become red
                            b_OnlyOneTimeNoSelection = false;
                            //     Debug.Log("2");

                            if (!b_OnlyOneTimeCanGrab)
                            {
                                b_OnlyOneTimeCanGrab = true;
                                s_Reticule.callMethodsListCanGrabReticule();
                            }

                        } 
                            
                        

                        // Debug.Log("Joystick Validation is pressed 2");
                        if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                        {
                            b_ValidationButtonPressed = true;
                            if (a_TakeObject) a_TakeObject.Play();

                           if (dragAndDropMode == 3) // Reticule become white
                             AP_GlobalPuzzleManager_Pc.instance.reticule.GetComponent<AP_Reticule_Pc>().callMethodsListReticuleSelected();
                            
                        }

                        puzzleObjectIsDetected = true;

                        //-> An object is selected. 
                        if (b_ValidationButtonPressed &&
                            hit.transform.GetComponent<AP_CheckTag_Pc>() && 
                            hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                            !hit.transform.GetComponent<Rigidbody>().isKinematic)
                        {
                            //Debug.Log("Here 3");
                            if (currentSelectedGameObject != hit.transform.gameObject)
                            {
                                currentTargetObjectPosition = hit.transform.gameObject.transform.position + new Vector3(0, .1f, 0);
                                currentSelectedGameObject = hit.transform.gameObject;

                                if (dragAndDropMode == 3)                           // Save the parent. Used when object is deselected
                                    objToDragCurrentParent = currentSelectedGameObject.transform.parent;

                                Ap_IsKinematic(hit.transform, false, false, true);

                            }
                        }
                    }
                    else{
                        if (dragAndDropMode == 3 && s_Reticule && s_Reticule.b_CanGrab){ // Reticule become white

                            if (dragAndDropMode == 1 && !b_OnlyOneTimeNoSelection)
                            {                  // Mode Free: Raycast
                                b_OnlyOneTimeNoSelection = true;
                                b_OnlyOneTimeCanGrab = false;
                                s_Reticule.callMethodsReticuleNoSelection();
                            }
                        }
                    }
                }
                #endregion
            }
            else if (dragAndDropMode == 1)                      // Mode Raycast
            {
                #region
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                RaycastHit hitMode1;
                if (refTrans)
                {
                   // Debug.Log("0");
                    //refTrans.AP_methodsLineModeRaycast();
                    if (Physics.Raycast(refTrans.transform.position, refTrans.transform.forward, out hitMode1, raycastDistance, myLayer))
                    {
                     //   Debug.Log("1: " + hitMode1.transform.name);
                        if (GearsLogicsInTheList((hitMode1.transform.gameObject)) && 
                            hitMode1.transform.GetComponent<AP_CheckTag_Pc>() &&
                            hitMode1.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                            !b_ValidationButtonPressed)
                        {
                            b_OnlyOneTimeNoSelection = false;
                            //     Debug.Log("2");

                            if (!b_OnlyOneTimeCanGrab)
                            {
                                b_OnlyOneTimeCanGrab = true;
                               refTrans.AP_CanGrabLogicOrGearMethods();
                            }

                            // Debug.Log("Joystick Validation is pressed 2");
                            if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                            {
                         //       Debug.Log("3");
                                
                                refTrans.AP_LogicOrGearSelectedMethods();
                                b_ValidationButtonPressed = true;
                                if (a_TakeObject) a_TakeObject.Play();
                            }

                            puzzleObjectIsDetected = true;

                            //-> An object is selected. 
                            if (b_ValidationButtonPressed &&
                                hitMode1.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                                !hitMode1.transform.GetComponent<Rigidbody>().isKinematic)
                            {
                           //     Debug.Log("4");
                                if (currentSelectedGameObject != hitMode1.transform.gameObject)
                                {
                                    currentTargetObjectPosition = hitMode1.transform.gameObject.transform.position + new Vector3(0, .1f, 0);
                                    currentSelectedGameObject = hitMode1.transform.gameObject;

                                    if (dragAndDropMode == 1)                           // Save the parent. Used when object is deselected
                                        objToDragCurrentParent = currentSelectedGameObject.transform.parent;
                                    Ap_IsKinematic(hitMode1.transform, false, false, true);
                                }
                            }
                        }
                        else if (hitMode1.transform.GetComponent<AP_CheckTag_Pc>() &&
                                    hitMode1.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleStateObject")
                        { b_OnlyOneTimeNoSelection = false; }
                        else
                        {
                            if (dragAndDropMode == 1 && !b_OnlyOneTimeNoSelection)
                            {                   // Mode Free: Raycast
                                b_OnlyOneTimeNoSelection = true;
                                b_OnlyOneTimeCanGrab = false;
                                refTrans.AP_LogicOrGearNoSelectionMethods();
                            }
                        }
                    }
                    else
                    {
                        if (dragAndDropMode == 1 && !b_OnlyOneTimeNoSelection)
                        {                  // Mode Free: Raycast
                            b_OnlyOneTimeNoSelection = true;
                            b_OnlyOneTimeCanGrab = false;
                            refTrans.AP_LogicOrGearNoSelectionMethods();
                        }
                    }
                }
                #endregion

            }
            else if (dragAndDropMode == 2)                      // Mode Hand
            {
                #region
                //Debug.Log("Here 0");
                //Debug.Log("0");
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                if (refTrans &&
                    refTrans.SelectedObjectLogicOrGear &&
                    GearsLogicsInTheList((refTrans.SelectedObjectLogicOrGear.transform.gameObject)) &&
                    refTrans.SelectedObjectLogicOrGear.transform.GetComponent<AP_CheckTag_Pc>() &&
                    refTrans.SelectedObjectLogicOrGear.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                    !b_ValidationButtonPressed)
                {
                    //Debug.Log("1: " + refTrans.SelectedObjectLogicOrGear.name);
                    if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))))
                    {
                       // Debug.Log("2");
                        // Debug.Log("Here 2");
                        b_ValidationButtonPressed = true;
                        if (a_TakeObject) a_TakeObject.Play();
                    }

                    puzzleObjectIsDetected = true;

                    //-> An object is selected. 
                    if (b_ValidationButtonPressed &&
                        refTrans &&
                        refTrans.SelectedObjectLogicOrGear &&
                        !refTrans.SelectedObjectLogicOrGear.transform.GetComponent<Rigidbody>().isKinematic)
                    {
                         //Debug.Log("3");
                        if (currentSelectedGameObject != refTrans.SelectedObjectLogicOrGear.transform.gameObject)
                        {
                            //Debug.Log("Here 4");
                            currentTargetObjectPosition = refTrans.SelectedObjectLogicOrGear.transform.gameObject.transform.position + new Vector3(0, .1f, 0);
                            currentSelectedGameObject = refTrans.SelectedObjectLogicOrGear.gameObject;

                            if (dragAndDropMode == 2)                           // Save the parent. Used when object is deselected
                                objToDragCurrentParent = currentSelectedGameObject.transform.parent;

                            refTrans.b_ObjSelected = true;

                            if (currentSelectedGameObject.transform.GetComponent<Collider>())
                                Ap_IsKinematic(currentSelectedGameObject.transform, false, false, true);

                        }
                    }
                }
                #endregion
            }

            Vector3 temp = Input.mousePosition;
            temp.z = distanceFromTheCamera;

            if (b_ValidationButtonPressed)
            {
                #region
                if (dragAndDropMode == 0) // Mode Focus
                {currentTargetObjectPosition = objCamera.ScreenToWorldPoint(temp);}
                else if (dragAndDropMode == 1)  // Mode Raycast
                {Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent, Vector3.zero, Vector3.zero);}
                else if (dragAndDropMode == 2)  // Mode Hand
                {Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent, Vector3.zero, Vector3.zero);}
                else if (dragAndDropMode == 3)  // Mode Reticule
                {Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent.transform.parent, Vector3.zero, Vector3.zero);}
                #endregion
            }

            if (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyUp(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationUp.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationUp, 0))) &&
                b_ValidationButtonPressed == true)
            {
                b_ValidationButtonPressed = false;
                StartCoroutine(waitDeselectObject(true));
            }


            //-> Check if Puzzle ref position is detected
            if (dragAndDropMode == 0 || dragAndDropMode == 3)   // Mode Focus or Free Reticule
            {
                #region
                RaycastHit hit2;
                if (Physics.Raycast(ray, out hit2, raycastDistance, myLayer2))
                {
                    if (hit2.transform.GetComponent<AP_CheckTag_Pc>() &&
                        hit2.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition")
                    {
                        if (hit2.transform.gameObject.GetComponent<SpriteRenderer>() && HandInTheList((hit2.transform.gameObject)))
                        {
                            SpriteRenderer _currentHit = hit2.transform.gameObject.GetComponent<SpriteRenderer>();

                            if (_currentHit.color.a == 0)
                            {
                                _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                currentSelectedPuzzlePosition = hit2.transform.gameObject;
                            }
                            b_PuzzleRefPosition = true;
                        }
                    }
                    else
                    {
                        b_PuzzleRefPosition = false;
                    }
                }
                #endregion
            }
            else if (dragAndDropMode == 1)                      // Mode Raycast
            {
                #region
                // Debug.Log("Here 1");
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                RaycastHit hitMode1Part2;
                if (refTrans)
                {
                    if (Physics.Raycast(refTrans.transform.position, refTrans.transform.forward, out hitMode1Part2, raycastDistance, myLayer2))
                    {
                        //   Debug.Log("Here 2");
                        if (hitMode1Part2.transform.GetComponent<AP_CheckTag_Pc>() &&
                            hitMode1Part2.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition")
                        {
                            //     Debug.Log("Here 3");
                            if (hitMode1Part2.transform.gameObject.GetComponent<SpriteRenderer>() && HandInTheList((hitMode1Part2.transform.gameObject)))
                            {
                                SpriteRenderer _currentHit = hitMode1Part2.transform.gameObject.GetComponent<SpriteRenderer>();

                                if (_currentHit.color.a == 0)
                                {
                                    _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                    currentSelectedPuzzlePosition = hitMode1Part2.transform.gameObject;
                                }
                                b_PuzzleRefPosition = true;
                            }
                        }
                        else
                        {
                            b_PuzzleRefPosition = false;
                        }
                    }


                }

                #endregion
            }
            else if (dragAndDropMode == 2)                      // Mode: Hand
            {
                #region
                AP_DragAndDropParent_Pc refTrans = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent;
                if (refTrans)
                {
                    if (refTrans.SelectedObjectAxis)
                    {
                        SpriteRenderer _currentHit = refTrans.SelectedObjectAxis.transform.gameObject.GetComponent<SpriteRenderer>();
                        if (_currentHit.color.a == 0)
                        {
                            _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                            currentSelectedPuzzlePosition = refTrans.SelectedObjectAxis.gameObject;
                        }
                        b_PuzzleRefPosition = true;
                    }
                    else
                    {
                        b_PuzzleRefPosition = false;
                    }
                }

                #endregion
            }

            saveTheLastSelectedPuzzlePosition(b_PuzzleRefPosition);
            initSelectedPuzzlePositionSPriteColor();
            #endregion
        }

        // Mobile Reticule Case. The playerclick on the Icon btn_GrabObjectMobile 
        public void AP_MobileSelectObjectToGrab(bool b_MobileGrap){
            #region
            if(currentSelectedGameObject && !b_MobileGrap){     // Select an gear or Logics
                objToDragCurrentParent = currentSelectedGameObject.transform.parent;
                Ap_VhangeParentAndPosition(currentSelectedGameObject.transform, AP_GlobalPuzzleManager_Pc.instance.dragAndDropParent.transform.parent, Vector3.zero, Vector3.zero);
                Ap_IsKinematic(currentSelectedGameObject.transform, false, false, true);  
                if (a_TakeObject) a_TakeObject.Play();
                b_MobileGrap = true;
            }
            else if(b_MobileGrap){                              // Ddeselect Gear or Logic
                StartCoroutine(waitDeselectObject(true));
                b_MobileGrap = false;
            }
            #endregion
        }

        //--> Mobile Case : Detect object to drag 
        private void CheckObjectWithTag_Mobile(bool b_PuzzleRefPosition, bool b_Wait, Camera objCamera)
        {
            #region
                //prevent bug
                if (Input.touchCount == 0 && b_ValidationButtonPressed)
                {
                    fingerNum = 0;
                    b_ValidationButtonPressed = false;
                    //DeselectObject(true);
                    StartCoroutine(waitDeselectObject(true));
                }

            if (dragAndDropMode == 3){
                Ray ray = objCamera.ScreenPointToRay(new Vector3(s_Reticule.transform.position.x, s_Reticule.transform.position.y, 0));

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
                {
                    if (GearsLogicsInTheList((hit.transform.gameObject)) &&
                        hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                       hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject"/* &&
                        !AP_GlobalPuzzleManager_Pc.instance.currentPuzzleWithNoFocus.b_PuzzleIsSolved*/)
                    {
                        if (dragAndDropMode == 3 && !btn_GrabObjectMobile.activeSelf) // Activate Icon btn_GrabObjectMobile
                            btn_GrabObjectMobile.SetActive(true);

                        if (currentSelectedGameObject != hit.transform.gameObject)
                            currentSelectedGameObject = hit.transform.gameObject;
                    }
                    else
                    {
                        if (dragAndDropMode == 3 && btn_GrabObjectMobile.activeSelf) // Activate Icon btn_GrabObjectMobile
                            btn_GrabObjectMobile.SetActive(false);
                    }
                }
                else{
                    if (dragAndDropMode == 3 && btn_GrabObjectMobile.activeSelf) // Activate Icon btn_GrabObjectMobile
                        btn_GrabObjectMobile.SetActive(false);
                }
            }
           


                for (int i = 0; i < Input.touchCount; ++i)
                {
                    if (dragAndDropMode == 0)   // Mode Focus or Free Reticule
                    {
                        
                        Ray ray = objCamera.ScreenPointToRay(Input.GetTouch(i).position);
                        RaycastHit hit;
                        #region
                        if (Physics.Raycast(ray, out hit, raycastDistance, myLayer))
                        {

                        if (GearsLogicsInTheList((hit.transform.gameObject)) &&
                            hit.transform.GetComponent<AP_CheckTag_Pc>() &&
                            hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                            !b_ValidationButtonPressed)
                        {
                               // if (dragAndDropMode == 3 && !btn_GrabObjectMobile.activeSelf) // Activate Icon btn_GrabObjectMobile
                                 //   btn_GrabObjectMobile.SetActive(true);
                            
                                if (Input.GetTouch(i).phase == TouchPhase.Began)
                                {
                                    //Debug.Log("Here 2");
                                    // Debug.Log("Joystick Validation is pressed 2");
                                    if (Input.GetKeyDown(AP_GlobalPuzzleManager_Pc.instance.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0)))
                                    {
                                        b_ValidationButtonPressed = true;
                                        if (a_TakeObject) a_TakeObject.Play();

                                        //if (dragAndDropMode == 3) // Reticule become white
                                          //  AP_GlobalPuzzleManager_Pc.instance.reticule.GetComponent<AP_Reticule>().callMethodsListReticuleSelected();

                                        fingerNum = i;

                                    }

                                    puzzleObjectIsDetected = true;

                                    //-> An object is selected. 
                                    if (b_ValidationButtonPressed &&
                                        hit.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleObject" &&
                                        !hit.transform.GetComponent<Rigidbody>().isKinematic)
                                    {
                                        //Debug.Log("Here 3");
                                        if (currentSelectedGameObject != hit.transform.gameObject)
                                        {
                                            currentTargetObjectPosition = hit.transform.gameObject.transform.position + new Vector3(0, .1f, 0);
                                            currentSelectedGameObject = hit.transform.gameObject;

                                            //if (dragAndDropMode == 3)                           // Save the parent. Used when object is deselected
                                              //  objToDragCurrentParent = currentSelectedGameObject.transform.parent;

                                            Ap_IsKinematic(hit.transform, false, false, true);

                                        }
                                    }
                                }
                            }


                        }
                    #endregion
                    }

                if (dragAndDropMode == 0){  // Mode Focus
                    if (fingerNum == i)
                    {
                        Vector3 temp = Input.GetTouch(i).position;
                        temp.z = distanceFromTheCamera;
                        //-> Follow object
                        if (b_ValidationButtonPressed)
                        {
                            currentTargetObjectPosition = objCamera.ScreenToWorldPoint(temp);
                        }

                        //-> Follow object
                        if (Input.GetTouch(i).phase == TouchPhase.Moved && b_ValidationButtonPressed)
                        {
                            Ray ray2 = objCamera.ScreenPointToRay(Input.GetTouch(i).position);
                            RaycastHit hit3;
                            if (Physics.Raycast(ray2, out hit3, raycastDistance, myLayer))
                            {
                                currentSelectedGameObject.transform.position = hit3.point;
                            }
                        }

                        if (Input.GetTouch(i).phase == TouchPhase.Ended)
                        {
                            fingerNum = 0;
                            b_ValidationButtonPressed = false;
                            //DeselectObject(true);
                            StartCoroutine(waitDeselectObject(true));
                        }  
                }
                    



                        //if (fingerNum == i)
                        //{
                    Ray ray = objCamera.ScreenPointToRay(Input.GetTouch(i).position);
                        RaycastHit hit2;
                        if (Physics.Raycast(ray, out hit2, raycastDistance, myLayer2))
                        {

                            if (hit2.transform.GetComponent<AP_CheckTag_Pc>() && 
                                hit2.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition")
                            {
                                if (dragAndDropMode == 0)
                                {  // Mode Focus
                                    if (hit2.transform.gameObject.GetComponent<SpriteRenderer>() && HandInTheList((hit2.transform.gameObject)))
                                    {

                                        SpriteRenderer _currentHit = hit2.transform.gameObject.GetComponent<SpriteRenderer>();

                                        if (_currentHit.color.a == 0 && currentSelectedGameObject != null)
                                        {
                                            //Debug.Log("Ray");
                                            _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                            currentSelectedPuzzlePosition = hit2.transform.gameObject;
                                        }
                                        b_PuzzleRefPosition = true;
                                    }
                                }
                            }
                            else
                            {
                                b_PuzzleRefPosition = false;
                            }
                        }
                    }
                }

            if (dragAndDropMode == 3)   // Mode Reticule
            {
                Ray ray = objCamera.ScreenPointToRay(new Vector3(s_Reticule.transform.position.x, s_Reticule.transform.position.y, 0));
                RaycastHit hit2;
                if (Physics.Raycast(ray, out hit2, raycastDistance, myLayer2))
                {

                    if (hit2.transform.GetComponent<AP_CheckTag_Pc>() &&
                        hit2.transform.GetComponent<AP_CheckTag_Pc>()._Tag == "PuzzleRefPosition")
                    {
                            if (hit2.transform.gameObject.GetComponent<SpriteRenderer>() && HandInTheList((hit2.transform.gameObject)))
                            {
                                SpriteRenderer _currentHit = hit2.transform.gameObject.GetComponent<SpriteRenderer>();

                                if (_currentHit.color.a == 0 && currentSelectedGameObject != null)
                                {
                                    //Debug.Log("Ray");
                                    _currentHit.color = new Color(_currentHit.color.r, _currentHit.color.g, _currentHit.color.b, 0.5f);
                                    currentSelectedPuzzlePosition = hit2.transform.gameObject;
                                }
                                b_PuzzleRefPosition = true;
                            }
                    }
                    else
                    {
                        b_PuzzleRefPosition = false;
                    }
                }
            }


            saveTheLastSelectedPuzzlePosition(b_PuzzleRefPosition);
            initSelectedPuzzlePositionSPriteColor();
            #endregion
        }

        //-> Save the last Selectected Puzzle Position
        private void saveTheLastSelectedPuzzlePosition(bool b_PuzzleRefPosition)
        {
            #region
            if (!b_PuzzleRefPosition)
            {
                lastSelectedPuzzlePosition = currentSelectedPuzzlePosition;
                currentSelectedPuzzlePosition = null;
            }
            #endregion
        }

        //-> Init the sprite alpha for the last Selected Puzzle position
        public void initSelectedPuzzlePositionSPriteColor()
        {
            #region
            if (currentSelectedPuzzlePosition == null && lastSelectedPuzzlePosition)
            {
                lastSelectedPuzzlePosition.GetComponent<SpriteRenderer>().color =
                    new Color(lastSelectedPuzzlePosition.GetComponent<SpriteRenderer>().color.r,
                              lastSelectedPuzzlePosition.GetComponent<SpriteRenderer>().color.g,
                              lastSelectedPuzzlePosition.GetComponent<SpriteRenderer>().color.b, 0f);
            }


            if (dragAndDropMode == 2 &&
               AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent &&
               !AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectAxis)
            {
                foreach (SpriteRenderer rend in listOfSelectedPuzzlePosition)
                {

                    rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0f);

                }
            }


            foreach (SpriteRenderer rend in listOfSelectedPuzzlePosition)
            {
                if (currentSelectedPuzzlePosition != null &&
                   rend.gameObject != currentSelectedPuzzlePosition)
                {
                    rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0f);
                }
            }
            #endregion
        }

        // init all the sprite when puzzle is solved
        public void initAllSpriteWhenPuzzleIsSolved()
        {
            #region
            foreach (SpriteRenderer rend in listOfSelectedPuzzlePosition)
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0f);

            if (dragAndDropMode == 3 && btn_GrabObjectMobile.activeSelf) // Activate Icon btn_GrabObjectMobile
                btn_GrabObjectMobile.SetActive(false);
            #endregion
        }

        private Vector3 returnNewPosition(Vector3 pos)
        {
            #region
            Vector3 result = new Vector3(pos.x, .1f, pos.z);
            return result;
            #endregion
        }

        public GameObject returnCurrentSelectedObject(){return currentSelectedGameObject;}

        public void defaultColor() { ImageObjDetected.color = DefaultColor; }

        public void InitListOfHands(List<GameObject> listFromThePuzzle)
        {
            foreach (GameObject obj in listFromThePuzzle)
                listOfHandsObj.Add(obj);
        }


        bool HandInTheList(GameObject objToTest)
        {
            foreach (GameObject obj in listOfHandsObj)
            {
                if (obj == objToTest)
                    return true;
            }
            return false;
        }

        public void InitListOfGearsLogics(List<GameObject> listFromThePuzzle)
        {
            foreach (GameObject obj in listFromThePuzzle)
                listOfGearsLogicsObj.Add(obj);
        }

         
        bool GearsLogicsInTheList(GameObject objToTest)
        {
            foreach (GameObject obj in listOfGearsLogicsObj)
            {
                if (obj == objToTest)
                    return true;
            }
            return false;
        }

        public void InitHandsWhenThePlayerLeavesPuzzleFocusMode()
        {
            #region
            foreach (GameObject obj in listOfHandsObj)
            {
                SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
                objRenderer.color = new Color(objRenderer.color.r, objRenderer.color.g, objRenderer.color.b, 0f);
            }
            #endregion
        }
    }

}
