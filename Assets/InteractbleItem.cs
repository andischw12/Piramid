using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DiasGames.ThirdPersonSystem;
using Cinemachine;

public class InteractbleItem : MonoBehaviour
{
    enum InteracticState {Off,OnCollisonEnter,LeftMouseClick,OnCollisonExit,RightMouseClick};
    [SerializeField] GameObject ItemCamera, InfoGraphic;
    [SerializeField] bool OutLine;
    [SerializeField] InteracticState CameraState,MainActionState,InfoGraphicState,SecondaryActionState;
    [SerializeField] public float timeBetweenCamAndAcation;
    public UnityEvent MainAction,SecondaryAction;
    
    bool isColliding;




    private void Start()
    {
        if (OutLine) 
        {
            gameObject.AddComponent<Outline>();
            GetComponent<Outline>().OutlineColor = new Color32(255,145,0,150);  
            GetComponent<Outline>().OutlineWidth = 5;
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
            GetComponent<Outline>().enabled = false;
        }
        InfoGraphic.SetActive(false);
        ItemCamera.SetActive(false);
           
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
             
            if (OutLine)
                GetComponent<Outline>().enabled = true;
            if(InfoGraphicState == InteracticState.OnCollisonEnter)
                InfoGraphic.SetActive(true);
            if (CameraState == InteracticState.OnCollisonEnter && MainActionState == InteracticState.OnCollisonEnter)
                StartCoroutine(CamAndAction());
            else if (CameraState == InteracticState.OnCollisonEnter)
                ItemCamera.SetActive(true);
            else if (MainActionState == InteracticState.OnCollisonEnter)
                MainAction.Invoke();
            isColliding = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            
            if (OutLine)
                GetComponent<Outline>().enabled = false;
            InfoGraphic.SetActive(false);
            
            if(SecondaryActionState == InteracticState.OnCollisonExit)
                SecondaryAction.Invoke();
            isColliding = false;
        }
    }



    public void TurnItemCamOn()
    {
        if (OutLine)
            GetComponent<Outline>().enabled = false;
        FindObjectOfType<ThirdPersonSystem>().GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ItemCamera.SetActive(true);
        CinemachineBrain.SoloCamera = ItemCamera.GetComponent<CinemachineVirtualCamera>();
        //GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonSystem>().enabled = false;
        print("look button was click");
    }

    public void TurnItemCamOff()
    {
        if (isColliding && OutLine)
            GetComponent<Outline>().enabled = true;
        FindObjectOfType<ThirdPersonSystem>().GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        ItemCamera.SetActive(false);
        CinemachineBrain.SoloCamera = null;
       // GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonSystem>().enabled = true;
        //actionIcon.SetActive(false);
    }


    IEnumerator CamAndAction()
    {
        //if(CameraState == InteracticState.MouseClick)
            TurnItemCamOn();
        yield return new WaitForSeconds(timeBetweenCamAndAcation);
        if(MainActionState != InteracticState.Off)
            MainAction.Invoke();
    }


    private void FixedUpdate()
    {
         
            if (isColliding && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = LayerMask.GetMask("InteractableItem");

                if (Physics.Raycast(ray, out hit, 100.0f,mask))
                {
                    if (hit.transform.tag == "ItemIcon")
                    {
                        StartCoroutine(CamAndAction());
                    }
                    Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                }
            }




        if (isColliding && Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            TurnItemCamOff();
            if(SecondaryActionState == InteracticState.RightMouseClick)
                SecondaryAction.Invoke();
        }
            
    }
}
