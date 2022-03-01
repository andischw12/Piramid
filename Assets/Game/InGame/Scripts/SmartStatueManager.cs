using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartStatueManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject LookAtCam;

    public void Action() 
    {
        StartCoroutine(ActionProcess());
    }

    IEnumerator ActionProcess() 
    {
        GameManager.instance.AcceptPlayerInput = false;
        SetKeyInPlace();
        yield return new WaitForSeconds(0.5f);
        
         
        if (LookAtCam!=null) 
        {
            print("hry");
            LookAtCam.SetActive(true);
        }
           
        MoveStatue();
        yield return new WaitForSeconds(7);
        if (LookAtCam != null)
            LookAtCam.gameObject.SetActive(false);
        GameManager.instance.AcceptPlayerInput = true;

    }





    void MoveStatue()
    {
        GetComponent<Animator>().SetTrigger("ActivateStatue");
    }

    void SetKeyInPlace() 
    {

    }



}
