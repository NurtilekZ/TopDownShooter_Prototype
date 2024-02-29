using UnityEngine;

namespace Core.Enemy
{
    public class EnemyFollowRotate : EnemyFollowBase
    {
        [SerializeField] private Transform _transformForRotate;

        private void Update()
        {
            if (Enabled) RotateToTarget();
        }

        private void RotateToTarget()
        {
            var positionDelta = HeroTransform.position - _transformForRotate.position;
            positionDelta.y = _transformForRotate.position.y;

            _transformForRotate.rotation = Quaternion.Lerp(
                _transformForRotate.rotation,
                Quaternion.LookRotation(positionDelta),
                2f * Time.deltaTime
            );
        }
    }
}