using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] 
        private float _moveSpeed = 5f;
        
        private InputSystem_Actions _inputAction;
        private Vector2 _inputVector;
        private float _deadZone;

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
            // カメラの方向に向かって移動する
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
            
            if (_inputVector.y > _deadZone || _inputVector.y < -_deadZone)
            {
                transform.Translate(cameraForward * (_inputVector.y * Time.fixedDeltaTime * _moveSpeed));    
            }

            if (_inputVector.x > _deadZone || _inputVector.x < -_deadZone)
            {
                transform.Translate(cameraRight * (_inputVector.x * Time.fixedDeltaTime * _moveSpeed));
            }
        }

        // 簡易的な移動の実装
        private void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
        }
    }
}