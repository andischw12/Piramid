using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 [ExecuteInEditMode]
public class changes : MonoBehaviour
{
     [SerializeField] float size;
    // Start is called before the first frame update
    void Start()
    {
        /*
        Transform[] allObjects = GetComponentsInChildren<Transform>();
        foreach (Transform T in allObjects)
            if (T.GetComponent<MeshRenderer>() != null)
                T.transform.parent = this.transform;

        foreach (Transform T in allObjects)
            if (T.GetComponent<MeshRenderer>() != null)
                T.transform.parent = trash.transform;
       */
        SetLightMapSize(FindObjectsOfType<GameObject>(), size);
    }

      

    void SetLightMapSize(GameObject[] arr,float size) 
    {
        foreach(GameObject GM in arr) 
        {
            if (GM.GetComponent<MeshRenderer>()!= null)
            {
                GM.GetComponent<MeshRenderer>().scaleInLightmap = size;
            }
        }
    }


    
    private void GroupByname(GameObject[] arr)
    {
        int i=0, j=0;
         
        string[] groupNames = {""};
        while (i < arr.Length) 
        {
            
            if (!isInList(arr, arr[i].name)) 
            {
                
            }
            i++;
        }
    }

    bool isInList(GameObject[] arr, string s) 
    {
        for (int i = 0; i < arr.Length; i++)
            if (arr[i].name[0].Equals(s))
                return true;
        return false;
    }
    
}
