using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class BasicReverseNoteController : MonoBehaviour
    {
        [Header("Note Settings")]
        public bool isReadable;

        [Header("Reverve Note Background Settings")]
        [SerializeField] private Vector2 pageScale = new Vector2(900, 900);

        [Header("Note Pages")]
        [SerializeField] private bool hasMultPages = false;
        [Tooltip("Add the image from your project panel to this slot, as a note background")]
        [Space(10)][SerializeField] private Sprite[] notePageImages = null;
        private int pageNum = 0;

        [Space(10)] [TextArea(4, 8)] public string[] noteReverseText;

        [Header("Reverse Pop-out Scale Settings")]
        [Tooltip("This is the scale of where the text is applied, usually slightly smaller than the object below")]
        [SerializeField] private Vector2 noteTextAreaScale = new Vector2(1045, 300);
        [Tooltip("This is the scale of background image for the reverse text")]
        [SerializeField] private Vector2 customTextBGScale = new Vector2(1160, 300);
        [Tooltip("This is the background colour of the reverse text - Make sure the alpha value is set to 1")]
        [SerializeField] private Color customTextBGColor = Color.white;

        [Header("Reverse Text Font Settings")]
        [Space(5)][SerializeField] private int textSize = 25;
        [SerializeField] private Font fontType = null;
        [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
        [Tooltip("Make sure the alpha value is set to 1")]
        [SerializeField] private Color fontColor = Color.black;

        private NotesRaycast notesRaycastScript;
        private BoxCollider boxCollider;
        private bool canReverse;
        private bool canClick;
        [HideInInspector] public bool isNoteActive;

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
            BasicReverseNoteUIManager.instance.noteController = gameObject.GetComponent<BasicReverseNoteController>();
            BasicReverseNoteUIManager noteController = BasicReverseNoteUIManager.instance; 
            StartCoroutine(WaitTime());

            if (pageNum <= 1)
            {
                BasicReverseNoteUIManager.instance.previousButton.SetActive(false);
            }

            if (hasMultPages)
            {
                noteController.ShowPageButtons(true);
            }

            noteController.reverseNoteTextUI.rectTransform.sizeDelta = noteTextAreaScale;
            noteController.reverseNoteTextUI.text = noteReverseText[pageNum];
            noteController.reverseNoteTextUI.fontSize = textSize;
            noteController.reverseNoteTextUI.fontStyle = fontStyle;
            noteController.reverseNoteTextUI.font = fontType;
            noteController.reverseNoteTextUI.color = fontColor;

            noteController.reverseNotePageUI.sprite = notePageImages[pageNum];
            noteController.reverseNotePageUI.rectTransform.sizeDelta = pageScale;

            noteController.reverseNoteTextImage.rectTransform.sizeDelta = customTextBGScale;
            noteController.reverseNoteTextImage.color = customTextBGColor;

            NoteAudioManager.instance.Play(noteFlipAudio);
            noteController.reverseNoteMainUI.SetActive(true);
            NoteDisableManager.instance.DisablePlayer(true);
            notesRaycastScript.enabled = false;
            boxCollider.enabled = false;
            isNoteActive = true;

            if (allowAudioPlayback)
            {
                noteController.ShowAudioPrompt();
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

        public void NextPage()
        {
            if (pageNum < notePageImages.Length - 1)
            {
                pageNum++;
                BasicReverseNoteUIManager.instance.reverseNotePageUI.sprite = notePageImages[pageNum];
                EnabledButtons();
                BasicReverseNoteUIManager.instance.reverseNoteTextUI.text = noteReverseText[pageNum];
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum >= notePageImages.Length - 1)
                {
                    BasicReverseNoteUIManager.instance.nextButton.SetActive(false);
                }
            }
        }

        void EnabledButtons()
        {
            BasicReverseNoteUIManager.instance.previousButton.SetActive(true);
            BasicReverseNoteUIManager.instance.nextButton.SetActive(true);
        }

        public void BackPage()
        {
            if (pageNum >= 1)
            {
                pageNum--;
                BasicReverseNoteUIManager.instance.reverseNotePageUI.sprite = notePageImages[pageNum];
                EnabledButtons();
                BasicReverseNoteUIManager.instance.reverseNoteTextUI.text = noteReverseText[pageNum];
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum < 1)
                {
                    BasicReverseNoteUIManager.instance.previousButton.SetActive(false);
                }
            }
        }

        public void ReverseNoteAction()
        {
            if (isNoteActive)
            {
                canReverse = !canReverse;

                if (canReverse)
                {
                    BasicReverseNoteUIManager.instance.reverseNoteTextPanelUI.SetActive(true);
                }
                else
                {
                    BasicReverseNoteUIManager.instance.reverseNoteTextPanelUI.SetActive(false);
                }
            }
        }

        void ResetNote()
        {
            BasicReverseNoteUIManager.instance.previousButton.SetActive(false);
            BasicReverseNoteUIManager.instance.nextButton.SetActive(true);
            pageNum = 0;
        }

        public void CloseNote()
        {
            BasicReverseNoteUIManager.instance.reverseNoteMainUI.SetActive(false);
            BasicReverseNoteUIManager.instance.reverseNoteTextPanelUI.SetActive(false);
            canReverse = false;

            NoteDisableManager.instance.DisablePlayer(false);
            notesRaycastScript.enabled = true;
            boxCollider.enabled = true;
            isNoteActive = false;
            ResetNote();
            enabled = false;


            if (hasMultPages)
            {
                BasicReverseNoteUIManager.instance.ShowPageButtons(false);
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
