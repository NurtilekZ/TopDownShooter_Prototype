using Core.Logic;

namespace Services.LevelProgress
{
    public interface ILevelProgressService
    {
        public LevelProgressWatcher LevelProgressWatcher { get; set; }

        void InitForLevel(LevelProgressWatcher levelProgressWatcher);
    }
}