using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Talk
{
    public class TalkView : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _contentText;

        [Header("Choices")]
        [SerializeField] private Button _choiceButtonPrefab;
        [SerializeField] private Transform _choiceRoot;

        [Header("Canvas")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private float _fadeDuration = 0.25f;

        public Action<int> OnChoiceSelected;

        private readonly List<Button> _choiceButtons = new();
        private CancellationTokenSource _fadeCts;
        private CancellationTokenSource _typingCts;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask OpenAsync(TalkNode node)
        {
            CancelFade();

            _canvasGroup.blocksRaycasts = true;
            await FadeAsync(1f);

            Draw(node);
        }

        public async UniTask CloseAsync()
        {
            CancelTyping();
            CancelFade();

            _canvasGroup.blocksRaycasts = false;
            await FadeAsync(0f);

            ClearChoices();
        }

        public void Draw(TalkNode node)
        {
            CancelTyping();

            _nameText.text = node.Speaker;
            _ = StartTypingAsync(node.Text);

            CreateChoices(node.Choices);
        }

        private async UniTask FadeAsync(float targetAlpha)
        {
            _fadeCts = new CancellationTokenSource();
            var token = _fadeCts.Token;

            float start = _canvasGroup.alpha;
            float time = 0f;

            try
            {
                while (time < _fadeDuration)
                {
                    token.ThrowIfCancellationRequested();

                    time += Time.deltaTime;
                    _canvasGroup.alpha = Mathf.Lerp(start, targetAlpha, time / _fadeDuration);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                _canvasGroup.alpha = targetAlpha;
            }
            catch (OperationCanceledException) { }
        }

        private void CancelFade()
        {
            _fadeCts?.Cancel();
            _fadeCts?.Dispose();
            _fadeCts = null;
        }

        private async UniTaskVoid StartTypingAsync(string text)
        {
            _typingCts = new CancellationTokenSource();
            var token = _typingCts.Token;

            _contentText.text = "";

            try
            {
                foreach (char c in text)
                {
                    token.ThrowIfCancellationRequested();

                    _contentText.text += c;
                    await UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: token);
                }
            }
            catch (OperationCanceledException) { }
        }

        private void CancelTyping()
        {
            _typingCts?.Cancel();
            _typingCts?.Dispose();
            _typingCts = null;
        }

        private void CreateChoices(List<TalkChoice> choices)
        {
            ClearChoices();

            for (int i = 0; i < choices.Count; i++)
            {
                int index = i;
                Button button = Instantiate(_choiceButtonPrefab, _choiceRoot);
                button.GetComponentInChildren<Text>().text = choices[i].Text;
                button.onClick.AddListener(() => OnChoiceSelected?.Invoke(index));
                _choiceButtons.Add(button);
            }
        }

        private void ClearChoices()
        {
            foreach (Button button in _choiceButtons)
                Destroy(button.gameObject);

            _choiceButtons.Clear();
        }

        private void OnDestroy()
        {
            CancelFade();
            CancelTyping();
        }
    }
}
