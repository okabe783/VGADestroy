using UnityEngine;
using UnityEngine.InputSystem;

namespace VGADestroy.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private bool _isStop = false;
        
        [Header("Animation Settings")]
        [SerializeField] private Animator _animator;
        
        private InputSystem_Actions _inputAction;

        private float _turnInputSmoothed;
        private float _turnInput;
        
        // AnimationHash
        private static readonly int Run = Animator.StringToHash("Run");
        private void Awake()
        {
            _inputAction = new InputSystem_Actions();
            _inputAction.Player.Move.started += OnMove;
            _inputAction.Player.Move.performed += OnMove;
            _inputAction.Player.Move.canceled += OnMove;
        }

        private void OnEnable()
        {
            _inputAction.Enable();
        }
        
        private void OnDisable()
        {
            _inputAction.Dispose();
        }

        // 行動可能かを制御する
        public void Stop(bool isStoped)
        {
            _isStop = isStoped;
        }

        // Playerの移動
        public void Move(float moveSpeed,float turnSpeed,float turnSmooth)
        {
            if(_isStop) return;
            
            // 入力をSmoothing(曲げる)
            _turnInputSmoothed = Mathf.Lerp(_turnInputSmoothed,_turnInput,turnSmooth *  Time.fixedDeltaTime);
            // Player自身を回転
            transform.Rotate(0,_turnInputSmoothed * turnSpeed *  Time.fixedDeltaTime,0);
            // Playerが前に進む
            transform.Translate(Vector3.forward * (moveSpeed * Time.fixedDeltaTime));
            _animator.SetFloat(Run, moveSpeed);
        }

        // 簡易的な移動の実装
        private void OnMove(InputAction.CallbackContext context)
        {
            _turnInput = context.ReadValue<float>();
        }
    }
}