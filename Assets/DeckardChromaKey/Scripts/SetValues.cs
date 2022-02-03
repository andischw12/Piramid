using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValues : MonoBehaviour
{
    public Material chromaMaterial;
    public float val;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChromaValue(float _float)
    {
        if(chromaMaterial != null) {
            chromaMaterial.SetFloat("_Threshold" , _float);
        }
    }
}
