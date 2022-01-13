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
     
    // Start is called before the first frame update

    private void Start()
    {
         
        HideNotes();
    }

    public void ShowNote(int num) 
    {
        if (UserNotificationManager.instance.Notifications[2].counter < 1)
            UserNotificationManager.instance.ShowNotification(2);
        Background.SetActive(true);
        Letters[num].SetActive(true);
        GameManager.instance.AcceptPlayerInput = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

    }


    public void HideNotes()
    {
        Background.SetActive(false);
        GameManager.instance.AcceptPlayerInput = true;
        CinemachineBrain.SoloCamera = null;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        try { currentNote.HostingItem.SecondaryAction.Invoke(); } catch { }//invoke the closing action of hosting item object
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
                    
                    currentNote = hit.transform.GetComponent<NoteItem>();
                    ShowNote(currentNote.NoteNumber);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }

}
