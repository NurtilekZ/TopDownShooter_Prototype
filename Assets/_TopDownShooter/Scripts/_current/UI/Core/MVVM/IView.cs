using System;

namespace _current.UI.Core.MVVM
{
    public interface IView :  IDisposable
    {
        public event Action OnShow;
        public event Action OnHide;
        void ShowAndBind(IViewModel viewModel);
        void Show();
        void HideAndUnbind();
        void Hide();
    }
}