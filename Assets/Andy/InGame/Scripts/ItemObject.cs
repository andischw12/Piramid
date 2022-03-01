using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DiasGames.ThirdPersonSystem;

public class ItemObject : MonoBehaviour
{
    [SerializeField] GameObject LookAtcam;
    [SerializeField] GameObject lookInsideIcon, actionIcon;
    [SerializeField] bool lookInsideIconB =true, actionIconB=true, outline,isPuzzle;
    [SerializeField]public  float timeBetween;
    [SerializeField] GameObject outlinerGameObject;
    public UnityEvent myUnityEvent;
    //[SerializeField] BoxCollider ActionColiider;
    // Start is called before the first frame update



    private void Start()
    {
        if(lookInsideIcon)
            LookAtcam.SetActive(false);
        if(lookInsideIcon)
             lookInsideIcon.SetActive(false);
        if(actionIcon)
        actionIcon.SetActive(false);
        if(outline)
        outlinerGameObject.GetComponent<Outline>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
             
            if(actionIconB)
                actionIcon.SetActive(true);
            
            if (outline)
                 
                outlinerGameObject.GetComponent<Outline>().enabled = true;
        }
    }

 

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Bye");
            if (lookInsideIconB)
                lookInsideIcon.SetActive(false);
            if (actionIconB)
                actionIcon.SetActive(false);
            if (outline)
                outlinerGameObject.GetComponent<Outline>().enabled = false;
        }
    }
    public void OnActionIconClick()
    {
        
        myUnityEvent.Invoke();
        print("action button was click");
    }

    public void OnLookAtIconClick() 
    {
        LookAtcam.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonSystem>().enabled = false;
        print("look button was click");
    }

    public void TurnLookAtCamOff() 
    {
        LookAtcam.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonSystem>().enabled = true;
        //actionIcon.SetActive(false);
    }
}
