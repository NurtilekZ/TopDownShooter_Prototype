using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Enemy
{
    public abstract class EnemyFollowBase : MonoBehaviour
    {
        protected bool Enabled;
        protected Transform HeroTransform;

        public virtual void Initialize(GameObject hero)
        {
            HeroTransform = hero.transform;
            HeroTransform.OnDestroyAsObservable().Subscribe(_ => Stop());
        }

        public virtual void FollowTo(Transform hero = null)
        {
            enabled = true;
            if (hero != null) HeroTransform = hero.transform;
        }

        public void Stop()
        {
            Enabled = false;
        }
    }
}