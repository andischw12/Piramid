using System.Collections;
using UnityEngine;

namespace NoteSystem
{
    public class ReverseCustomNoteController : MonoBehaviour
    {
        [Header("Note Settings")]
        public bool isReadable;

        [Header("Note Page Settings")]
        [SerializeField] private Sprite pageImage = null;
        [SerializeField] private Vector2 pageScale = new Vector2(900, 900);

        [Header("Note Pages")]
        [SerializeField] private bool hasMultPages = false;
        [TextArea(4, 8)] public string[] noteReverseText;
        private int pageNum = 0;

        [Header("Main Note Font Settings")]
        [SerializeField] private Vector2 mainTextAreaScale = new Vector2(495, 795);
        [SerializeField] private int mainTextSize = 25;
        [SerializeField] private Font mainFontType = null;
        [SerializeField] private FontStyle mainFontStyle = FontStyle.Normal;
        [SerializeField] private Color mainFontColor = Color.black;

        [Header("Reverse Pop-out Settings")]
        [SerializeField] private Color noteTextBGColor = Color.white;

        [Header("Custom Reverse Pop-out Font Settings")]
        [SerializeField] private int flipTextSize = 25;
        [SerializeField] private Font flipFontType = null;
        [SerializeField] private FontStyle flipFontStyle = FontStyle.Normal;
        [SerializeField] private Vector2 flipTextAreaScale = new Vector2(1045, 300);
        [SerializeField] private Vector2 flipTextBGScale = new Vector2(1160, 300);
        [SerializeField] private Color flipFontColor = Color.black;

        private NotesRaycast notesRaycastScript;
        private BoxCollider boxCollider;
        private bool canReverse;
        private bool canClick;
        private bool isNoteActivate;

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
            ReverseCustomNoteUIManager.instance.noteController = gameObject.GetComponent<ReverseCustomNoteController>();
            ReverseCustomNoteUIManager noteController = ReverseCustomNoteUIManager.instance;
            StartCoroutine(WaitTime());

            if (pageNum <= 1)
            {
                ReverseCustomNoteUIManager.instance.previousButton.SetActive(false);
            }

            if (hasMultPages)
            {
                noteController.ShowPageButtons(true);
            }

            noteController.customReverseNoteTextUI.text = noteReverseText[pageNum];
            noteController.customReverseNoteTextUI.fontSize = mainTextSize;
            noteController.customReverseNoteTextUI.fontStyle = mainFontStyle;
            noteController.customReverseNoteTextUI.font = mainFontType;
            noteController.customReverseNoteTextUI.color = mainFontColor;
            noteController.customReverseNoteTextUI.rectTransform.sizeDelta = mainTextAreaScale;
            noteController.customReverseNotePageUI.sprite = pageImage;
            noteController.customReverseNotePageUI.rectTransform.sizeDelta = pageScale;
            noteController.customReverseNoteTextImage.color = noteTextBGColor;

            noteController.customReverseFlipNoteTextUI.rectTransform.sizeDelta = flipTextAreaScale;
            noteController.customReverseFlipNoteTextUI.text = noteReverseText[pageNum];
            noteController.customReverseFlipNoteTextUI.fontSize = flipTextSize;
            noteController.customReverseFlipNoteTextUI.fontStyle = flipFontStyle;
            noteController.customReverseFlipNoteTextUI.font = flipFontType;
            noteController.customReverseFlipNoteTextUI.color = flipFontColor;

            noteController.customReverseNoteTextImage.rectTransform.sizeDelta = flipTextBGScale;

            NoteAudioManager.instance.Play(noteFlipAudio);
            noteController.customReverseMainNoteUI.SetActive(true);
            NoteDisableManager.instance.DisablePlayer(true);
            notesRaycastScript.enabled = false;
            boxCollider.enabled = false;
            isNoteActivate = true;

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

        public void ReverseNoteAction()
        {
            if (isNoteActivate)
            {
                canReverse = !canReverse;

                if (canReverse)
                {
                    ReverseCustomNoteUIManager.instance.customReverseNoteTextPanelBG.SetActive(true);
                }
                else
                {
                    ReverseCustomNoteUIManager.instance.customReverseNoteTextPanelBG.SetActive(false);
                }
            }
        }

        void ResetNote()
        {
            ReverseCustomNoteUIManager.instance.previousButton.SetActive(false);
            ReverseCustomNoteUIManager.instance.nextButton.SetActive(true);
            pageNum = 0;
        }

        public void CloseNote()
        {
            ReverseCustomNoteUIManager.instance.customReverseMainNoteUI.SetActive(false);
            ReverseCustomNoteUIManager.instance.customReverseNoteTextPanelBG.SetActive(false);
            canReverse = false;
            NoteDisableManager.instance.DisablePlayer(false);
            notesRaycastScript.enabled = true;
            boxCollider.enabled = true;
            isNoteActivate = false;
            ResetNote();
            enabled = false;

            if (hasMultPages)
            {
                ReverseCustomNoteUIManager.instance.ShowPageButtons(false);
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
            if (pageNum < noteReverseText.Length - 1)
            {
                pageNum++;           
                ReverseCustomNoteUIManager.instance.customReverseNoteTextUI.text = noteReverseText[pageNum];
                EnabledButtons();
                ReverseCustomNoteUIManager.instance.customReverseFlipNoteTextUI.text = noteReverseText[pageNum];
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum >= noteReverseText.Length - 1)
                {
                    ReverseCustomNoteUIManager.instance.nextButton.SetActive(false);
                }
            }
        }

        void EnabledButtons()
        {
            ReverseCustomNoteUIManager.instance.previousButton.SetActive(true);
            ReverseCustomNoteUIManager.instance.nextButton.SetActive(true);
        }

        public void BackPage()
        {
            if (pageNum >= 1)
            {
                pageNum--;
                ReverseCustomNoteUIManager.instance.customReverseNoteTextUI.text = noteReverseText[pageNum];
                EnabledButtons();
                ReverseCustomNoteUIManager.instance.customReverseFlipNoteTextUI.text = noteReverseText[pageNum];
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum < 1)
                {
                    ReverseCustomNoteUIManager.instance.previousButton.SetActive(false);
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
