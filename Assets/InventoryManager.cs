using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum InventoryItems {None,GoldenKey,SilverKey,BronzeKey};
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryItem[] GuiInventoryArr;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GuiInventoryArr = GetComponentsInChildren<InventoryItem>();
        HideAll();
         
    }
    public void LoadInventory() 
    {

    }

    void HideAll() 
    {
        foreach (InventoryItem gm in GuiInventoryArr)
        {
             gm.gameObject.SetActive(false);
        }
        ShowOrHideInventory();

    }

    public void AddItem(InventoryItems i) 
    {

        
        foreach(InventoryItem gm in GuiInventoryArr) 
        {
            if (gm.item == i)
                gm.gameObject.SetActive(true);
        }
        ShowOrHideInventory();
    }


    public void RemoveItem(InventoryItems i) 
    {
        
        foreach (InventoryItem gm in GuiInventoryArr)
        {
            if (gm.item == i)
                gm.gameObject.SetActive(false);
        }
        ShowOrHideInventory();
    }

    public bool CheckIfGotItem(InventoryItems i) 
    {
        foreach (InventoryItem gm in GuiInventoryArr)
        {
            if (gm.item == i && gm.isActiveAndEnabled)
                return true;
        }
        return false;
    }
    //Show or Hide Invetory 
    void ShowOrHideInventory() 
    {
        int count = 0;
        foreach (InventoryItem gm in GuiInventoryArr)
        if(gm.gameObject.activeInHierarchy)
                count++;
        if (count == 0 && GetComponent<Animator>().GetBool("Show"))
            GetComponent<Animator>().SetBool("Show", false);
        else if(count>0 && !GetComponent<Animator>().GetBool("Show"))
            GetComponent<Animator>().SetBool("Show", true);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray;

            try
            { ray = Camera.main.ScreenPointToRay(Input.mousePosition); }
            catch { print("EROR"); return; }

            LayerMask mask = LayerMask.GetMask("InventoryItem");

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                if (hit.transform.tag == "InventoryItem")
                {

                    AddItem(hit.transform.GetComponent<InventoryItem>().item);
                    Destroy(hit.transform.gameObject);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }


}
