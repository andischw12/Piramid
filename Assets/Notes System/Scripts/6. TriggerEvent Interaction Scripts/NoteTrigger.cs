using UnityEngine;

namespace NoteSystem
{
    public class NoteTrigger : MonoBehaviour
    {
        [Header("Keypad Object")]
        [SerializeField] private NoteController myNote = null;

        [Header("UI Prompt")]
        public GameObject interactPrompt;

        private bool canUse;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canUse = true;
                interactPrompt.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canUse = false;
                interactPrompt.SetActive(false);
            }
        }

        private void Update()
        {
            if (canUse)
            {
                if (Input.GetKeyDown(NoteInputManager.instance.triggerInteractKey))
                {
                    myNote.DisplayNotes();
                    //NoteDisableManager.instance.DisablePlayer(true);
                }
            }
        }
    }
}
