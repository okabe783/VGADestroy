using UnityEngine;

namespace VGADestroy.Character
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] private float _power;
        
        [Header("Move Settings")]
        [SerializeField] 
        private float _moveSpeed = 5f;
        [SerializeField]
        private float _turnSpeed = 120f;

        // イベントを発行してアイテムはそれを発火する
        public float Speed => _moveSpeed;
        public float Power => _power;

        public float TurnSpeed => _turnSpeed;

        public void AddSpeed(float value)
        {
            _moveSpeed += value;
        }

        public void AddPower(float value)
        {
            _power += value;
        }
    }
}