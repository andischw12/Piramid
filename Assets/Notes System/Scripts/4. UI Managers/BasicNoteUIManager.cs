using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class BasicNoteUIManager : MonoBehaviour
    {
        [Header("Audio Prompt UI")]
        public GameObject audioPromptUI = null;

        [Header("Page Buttons UI")]
        public GameObject pageButtons;
        public GameObject nextButton;
        public GameObject previousButton;

        [Header("Default Note UI")]
        public GameObject basicNoteMainUI = null;
        public Image basicNotePageUI = null;

        [HideInInspector] public BasicNoteController noteController;

        [Header("Help Panel Visibility")]
        [SerializeField] private GameObject examineHelpUI = null;
        [SerializeField] private bool showHelp = false;

        public static BasicNoteUIManager instance;

        private void Awake()
        {
            if (instance == null) { instance = this; }
        }

        public void ShowPageButtons(bool shouldShow)
        {
            if (shouldShow)
            {
                pageButtons.SetActive(true);
            }
            else
            {
                pageButtons.SetActive(false);
            }
        }

        public void ShowAudioPrompt()
        {
            audioPromptUI.SetActive(true);
        }

        public void PlayPauseAudio()
        {
            noteController.NoteReadingAudio();
        }

        public void RepeatAudio()
        {
            noteController.RepeatReadingAudio();
        }

        public void CloseButton()
        {
            noteController.CloseNote();
        }

        public void NextPage()
        {
            noteController.NextPage();
        }

        public void BackPage()
        {
            noteController.BackPage();
        }

        private void Start()
        {
            if (showHelp)
            {
                examineHelpUI.SetActive(true);
            }
            else
            {
                examineHelpUI.SetActive(false);
            }
        }
    }
}
