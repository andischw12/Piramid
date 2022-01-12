using UnityEngine;

namespace NoteSystem
{
    public class NoteController : MonoBehaviour
    {
        [Header("Item UI Type")]
        [SerializeField] private UIType _NoteType = UIType.None;

        public enum UIType { None, BasicNote, BasicReverseNote, NormalCustomNote, ReverseCustomNote }

        public void DisplayNotes()
        {
            switch (_NoteType)
            {
                case UIType.BasicNote:
                    BasicNoteController basicNoteController = GetComponent<BasicNoteController>();
                    if (basicNoteController.isReadable)
                    {
                        basicNoteController.enabled = true;
                        basicNoteController.ShowNote();
                    }
                    break;
                case UIType.BasicReverseNote:
                    BasicReverseNoteController basicReverseNoteController = GetComponent<BasicReverseNoteController>();
                    if (basicReverseNoteController.isReadable)
                    {
                        basicReverseNoteController.enabled = true;
                        basicReverseNoteController.ShowNote();
                    }
                    break;
                case UIType.NormalCustomNote:
                    NormalCustomNoteController normalCustomNoteController = GetComponent<NormalCustomNoteController>();
                    if (normalCustomNoteController.isReadable)
                    {
                        normalCustomNoteController.enabled = true;
                        normalCustomNoteController.ShowNote();
                    }
                    break;
                case UIType.ReverseCustomNote:
                    ReverseCustomNoteController reverseCustomController = GetComponent<ReverseCustomNoteController>();
                    if (reverseCustomController.isReadable)
                    {
                        reverseCustomController.enabled = true;
                        reverseCustomController.ShowNote();
                    }
                    break;
            }
        }
    }
}
