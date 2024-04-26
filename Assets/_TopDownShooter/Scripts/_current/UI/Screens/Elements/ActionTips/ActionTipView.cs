using System.Linq;
using _current.Infrastructure.AssetManagement;
using _current.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Elements.ActionTips
{
    public class ActionTipView : SubView<ActionTipViewData>
    {
        [SerializeField] private TMP_Text _actionNameText;
        [SerializeField] private Image _inputButtonImage;
        
        private ActionTipViewData _viewData;
        
        protected override void Bind(ActionTipViewData viewData)
        {
            _viewData = viewData;
            _actionNameText.text = viewData.ActionText;
        }

        public void UpdateButtonIcon(string currentDeviceName)
        {
            var actionInputKey = _viewData.InputAction.controls.First(x=>x.device.displayName == currentDeviceName).displayName;
            Debug.Log(actionInputKey);
            var inputIconName = $"Input_{actionInputKey}";
            _inputButtonImage.sprite = Icons.GetIcon(inputIconName);
        }

        protected override void Unbind()
        {
        }

        private void OnDestroy()
        {
            Dispose();
            Unbind();
        }
    }
}