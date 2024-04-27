using _current.Utils;

namespace _current.Core.Pawns.Components
{
    public class SpawnerPawnAnimationHandler : AnimationHandlerBase
    { 
        public void PlaySpawn() => animator.SetTrigger(AnimationStatics.Spawn);
    }
}