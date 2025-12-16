using UnityEngine;
using UnityEngine.InputSystem;

namespace VGADestroy.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;

        [SerializeField, Header("Rotation Settings")]
        private float _rotationSpeed = 720f;

        [SerializeField, Header("Debug Settings")]
        private bool _isStop = false;

        [Header("Animation Settings")] [SerializeField]
        private Animator _animator;

        private InputSystem_Actions _inputAction;

        private Camera _mainCamera;
        private Vector2 _moveInput;
        private Rigidbody _rb;

        // AnimationHash
        private static readonly int Run = Animator.StringToHash("Run");

        private void Awake()
        {
            _inputAction = new InputSystem_Actions();
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

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _mainCamera = Camera.main;

            if (!_mainCamera) Debug.LogError("シーンに 'MainCamera' タグが設定されたカメラがありません。");
        }

        private void FixedUpdate()
        {
            if (_isStop) return;

            ApplyMovementAndRotation();
        }

        // 物理ベースの移動と回転の適用 (カメラ基準の移動に変更)
        private void ApplyMovementAndRotation()
        {
            if (!_mainCamera) return;
            
            Transform cameraTransform = _mainCamera.transform;

            // カメラの向きから、Y軸回転のみを考慮した前方ベクトルと右方ベクトルを計算
            // Vector3.up (Y軸) に射影することで、上り坂・下り坂による意図しないY方向の動きを排除します
            Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            // 入力 (MoveInput) とカメラの向きを組み合わせて、ワールド座標での目標移動方向を決定
            Vector3 targetDirection = forward * _moveInput.y + right * _moveInput.x;

            // 目標速度 = 目標方向 * スピード
            Vector3 targetVelocity = targetDirection.normalized * _playerStatus.Speed;

            // Y軸の速度（重力など）を保持しつつ、XZ平面の速度を更新
            _rb.linearVelocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);

            // 目標方向（水平方向の移動成分のみ）に十分に動いている場合のみ回転
            if (targetDirection.magnitude > 0.1f)
            {
                // 目標の移動方向を向くためのQuaternionを計算
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // 現在の回転から目標の回転へスムーズに補間
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.fixedDeltaTime
                );
            }
            
            _animator.SetFloat(Run, _rb.linearVelocity.magnitude);
        }
        
        // 行動可能かを制御する
        public void Stop(bool isStoped)
        {
            _isStop = isStoped;
            if (!_isStop) return;

            _rb.linearVelocity = Vector3.zero;
            _animator.SetFloat(Run, 0f);
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
    }
}