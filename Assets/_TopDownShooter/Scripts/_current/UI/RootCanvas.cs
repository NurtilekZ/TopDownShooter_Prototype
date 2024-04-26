using UnityEngine;

namespace _current.UI
{
    [RequireComponent(typeof(Canvas))]
    public class RootCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _screensGroup;
        [SerializeField] private CanvasGroup _overlaysGroup;
        [SerializeField] private CanvasGroup _popupsGroup;

        public CanvasGroup ScreensGroup => _screensGroup;
        public CanvasGroup PopupsGroup => _popupsGroup;
        public CanvasGroup OverlaysGroup => _overlaysGroup;
    }
}