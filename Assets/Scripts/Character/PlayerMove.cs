using UnityEngine;
using UnityEngine.InputSystem;

namespace VGADestroy.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [Header("Move Settings")]
        [SerializeField] 
        private float _moveSpeed = 5f;
        [SerializeField]
        private float _turnSpeed = 120f;

        [Header("Smooth Settings")] 
        [SerializeField]
        private float _turnSmooth = 8f;
        
        private InputSystem_Actions _inputAction;

        private float _turnInputSmoothed;
        private float _turnInput;

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

        private void FixedUpdate()
        {
            // 入力をSmoothing(曲げる)
            _turnInputSmoothed = Mathf.Lerp(_turnInputSmoothed,_turnInput,_turnSmooth *  Time.fixedDeltaTime);
            // Player自身を回転
            transform.Rotate(0,_turnInputSmoothed * _turnSpeed *  Time.fixedDeltaTime,0);
            // Playerが前に進む
            transform.Translate(Vector3.forward * (_moveSpeed * Time.fixedDeltaTime));
        }

        // 簡易的な移動の実装
        private void OnMove(InputAction.CallbackContext context)
        {
            _turnInput = context.ReadValue<float>();
        }
    }
}