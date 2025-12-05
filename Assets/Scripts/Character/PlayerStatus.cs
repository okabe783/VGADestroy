using UnityEngine;

namespace VGADestroy.Character
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _power;

        public void AddSpeed(float value)
        {
            Debug.Log("Adding Speed");
            _speed += value;
        }

        public void AddPower(float value)
        {
            Debug.Log("Adding Power");
            _power += value;
        }
    }
}