using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseClick : MonoBehaviour
{
    public bool actionIcon, LookAtIcon;
    ItemObject itemObjectParant;
    // Start is called before the first frame update


    private void Start()
    {
        itemObjectParant = GetComponentInParent<ItemObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = LayerMask.GetMask("ItemIcon");
                if (Physics.Raycast(ray, out hit, 100.0f,mask))
                {
                    if (hit.transform.tag == "ItemIcon") 
                    {
                        StartCoroutine(CamAndAction(hit));
                       
                    }
                    Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                }
            }


            
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            if (LookAtIcon)
                itemObjectParant.TurnLookAtCamOff();
        }


    }


    IEnumerator CamAndAction(RaycastHit hit) 
    {
        if (hit.transform.GetComponentInParent<OnMouseClick>() != null && hit.transform.GetComponentInParent<OnMouseClick>().LookAtIcon)
            GetComponentInParent<ItemObject>().OnLookAtIconClick();
        yield return new  WaitForSeconds(itemObjectParant.timeBetween);
        if (hit.transform.GetComponentInParent<OnMouseClick>() != null && hit.transform.GetComponentInParent<OnMouseClick>().actionIcon)
            GetComponentInParent<ItemObject>().OnActionIconClick();
        
    }

     
    

}
