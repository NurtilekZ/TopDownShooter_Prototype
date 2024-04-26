using _current.Core.Pawns.Player;
using _current.UI.Core;
using UnityEngine;

namespace _current.UI.Screens.GameHUD.SubViews
{
    public class PlayerStatusView : SubView<PlayerHealth>
    {
        [SerializeField] private HealthBarSubView _healthBarSubView;
        [SerializeField] private GunInfoView _gunInfoView;

        protected override void Bind(PlayerHealth data)
        {
            _healthBarSubView.Show(data);
            _gunInfoView.Show(data.GetComponent<PlayerWeaponSpawner>());
        }

        protected override void Unbind()
        {
            _healthBarSubView.Hide();
            _gunInfoView.Hide();
        }
    }
}