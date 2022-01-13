using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] public GameObject Background;
    [SerializeField] public GameObject[] Letters;
    [SerializeField] Camera[] cameras;
     
    // Start is called before the first frame update

    private void Start()
    {
         
        HideNotes();
    }

    public void ShowNote(int num) 
    {
        Background.SetActive(true);
        Letters[num].SetActive(true);
        Time.timeScale = 0;
        
    }


    public void HideNotes()
    {
        Background.SetActive(false);
        Time.timeScale = 1;
        CinemachineBrain.SoloCamera = null;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        foreach (GameObject gm in Letters) 
        {
            gm.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray;
            
            try 
            {ray = Camera.main.ScreenPointToRay(Input.mousePosition); } 
            catch { print("EROR"); return;}
           
            LayerMask mask = LayerMask.GetMask("Note");

            if (Physics.Raycast(ray, out hit, 100.0f,mask))
            {
                if (hit.transform.tag == "Note")
                {
                    ShowNote(0);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }

}
