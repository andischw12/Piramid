using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class QualityControl : MonoBehaviour
{
    [SerializeField] MeshRenderer[] AllMeshes;
    [SerializeField] SkinnedMeshRenderer[] AllSkinedMeshes;

    [SerializeField] Shader LowShader;
    [SerializeField] Shader HighShader;
    [SerializeField]  Light CharactherLight;

    // Start is called before the first frame update
    void Start()
    {

        AllMeshes = FindObjectsOfType<MeshRenderer>();
        AllSkinedMeshes = FindObjectsOfType<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Camera.main.GetComponent<BeautifyEffect.Beautify>().enabled = true;
            Camera.main.GetComponent<FXAA>().enabled = true;
            ChangeShader(HighShader);
            CharactherLight.enabled = true;

        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CharactherLight.enabled = false;

            Camera.main.GetComponent<BeautifyEffect.Beautify>().enabled = false;
            Camera.main.GetComponent<FXAA>().enabled = false;
            ChangeShader(LowShader);
        }
         
    }


    void ChangeShader(Shader s) 
    {
        foreach (MeshRenderer m in AllMeshes)
        {
            if(m.transform.tag!="Shadow")
            m.sharedMaterial.shader = Shader.Find(s.name);
        }

        foreach (SkinnedMeshRenderer m in AllSkinedMeshes)
        {
            m.sharedMaterial.shader = Shader.Find(s.name);
        }
    }
}
