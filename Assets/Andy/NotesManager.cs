using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] public GameObject Background;
    [SerializeField] public GameObject[] Letters;
    [SerializeField] Camera[] cameras;
    NoteItem currentNote;
    [SerializeField] LayerMask mask;

    // Start is called before the first frame update

    private void Start()
    {
         
        HideNotes();
    }

    public void ShowNote(int num) 
    {
        Background.SetActive(true);
        Letters[num].SetActive(true);
        GameManager.instance.AcceptPlayerInput = false;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

    }


    public void HideNotes()
    {
        Background.SetActive(false);
        foreach (GameObject gm in Letters) 
        {
            gm.gameObject.SetActive(false);
        }
        try { currentNote.HostingItem.ExitFromObject(); }catch{ }
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
           
            //LayerMask mask = LayerMask.GetMask("Note");

            if (Physics.Raycast(ray, out hit, 100.0f,mask))
            {
                if (hit.transform.tag == "Note")
                {
                    
                    currentNote = hit.transform.GetComponent<NoteItem>();
                    ShowNote(currentNote.NoteNumber);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }

}
