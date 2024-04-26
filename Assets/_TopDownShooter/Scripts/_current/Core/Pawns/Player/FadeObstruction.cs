using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _current.Core.Pawns.Components;
using UnityEngine;
using UnityEngine.Rendering;

namespace _current.Core.Pawns.Player
{
    public class FadeObstruction : PawnComponent
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _fadedAlpha = 0.33f;
        [SerializeField] private bool _retainShadow = true;
        [SerializeField] private Vector3 _targetPositionOffset = Vector3.up;
        [SerializeField] private float _fadeSpeed = 1;
        [SerializeField] private Camera _camera;

        [SerializeField] private List<FadingObject> _objectsBlockingView = new();
        private readonly Dictionary<FadingObject, Coroutine> _runningCoroutines = new();
        private readonly RaycastHit[] _hits = new RaycastHit[10];
        
        #region ShaderProperties
        
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int Surface = Shader.PropertyToID("_Surface");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        #endregion

        protected override void Bind()
        {
        }

        protected override void Unbind()
        {
        }

        private void LateUpdate()
        {
            CheckForObject();
        }

        private void CheckForObject()
        {
            while (true)
            {
                var camPos = _camera.transform.position;
                var position = transform.position;
                int hits = Physics.RaycastNonAlloc
                (
                    camPos,
                    (position + _targetPositionOffset - camPos).normalized,
                    _hits,
                    Vector3.Distance(camPos, position + _targetPositionOffset),
                    _layerMask
                );
                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        FadingObject[] fadingObjects = GetFadingObjectFromHit(_hits[i]);

                        if (fadingObjects == null) continue;
                        foreach (var fadingObject in fadingObjects)
                        {
                            if (_objectsBlockingView.Contains(fadingObject)) continue;
                            TryStopRunningCoroutine(fadingObject);

                            _runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                            _objectsBlockingView.Add(fadingObject);
                        }
                    }
                }

                FadeObjectsNoLongerBeingHit();
                ClearHits();
            }
        }

        private void FadeObjectsNoLongerBeingHit()
        {
            List<FadingObject> objectsToRemove = new List<FadingObject>(_objectsBlockingView.Count);

            foreach (var fadingObject in _objectsBlockingView)
            {
                bool objectIsBeingHit = false;
                for (int i = 0; i < _hits.Length; i++)
                {
                    FadingObject[] hitFadingObject = GetFadingObjectFromHit(_hits[i]);
                    if (hitFadingObject != null && hitFadingObject.Contains(fadingObject))
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }

                if (!objectIsBeingHit)
                {
                    TryStopRunningCoroutine(fadingObject);

                    _runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                    objectsToRemove.Add(fadingObject);
                }
            }

            foreach (var removeObjects in objectsToRemove)
            {
                _objectsBlockingView.Remove(removeObjects);
            }
        }

        private IEnumerator FadeObjectOut(FadingObject fadingObject)
        {
            foreach (var material in fadingObject.Materials)
            {
                material.SetInt(Surface, 1);
                material.SetInt(SrcBlend, (int)BlendMode.SrcAlpha);
                material.SetInt(DstBlend, (int)BlendMode.OneMinusSrcAlpha);
                
                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", _retainShadow);
                
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

                material.renderQueue = (int)RenderQueue.Transparent;
            }

            float time = 0;

            while (fadingObject.Materials[0].color.a > _fadedAlpha)
            {
                foreach (var renderer1 in fadingObject.Materials)
                {
                    var color = renderer1.color;
                    renderer1.SetColor(BaseColor, new Color(
                        color.r, color.g, color.b,
                        Mathf.Lerp(fadingObject.InitialAlpha, _fadedAlpha, time * _fadeSpeed)));
                }

                time += Time.deltaTime;
                yield return null;
            }

            foreach (var material in fadingObject.Materials)
                material.SetInt(ZWrite, 0);

            TryStopRunningCoroutine(fadingObject);
        }

        private IEnumerator FadeObjectIn(FadingObject fadingObject)
        {
            foreach (var material in fadingObject.Materials)
                material.SetInt(ZWrite, 1);

            float time = 0;

            while (fadingObject.Materials[0].color.a < fadingObject.InitialAlpha)
            {
                foreach (var renderer1 in fadingObject.Materials)
                {
                    var color = renderer1.color;
                    renderer1.SetColor(BaseColor, new Color(
                        color.r, color.g, color.b,
                        Mathf.Lerp(fadingObject.InitialAlpha, _fadedAlpha, time * _fadeSpeed)));
                }

                time += Time.deltaTime;
                yield return null;
            }
            
            foreach (var material in fadingObject.Materials)
            {
                material.SetInt(Surface, 0);
                material.SetInt(SrcBlend, (int)BlendMode.One);
                material.SetInt(DstBlend, (int)BlendMode.Zero);
                
                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", true);
                
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                
                material.renderQueue = (int)RenderQueue.Geometry;
            }
            
            TryStopRunningCoroutine(fadingObject);
        }

        private void ClearHits()
        {
            Array.Clear(_hits, 0, _hits.Length);
        }

        private FadingObject[] GetFadingObjectFromHit(RaycastHit hit)
        {
            return hit.collider ? hit.collider.GetComponentsInChildren<FadingObject>() : null;
        }

        private void TryStopRunningCoroutine(FadingObject fadingObject)
        {
            if (_runningCoroutines.TryGetValue(fadingObject, out var coroutine))
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                _runningCoroutines.Remove(fadingObject);
            }
        }
    }
}