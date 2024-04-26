using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Popups
{
    public class TwoButtonPopupView : BasePopupView
    {
        [SerializeField] private Button _cancelButton;

        protected override void Bind()
        {
            base.Bind();
            var twoButtonViewModel = (TwoButtonPopupViewModel) _viewModel;
            _cancelButton.onClick.AddListener(twoButtonViewModel.Cancel);
        }
    }
}