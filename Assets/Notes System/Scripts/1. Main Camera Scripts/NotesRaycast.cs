using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    [RequireComponent(typeof(Camera))]
    public class NotesRaycast : MonoBehaviour
    {
        [Header("Raycast Features")]
        [SerializeField] private float rayLength = 5;
        private NoteController viewableNote;
        private Camera _camera;

        [Header("Crosshair")]
        [SerializeField] private Image uiCrosshair = null;

        public bool IsLookingAtNote
        {
            get { return viewableNote != null; }
        }

        void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayLength))
            {
                var noteItem = hit.collider.GetComponent<NoteController>();
                if (noteItem != null)
                {
                    viewableNote = noteItem;
                    CrosshairChange(true);
                }
                else
                {
                    ClearExaminable();
                }
            }
            else
            {
                ClearExaminable();
            }

            if (IsLookingAtNote)
            {
                if (Input.GetKeyDown(NoteInputManager.instance.interactKey))
                {
                    viewableNote.DisplayNotes();
                }
            }
        }

        private void ClearExaminable()
        {
            if (viewableNote != null)
            {
                CrosshairChange(false);
                viewableNote = null;
            }
        }

        void CrosshairChange(bool on)
        {
            if (on)
            {
                uiCrosshair.color = Color.red;
            }
            else
            {
                uiCrosshair.color = Color.white;
            }
        }
    }
}
