using System;
using UnityEngine;

namespace VGADestroy.Character
{
    // 衝突判定などを管理する
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;
        [SerializeField] private PlayerMove _playerMove;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _playerMove.Move(_playerStatus.Speed,_playerStatus.TurnSpeed,_playerStatus.TurnSmooth);
        }
    }
}