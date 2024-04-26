using System.Collections;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public class ChangeColorFromDamage : PawnComponent
    {
        [SerializeField] private PawnHealth _health;
        [SerializeField] private Renderer _renderer;
        
        private MaterialPropertyBlock _materialPropertyBlock;
        private Coroutine _damageColorCoroutine;
        
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        protected override void Bind()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            if (_health) _health.OnTakeDamage += OnTakeDamage;
        }

        protected override void Unbind()
        {
            if (_health) _health.OnTakeDamage -= OnTakeDamage;
        }

        private void OnTakeDamage(float obj)
        {
            if (_damageColorCoroutine != null)
                StopCoroutine(_damageColorCoroutine);
            _damageColorCoroutine = StartCoroutine(ChangeColor());
        }

        private IEnumerator ChangeColor()
        {
            _materialPropertyBlock.SetColor(BaseColor, Color.red);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
            yield return new WaitForSeconds(0.2f);
            _materialPropertyBlock.SetColor(BaseColor, Color.white);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}