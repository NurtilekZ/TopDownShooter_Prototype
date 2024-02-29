using System.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
        private const string ClickEffectKey = "SFX_buttonClick";

        [Header("Appearance settings")] [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] [CanBeNull] private RectTransform _windowPanel;
        [SerializeField] [Range(0.1f, 10f)] private float _openingDuration;
        [SerializeField] [Range(0f, 1f)] private float _openingInitialScale;

        [Space] [Header("Text elements")] [SerializeField]
        private TMP_Text _windowTitle;

        [SerializeField] private TMP_Text _windowText;

        protected byte UserAccepted;

        private void Awake()
        {
            SetInitialAppearance();
        }

        [Inject]
        private void Construct()
        {
            //sound service;
        }

        public virtual bool InitAndShow<T>(T data, string titleText = "")
        {
            var text = data as string;

            if (_windowTitle && !string.IsNullOrEmpty(titleText))
                _windowTitle.text = titleText;

            if (_windowText && !string.IsNullOrEmpty(text))
                _windowText.text = text;

            SetVisible(true);

            return true;
        }

        protected virtual void Close()
        {
            SetVisible(false);
        }

        protected void PlaySoundEffect()
        {
            // _soundService.Play(clickEffectKey)
        }

        private void SetInitialAppearance()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.alpha = 0;
            }

            if (_windowPanel != null) _windowPanel.localScale = Vector3.one * _openingInitialScale;
        }

        private async Task<bool> SetVisible(bool value)
        {
            if (_canvasGroup)
                _canvasGroup.blocksRaycasts = value;

            var b = false;
            var animationPromise = new Task<bool>(() => b);

            var se = DOTween.Sequence()
                .Append(_canvasGroup
                    .DOFade(value ? 1 : 0, _openingDuration)
                    .SetEase(Ease.OutQuad))
                .Join(_windowPanel
                    .DOScale(Vector3.one * (value ? 1 : 0.5f), _openingDuration)
                    .SetEase(value ? Ease.OutBounce : Ease.OutQuad))
                .Play()
                .onComplete += OnComplete;

            return await animationPromise;

            void OnComplete()
            {
                b = true;
            }
        }
    }
}