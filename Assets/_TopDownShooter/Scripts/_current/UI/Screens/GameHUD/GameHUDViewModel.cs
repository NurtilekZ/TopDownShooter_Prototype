using _current.Core.Pawns.Player;
using _current.Data.Data;
using _current.Infrastructure.AssetManagement;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;
using _current.UI.Core.MVVM;

namespace _current.UI.Screens.GameHUD
{
    public class GameHUDViewModel : IViewModel
    {
        private readonly LevelProgressData _levelProgressData;
        private readonly LevelStaticData _levelStaticData;
        public bool IsInSceneCanvas => true;
        public string AssetPath => AssetsPath.GameHUDScreen;
        public PlayerHealth Pawn { get; }

        public GameHUDViewModel(LevelProgressData levelProgressData, LevelStaticData levelStaticData)
        {
            _levelProgressData = levelProgressData;
            _levelStaticData = levelStaticData;
            Pawn = levelProgressData.Hero.GetComponent<PlayerHealth>();
        }


        public void Dispose()
        {
            
        }
    }
}