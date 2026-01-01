using System;
using Talk;
using UnityEngine;

namespace VGADestroy.Character
{
    public class PlayerController : MonoBehaviour
    {
        private InputSystem_Actions _inputAction;

        // 入力イベント
        public event Action<Vector2> OnMove;
        public event Action<bool> OnDash;
        public event Action OnInteract;

        private bool _canControl = true;

        private void Awake()
        {
            _inputAction = new InputSystem_Actions();

            _inputAction.Player.Move.performed += ctx =>
            {
                if (!_canControl) return;
                OnMove?.Invoke(ctx.ReadValue<Vector2>());
            };

            _inputAction.Player.Move.canceled += _ =>
            {
                if (!_canControl) return;
                OnMove?.Invoke(Vector2.zero);
            };

            _inputAction.Player.Dash.performed += _ =>
            {
                if (!_canControl) return;
                OnDash?.Invoke(true);
            };

            _inputAction.Player.Dash.canceled += _ =>
            {
                if (!_canControl) return;
                OnDash?.Invoke(false);
            };

            _inputAction.Player.Interact.performed += _ =>
            {
                OnInteract?.Invoke();
            };
        }

        private void OnEnable()
        {
            _inputAction.Enable();

            TalkSystem.I.OnTalkBegin += HandleTalkBegin;
            TalkSystem.I.OnTalkEnd += HandleTalkEnd;
        }

        private void OnDisable()
        {
            _inputAction.Disable();

            TalkSystem.I.OnTalkBegin -= HandleTalkBegin;
            TalkSystem.I.OnTalkEnd -= HandleTalkEnd;
        }

        private void HandleTalkBegin()
        {
            _canControl = false;

            // 強制停止
            OnMove?.Invoke(Vector2.zero);
            OnDash?.Invoke(false);
        }

        private void HandleTalkEnd()
        {
            _canControl = true;
        }
    }
}
