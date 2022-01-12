using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class BasicNoteController : MonoBehaviour
    {
        [Header("Note Settings")]
        public bool isReadable;

        [Header("Note Scale")]
        [Tooltip("Overal X, Y scale of the note")]
        [SerializeField] private Vector2 pageScale = new Vector2(900, 900);

        [Header("Note Pages")]   
        [SerializeField] private bool hasMultPages = false;
        [Tooltip("Add the image from your project panel to this slot, as a note background")]
        [Space(5)] [SerializeField] private Sprite[] pageImages = null;
        private int pageNum = 0;

        private NotesRaycast notesRaycastScript;
        private BoxCollider boxCollider;
        private bool canClick;

        [Header("Allow playable note audio?")]
        [SerializeField] private bool allowAudioPlayback = false;

        [Header("Audio Clip Settings")]
        [SerializeField] private bool playOnOpen = false;
        [SerializeField] private string noteAudioName = "AudioClip";
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
            BasicNoteUIManager.instance.noteController = gameObject.GetComponent<BasicNoteController>();
            StartCoroutine(WaitTime());

            if (pageNum <= 1)
            {
                BasicNoteUIManager.instance.previousButton.SetActive(false);
            }

            if (hasMultPages)
            {
                BasicNoteUIManager.instance.ShowPageButtons(true);
            }

            notesRaycastScript.enabled = false;
            boxCollider.enabled = false;

            BasicNoteUIManager.instance.basicNotePageUI.sprite = pageImages[pageNum];
            BasicNoteUIManager.instance.basicNotePageUI.rectTransform.sizeDelta = pageScale;
            NoteAudioManager.instance.Play(noteFlipAudio);
            BasicNoteUIManager.instance.basicNoteMainUI.SetActive(true);
            NoteDisableManager.instance.DisablePlayer(true);

            if (allowAudioPlayback)
            {
                BasicNoteUIManager.instance.ShowAudioPrompt();
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
            if (pageNum < pageImages.Length - 1)
            {
                pageNum++;
                BasicNoteUIManager.instance.basicNotePageUI.sprite = pageImages[pageNum];
                EnabledButtons();
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum >= pageImages.Length - 1)
                {
                    BasicNoteUIManager.instance.nextButton.SetActive(false);
                }
            }
        }

        void EnabledButtons()
        {
            BasicNoteUIManager.instance.previousButton.SetActive(true);
            BasicNoteUIManager.instance.nextButton.SetActive(true);
        }

        public void BackPage()
        {        
            if (pageNum >= 1)
            {
                pageNum--;
                BasicNoteUIManager.instance.basicNotePageUI.sprite = pageImages[pageNum];
                EnabledButtons();
                NoteAudioManager.instance.Play(noteFlipAudio);
                if (pageNum < 1)
                {
                    BasicNoteUIManager.instance.previousButton.SetActive(false);
                }
            }
        }

        void ResetNote()
        {
            BasicNoteUIManager.instance.previousButton.SetActive(false);
            BasicNoteUIManager.instance.nextButton.SetActive(true);
            pageNum = 0;
        }

        public void CloseNote()
        {
            BasicNoteUIManager.instance.basicNoteMainUI.SetActive(false);
            NoteDisableManager.instance.DisablePlayer(false);
            notesRaycastScript.enabled = true;
            boxCollider.enabled = true;
            isReadable = true;
            ResetNote();
            enabled = false;

            if (hasMultPages)
            {
                BasicNoteUIManager.instance.ShowPageButtons(false);
                BasicNoteUIManager.instance.audioPromptUI.SetActive(false);
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

        private void PlayAudio()
        {
            NoteAudioManager.instance.Play(noteAudioName);
            audioPlaying = true;
        }

        private void StopAudio()
        {
            NoteAudioManager.instance.StopPlaying(noteAudioName);
            audioPlaying = false;
        }

        private void PauseAudio()
        {
            NoteAudioManager.instance.PausePlaying(noteAudioName);
            audioPlaying = false;
        }
    }
}
