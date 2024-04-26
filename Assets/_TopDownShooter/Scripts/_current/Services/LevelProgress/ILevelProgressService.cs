using _current.Core.Logic;

namespace _current.Services.LevelProgress
{
    public interface ILevelProgressService
    {
        public LevelProgressWatcher LevelProgressWatcher { get; set; }

        void InitForLevel(LevelProgressWatcher levelProgressWatcher);
    }
}