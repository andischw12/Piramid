using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class ReverseCustomNoteUIManager : MonoBehaviour
    {
        [Header("Audio Prompt UI")]
        public GameObject audioPromptUI = null;

        [Header("Page Buttons UI")]
        public GameObject pageButtons = null;
        public GameObject nextButton = null;
        public GameObject previousButton = null;

        [Header("Main Note Settings")]
        public GameObject customReverseMainNoteUI = null;
        public Image customReverseNotePageUI = null;
        public Text customReverseNoteTextUI = null;

        [Header("Custom Reverse Pop-out Settings")]
        public GameObject customReverseNoteTextPanelBG = null;
        public Image customReverseNoteTextImage = null;
        public Text customReverseFlipNoteTextUI = null;

        [HideInInspector] public ReverseCustomNoteController noteController;

        [Header("Help Panel Visibility")]
        [SerializeField] private GameObject examineHelpUI = null;
        [SerializeField] private bool showHelp = false;

        public static ReverseCustomNoteUIManager instance;

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
