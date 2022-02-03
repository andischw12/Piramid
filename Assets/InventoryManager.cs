using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum InventoryItemType {None,GoldenKey,SilverKey,BronzeKey};
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryItemUI[] GuiInventoryArr;
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
        GuiInventoryArr = GetComponentsInChildren<InventoryItemUI>();
        HideAll();
         
    }
    public void LoadInventory() 
    {

    }

    void HideAll() 
    {
        foreach (InventoryItemUI gm in GuiInventoryArr)
        {
             gm.gameObject.SetActive(false);
        }
        ShowOrHideInventory();

    }

    public void AddItem(InventoryItemType i) 
    {

        
        foreach(InventoryItemUI gm in GuiInventoryArr) 
        {
            if (gm.type == i)
                gm.gameObject.SetActive(true);
        }
        ShowOrHideInventory();
    }


    public void RemoveItem(InventoryItemType i) 
    {
        
        foreach (InventoryItemUI gm in GuiInventoryArr)
        {
            if (gm.type == i)
                gm.gameObject.SetActive(false);
        }
        ShowOrHideInventory();
    }

    public bool CheckIfGotItem(InventoryItemType i) 
    {
        foreach (InventoryItemUI gm in GuiInventoryArr)
        {
            if (gm.type == i && gm.isActiveAndEnabled)
                return true;
        }
        return false;
    }
    //Show or Hide Invetory 
    void ShowOrHideInventory() 
    {
        int count = 0;
        foreach (InventoryItemUI gm in GuiInventoryArr)
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
                if (hit.transform.tag == "InventoryItem" && Vector3.Distance(PlayerManager.instance.transform.position,hit.transform.position)<2)
                {

                    AddItem(hit.transform.GetComponent<InventoryPickUpItem>().type);
                    hit.transform.GetComponent<InventoryPickUpItem>().PickUp();
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }


}
