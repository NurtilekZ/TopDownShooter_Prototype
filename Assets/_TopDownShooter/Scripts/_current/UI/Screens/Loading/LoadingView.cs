using System.Collections.Generic;
using _current.UI.Core;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _current.UI.Screens.Loading
{
    public class LoadingView : BaseView<LoadingScreenViewModel>
    {
        [SerializeField] private TMP_Text _tipText;
        [SerializeField] private List<string> _tipsList;
        
        protected override void Bind()
        {
            _viewModel.OnClickNextTip += ShowNextTip;
        }

        protected override void Unbind()
        {
            _viewModel.OnClickNextTip += ShowNextTip;
        }

        private void ShowNextTip()
        {
            _tipText.text = GetRandomTip();
        }

        private string GetRandomTip()
        {
            return _tipsList[Random.Range(0, _tipsList.Count)];
        }
    }
}