using UnityEngine;
using VGADestroy.Common;

namespace VGADestroy.Character
{
    // 衝突判定などを管理する
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;
        
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}