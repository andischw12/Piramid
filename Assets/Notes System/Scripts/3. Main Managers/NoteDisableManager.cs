using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using DiasGames.ThirdPersonSystem;

namespace NoteSystem
{
    public class NoteDisableManager : MonoBehaviour
    {
        public static NoteDisableManager instance;

        [SerializeField] private ThirdPersonSystem player = null;
        [SerializeField] private Image crosshair = null; 

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void DisablePlayer(bool disable)
        {
            if (disable)
            {
                /*
                player.enabled = false;
                crosshair.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;*/
            }

            else
            {
                /*
                player.enabled = true;
                crosshair.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;*/
            }
        }
    }
}
