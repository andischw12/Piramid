using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DiasGames.ThirdPersonSystem;

public class InteractbleItem : MonoBehaviour
{
    enum InteracticState {Off,OnCollisonEnter,MouseClick};
    [SerializeField] GameObject ItemCamera, InfoGraphic;
    [SerializeField] bool OutLine,_ActionOnCollisonExit;//,ActionOnclick,CameraOnClick;
    [SerializeField] InteracticState CameraState,ActionState,InfoGraphicState;
    [SerializeField] public float timeBetweenCamAndAcation;
    public UnityEvent MainAction,ActionOnExit;
    
    bool isColliding;




    private void Start()
    {
        if (OutLine) 
        {
            gameObject.AddComponent<Outline>();
            GetComponent<Outline>().OutlineColor = new Color32(255,145,0,255);  
            GetComponent<Outline>().OutlineWidth = 5;
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
            GetComponent<Outline>().enabled = false;
        }
        InfoGraphic.SetActive(false);

           
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (OutLine)
                GetComponent<Outline>().enabled = true;
            if(InfoGraphicState == InteracticState.OnCollisonEnter)
                InfoGraphic.SetActive(true);
            if (CameraState == InteracticState.OnCollisonEnter && ActionState == InteracticState.OnCollisonEnter)
                StartCoroutine(CamAndAction());
            else if (CameraState == InteracticState.OnCollisonEnter)
                ItemCamera.SetActive(true);
            else if (ActionState == InteracticState.OnCollisonEnter)
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
            isColliding = false;
            if(_ActionOnCollisonExit)
                ActionOnExit.Invoke();

        }
    }



    public void TurnItemCamOn()
    {
        FindObjectOfType<ThirdPersonSystem>().GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ItemCamera.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonSystem>().enabled = false;
        print("look button was click");
    }

    public void TurnItemCamOff()
    {
        FindObjectOfType<ThirdPersonSystem>().GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
        ItemCamera.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonSystem>().enabled = true;
        //actionIcon.SetActive(false);
    }


    IEnumerator CamAndAction()
    {
        //if(CameraState == InteracticState.MouseClick)
            TurnItemCamOn();
        yield return new WaitForSeconds(timeBetweenCamAndAcation);
       // if(ActionState == InteracticState.MouseClick)
            MainAction.Invoke();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.GetMouseButtonDown(0))
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



        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
            TurnItemCamOff();
    }
}
