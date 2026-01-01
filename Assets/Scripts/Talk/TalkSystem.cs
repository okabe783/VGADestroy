using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Talk
{
    public class TalkSystem : SingletonMonoBehaviour<TalkSystem>
    {
        [Header("View")]
        [SerializeField] private TalkView _talkView;

        [Header("Talk Data")]
        [SerializeField] private string _url;

        private TalkManager _talkManager;
        private Dictionary<string, TalkData> _talkDataMap;
        private InputSystem_Actions _inputActions;
        public TalkManager TalkManager => _talkManager;
        public bool IsTalking => _talkManager is { IsTalking: true };
        
        private float _lastTalkEndTime = -10f;
        [SerializeField] private float _reTalkLockTime = 0.2f;

        public bool CanStartTalk =>
            Time.time - _lastTalkEndTime >= _reTalkLockTime;

        public event Action OnTalkBegin;
        public event Action OnTalkEnd;

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTask Initialize()
        {
            TalkDataLoader loader = new();
            _talkDataMap = await loader.LoadFromUrl(_url);
            _talkManager = new TalkManager();
            _talkManager.SetView(_talkView);
            _inputActions = new InputSystem_Actions();
            _inputActions.Enable();
            _inputActions.UI.Submit.performed += OnSubmit;
        }

        public void BeginTalk()
        {
            // UI だけ有効化
            _inputActions.UI.Enable();

            OnTalkBegin?.Invoke();
        }

        public void EndTalk()
        {
            _lastTalkEndTime = Time.time;

            _inputActions.UI.Disable();
            OnTalkEnd?.Invoke();
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (!IsTalking)
                return;

            _talkManager.RequestNextOrEndByInput();
        }

        public bool TryGetTalkData(string talkID, out TalkData talkData)
        {
            if (_talkDataMap != null)
                return _talkDataMap.TryGetValue(talkID, out talkData);

            talkData = null;
            return false;
        }
    }
}
