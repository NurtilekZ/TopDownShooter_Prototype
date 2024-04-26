using _current.Infrastructure.AssetManagement;

namespace _current.UI.Popups
{
    public class TwoButtonPopupViewModel : BasePopupViewModel
    {
        public override string AssetPath => AssetsPath.OneButtonPopup;

        public TwoButtonPopupViewModel(bool isInSceneCanvas) : base(isInSceneCanvas)
        {
        }

        public void Cancel()
        {
            
        }
    }
}