using _current.Infrastructure.AssetManagement;
using _current.Infrastructure.SceneManagement;
using _current.Infrastructure.States;
using _current.StaticData;
using _current.StaticData.ScriptableObjects;
using _current.UI.Core.MVVM;

namespace _current.UI.Screens.MainMenu
{
    public class MenuScreenViewModel : IViewModel
    {
        public bool IsInSceneCanvas => true;
        public string AssetPath => AssetsPath.MainMenuScreen;

        private readonly GameStateMachine _gameStateMachine;
        private readonly LevelStaticData _levelStaticData;
        private SceneName _selectedLevel;

        public MenuScreenViewModel(GameStateMachine gameStateMachine, LevelStaticData levelStaticData)
        {
            _gameStateMachine = gameStateMachine;
            _levelStaticData = levelStaticData;
        }

        public void StartNewGame()
        {
            _gameStateMachine.Enter<LoadLevelState, LevelStaticData>(_levelStaticData);
        }

        public void ContinueGame()
        {
            _gameStateMachine.Enter<LoadLevelState, LevelStaticData>(_levelStaticData);
        }

        public void SelectLevel(SceneName level)
        {
            _selectedLevel = level;
        }

        public void OpenSelectLevel()
        {
            // TODO _uiService.Open(new SelectLevelView());
        }

        public void OpenSelectGameMode()
        {
            // TODO: _uiService.Open(new GameModeScreen());
        }

        public void OpenSettings()
        {
            // TODO: _uiService.Open(new SettingsScreen());
        }

        public void ExitGame()
        {
            // TODO: _uiService.Open(new TwoButtonPopupViewModel());
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}