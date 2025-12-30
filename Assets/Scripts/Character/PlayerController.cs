using System;
using UnityEngine;

namespace VGADestroy.Character
{
    // 衝突判定などを管理する
    public class PlayerController : MonoBehaviour
    {
        private InputSystem_Actions _inputAction;
        
        public InputSystem_Actions Action => _inputAction;

        private void Awake()
        {
            _inputAction = new InputSystem_Actions();
        }
        
        private void OnEnable()
        {
            _inputAction.Enable();
        }

        private void OnDisable()
        {
            _inputAction.Dispose();
        }
    }
}