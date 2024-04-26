using UnityEngine.InputSystem;

namespace _current.UI.Elements.ActionTips
{
    public class ActionTipViewData
    {
        public string ActionText { get; }
        public InputAction InputAction { get; }

        public ActionTipViewData(string actionText, InputAction inputAction)
        {
            ActionText = actionText;
            InputAction = inputAction;
        }

        public void Dispose()
        {
            
        }
    }
}