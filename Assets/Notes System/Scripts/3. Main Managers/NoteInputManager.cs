using UnityEngine;

namespace NoteSystem
{
    public class NoteInputManager : MonoBehaviour
    {
        [Header("Note Pickup Input")]
        public KeyCode interactKey;
        public KeyCode closeKey;
        public KeyCode reverseKey;

        [Header("Note Extra Feature Inputs")]
        public KeyCode playAudioKey;

        [Header("Trigger Inputs")]
        public KeyCode triggerInteractKey;

        public static NoteInputManager instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }
    }
}
