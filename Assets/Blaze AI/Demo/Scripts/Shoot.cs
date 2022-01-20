using UnityEngine;
using System.Collections;

namespace BlazeAIDemo
{
    public class Shoot : MonoBehaviour
    {
        [HideInInspector] public LineRenderer lr;
        [HideInInspector] public BlazeAI blaze;

        public Transform gun;
        public Material aimMaterial;
        public Material shootMaterial;
        public AudioSource gunShot;

        bool turnOff = true;

        void Start() 
        {
            blaze = GetComponent<BlazeAI>(); 
            lr = GetComponent<LineRenderer>();

            lr.enabled = false;
            lr.SetWidth(0.05f, 0.03f);   
        }

        void Update()
        {
            if (turnOff) lr.enabled = false;
            else {
                lr.enabled = true;
                lr.SetPosition(0, gun.position + new Vector3(0f, 0.2f, 0f));
                lr.SetPosition(1, blaze.enemyToAttack.transform.position + new Vector3(0f, 1.2f, 0f));
            }

            if (!blaze.isAttacking) turnOff = true;
        }

        public void ShotFrame()
        {
            lr.material = shootMaterial;
            gunShot.Play();
            StartCoroutine(TurnRendererOff());
        }

        public void Aiming()
        {
            turnOff = false;
            lr.material = aimMaterial;
        }

        IEnumerator TurnRendererOff()
        {
            yield return new WaitForSeconds(0.3f);
            turnOff = true;
        }
    }
}

