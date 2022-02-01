using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvincible : PowerUp
{
    public Shader InvincibleShader;
    public MeshRenderer[] matArr;
    public SkinnedMeshRenderer[] matArr2;

    private void OnAwake()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        matArr = Player.GetComponentsInChildren<MeshRenderer>();
    }
     
    public override IEnumerator UseProcess()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        matArr = Player.GetComponentsInChildren<MeshRenderer>();
        matArr2 = Player.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (MeshRenderer M in matArr) 
        {
            foreach (var mat in M.materials)
            {
                mat.SetFloat("_Mode", 3);

                mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f));
                mat.renderQueue = 3000;
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            foreach (Material m in M.materials) 
            {
                m.color = new Color(m.color.r, m.color.g, m.color.b, 0.35f);
            }
        }

        foreach (SkinnedMeshRenderer M in matArr2)
        {

            foreach (var mat in M.materials)
            {
                mat.SetFloat("_Mode", 3);

                mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f));
                mat.renderQueue = 3000;
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            foreach (Material m in M.materials)
            {
                m.color = new Color(m.color.r, m.color.g, m.color.b, 0.35f);
            }
        }
        yield return new WaitForSeconds(1);
        
    }
}
