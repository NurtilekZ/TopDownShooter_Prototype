using _current.Core.Logic;

namespace _current.Services.LevelProgress
{
    public class LevelProgressService : ILevelProgressService
    {
        public LevelProgressWatcher LevelProgressWatcher { get; set; }

        public void InitForLevel(LevelProgressWatcher levelController)
            => LevelProgressWatcher = levelController;
    }
}