using _current.Core.Pawns.Player;
using _current.UI.Core;
using _current.UI.Screens.GameHUD.SubViews;
using UnityEngine;

namespace _current.UI.Screens.GameHUD
{
    public class GameHUDView : BaseView<GameHUDViewModel>
    {
        [SerializeField] private PlayerStatusView _playerInfoView;
        [SerializeField] private CrosshairView _crosshairView;

        protected override void Bind()
        {
            _playerInfoView.Show(_viewModel.Pawn);
            _crosshairView.Show(_viewModel.Pawn.GetComponent<PlayerWeaponSpawner>());
        }
        
        protected override void Unbind()
        {
        }
    }
}