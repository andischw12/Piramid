using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class BasicReverseNoteUIManager : MonoBehaviour
    {
        [Header("Audio Prompt UI")]
        public GameObject audioPromptUI = null;

        [Header("Page Buttons UI")]
        public GameObject pageButtons = null;
        public GameObject nextButton = null;
        public GameObject previousButton = null;

        [Header("Reverse Note Main UI's")]
        public GameObject reverseNoteMainUI = null;
        public Image reverseNotePageUI = null;

        [Header("Reverse Note Text UI's")]
        public GameObject reverseNoteTextPanelUI = null;
        public Image reverseNoteTextImage = null;
        public Text reverseNoteTextUI = null;

        [HideInInspector] public BasicReverseNoteController noteController;

        [Header("Help Panel Visibility")]
        [SerializeField] private GameObject examineHelpUI = null;
        [SerializeField] private bool showHelp = false;

        public static BasicReverseNoteUIManager instance;

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

        public void ReverseNoteButton()
        {
            noteController.ReverseNoteAction();
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
