using _current.Infrastructure.AssetManagement;

namespace _current.UI.Popups
{
    public class BasePopupViewModel : IPopupViewModel
    {
        public BasePopupViewModel(bool isInSceneCanvas)
        {
            IsInSceneCanvas = isInSceneCanvas;
        }

        public void Confirm()
        {
            
        }
        
        public void Dispose()
        {
            
        }

        public bool IsInSceneCanvas { get; }
        public virtual string AssetPath => AssetsPath.OneButtonPopup;
    }
}