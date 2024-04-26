using _current.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Popups
{
    public class BasePopupView : BaseView<BasePopupViewModel>
    {
        [SerializeField] private Button _confirmButton;

        protected override void Bind()
        {
            _confirmButton.onClick.AddListener(_viewModel.Confirm);
        }

        protected override void Unbind()
        {
            _confirmButton.onClick.RemoveListener(_viewModel.Confirm);
        }
    }
}