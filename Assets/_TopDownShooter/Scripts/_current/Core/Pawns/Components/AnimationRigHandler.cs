using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _current.Core.Pawns.Components
{
    public class AnimationRigHandler : PawnComponent
    {
        [SerializeField] private float _weightsTransitionSpeed = 2f;
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private MultiAimConstraint[] _aimConstraints;
        [SerializeField] private TwoBoneIKConstraint _handConstraintL;
        [SerializeField] private TwoBoneIKConstraint _handConstraintR;

        private Coroutine weightsCoroutine;

        protected override void Bind()
        {
        }

        protected override void Unbind()
        {
        }
        
        public void BuildRig()
        {
            _rigBuilder.Build();
        }

        public void SetAimWeights(float weight)
        {
            StartCoroutine(SetWeights(_aimConstraints, weight));
        }

        public void SetHandsWeights(float weight)
        {
            StartCoroutine(SetWeights(
                new IRigConstraint[] { _handConstraintL, _handConstraintR }, 
                weight));
        }

        private IEnumerator SetWeights(IRigConstraint[] aimConstraints, float weight)
        {
            float time = 0;

            while (Math.Abs(aimConstraints[0].weight - weight) == 0)
            {
                foreach (var constraint in aimConstraints)
                {
                    constraint.weight = Mathf.Lerp(constraint.weight, weight, time * _weightsTransitionSpeed);
                }

                time += Time.deltaTime;
                yield return null;
            }
        }

        public void SetHandGrips(Transform rigGripR, Transform rigGripL)
        {
            _handConstraintR.data.target = rigGripR;
            _handConstraintL.data.target = rigGripL;
        }
    }
}