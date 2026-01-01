using UnityEngine;

namespace VGADestroy.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Animator _animator;

        [Header("Rotation Settings")]
        [SerializeField] private float _rotationSpeed = 720f;

        private Rigidbody _rb;
        private Camera _mainCamera;

        private Vector2 _moveInput;
        private bool _isDash;

        private static readonly int Run = Animator.StringToHash("Run");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;

            _mainCamera = Camera.main;
            if (!_mainCamera)
                Debug.LogError("MainCamera が見つかりません");
        }

        private void Start()
        {
            // Controllerから入力を受け取る
            _playerController.OnMove += v => _moveInput = v;
            _playerController.OnDash += d => _isDash = d;
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            if (!_mainCamera) return;

            Vector3 forward = Vector3.ProjectOnPlane(
                _mainCamera.transform.forward, Vector3.up).normalized;

            Vector3 right = Vector3.ProjectOnPlane(
                _mainCamera.transform.right, Vector3.up).normalized;

            Vector3 direction = forward * _moveInput.y + right * _moveInput.x;

            float speed = _isDash
                ? _playerStatus.DashSpeed
                : _playerStatus.Speed;

            Vector3 velocity = direction.normalized * speed;

            _rb.linearVelocity = new Vector3(
                velocity.x,
                _rb.linearVelocity.y,
                velocity.z
            );

            // 回転
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.fixedDeltaTime
                );
            }

            // Animator
            float animSpeed = _rb.linearVelocity.magnitude / _playerStatus.DashSpeed;
            _animator.SetFloat(Run, Mathf.Clamp01(animSpeed));
        }
    }
}
