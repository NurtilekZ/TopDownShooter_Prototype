using _current.Data;
using _current.UI.Core;
using TMPro;
using UnityEngine;

namespace _current.UI.Screens.GameHUD.SubViews
{
    public class LootCounterView : SubView<Loot>
    {
        [SerializeField] private TMP_Text _valueText;

        protected override void Bind(Loot data)
        {
            _valueText.text = $"{data.value}";
        }

        protected override void Unbind()
        {
        }
    }
}