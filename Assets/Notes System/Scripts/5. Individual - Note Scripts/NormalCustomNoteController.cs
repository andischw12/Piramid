using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class NormalCustomNoteController : MonoBehaviour
    {
        [Header("Note Settings")]
        public bool isReadable;

        [Header("Note Page Settings")]
        [SerializeField] private Sprite pageImage = null;
        [SerializeField] private Vector2 pageScale = new Vector2(900, 900);
        private int pageNum = 0;

        [Header("Note Text")]
        [SerializeField] private bool hasMultPages = false;
        [Space(5)][TextArea(4, 8)] public string[] noteText;

        [Header("Font Settings")]
        [SerializeField] private Vector2 noteTextAreaScale = new Vector2(495, 795);
        [Space(5)][SerializeField] private int textSize = 25;
        [SerializeField] private Font fontType = null;
        [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
        [SerializeField] private Color fontColor = Color.black;

        private NotesRaycast notesRaycastScript;
        private BoxCollider boxCollider;
        private bool canClick;

        [Header("Allow playable note audio?")]
        [SerializeField] private bool allowAudioPlayback = false;

        [Header("Audio Clip Settings")]
        [SerializeField] private bool playOnOpen = false;
        [SerializeField] private string noteAudio = "AudioClip";
        [SerializeField] private string noteFlipAudio = "NoteOpen";
        private bool audioPlaying;

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private bool isNoteTrigger = false;
        [SerializeField] private NoteTrigger triggerObject = null;

        private void Start()
        {
            canClick = false;
            notesRaycastScript = Camera.main.GetComponent<NotesRaycast>();
            boxCollider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (canClick)
            {
                if (Input.GetKeyDown(NoteInputManager.instance.closeKey))
                {
                    CloseNote();
                }
            }
        }    

        public void ShowNote()
        {
            CustomNoteUIManager.instance.noteController = gameObject.GetComponent<NormalCustomNoteController>();
            CustomNoteUIManager noteController = CustomNoteUIManager.instance;
            StartCoroutine(WaitTime());

            if (pageNum <= 1)
            {
                CustomNoteUIManager.instance.previousButton.SetActive(false);
            }

            if (hasMultPages)
            {
                noteController.ShowPageButtons(true);
            }

            noteController.customNoteTextUI.text = noteText[pageNum];
            noteController.customNoteTextUI.fontSize = textSize;
            noteController.customNoteTextUI.fontStyle = fontStyle;
            noteController.customNoteTextUI.font = fontType;
            noteController.customNoteTextUI.color = fontColor;
            noteController.customNoteTextUI.rectTransform.sizeDelta = noteTextAreaScale;
            noteController.customNotePageUI.sprite = pageImage;
            noteController.customNotePageUI.rectTransform.sizeDelta = pageScale;

            NoteAudioManager.instance.Play(noteFlipAudio);
            noteController.customNoteMainUI.SetActive(true);
            NoteDisableManager.instance.DisablePlayer(true);
            notesRaycastScript.enabled = false;
            boxCollider.enabled = false;

            if (allowAudioPlayback)
            {
                noteController.audioPromptUI.SetActive(true);
                if (playOnOpen)
                {
                    PlayAudio();
                }
            }

            if (isNoteTrigger)
            {
                triggerObject.interactPrompt.SetActive(false);
                triggerObject.enabled = false;
            }
        }

        void ResetNote()
        {
            CustomNoteUIManager.instance.previousButton.SetActive(false);
            CustomNoteUIManager.instance.nextButton.SetActive(true);
            pageNum = 0;
        }

        public void CloseNote()
        {
            CustomNoteUIManager.instance.customNoteMainUI.SetActive(false);
            NoteDisableManager.instance.DisablePlayer(false);
            notesRaycastScript.enabled = true;
            boxCollider.enabled = true;
            ResetNote();
            enabled = false;

            if (hasMultPages)
            {
                CustomNoteUIManager.instance.ShowPageButtons(false);
            }

            if (playOnOpen || allowAudioPlayback)
            {
                StopAudio();
            }

            if (isNoteTrigger)
            {
                triggerObject.interactPrompt.SetActive(true);
                triggerObject.enabled = true;
            }
        }

        public void NextPage()
        {
            if (pageNum < noteText.Length - 1)
            {
                pageNum++;
                CustomNoteUIManager.instance.customNoteTextUI.text = noteText[pageNum];
                EnabledButtons();
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum >= noteText.Length - 1)
                {
                    CustomNoteUIManager.instance.nextButton.SetActive(false);
                }
            }
        }

        void EnabledButtons()
        {
            CustomNoteUIManager.instance.previousButton.SetActive(true);
            CustomNoteUIManager.instance.nextButton.SetActive(true);
        }

        public void BackPage()
        {
            if (pageNum >= 1)
            {
                pageNum--;
                CustomNoteUIManager.instance.customNoteTextUI.text = noteText[pageNum];
                EnabledButtons();
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum < 1)
                {
                    CustomNoteUIManager.instance.previousButton.SetActive(false);
                }
            }
        }

        IEnumerator WaitTime()
        {
            const float WaitTimer = 0.1f;
            yield return new WaitForSeconds(WaitTimer);
            canClick = true;
        }

        public void NoteReadingAudio()
        {
            if (!audioPlaying)
            {
                PlayAudio();
            }
            else
            {
                PauseAudio();
            }
        }

        public void RepeatReadingAudio()
        {
            StopAudio();
            PlayAudio();
        }

        public void PlayAudio()
        {
            NoteAudioManager.instance.Play(noteAudio);
            audioPlaying = true;
        }

        public void StopAudio()
        {
            NoteAudioManager.instance.StopPlaying(noteAudio);
            audioPlaying = false;
        }

        public void PauseAudio()
        {
            NoteAudioManager.instance.PausePlaying(noteAudio);
            audioPlaying = false;
        }
    }
}
